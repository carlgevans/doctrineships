namespace DoctrineShips.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DoctrineShips.Entities;
    using DoctrineShips.Validation;
    using LinqToTwitter = LinqToTwitter;
    using Tools;

    /// <summary>
    /// An interface representing available Doctrine Ships services. A list of methods exposing all of the Doctrine Ships functionality available to clients.
    /// </summary>
    public interface IDoctrineShipsServices
    {
        /// <summary>
        /// Fetches and returns a Doctrine Ships customer.
        /// </summary>
        /// <param name="customerId">The id of the customer for which a customer object should be returned.</param>
        /// <returns>A customer object.</returns>
        Customer GetCustomer(int customerId);

        /// <summary>
        /// Fetches and returns a Doctrine Ships ship fit.
        /// </summary>
        /// <param name="shipFitId">The id for which a ship fit object should be returned.</param>
        /// <param name="accountId">The account id for security checking and settings profile lookup.</param>
        /// <returns>A ship fit object.</returns>
        ShipFit GetShipFitDetail(int shipFitId, int accountId);

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships corporation customers.
        /// </summary>
        /// <returns>A list of customer objects.</returns>
        IEnumerable<Customer> GetCorporationCustomers();

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships customers.
        /// </summary>
        /// <returns>A list of customer objects.</returns>
        IEnumerable<Customer> GetCustomers();

        /// <summary>
        /// <para>Adds a customer.</para>
        /// </summary>
        /// <param name="name">The name of the new customer. This must be identical to the in-game name.</param>
        /// <param name="type">Is this customer a corporation, an alliance or an individual character?</param>
        /// <returns>Returns a validation result object.</returns>
        IValidationResult AddCustomer(string name, int type);

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <param name="customerId">The customer Id being deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        bool DeleteCustomer(int customerId);

        /// <summary>
        /// Returns a list of contracts for a given corporation.
        /// </summary>
        /// <param name="customerId">The id of the customer for which contracts should be returned.</param>
        /// <returns>A list of customer contract objects.</returns>
        IEnumerable<Contract> GetCustomerContracts(int customerId);

        /// <summary>
        /// Returns a list of contracts for a given sales agent.
        /// </summary>
        /// <param name="salesAgentId">The id of the sales agent for which contracts should be returned.</param>
        /// <returns>A list of sales agent contract objects.</returns>
        IEnumerable<Contract> GetSalesAgentContracts(int salesAgentId);

        /// <summary>
        /// Fetches and returns a Doctrine Ships sales agent.
        /// </summary>
        /// <param name="salesAgentId">The id of the sales agent for which a sales agent object should be returned.</param>
        /// <returns>A sales agents object.</returns>
        SalesAgent GetSalesAgent(int salesAgentId);

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships sales agents for a particular account.
        /// </summary>
        /// <param name="accountId">The account for which sales agents should be returned.</param>
        /// <returns>A list of sales agents objects.</returns>
        IEnumerable<SalesAgent> GetSalesAgents(int accountId);

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships notification recipients for a particular account.
        /// </summary>
        /// <param name="accountId">The account for which notification recipients should be returned.</param>
        /// <returns>A list of notification recipients objects.</returns>
        IEnumerable<NotificationRecipient> GetNotificationRecipients(int accountId);

        /// <summary>
        /// Returns a list of all Doctrine Ships ship fits for a given account.
        /// </summary>
        /// <param name="accountId">The account for which the ship fits should be returned.</param>
        /// <returns>A list of ship fit objects.</returns>
        IEnumerable<ShipFit> GetShipFitList(int accountId);

        /// <summary>
        /// Deletes a ship fit, its components and all related contracts.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the ship fit being deleted.</param>
        /// <param name="shipFitId">The ship fit Id to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        bool DeleteShipFit(int accountId, int shipFitId);

        /// <summary>
        /// Import a Doctrine Ships ship fitting from various sources.
        /// </summary>
        /// <param name="toParse">A ship fitting string in a supported format to be parsed.</param>
        /// <param name="accountId">The account for which the ship fit should be imported.</param>
        /// <returns>Returns a list of populated ship fit objects or an empty list if the process fails.</returns>
        IEnumerable<ShipFit> ImportShipFit(string toParse, int accountId);

        /// <summary>
        /// Refresh the contracts of the sales agents with the oldest refresh time and update contract counts.
        /// </summary>
        /// <param name="force">Indicates that all sales agents should be refreshed, ignoring their LastContractRefresh values.</param>
        /// <param name="batchSize">The number of sales agents to refresh at a time.</param>
        void RefreshContracts(bool force, int batchSize);

        /// <summary>
        /// Refresh all ship fit data.
        /// </summary>
        void RefreshShipFits();

        /// <summary>
        /// Perform daily maintenance tasks.
        /// </summary>
        /// <param name="twitterContext">A twitter context for the sending of messages.</param>
        /// <returns>A Task boolean value.</returns>
        Task DailyMaintenance(LinqToTwitter::TwitterContext twitterContext);

        /// <summary>
        /// Perform hourly maintenance tasks.
        /// </summary>
        /// <param name="corpApiId">A valid eve api id (keyID) for the Doctrine Ships in-game corporation.</param>
        /// <param name="corpApiKey">A valid eve api key (vCode) for the Doctrine Ships in-game corporation.</param>
        /// <param name="twitterContext">A twitter context for the sending of messages.</param>
        Task HourlyMaintenance(int corpApiId, string corpApiKey, LinqToTwitter::TwitterContext twitterContext);

        /// <summary>
        /// Fetches and returns a Doctrine Ships account.
        /// </summary>
        /// <returns>An account object.</returns>
        Account GetAccount(int accountId);

        /// <summary>
        /// <para>Adds an account with a default setting profile and an account admin access code.</para>
        /// </summary>
        /// <param name="description">A short description for the new account.</param>
        /// <param name="generatedKey">An out string parameter containing the account admin key or an emptry string on failure.</param>
        /// <param name="newAccountId">An out int parameter containing the new account id or 0 on failure.</param>
        /// <returns>Returns a validation result object.</returns>
        IValidationResult AddAccount(string description, out string generatedKey, out int newAccountId);

        /// <summary>
        /// Deletes an account and all access codes, ship fits, sales agents and their contracts.
        /// </summary>
        /// <param name="accountId">The account Id being deleted.</param>
        /// <returns>Returns a validation result object.</returns>
        IValidationResult DeleteAccount(int accountId);

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships accounts.
        /// </summary>
        /// <returns>A list of account objects.</returns>
        IEnumerable<Account> GetAccounts();

        /// <summary>
        /// Authenticate an account id and key combination.
        /// </summary>
        /// <param name="accountId">The account id to be authenticated.</param>
        /// <param name="key">The passcode to be authenticated against the account id.</param>
        /// <param name="bypassAccountChecks">Bypass account id checking so that any account id may be used.</param>
        /// <returns>Returns a role. The role will be 'None' if authentication has failed.</returns>
        Role Authenticate(int accountId, string key, bool bypassAccountChecks = false);

        /// <summary>
        /// Add an access key to an account.
        /// </summary>
        /// <param name="accountId">The id of the account for which an access code should be added.</param>
        /// <param name="description">A short description for the new access code.</param>
        /// <param name="role">The role that.</param>
        /// <returns>Returns a randomly generated key.</returns>
        string AddAccessCode(int accountId, string description, Role role);

        /// <summary>
        /// <para>Deletes an access code from an accountId and a accessCodeId.</para>
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the access code being deleted.</param>
        /// <param name="accessCode">The Id of the access code to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        bool DeleteAccessCode(int accountId, int accessCodeId);

        /// <summary>
        /// Get a list of account access codes.
        /// </summary>
        /// <param name="accountId">The id of the account for which an access code list should be returned.</param>
        /// <returns>Returns a list of access codes.</returns>
        IEnumerable<AccessCode> GetAccessCodes(int accountId);

        /// <summary>
        /// Fetches and returns a setting profile for a particular account.
        /// </summary>
        /// <param name="accountId">The id of the account for which a setting profile object should be returned.</param>
        /// <returns>A setting profile object.</returns>
        SettingProfile GetAccountSettingProfile(int accountId);

        /// <summary>
        /// Updates a setting profile for a particular account.
        /// </summary>
        /// <param name="settingProfile">A setting profile object to be updated.</param>
        /// <returns>Returns true if the update was successful or false if not.</returns>
        bool UpdateSettingProfile(SettingProfile settingProfile);

        /// <summary>
        /// Get a list of available articles. Articles with the 'IsUnlisted' flag will be omitted by default.
        /// </summary>
        /// <param name="includeUnlisted">A flag to indicate if articles marked as 'IsUnlisted' should be returned.</param>
        /// <returns>Returns a list of articles.</returns>
        IEnumerable<Article> GetArticles(bool includeUnlisted = false);

        /// <summary>
        /// Fetches and returns a Doctrine Ships article.
        /// </summary>
        /// <param name="articleId">The id of the article for which an article object should be returned.</param>
        /// <returns>An individual article.</returns>
        Article GetArticle(int articleId);

        /// <summary>
        /// <para>Adds a sales agent from an api key and account id.</para>
        /// </summary>
        /// <param name="apiId">A valid eve api id (keyID).</param>
        /// <param name="apiKey">A valid eve api key (vCode).</param>
        /// <param name="accountId">The id of the account for which a sales agent should be added.</param>
        /// <returns>Returns a validation result object.</returns>
        IValidationResult AddSalesAgent(int apiId, string apiKey, int accountId);

        /// <summary>
        /// <para>Deletes a sales agent from an accountId and a salesAgentId.</para>
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the sales agent being deleted.</param>
        /// <param name="salesAgent">The Id of the sales agent to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        bool DeleteSalesAgent(int accountId, int salesAgentId);

        /// <summary>
        /// Fetches and returns log entries for a given time period.
        /// </summary>
        /// <param name="logPeriod">A period of time (TimeSpan) to return logs for.</param>
        /// <returns>A list of log messages.</returns>
        IEnumerable<LogMessage> GetLogMessages(TimeSpan logPeriod);

        /// <summary>
        /// Clears all log entries.
        /// </summary>
        void ClearLog();

        /// <summary>
        /// Updates the state of an access code.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the access code being changed.</param>
        /// <param name="accessCodeId">The id of the access code to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        bool UpdateAccessCodeState(int accountId, int accessCodeId, bool isActive);

        /// <summary>
        /// Updates the state of an account.
        /// </summary>
        /// <param name="accountId">The id of the account to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        bool UpdateAccountState(int accountId, bool isActive);

        /// <summary>
        /// Updates the state of a sales agent.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the sales agent being changed.</param>
        /// <param name="salesAgentId">The id of the sales agent to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        bool UpdateSalesAgentState(int accountId, int salesAgentId, bool isActive);

        /// <summary>
        /// Updates the state of a ship fit's contract availability monitoring.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the ship fit being changed.</param>
        /// <param name="shipFitId">The id of the ship fit to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        bool UpdateShipFitMonitoringState(int accountId, int shipFitId, bool isActive);

        /// <summary>
        /// Forces a contract refresh for a single sales agent. This operation is only permitted once every 30 minutes.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the sales agent being refreshed.</param>
        /// <param name="salesAgentId">The id of the sales agent for which a contract refresh is to be forced.</param>
        /// <returns>Returns true if the force was successful or false if not.</returns>
        bool ForceContractRefresh(int accountId, int salesAgentId);

        /// <summary>
        /// <para>Deletes a notification recipient from an accountId and a notificationRecipientId.</para>
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the notification recipient being deleted.</param>
        /// <param name="notificationRecipientId">The Id of the notification recipient to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        bool DeleteNotificationRecipient(int accountId, int notificationRecipientId);

        /// <summary>
        /// <para>Adds a notification recipient for an account.</para>
        /// </summary>
        /// <param name="accountId">The id of the account for which a recipient should be added.</param>
        /// <param name="twitterHandle">The twitter handle for the recipient.</param>
        /// <param name="description">A short description for the recipient.</param>
        /// <returns>Returns a validation result object.</returns>
        IValidationResult AddNotificationRecipient(int accountId, string twitterHandle, string description);

        /// <summary>
        /// Updates a notification recipient for a particular account.
        /// </summary>
        /// <param name="notificationRecipient">A partially populated notification recipient object to be updated.</param>
        /// <returns>Returns a validation result object.</returns>
        IValidationResult UpdateNotificationRecipient(NotificationRecipient notificationRecipient);

        /// <summary>
        /// Updates a ship fit for a particular account.
        /// </summary>
        /// <param name="shipFit">A partially populated ship fit object to be updated.</param>
        /// <returns>Returns a validation result object.</returns>
        IValidationResult UpdateShipFit(ShipFit shipFit);

        /// <summary>
        /// Generate and add a short url from a passed long url.
        /// <param name="longUrl">A long url to be shortened.</param>
        /// <returns>Returns a shortUrlId string.</returns>
        /// </summary>
        string AddShortUrl(string longUrl);

        /// <summary>
        /// Fetches and returns a long url from a short url id.
        /// <param name="shortUrlId">A shortUrlId relating to the stored longUrl.</param>
        /// <returns>Returns a longUrl string.</returns>
        /// </summary>
        string GetLongUrl(string shortUrlId);

        /// <summary>
        /// Generate and returns an EFT fitting string for a ship fit.
        /// </summary>
        /// <param name="shipFitId">The id of a doctrine ships ship fit.</param>
        /// <param name="accountId">The currently logged-in account id for security checking.</param>
        /// <returns>Returns a string containing an EFT fitting or an empty string if an error occurs.</returns>
        string GetEftFittingString(int shipFitId, int accountId);
    }
}
