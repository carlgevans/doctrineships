namespace DoctrineShips.Service.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Validation;
    using LinqToTwitter = LinqToTwitter;
    using Tools;

    /// <summary>
    /// A class dealing with Doctrine Ships scheduled, maintenance or site related tasks.
    /// </summary>
    internal sealed class TaskManager
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;
        private readonly IDoctrineShipsValidation doctrineShipsValidation;
        private readonly ISystemLogger logger;

        /// <summary>
        /// Initialises a new instance of a Task Manager.
        /// </summary>
        /// <param name="doctrineShipsRepository">An IDoctrineShipsRepository instance.</param>
        /// <param name="doctrineShipsValidation">An IDoctrineShipsValidation instance.</param>
        /// <param name="logger">An ISystemLogger logger instance.</param>
        internal TaskManager(IDoctrineShipsRepository doctrineShipsRepository, IDoctrineShipsValidation doctrineShipsValidation, ISystemLogger logger)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
            this.doctrineShipsValidation = doctrineShipsValidation;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches and returns log entries for a given time period.
        /// </summary>
        /// <param name="logPeriod">A period of time (TimeSpan) to return logs for.</param>
        /// <returns>A list of log messages.</returns>
        internal IEnumerable<LogMessage> GetLogMessages(TimeSpan logPeriod)
        {
            return this.doctrineShipsRepository.GetLogMessages(logPeriod);
        }

        /// <summary>
        /// Clears all log entries.
        /// </summary>
        internal void ClearLog()
        {
            this.doctrineShipsRepository.ClearLog();
            this.logger.LogMessage("Log Cleared.", 2, "Message", MethodBase.GetCurrentMethod().Name);
            this.doctrineShipsRepository.Save();
        }

        /// <summary>
        /// Deletes log entries older than 7 days.
        /// </summary>
        internal void DeleteOldLogs()
        {
            this.doctrineShipsRepository.DeleteLogsOlderThanDate(DateTime.UtcNow - TimeSpan.FromDays(7));
            this.logger.LogMessage("Log Entries Older Than 7 Days Deleted.", 2, "Message", MethodBase.GetCurrentMethod().Name);
            this.doctrineShipsRepository.Save();
        }

        /// <summary>
        /// Send out daily ship fit availability summaries for all accounts.
        /// <param name="twitterContext">A twitter context for the sending of messages.</param>
        /// </summary>
        internal async Task SendDailySummary(LinqToTwitter::TwitterContext twitterContext)
        {
            // Fetch all enabled accounts and their notification recipients.
            IEnumerable<Account> accounts = this.doctrineShipsRepository.GetAccountsForNotifications();

            // Are there any enabled accounts?
            if (accounts != null && accounts.Any() == true)
            {
                foreach (var account in accounts)
                {
                    // Are there any recipients for this account?
                    if (account.NotificationRecipients != null && account.NotificationRecipients.Any() == true)
                    {
                        // Get the summary message for the current account.
                        string summaryMessage = GetDailySummaryMessage(account);

                        // As long as the summary message is not empty, continue.
                        if (summaryMessage != string.Empty)
                        {
                            foreach (var recipient in account.NotificationRecipients)
                            {
                                // If the current recipient is set to receive daily summaries, send a message.
                                if (recipient.ReceivesDailySummary == true)
                                {
                                    await this.SendDirectMessage(twitterContext, recipient.TwitterHandle, summaryMessage);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Send out any ship fit availability alerts for all accounts.
        /// <param name="twitterContext">A twitter context for the sending of messages.</param>
        /// </summary>
        internal async Task SendAvailabilityAlert(LinqToTwitter::TwitterContext twitterContext)
        {
            // Fetch all enabled accounts and their notification recipients.
            IEnumerable<Account> accounts = this.doctrineShipsRepository.GetAccountsForNotifications();

            // Are there any enabled accounts?
            if (accounts != null && accounts.Any() == true)
            {
                foreach (var account in accounts)
                {
                    // Are there any recipients for this account?
                    if (account.NotificationRecipients != null && account.NotificationRecipients.Any() == true)
                    {
                        // Get the alert message for the current account.
                        string alertMessage = GetAvailabilityAlertMessage(account);

                        // As long as the alert message is not empty, continue.
                        if (alertMessage != string.Empty)
                        {
                            foreach (var recipient in account.NotificationRecipients)
                            {
                                // If the current recipient is set to receive alerts and has not received one too recently, send a message.
                                if (recipient.ReceivesAlerts == true && Time.HasElapsed(recipient.LastAlert, TimeSpan.FromHours(recipient.AlertIntervalHours)))
                                {
                                    await this.SendDirectMessage(twitterContext, recipient.TwitterHandle, alertMessage);

                                    // Update the LastAlert timestamp for the current recipient.
                                    recipient.LastAlert = DateTime.UtcNow;
                                    this.doctrineShipsRepository.UpdateNotificationRecipient(recipient);
                                    this.doctrineShipsRepository.Save();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates a summary message containing ship fit contract counts.
        /// </summary>
        /// <param name="account">The account for which a summary message should be generated.</param>
        /// <returns>A string containing a summary message.</returns>
        internal string GetDailySummaryMessage(Account account)
        {
            string summaryMessage = string.Empty;
            int notMonitoredCount = 0, totalCount = 0, alertCount = 0, okCount = 0;

            // Are there any ship fits for this account and is there a setting profile?
            if (account.ShipFits != null && account.SettingProfile != null)
            {
                foreach (var shipFit in account.ShipFits)
                {
                    // Add the contract count for the current ship fit to the running total.
                    if (shipFit.ContractsAvailable >= 0) totalCount += shipFit.ContractsAvailable;
                    
                    // Count the contracts of each type for the current ship fit.
                    if (shipFit.IsMonitored == false) notMonitoredCount++;
                    else if (shipFit.ContractsAvailable >= 0 && shipFit.ContractsAvailable <= account.SettingProfile.AlertThreshold) alertCount++;
                    else okCount++;
                }

                // Generate the message.
                summaryMessage += "Daily Summary - Total Valid Contracts:" + totalCount + ", Fits Below Alert Threshold:" + alertCount + ", Fits Above Alert Threshold:" + okCount + ", Fits Not Monitored:" + notMonitoredCount;
            }

            return summaryMessage;
        }

        /// <summary>
        /// Generates a ship fit availability alert message.
        /// </summary>
        /// <param name="account">The account for which an alert message should be generated.</param>
        /// <returns>A string containing an alert message.</returns>
        internal string GetAvailabilityAlertMessage(Account account)
        {
            string alertMessage = string.Empty;
            int notMonitoredCount = 0, alertCount = 0;

            // Are there any ship fits for this account and is there a setting profile?
            if (account.ShipFits != null && account.SettingProfile != null)
            {
                foreach (var shipFit in account.ShipFits)
                {
                    // Count number of contracts that are at or below the alert threshold, ignore fits that are not monitored or if levels are ok.
                    if (shipFit.IsMonitored == false) notMonitoredCount++;
                    else if (shipFit.ContractsAvailable >= 0 && shipFit.ContractsAvailable <= account.SettingProfile.AlertThreshold) alertCount++;
                }

                // If there are ship fits that are at or below the alert threshold, generate an alert message.
                if (alertCount > 0)
                {
                    // Generate the message.
                    alertMessage += "Ship Fit Availability Alert - Fits Below Alert Threshold:" + alertCount;
                }
            }

            return alertMessage;
        }

        /// <summary>
        /// Send a twitter direct message.
        /// <param name="twitterContext">A twitter context for the sending of messages.</param>
        /// </summary>
        internal async Task<bool> SendDirectMessage(LinqToTwitter::TwitterContext twitterContext, string screenName, string message)
        {
            try
            {
                var result = await twitterContext.NewDirectMessageAsync(screenName, message);

                if (result != null)
                {
                    this.logger.LogMessage("Sent A Direct Message To '" + result.RecipientScreenName + "' With Message '" + result.Text + "'.", 2, "Message", MethodBase.GetCurrentMethod().Name);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                logger.LogMessage("An error occured while sending a twitter message. Are the twitter api credentials correct?", 0, "Message", MethodBase.GetCurrentMethod().Name);
                logger.LogMessage(e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        /// <summary>
        /// Generate and add a short url from a passed long url.
        /// <param name="longUrl">A long url to be shortened.</param>
        /// <returns>Returns a shortUrlId string.</returns>
        /// </summary>
        internal string AddShortUrl(string longUrl)
        {
            if (!String.IsNullOrEmpty(longUrl))
            {
                // Populate a new ShortUrl object.
                ShortUrl newShortUrl = new ShortUrl();

                // Populate the remaining properties.
                newShortUrl.ShortUrlId = Security.GenerateRandomString(6); // 44,261,653,680 Permutations.
                newShortUrl.LongUrl = longUrl;
                newShortUrl.DateCreated = DateTime.UtcNow;

                // Add the new short url and log the event. 
                // An exception will be thrown if the key already exists. 1 in 44 billion chance and users will just request it again.
                this.doctrineShipsRepository.CreateShortUrl(newShortUrl);
                this.doctrineShipsRepository.Save();
                logger.LogMessage("Short Url '" + newShortUrl.ShortUrlId + "' Generated.", 2, "Message", MethodBase.GetCurrentMethod().Name);

                // Return the value.
                return newShortUrl.ShortUrlId;
            }
            else
            {
                return "Error";
            }
        }

        /// <summary>
        /// Fetches and returns a long url from a short url id.
        /// <param name="shortUrlId">A shortUrlId relating to the stored longUrl.</param>
        /// <returns>Returns a longUrl string.</returns>
        /// </summary>
        internal string GetLongUrl(string shortUrlId)
        {
            ShortUrl shortUrl = this.doctrineShipsRepository.GetShortUrl(shortUrlId);

            if (shortUrl != null)
            {
                return shortUrl.LongUrl;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Deletes short url entries older than 30 days.
        /// </summary>
        internal void DeleteOldShortUrls()
        {
            this.doctrineShipsRepository.DeleteShortUrlsOlderThanDate(DateTime.UtcNow - TimeSpan.FromDays(30));
            this.logger.LogMessage("Short Urls Older Than 30 Days Deleted.", 2, "Message", MethodBase.GetCurrentMethod().Name);
            this.doctrineShipsRepository.Save();
        }
    }
}
