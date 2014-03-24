namespace DoctrineShips.Repository
{
    using System;
    using System.Collections.Generic;
    using DoctrineShips.Entities;
    using Tools;
    
    public interface IDoctrineShipsRepository
    {
        // Access Code Operations.
        void DeleteAccessCode(int accessCodeId);
        void UpdateAccessCode(AccessCode accessCode);
        AccessCode AddAccessCode(AccessCode accessCode);
        AccessCode CreateAccessCode(AccessCode accessCode);
        AccessCode GetAccessCode(int accessCodeId);
        IEnumerable<AccessCode> GetAccessCodes();
        IEnumerable<AccessCode> GetSiteAdminAccessCodes();
        IEnumerable<AccessCode> GetAccessCodesForAccount(int accountId);

        // Account Operations.
        void DeleteAccount(int accountId);
        void UpdateAccount(Account account);
        Account AddAccount(Account account);
        Account CreateAccount(Account account);
        Account GetAccount(int accountId);
        IEnumerable<Account> GetAccounts();
        IEnumerable<Account> GetAccountsForNotifications();

        // Article Operations.
        void DeleteArticle(int articleId);
        void UpdateArticle(Article article);
        Article AddArticle(Article article);
        Article CreateArticle(Article article);
        Article GetArticle(int articleId);
        IEnumerable<Article> GetArticles(bool includeUnlisted = false);

        // Component Operations.
        void DeleteComponent(int componentId);
        void UpdateComponent(Component component);
        Component AddComponent(Component component);
        Component CreateComponent(Component component);
        Component GetComponent(int componentId);
        IEnumerable<Component> GetComponents();

        // Contract Operations.
        void DeleteContract(long contractId);
        int DeleteExpiredContracts();
        void DeleteContractsByShipFitId(int shipFitId);
        void UpdateContract(Contract contract);
        Contract AddContract(Contract contract);
        Contract CreateContract(Contract contract);
        Contract GetContract(long contractId);
        IEnumerable<Contract> GetContracts();
        IEnumerable<Contract> GetAssigneeContracts(int assigneeId);
        IEnumerable<Contract> GetIssuerContracts(int salesAgentId);
        HashSet<long> GetSalesAgentContractIds(int salesAgentId, bool isCorp = false);
        Dictionary<int, int> GetContractShipFitCounts();
        Dictionary<long, int> GetContractSalesAgentCounts();

        // Sales Agent Operations.
        void DeleteSalesAgent(int salesAgentId);
        void UpdateSalesAgent(SalesAgent salesAgent);
        SalesAgent AddSalesAgent(SalesAgent salesAgent);
        SalesAgent CreateSalesAgent(SalesAgent salesAgent);
        SalesAgent GetSalesAgent(int salesAgentId);
        IEnumerable<SalesAgent> GetSalesAgents(int accountId);
        IEnumerable<SalesAgent> GetSalesAgentsForContractCount();
        IEnumerable<SalesAgent> GetSalesAgentsForRefresh(bool force, int batchSize = 10);

        // Setting Profile Operations.
        void DeleteSettingProfile(int settingProfileId);
        void UpdateSettingProfile(SettingProfile settingProfile);
        SettingProfile AddSettingProfile(SettingProfile settingProfile);
        SettingProfile CreateSettingProfile(SettingProfile settingProfile);
        SettingProfile GetSettingProfile(int settingProfileId);
        SettingProfile GetSettingProfileReadOnly(int settingProfileId);
        SettingProfile GetSettingProfileForAccount(int accountId);
        IEnumerable<SettingProfile> GetSettingProfiles();

        // Customer Operations.
        void DeleteCustomer(int customerId);
        void UpdateCustomer(Customer customer);
        Customer AddCustomer(Customer customer);
        Customer CreateCustomer(Customer customer);
        Customer GetCustomer(int customerId);
        IEnumerable<Customer> GetCustomers();
        IEnumerable<Customer> GetCorporationCustomers();
        IEnumerable<Customer> GetCharacterCustomers();

        // Log Message Operations.
        IEnumerable<LogMessage> GetLogMessages(TimeSpan logPeriod);
        void DeleteLogsOlderThanDate(DateTime olderThanDate);
        void ClearLog();

        // Notification Recipient Operations.
        void DeleteNotificationRecipient(int notificationRecipientId);
        void UpdateNotificationRecipient(NotificationRecipient notificationRecipient);
        NotificationRecipient AddNotificationRecipient(NotificationRecipient notificationRecipient);
        NotificationRecipient CreateNotificationRecipient(NotificationRecipient notificationRecipient);
        NotificationRecipient GetNotificationRecipient(int notificationRecipientId);
        NotificationRecipient GetNotificationRecipientReadOnly(int notificationRecipientId);
        IEnumerable<NotificationRecipient> GetNotificationRecipients(int accountId);

        // ShipFit Operations.
        void DeleteShipFit(int shipFitId);
        void UpdateShipFit(ShipFit shipFit);
        ShipFit AddShipFit(ShipFit shipFit);
        ShipFit CreateShipFit(ShipFit shipFit);
        ShipFit GetShipFit(int shipFitId);
        IEnumerable<ShipFit> GetShipFits();
        IEnumerable<ShipFit> GetShipFitsWithComponents();
        IEnumerable<ShipFit> GetShipFitsForContractCount();
        IEnumerable<ShipFit> GetShipFitsForAccount(int accountId);
        Dictionary<string, int> GetShipFitList();

        // ShipFitComponent Operations.
        void DeleteShipFitComponent(int shipFitComponentId);
        void DeleteShipFitComponentsByShipFitId(int shipFitId);
        void UpdateShipFitComponent(ShipFitComponent shipFitComponent);
        ShipFitComponent AddShipFitComponent(ShipFitComponent shipFitComponent);
        ShipFitComponent CreateShipFitComponent(ShipFitComponent shipFitComponent);
        ShipFitComponent GetShipFitComponent(int shipFitComponentId);
        IEnumerable<ShipFitComponent> GetShipFitComponents(int shipFitId);

        // ShortUrl Operations.
        void DeleteShortUrl(string shortUrlId);
        void UpdateShortUrl(ShortUrl shortUrl);
        ShortUrl AddShortUrl(ShortUrl shortUrl);
        ShortUrl CreateShortUrl(ShortUrl shortUrl);
        ShortUrl GetShortUrl(string shortUrlId);
        IEnumerable<ShortUrl> GetShortUrls();
        void DeleteShortUrlsOlderThanDate(DateTime olderThanDate);

        // Misc Operations.
        void Save();
    }
}