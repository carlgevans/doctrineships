namespace DoctrineShips.Repository
{
    using System;
    using System.Collections.Generic;
    using DoctrineShips.Repository.Operations;
    using DoctrineShips.Entities;
    using GenericRepository;
    using Tools;

    public class DoctrineShipsRepository : IDoctrineShipsRepository, ISystemLoggerStore
    {
        // External Dependencies.
        private readonly IUnitOfWork unitOfWork;

        // Internal Dependencies (Instantiated On-Demand By Accessors).
        private AccessCodeOperations accessCodeOperations;
        private AccountOperations accountOperations;
        private ArticleOperations articleOperations;
        private ComponentOperations componentOperations;
        private ContractOperations contractOperations;
        private CustomerOperations customerOperations;
        private DoctrineOperations doctrineOperations;
        private DoctrineShipFitOperations doctrineShipFitOperations;
        private NotificationRecipientOperations notificationRecipientOperations;
        private LogMessageOperations logMessageOperations;
        private SalesAgentOperations salesAgentOperations;
        private SettingProfileOperations settingProfileOperations;
        private ShipFitOperations shipFitOperations;
        private ShipFitComponentOperations shipFitComponentOperations;
        private ShortUrlOperations shortUrlOperations;

        public DoctrineShipsRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal AccessCodeOperations AccessCodeOperations
        {
            get
            {
                if (this.accessCodeOperations == null)
                {
                    this.accessCodeOperations = new AccessCodeOperations(unitOfWork);
                }

                return this.accessCodeOperations;
            }
        }

        internal AccountOperations AccountOperations
        {
            get
            {
                if (this.accountOperations == null)
                {
                    this.accountOperations = new AccountOperations(unitOfWork);
                }

                return this.accountOperations;
            }
        }

        internal ArticleOperations ArticleOperations
        {
            get
            {
                if (this.articleOperations == null)
                {
                    this.articleOperations = new ArticleOperations(unitOfWork);
                }

                return this.articleOperations;
            }
        }

        internal ComponentOperations ComponentOperations
        {
            get
            {
                if (this.componentOperations == null)
                {
                    this.componentOperations = new ComponentOperations(unitOfWork);
                }

                return this.componentOperations;
            }
        }

        internal ContractOperations ContractOperations
        {
            get
            {
                if (this.contractOperations == null)
                {
                    this.contractOperations = new ContractOperations(unitOfWork);
                }

                return this.contractOperations;
            }
        }

        internal NotificationRecipientOperations NotificationRecipientOperations
        {
            get
            {
                if (this.notificationRecipientOperations == null)
                {
                    this.notificationRecipientOperations = new NotificationRecipientOperations(unitOfWork);
                }

                return this.notificationRecipientOperations;
            }
        }

        internal CustomerOperations CustomerOperations
        {
            get
            {
                if (this.customerOperations == null)
                {
                    this.customerOperations = new CustomerOperations(unitOfWork);
                }

                return this.customerOperations;
            }
        }

        internal DoctrineOperations DoctrineOperations
        {
            get
            {
                if (this.doctrineOperations == null)
                {
                    this.doctrineOperations = new DoctrineOperations(unitOfWork);
                }

                return this.doctrineOperations;
            }
        }

        internal DoctrineShipFitOperations DoctrineShipFitOperations
        {
            get
            {
                if (this.doctrineShipFitOperations == null)
                {
                    this.doctrineShipFitOperations = new DoctrineShipFitOperations(unitOfWork);
                }

                return this.doctrineShipFitOperations;
            }
        }

        internal LogMessageOperations LogMessageOperations
        {
            get
            {
                if (this.logMessageOperations == null)
                {
                    this.logMessageOperations = new LogMessageOperations(unitOfWork);
                }

                return this.logMessageOperations;
            }
        }

        internal SalesAgentOperations SalesAgentOperations
        {
            get
            {
                if (this.salesAgentOperations == null)
                {
                    this.salesAgentOperations = new SalesAgentOperations(unitOfWork);
                }

                return this.salesAgentOperations;
            }
        }

        internal SettingProfileOperations SettingProfileOperations
        {
            get
            {
                if (this.settingProfileOperations == null)
                {
                    this.settingProfileOperations = new SettingProfileOperations(unitOfWork);
                }

                return this.settingProfileOperations;
            }
        }

        internal ShipFitOperations ShipFitOperations
        {
            get
            {
                if (this.shipFitOperations == null)
                {
                    this.shipFitOperations = new ShipFitOperations(unitOfWork);
                }

                return this.shipFitOperations;
            }
        }

        internal ShipFitComponentOperations ShipFitComponentOperations
        {
            get
            {
                if (this.shipFitComponentOperations == null)
                {
                    this.shipFitComponentOperations = new ShipFitComponentOperations(unitOfWork);
                }

                return this.shipFitComponentOperations;
            }
        }

        internal ShortUrlOperations ShortUrlOperations
        {
            get
            {
                if (this.shortUrlOperations == null)
                {
                    this.shortUrlOperations = new ShortUrlOperations(unitOfWork);
                }

                return this.shortUrlOperations;
            }
        }

        public void DeleteAccessCode(int accessCodeId)
        {
            AccessCodeOperations.DeleteAccessCode(accessCodeId);
        }

        public void UpdateAccessCode(AccessCode accessCode)
        {
            AccessCodeOperations.UpdateAccessCode(accessCode);
        }

        public AccessCode AddAccessCode(AccessCode accessCode)
        {
            return AccessCodeOperations.AddAccessCode(accessCode);
        }

        public AccessCode CreateAccessCode(AccessCode accessCode)
        {
            return AccessCodeOperations.CreateAccessCode(accessCode);
        }

        public AccessCode GetAccessCode(int accessCodeId)
        {
            return AccessCodeOperations.GetAccessCode(accessCodeId);
        }

        public IEnumerable<AccessCode> GetAccessCodes()
        {
            return AccessCodeOperations.GetAccessCodes();
        }

        public IEnumerable<AccessCode> GetSiteAdminAccessCodes()
        {
            return AccessCodeOperations.GetSiteAdminAccessCodes();
        }

        public IEnumerable<AccessCode> GetAccessCodesForAccount(int accountId)
        {
            return AccessCodeOperations.GetAccessCodesForAccount(accountId);
        }

        public void DeleteAccount(int accountId)
        {
            AccountOperations.DeleteAccount(accountId);
        }

        public void UpdateAccount(Account account)
        {
            AccountOperations.UpdateAccount(account);
        }

        public Account AddAccount(Account account)
        {
            return AccountOperations.AddAccount(account);
        }

        public Account CreateAccount(Account account)
        {
            return AccountOperations.CreateAccount(account);
        }

        public Account GetAccount(int accountId)
        {
            return AccountOperations.GetAccount(accountId);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return AccountOperations.GetAccounts();
        }

        public IEnumerable<Account> GetAccountsForNotifications()
        {
            return AccountOperations.GetAccountsForNotifications();
        }

        public void DeleteArticle(int articleId)
        {
            ArticleOperations.DeleteArticle(articleId);
        }

        public void UpdateArticle(Article article)
        {
            ArticleOperations.UpdateArticle(article);
        }

        public Article AddArticle(Article article)
        {
            return ArticleOperations.AddArticle(article);
        }

        public Article CreateArticle(Article article)
        {
            return ArticleOperations.CreateArticle(article);
        }

        public Article GetArticle(int articleId)
        {
            return ArticleOperations.GetArticle(articleId);
        }

        public IEnumerable<Article> GetArticles(bool includeUnlisted = false)
        {
            return ArticleOperations.GetArticles(includeUnlisted);
        }

        public void DeleteComponent(int componentId)
        {
            ComponentOperations.DeleteComponent(componentId);
        }

        public void UpdateComponent(Component component)
        {
            ComponentOperations.UpdateComponent(component);
        }

        public Component AddComponent(Component component)
        {
            return ComponentOperations.AddComponent(component);
        }

        public Component CreateComponent(Component component)
        {
            return ComponentOperations.CreateComponent(component);
        }

        public Component GetComponent(int componentId)
        {
            return ComponentOperations.GetComponent(componentId);
        }

        public IEnumerable<Component> GetComponents()
        {
            return ComponentOperations.GetComponents();
        }

        public void DeleteContract(long contractId)
        {
            ContractOperations.DeleteContract(contractId);
        }

        public int DeleteExpiredContracts()
        {
            return ContractOperations.DeleteExpiredContracts();
        }

        public void DeleteContractsByShipFitId(int shipFitId)
        {
            ContractOperations.DeleteContractsByShipFitId(shipFitId);
        }

        public void UpdateContract(Contract contract)
        {
            ContractOperations.UpdateContract(contract);
        }

        public Contract AddContract(Contract contract)
        {
            return ContractOperations.AddContract(contract);
        }

        public Contract CreateContract(Contract contract)
        {
            return ContractOperations.CreateContract(contract);
        }

        public Contract GetContract(long contractId)
        {
            return ContractOperations.GetContract(contractId);
        }

        public IEnumerable<Contract> GetContracts()
        {
            return ContractOperations.GetContracts();
        }

        public IEnumerable<Contract> GetAssigneeContracts(int assigneeId)
        {
            return ContractOperations.GetAssigneeContracts(assigneeId);
        }

        public IEnumerable<Contract> GetIssuerContracts(int salesAgentId)
        {
            return ContractOperations.GetIssuerContracts(salesAgentId);
        }

        public IEnumerable<Contract> GetShipFitContracts(int shipFitId)
        {
            return ContractOperations.GetShipFitContracts(shipFitId);
        }

        public HashSet<long> GetSalesAgentContractIds(int salesAgentId, bool isCorp = false)
        {
            return ContractOperations.GetSalesAgentContractIds(salesAgentId, isCorp);
        }

        public Dictionary<int, int> GetContractShipFitCounts()
        {
            return ContractOperations.GetContractShipFitCounts();
        }

        public Dictionary<long, int> GetContractSalesAgentCounts()
        {
            return ContractOperations.GetContractSalesAgentCounts();
        }

        public void DeleteNotificationRecipient(int notificationRecipientId)
        {
            NotificationRecipientOperations.DeleteNotificationRecipient(notificationRecipientId);
        }

        public void UpdateNotificationRecipient(NotificationRecipient notificationRecipient)
        {
            NotificationRecipientOperations.UpdateNotificationRecipient(notificationRecipient);
        }

        public NotificationRecipient AddNotificationRecipient(NotificationRecipient notificationRecipient)
        {
            return NotificationRecipientOperations.AddNotificationRecipient(notificationRecipient);
        }

        public NotificationRecipient CreateNotificationRecipient(NotificationRecipient notificationRecipient)
        {
            return NotificationRecipientOperations.CreateNotificationRecipient(notificationRecipient);
        }

        public NotificationRecipient GetNotificationRecipient(int notificationRecipientId)
        {
            return NotificationRecipientOperations.GetNotificationRecipient(notificationRecipientId);
        }

        public NotificationRecipient GetNotificationRecipientReadOnly(int notificationRecipientId)
        {
            return NotificationRecipientOperations.GetNotificationRecipientReadOnly(notificationRecipientId);
        }

        public IEnumerable<NotificationRecipient> GetNotificationRecipients(int accountId)
        {
            return NotificationRecipientOperations.GetNotificationRecipients(accountId);
        }

        public void DeleteSalesAgent(int salesAgentId)
        {
            SalesAgentOperations.DeleteSalesAgent(salesAgentId);
        }

        public void UpdateSalesAgent(SalesAgent salesAgent)
        {
            SalesAgentOperations.UpdateSalesAgent(salesAgent);
        }

        public SalesAgent AddSalesAgent(SalesAgent salesAgent)
        {
            return SalesAgentOperations.AddSalesAgent(salesAgent);
        }

        public SalesAgent CreateSalesAgent(SalesAgent salesAgent)
        {
            return SalesAgentOperations.CreateSalesAgent(salesAgent);
        }

        public SalesAgent GetSalesAgent(int salesAgentId)
        {
            return SalesAgentOperations.GetSalesAgent(salesAgentId);
        }

        public IEnumerable<SalesAgent> GetSalesAgents(int accountId)
        {
            return SalesAgentOperations.GetSalesAgents(accountId);
        }

        public IEnumerable<SalesAgent> GetSalesAgentsForContractCount()
        {
            return SalesAgentOperations.GetSalesAgentsForContractCount();
        }

        public IEnumerable<SalesAgent> GetSalesAgentsForRefresh(bool force, int batchSize = 10)
        {
            return SalesAgentOperations.GetSalesAgentsForRefresh(force, batchSize);
        }

        public void DeleteStaleSalesAgents(DateTime olderThanDate)
        {
            SalesAgentOperations.DeleteStaleSalesAgents(olderThanDate);
        }

        public void DeleteSettingProfile(int settingProfileId)
        {
            SettingProfileOperations.DeleteSettingProfile(settingProfileId);
        }

        public void UpdateSettingProfile(SettingProfile settingProfile)
        {
            SettingProfileOperations.UpdateSettingProfile(settingProfile);
        }

        public SettingProfile AddSettingProfile(SettingProfile settingProfile)
        {
            return SettingProfileOperations.AddSettingProfile(settingProfile);
        }

        public SettingProfile CreateSettingProfile(SettingProfile settingProfile)
        {
            return SettingProfileOperations.CreateSettingProfile(settingProfile);
        }

        public SettingProfile GetSettingProfile(int settingProfileId)
        {
            return SettingProfileOperations.GetSettingProfile(settingProfileId);
        }

        public SettingProfile GetSettingProfileReadOnly(int settingProfileId)
        {
            return SettingProfileOperations.GetSettingProfileReadOnly(settingProfileId);
        }

        public SettingProfile GetSettingProfileForAccount(int accountId)
        {
            return SettingProfileOperations.GetSettingProfileForAccount(accountId);
        }

        public IEnumerable<SettingProfile> GetSettingProfiles()
        {
            return SettingProfileOperations.GetSettingProfiles();
        }

        public void DeleteCustomer(int customerId)
        {
            CustomerOperations.DeleteCustomer(customerId);
        }

        public void UpdateCustomer(Customer customer)
        {
            CustomerOperations.UpdateCustomer(customer);
        }

        public Customer AddCustomer(Customer customer)
        {
            return CustomerOperations.AddCustomer(customer);
        }

        public Customer CreateCustomer(Customer customer)
        {
            return CustomerOperations.CreateCustomer(customer);
        }

        public Customer GetCustomer(int customerId)
        {
            return CustomerOperations.GetCustomer(customerId);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return CustomerOperations.GetCustomers();
        }

        public IEnumerable<Customer> GetCorporationCustomers()
        {
            return CustomerOperations.GetCorporationCustomers();
        }

        public IEnumerable<Customer> GetCharacterCustomers()
        {
            return CustomerOperations.GetCharacterCustomers();
        }

        public LogMessage AddLogMessage(LogMessage logMessage)
        {
            return LogMessageOperations.AddLogMessage(logMessage);
        }

        public LogMessage GetLogMessage(int logMessageId)
        {
            return LogMessageOperations.GetLogMessage(logMessageId);
        }

        public IEnumerable<LogMessage> GetLogMessages(TimeSpan logPeriod)
        {
            return LogMessageOperations.GetLogMessages(logPeriod);
        }

        public void DeleteLogsOlderThanDate(DateTime olderThanDate)
        {
            LogMessageOperations.DeleteLogsOlderThanDate(olderThanDate);
        }

        public void ClearLog()
        {
            LogMessageOperations.ClearLog();
        }

        public void DeleteShipFit(int shipFitId)
        {
            ShipFitOperations.DeleteShipFit(shipFitId);
        }

        public void UpdateShipFit(ShipFit shipFit)
        {
            ShipFitOperations.UpdateShipFit(shipFit);
        }

        public ShipFit AddShipFit(ShipFit shipFit)
        {
            return ShipFitOperations.AddShipFit(shipFit);
        }

        public ShipFit CreateShipFit(ShipFit shipFit)
        {
            return ShipFitOperations.CreateShipFit(shipFit);
        }

        public ShipFit GetShipFit(int shipFitId)
        {
            return ShipFitOperations.GetShipFit(shipFitId);
        }

        public IEnumerable<ShipFit> GetShipFits()
        {
            return ShipFitOperations.GetShipFits();
        }

        public IEnumerable<ShipFit> GetShipFitsWithComponents()
        {
            return ShipFitOperations.GetShipFitsWithComponents();
        }

        public IEnumerable<ShipFit> GetShipFitsForContractCount()
        {
            return ShipFitOperations.GetShipFitsForContractCount();
        }

        public IEnumerable<ShipFit> GetShipFitsForAccount(int accountId)
        {
            return ShipFitOperations.GetShipFitsForAccount(accountId);
        }

        public Dictionary<string, int> GetShipFitList()
        {
            return ShipFitOperations.GetShipFitList();
        }

        public void DeleteShipFitComponent(int shipFitComponentId)
        {
            ShipFitComponentOperations.DeleteShipFitComponent(shipFitComponentId);
        }

        public void DeleteShipFitComponentsByShipFitId(int shipFitId)
        {
            ShipFitComponentOperations.DeleteShipFitComponentsByShipFitId(shipFitId);
        }

        public void UpdateShipFitComponent(ShipFitComponent shipFitComponent)
        {
            ShipFitComponentOperations.UpdateShipFitComponent(shipFitComponent);
        }

        public ShipFitComponent AddShipFitComponent(ShipFitComponent shipFitComponent)
        {
            return ShipFitComponentOperations.AddShipFitComponent(shipFitComponent);
        }

        public ShipFitComponent CreateShipFitComponent(ShipFitComponent shipFitComponent)
        {
            return ShipFitComponentOperations.CreateShipFitComponent(shipFitComponent);
        }

        public ShipFitComponent GetShipFitComponent(int shipFitComponentId)
        {
            return ShipFitComponentOperations.GetShipFitComponent(shipFitComponentId);
        }

        public IEnumerable<ShipFitComponent> GetShipFitComponents(int shipFitId)
        {
            return ShipFitComponentOperations.GetShipFitComponents(shipFitId);
        }

        public void DeleteShortUrl(string shortUrlId)
        {
            ShortUrlOperations.DeleteShortUrl(shortUrlId);
        }

        public void UpdateShortUrl(ShortUrl shortUrl)
        {
            ShortUrlOperations.UpdateShortUrl(shortUrl);
        }

        public ShortUrl AddShortUrl(ShortUrl shortUrl)
        {
            return ShortUrlOperations.AddShortUrl(shortUrl);
        }

        public ShortUrl CreateShortUrl(ShortUrl shortUrl)
        {
            return ShortUrlOperations.CreateShortUrl(shortUrl);
        }

        public ShortUrl GetShortUrl(string shortUrlId)
        {
            return ShortUrlOperations.GetShortUrl(shortUrlId);
        }

        public IEnumerable<ShortUrl> GetShortUrls()
        {
            return ShortUrlOperations.GetShortUrls();
        }

        public void DeleteShortUrlsOlderThanDate(DateTime olderThanDate)
        {
            ShortUrlOperations.DeleteShortUrlsOlderThanDate(olderThanDate);
        }

        public void DeleteDoctrine(int doctrineId)
        {
            DoctrineOperations.DeleteDoctrine(doctrineId);
        }

        public void DeleteDoctrinesByAccountId(int accountId)
        {
            DoctrineOperations.DeleteDoctrinesByAccountId(accountId);
        }

        public void UpdateDoctrine(Doctrine doctrine)
        {
            DoctrineOperations.UpdateDoctrine(doctrine);
        }

        public Doctrine AddDoctrine(Doctrine doctrine)
        {
            return DoctrineOperations.AddDoctrine(doctrine);
        }

        public Doctrine CreateDoctrine(Doctrine doctrine)
        {
            return DoctrineOperations.CreateDoctrine(doctrine);
        }

        public Doctrine GetDoctrine(int doctrineId)
        {
            return DoctrineOperations.GetDoctrine(doctrineId);
        }

        public IEnumerable<Doctrine> GetDoctrines()
        {
            return DoctrineOperations.GetDoctrines();
        }

        public IEnumerable<Doctrine> GetDoctrinesForAccount(int accountId)
        {
            return DoctrineOperations.GetDoctrinesForAccount(accountId);
        }

        public void DeleteDoctrineShipFit(int doctrineShipFitId)
        {
            DoctrineShipFitOperations.DeleteDoctrineShipFit(doctrineShipFitId);
        }

        public void DeleteDoctrineShipFitsByDoctrineId(int doctrineId)
        {
            DoctrineShipFitOperations.DeleteDoctrineShipFitsByDoctrineId(doctrineId);
        }

        public void DeleteDoctrineShipFitsByShipFitId(int shipFitId)
        {
            DoctrineShipFitOperations.DeleteDoctrineShipFitsByShipFitId(shipFitId);
        }

        public void UpdateDoctrineShipFit(DoctrineShipFit doctrineShipFit)
        {
            DoctrineShipFitOperations.UpdateDoctrineShipFit(doctrineShipFit);
        }

        public DoctrineShipFit AddDoctrineShipFit(DoctrineShipFit doctrineShipFit)
        {
            return DoctrineShipFitOperations.AddDoctrineShipFit(doctrineShipFit);
        }

        public DoctrineShipFit CreateDoctrineShipFit(DoctrineShipFit doctrineShipFit)
        {
            return DoctrineShipFitOperations.CreateDoctrineShipFit(doctrineShipFit);
        }

        public DoctrineShipFit GetDoctrineShipFit(int doctrineShipFitId)
        {
            return DoctrineShipFitOperations.GetDoctrineShipFit(doctrineShipFitId);
        }

        public IEnumerable<DoctrineShipFit> GetDoctrineShipFits(int doctrineId)
        {
            return DoctrineShipFitOperations.GetDoctrineShipFits(doctrineId);
        }

        public void Save()
        {
            unitOfWork.Save();
        }
    }
}
