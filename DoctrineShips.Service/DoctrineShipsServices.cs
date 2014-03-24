namespace DoctrineShips.Service
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Service.Managers;
    using DoctrineShips.Validation;
    using EveData;
    using LinqToTwitter = LinqToTwitter;
    using Tools;

    /// <summary>
    /// A concrete implementation of IDoctrineShipsServices.
    /// </summary>
    public class DoctrineShipsServices : IDoctrineShipsServices
    {
        // External Dependencies.
        private readonly IDoctrineShipsRepository doctrineShipsRepository;
        private readonly IEveDataSource eveDataSource;
        private readonly IDoctrineShipsValidation doctrineShipsValidation;
        private readonly ISystemLogger logger;

        // Internal Dependencies (Instantiated On-Demand By Accessors).
        private AccountManager accountManager;
        private ArticleManager articleManager;
        private ContractManager contractManager;
        private CustomerManager customerManager;
        private SalesAgentManager salesAgentManager;
        private ShipFitManager shipFitManager;
        private TaskManager taskManager;

        /// <summary>
        /// Initialises a new instance of Doctrine Ships Services.
        /// </summary>
        /// <param name="doctrineShipsRepository">An IDoctrineShipsRepository instance.</param>
        /// <param name="eveDataSource">An IEveDataSource instance.</param>
        /// <param name="doctrineShipsValidation">An IDoctrineShips Validation instance.</param>
        /// <param name="logger">An ISystemLogger logger instance.</param>
        public DoctrineShipsServices(IDoctrineShipsRepository doctrineShipsRepository, IEveDataSource eveDataSource, IDoctrineShipsValidation doctrineShipsValidation, ISystemLogger logger)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
            this.eveDataSource = eveDataSource;
            this.doctrineShipsValidation = doctrineShipsValidation;
            this.logger = logger;
        }

        /// <summary>
        /// AccountManager accessor. Instantiates a concrete AccountManager when accessed.
        /// </summary>
        internal AccountManager AccountManager
        {
            get
            {
                if (this.accountManager == null)
                {
                    this.accountManager = new AccountManager(doctrineShipsRepository, eveDataSource, doctrineShipsValidation, logger);
                }

                return this.accountManager;
            }
        }

        /// <summary>
        /// ArticleManager accessor. Instantiates a concrete ArticleManager when accessed.
        /// </summary>
        internal ArticleManager ArticleManager
        {
            get
            {
                if (this.articleManager == null)
                {
                    this.articleManager = new ArticleManager(doctrineShipsRepository, doctrineShipsValidation, logger);
                }

                return this.articleManager;
            }
        }

        /// <summary>
        /// ContractManager accessor. Instantiates a concrete ContractManager when accessed.
        /// </summary>
        internal ContractManager ContractManager
        {
            get
            {
                if (this.contractManager == null)
                {
                    this.contractManager = new ContractManager(doctrineShipsRepository, eveDataSource, doctrineShipsValidation, logger);
                }

                return this.contractManager;
            }
        }

        /// <summary>
        /// CustomerManager accessor. Instantiates a concrete CustomerManager when accessed.
        /// </summary>
        internal CustomerManager CustomerManager
        {
            get
            {
                if (this.customerManager == null)
                {
                    this.customerManager = new CustomerManager(doctrineShipsRepository, eveDataSource, doctrineShipsValidation, logger);
                }

                return this.customerManager;
            }
        }

        /// <summary>
        /// SalesAgentManager accessor. Instantiates a concrete SalesAgentManager when accessed.
        /// </summary>
        internal SalesAgentManager SalesAgentManager
        {
            get
            {
                if (this.salesAgentManager == null)
                {
                    this.salesAgentManager = new SalesAgentManager(doctrineShipsRepository, eveDataSource, doctrineShipsValidation, logger);
                }

                return this.salesAgentManager;
            }
        }

        /// <summary>
        /// ShipFitManager accessor. Instantiates a concrete ShipFitManager when accessed.
        /// </summary>
        internal ShipFitManager ShipFitManager
        {
            get
            {
                if (this.shipFitManager == null)
                {
                    this.shipFitManager = new ShipFitManager(doctrineShipsRepository, eveDataSource, doctrineShipsValidation, logger);
                }

                return this.shipFitManager;
            }
        }

        /// <summary>
        /// TaskManager accessor. Instantiates a concrete TaskManager when accessed.
        /// </summary>
        internal TaskManager TaskManager
        {
            get
            {
                if (this.taskManager == null)
                {
                    this.taskManager = new TaskManager(doctrineShipsRepository, doctrineShipsValidation, logger);
                }

                return this.taskManager;
            }
        }

        /// <summary>
        /// Fetches and returns a Doctrine Ships customer.
        /// </summary>
        /// <param name="customerId">The id of the customer for which a customer object should be returned.</param>
        /// <returns>A customer object.</returns>
        public Customer GetCustomer(int customerId)
        {
            return CustomerManager.GetCustomer(customerId);
        }

        /// <summary>
        /// Fetches and returns a Doctrine Ships ship fit.
        /// </summary>
        /// <param name="shipFitId">The id for which a ship fit object should be returned.</param>
        /// <param name="accountId">The currently logged-in account id for security checking and settings profile lookup.</param>
        /// <returns>A ship fit object or null if the account id is not authorised to view the fit or it is not found.</returns>
        public ShipFit GetShipFitDetail(int shipFitId, int accountId)
        {
            SettingProfile settingProfile;
            settingProfile = AccountManager.GetAccountSettingProfile(accountId);
            
            return ShipFitManager.GetShipFitDetail(shipFitId, accountId, settingProfile);
        }

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships corporation customers.
        /// </summary>
        /// <returns>A list of customer objects.</returns>
        public IEnumerable<Customer> GetCorporationCustomers()
        {
            return CustomerManager.GetCorporationCustomers();
        }

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships customers.
        /// </summary>
        /// <returns>A list of customer objects.</returns>
        public IEnumerable<Customer> GetCustomers()
        {
            return CustomerManager.GetCustomers();
        }

        /// <summary>
        /// <para>Adds a customer.</para>
        /// </summary>
        /// <param name="name">The name of the new customer. This must be identical to the in-game name.</param>
        /// <param name="type">Is this customer a corporation, an alliance or an individual character?</param>
        /// <returns>Returns a validation result object.</returns>
        public IValidationResult AddCustomer(string name, int type)
        {
            return CustomerManager.AddCustomer(name, type);
        }

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <param name="customerId">The customer Id being deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        public bool DeleteCustomer(int customerId)
        {
            return CustomerManager.DeleteCustomer(customerId);
        }

        /// <summary>
        /// Returns a list of contracts for a given corporation.
        /// </summary>
        /// <param name="customerId">The id of the customer for which contracts should be returned.</param>
        /// <returns>A list of customer contract objects.</returns>
        public IEnumerable<Contract> GetCustomerContracts(int customerId)
        {
            return CustomerManager.GetCustomerContracts(customerId);
        }

        /// <summary>
        /// Returns a list of contracts for a given sales agent.
        /// </summary>
        /// <param name="salesAgentId">The id of the sales agent for which contracts should be returned.</param>
        /// <returns>A list of sales agent contract objects.</returns>
        public IEnumerable<Contract> GetSalesAgentContracts(int salesAgentId)
        {
            return SalesAgentManager.GetSalesAgentContracts(salesAgentId);
        }

        /// <summary>
        /// Fetches and returns a Doctrine Ships sales agent.
        /// </summary>
        /// <param name="salesAgentId">The id of the sales agent for which a sales agent object should be returned.</param>
        /// <returns>A sales agents object.</returns>
        public SalesAgent GetSalesAgent(int salesAgentId)
        {
            return SalesAgentManager.GetSalesAgent(salesAgentId);
        }

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships sales agents for a particular account.
        /// </summary>
        /// <param name="accountId">The account for which sales agents should be returned.</param>
        /// <returns>A list of sales agents objects.</returns>
        public IEnumerable<SalesAgent> GetSalesAgents(int accountId)
        {
            return SalesAgentManager.GetSalesAgents(accountId);
        }

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships notification recipients for a particular account.
        /// </summary>
        /// <param name="accountId">The account for which notification recipients should be returned.</param>
        /// <returns>A list of notification recipients objects.</returns>
        public IEnumerable<NotificationRecipient> GetNotificationRecipients(int accountId)
        {
            return AccountManager.GetNotificationRecipients(accountId);
        }

        /// <summary>
        /// Returns a list of all Doctrine Ships ship fits for a given account.
        /// </summary>
        /// <param name="accountId">The account for which the ship fits should be returned.</param>
        /// <returns>A list of ship fit objects.</returns>
        public IEnumerable<ShipFit> GetShipFitList(int accountId)
        {
            return ShipFitManager.GetShipFitList(accountId);
        }

        /// <summary>
        /// Deletes a ship fit, its components and all related contracts.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the ship fit being deleted.</param>
        /// <param name="shipFitId">The ship fit Id to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        public bool DeleteShipFit(int accountId, int shipFitId)
        {
            return ShipFitManager.DeleteShipFit(accountId, shipFitId);
        }

        /// <summary>
        /// Import a Doctrine Ships ship fitting.
        /// </summary>
        /// <param name="toParse">A ship fitting string in a supported format to be parsed.</param>
        /// <param name="accountId">The account for which the ship fit should be imported.</param>
        /// <returns>Returns a list of populated ship fit objects or an empty list if the process fails.</returns>
        public IEnumerable<ShipFit> ImportShipFit(string toParse, int accountId)
        {
            return ShipFitManager.ImportShipFitEveXml(toParse, accountId);
        }

        /// <summary>
        /// Refresh the contracts of the sales agents with the oldest refresh time and update contract counts.
        /// </summary>
        /// <param name="force">Indicates that all sales agents should be refreshed, ignoring their LastContractRefresh values.</param>
        /// <param name="batchSize">The number of sales agents to refresh at a time.</param>
        public void RefreshContracts(bool force, int batchSize)
        {
            // Refresh the contracts.
            ContractManager.RefreshContracts(force, batchSize);

            // Refresh the number of valid contracts available for each ship fit.
            ShipFitManager.RefreshShipFitContractCounts();

            // Refresh the number of contracts available for each sales agent.
            SalesAgentManager.RefreshSalesAgentContractCounts();
        }

        /// <summary>
        /// Refresh all ship fit data.
        /// </summary>
        public void RefreshShipFits()
        {
            ShipFitManager.RefreshAllFittingStrings();
            ShipFitManager.RefreshAllFittingHashes();
            ShipFitManager.RefreshAllShipFitPackagedVolumes();
            ShipFitManager.RefreshShipFitContractCounts();
        }

        /// <summary>
        /// Perform daily maintenance tasks.
        /// </summary>
        /// <param name="twitterContext">A twitter context for the sending of messages.</param>
        public async Task DailyMaintenance(LinqToTwitter::TwitterContext twitterContext)
        {
            // Delete any contracts where the expired date has passed.
            ContractManager.DeleteExpiredContracts();

            // Deletes log entries older than 7 days.
            TaskManager.DeleteOldLogs();

            // Deletes short urls older than 30 days.
            TaskManager.DeleteOldShortUrls();

            // Send out daily ship fit availability summaries for all accounts.
            await TaskManager.SendDailySummary(twitterContext);
        }

        /// <summary>
        /// Perform hourly maintenance tasks.
        /// </summary>
        /// <param name="twitterContext">A twitter context for the sending of messages.</param>
        public async Task HourlyMaintenance(LinqToTwitter::TwitterContext twitterContext)
        {
            // Send out any ship fit availability alerts for all accounts.
            await TaskManager.SendAvailabilityAlert(twitterContext);
        }

        /// <summary>
        /// Fetches and returns a Doctrine Ships account.
        /// </summary>
        /// <returns>An account object.</returns>
        public Account GetAccount(int accountId)
        {
            return AccountManager.GetAccount(accountId);
        }

        /// <summary>
        /// <para>Adds an account with a default setting profile and an account admin access code.</para>
        /// </summary>
        /// <param name="description">A short description for the new account.</param>
        /// <param name="generatedKey">An out string parameter containing the account admin key or an emptry string on failure.</param>
        /// <param name="newAccountId">An out int parameter containing the new account id or 0 on failure.</param>
        /// <returns>Returns a validation result object.</returns>
        public IValidationResult AddAccount(string description, out string generatedKey, out int newAccountId)
        {
            return AccountManager.AddAccount(description, out generatedKey, out newAccountId);
        }

        /// <summary>
        /// Deletes an account and all access codes, ship fits, sales agents and their contracts.
        /// </summary>
        /// <param name="accountId">The account Id being deleted.</param>
        /// <returns>Returns a validation result object.</returns>
        public IValidationResult DeleteAccount(int accountId)
        {
            IValidationResult validationResult = new ValidationResult();

            // Delete all account ship fits, components and related contracts.
            var accountShipFits = ShipFitManager.GetShipFitList(accountId);
            foreach (var shipFit in accountShipFits)
            {
                if (ShipFitManager.DeleteShipFit(accountId, shipFit.ShipFitId) == false)
                {
                    validationResult.AddError(shipFit.ShipFitId.ToString(), "Error while deleting ship fit: " + shipFit.ShipFitId.ToString());
                }
            }

            // Delete all account sales agents.
            var accountSalesAgents = SalesAgentManager.GetSalesAgents(accountId);
            foreach (var salesAgent in accountSalesAgents)
            {
                if (SalesAgentManager.DeleteSalesAgent(accountId, salesAgent.SalesAgentId) == false)
                {
                    validationResult.AddError(salesAgent.SalesAgentId.ToString(), "Error while deleting sales agent: " + salesAgent.SalesAgentId.ToString());
                }
            }
                
            // Delete all account access codes.
            var accountAccessCodes = AccountManager.GetAccessCodes(accountId);
            foreach (var accessCode in accountAccessCodes)
            {
                if (AccountManager.DeleteAccessCode(accountId, accessCode.AccessCodeId) == false)
                {
                    validationResult.AddError(accessCode.AccessCodeId.ToString(), "Error while deleting access code: " + accessCode.AccessCodeId.ToString());
                }
            }

            // Delete all notification recipients.
            var accountNotificationRecipients = AccountManager.GetNotificationRecipients(accountId);
            foreach (var notificationRecipient in accountNotificationRecipients)
            {
                if (AccountManager.DeleteNotificationRecipient(accountId, notificationRecipient.NotificationRecipientId) == false)
                {
                    validationResult.AddError(notificationRecipient.NotificationRecipientId.ToString(), "Error while deleting notification recipient: " + notificationRecipient.NotificationRecipientId.ToString());
                }
            }

            // Delete the account.
            if (AccountManager.DeleteAccount(accountId) == false)
            {
                validationResult.AddError(accountId.ToString(), "Error while deleting account: " + accountId.ToString());
            }

            try
            {
                // Delete the account setting profile.
                var settingProfile = this.GetAccountSettingProfile(accountId);
                if (AccountManager.DeleteSettingProfile(accountId, settingProfile.SettingProfileId) == false)
                {
                    validationResult.AddError(settingProfile.SettingProfileId.ToString(), "Error while deleting setting profile: " + settingProfile.SettingProfileId.ToString());
                }
            }
            catch (System.ArgumentException e)
            {
                // The setting profile did not exist. Add an error to the validation result object.
                validationResult.AddError("SettingProfile.Exists" + accountId.ToString(), "The setting profile did not exist for account id: " + accountId.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return validationResult;
        }

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships accounts.
        /// </summary>
        /// <returns>A list of account objects.</returns>
        public IEnumerable<Account> GetAccounts()
        {
            return AccountManager.GetAccounts();
        }

        /// <summary>
        /// Authenticate an account id and key combination.
        /// </summary>
        /// <param name="accountId">The account id to be authenticated.</param>
        /// <param name="key">The passcode to be authenticated against the account id.</param>
        /// <param name="bypassAccountChecks">Bypass account id checking so that any account id may be used.</param>
        /// <returns>Returns a role. The role will be 'None' if authentication has failed.</returns>
        public Role Authenticate(int accountId, string key, bool bypassAccountChecks = false)
        {
            return AccountManager.Authenticate(accountId, key, bypassAccountChecks);
        }

        /// <summary>
        /// Add an access key to an account.
        /// </summary>
        /// <param name="accountId">The id of the account for which an access code should be added.</param>
        /// <param name="description">A short description for the new access code.</param>
        /// <param name="role">The role that.</param>
        /// <returns>Returns a randomly generated key.</returns>
        public string AddAccessCode(int accountId, string description, Role role)
        {
            return AccountManager.AddAccessCode(accountId, description, role);
        }

        /// <summary>
        /// <para>Deletes an access code from an accountId and a accessCodeId.</para>
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the access code being deleted.</param>
        /// <param name="accessCode">The Id of the access code to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        public bool DeleteAccessCode(int accountId, int accessCodeId)
        {
            return AccountManager.DeleteAccessCode(accountId, accessCodeId);
        }

        /// <summary>
        /// Get a list of account access codes.
        /// </summary>
        /// <param name="accountId">The id of the account for which an access code list should be returned.</param>
        /// <returns>Returns a list of access codes.</returns>
        public IEnumerable<AccessCode> GetAccessCodes(int accountId)
        {
            return AccountManager.GetAccessCodes(accountId);
        }

        /// <summary>
        /// Fetches and returns a setting profile for a particular account.
        /// </summary>
        /// <param name="accountId">The id of the account for which a setting profile object should be returned.</param>
        /// <returns>A setting profile object.</returns>
        public SettingProfile GetAccountSettingProfile(int accountId)
        {
            return AccountManager.GetAccountSettingProfile(accountId);
        }

        /// <summary>
        /// Updates a setting profile for a particular account.
        /// </summary>
        /// <param name="settingProfile">A setting profile object to be updated.</param>
        /// <returns>Returns true if the update was successful or false if not.</returns>
        public bool UpdateSettingProfile(SettingProfile settingProfile)
        {
            return AccountManager.UpdateSettingProfile(settingProfile);
        }

        /// <summary>
        /// Get a list of available articles. Articles with the 'IsUnlisted' flag will be omitted by default.
        /// </summary>
        /// <param name="includeUnlisted">A flag to indicate if articles marked as 'IsUnlisted' should be returned.</param>
        /// <returns>Returns a list of articles.</returns>
        public IEnumerable<Article> GetArticles(bool includeUnlisted = false)
        {
            return ArticleManager.GetArticles(includeUnlisted);
        }

        /// <summary>
        /// Fetches and returns a Doctrine Ships article.
        /// </summary>
        /// <param name="articleId">The id of the article for which an article object should be returned.</param>
        /// <returns>An individual article.</returns>
        public Article GetArticle(int articleId)
        {
            return ArticleManager.GetArticle(articleId);
        }

        /// <summary>
        /// <para>Adds a sales agent from an api key and account id.</para>
        /// </summary>
        /// <param name="apiId">A valid eve api id (keyID).</param>
        /// <param name="apiKey">A valid eve api key (vCode).</param>
        /// <param name="accountId">The id of the account for which a sales agent should be added.</param>
        /// <returns>Returns a validation result object.</returns>
        public IValidationResult AddSalesAgent(int apiId, string apiKey, int accountId)
        {
            return SalesAgentManager.AddSalesAgent(apiId, apiKey, accountId);
        }

        /// <summary>
        /// <para>Deletes a sales agent from an accountId and a salesAgentId.</para>
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the sales agent being deleted.</param>
        /// <param name="salesAgent">The Id of the sales agent to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        public bool DeleteSalesAgent(int accountId, int salesAgentId)
        {
            return SalesAgentManager.DeleteSalesAgent(accountId, salesAgentId);
        }

        /// <summary>
        /// Fetches and returns log entries for a given time period.
        /// </summary>
        /// <param name="logPeriod">A period of time (TimeSpan) to return logs for.</param>
        /// <returns>A list of log messages.</returns>
        public IEnumerable<LogMessage> GetLogMessages(TimeSpan logPeriod)
        {
            return TaskManager.GetLogMessages(logPeriod);
        }

        /// <summary>
        /// Clears all log entries.
        /// </summary>
        public void ClearLog()
        {
            TaskManager.ClearLog();
        }

        /// <summary>
        /// Updates the state of an access code.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the access code being changed.</param>
        /// <param name="accessCodeId">The id of the access code to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        public bool UpdateAccessCodeState(int accountId, int accessCodeId, bool isActive)
        {
            return AccountManager.UpdateAccessCodeState(accountId, accessCodeId, isActive);
        }

        /// <summary>
        /// Updates the state of an account.
        /// </summary>
        /// <param name="accountId">The id of the account to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        public bool UpdateAccountState(int accountId, bool isActive)
        {
            return AccountManager.UpdateAccountState(accountId, isActive);
        }

        /// <summary>
        /// Updates the state of a sales agent.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the sales agent being changed.</param>
        /// <param name="salesAgentId">The id of the sales agent to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        public bool UpdateSalesAgentState(int accountId, int salesAgentId, bool isActive)
        {
            return SalesAgentManager.UpdateSalesAgentState(accountId, salesAgentId, isActive);
        }

        /// <summary>
        /// Updates the state of a ship fit's contract availability monitoring.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the ship fit being changed.</param>
        /// <param name="shipFitId">The id of the ship fit to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        public bool UpdateShipFitMonitoringState(int accountId, int shipFitId, bool isActive)
        {
            return ShipFitManager.UpdateShipFitMonitoringState(accountId, shipFitId, isActive);
        }

        /// <summary>
        /// Forces a contract refresh for a single sales agent. This operation is only permitted once every 30 minutes.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the sales agent being refreshed.</param>
        /// <param name="salesAgentId">The id of the sales agent for which a contract refresh is to be forced.</param>
        /// <returns>Returns true if the force was successful or false if not.</returns>
        public bool ForceContractRefresh(int accountId, int salesAgentId)
        {
            // Force a contract refresh for this sales agent and store the result.
            var result = ContractManager.ForceContractRefresh(accountId, salesAgentId);

            // Refresh the number of valid contracts available for each ship fit.
            ShipFitManager.RefreshShipFitContractCounts();

            // Refresh the number of contracts available for each sales agent.
            SalesAgentManager.RefreshSalesAgentContractCounts();

            return result;
        }

        /// <summary>
        /// <para>Deletes a notification recipient from an accountId and a notificationRecipientId.</para>
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the notification recipient being deleted.</param>
        /// <param name="notificationRecipientId">The Id of the notification recipient to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        public bool DeleteNotificationRecipient(int accountId, int notificationRecipientId)
        {
            return AccountManager.DeleteNotificationRecipient(accountId, notificationRecipientId);
        }

        /// <summary>
        /// <para>Adds a notification recipient for an account.</para>
        /// </summary>
        /// <param name="accountId">The id of the account for which a recipient should be added.</param>
        /// <param name="twitterHandle">The twitter handle for the recipient.</param>
        /// <param name="description">A short description for the recipient.</param>
        /// <returns>Returns a validation result object.</returns>
        public IValidationResult AddNotificationRecipient(int accountId, string twitterHandle, string description)
        {
            return AccountManager.AddNotificationRecipient(accountId, twitterHandle, description);
        }

        /// <summary>
        /// Updates a notification recipient for a particular account.
        /// </summary>
        /// <param name="notificationRecipient">A partially populated notification recipient object to be updated.</param>
        /// <returns>Returns a validation result object.</returns>
        public IValidationResult UpdateNotificationRecipient(NotificationRecipient notificationRecipient)
        {
            return AccountManager.UpdateNotificationRecipient(notificationRecipient);
        }

        /// <summary>
        /// Updates a ship fit for a particular account.
        /// </summary>
        /// <param name="shipFit">A partially populated ship fit object to be updated.</param>
        /// <returns>Returns a validation result object.</returns>
        public IValidationResult UpdateShipFit(ShipFit shipFit)
        {
            return ShipFitManager.UpdateShipFit(shipFit);
        }

        /// <summary>
        /// Generate and add a short url from a passed long url.
        /// <param name="longUrl">A long url to be shortened.</param>
        /// <returns>Returns a shortUrlId string.</returns>
        /// </summary>
        public string AddShortUrl(string longUrl)
        {
            return TaskManager.AddShortUrl(longUrl);
        }

        /// <summary>
        /// Fetches and returns a long url from a short url id.
        /// <param name="shortUrlId">A shortUrlId relating to the stored longUrl.</param>
        /// <returns>Returns a longUrl string.</returns>
        /// </summary>
        public string GetLongUrl(string shortUrlId)
        {
            return TaskManager.GetLongUrl(shortUrlId);
        }

        /// <summary>
        /// Generate and returns an EFT fitting string for a ship fit.
        /// </summary>
        /// <param name="shipFitId">The id of a doctrine ships ship fit.</param>
        /// <param name="accountId">The currently logged-in account id for security checking.</param>
        /// <returns>Returns a string containing an EFT fitting or an empty string if an error occurs.</returns>
        public string GetEftFittingString(int shipFitId, int accountId)
        {
            return ShipFitManager.GetEftFittingString(shipFitId, accountId);
        }
    }
}
