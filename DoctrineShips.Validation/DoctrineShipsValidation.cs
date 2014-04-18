namespace DoctrineShips.Validation
{
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Validation.Checks;
    using EveData.Entities;
    using System.Collections.Generic;

    public class DoctrineShipsValidation : IDoctrineShipsValidation
    {
         // External Dependencies.
        private readonly IDoctrineShipsRepository doctrineShipsRepository;

        // Internal Dependencies (Instantiated On-Demand By Accessors).
        private AccountCheck accountCheck;
        private ContractCheck contractCheck;
        private CustomerCheck customerCheck;
        private SalesAgentCheck salesAgentCheck;
        private ShipFitCheck shipFitCheck;

        public DoctrineShipsValidation(IDoctrineShipsRepository doctrineShipsRepository)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
        }

        internal AccountCheck AccountCheck
        {
            get
            {
                if (this.accountCheck == null)
                {
                    this.accountCheck = new AccountCheck(doctrineShipsRepository);
                }

                return this.accountCheck;
            }
        }

        internal ContractCheck ContractCheck
        {
            get
            {
                if (this.contractCheck == null)
                {
                    this.contractCheck = new ContractCheck(doctrineShipsRepository);
                }

                return this.contractCheck;
            }
        }

        internal CustomerCheck CustomerCheck
        {
            get
            {
                if (this.customerCheck == null)
                {
                    this.customerCheck = new CustomerCheck(doctrineShipsRepository);
                }

                return this.customerCheck;
            }
        }

        internal SalesAgentCheck SalesAgentCheck
        {
            get
            {
                if (this.salesAgentCheck == null)
                {
                    this.salesAgentCheck = new SalesAgentCheck(doctrineShipsRepository);
                }

                return this.salesAgentCheck;
            }
        }

        internal ShipFitCheck ShipFitCheck
        {
            get
            {
                if (this.shipFitCheck == null)
                {
                    this.shipFitCheck = new ShipFitCheck(doctrineShipsRepository);
                }

                return this.shipFitCheck;
            }
        }

        public IValidationResult Contract(Contract contract)
        {
            return ContractCheck.Contract(contract);
        }

        public IValidationResult SalesAgent(SalesAgent salesAgent)
        {
            return SalesAgentCheck.SalesAgent(salesAgent);
        }

        public IValidationResult ApiKey(IEveDataApiKey apiKeyInfo)
        {
            return SalesAgentCheck.ApiKey(apiKeyInfo);
        }

        public IValidationResult Customer(Customer customer)
        {
            return CustomerCheck.Customer(customer);
        }

        public IValidationResult ShipFit(ShipFit shipFit)
        {
            return ShipFitCheck.ShipFit(shipFit);
        }

        public IValidationResult ShipFitComponent(ShipFitComponent shipFitComponent)
        {
            return ShipFitCheck.ShipFitComponent(shipFitComponent);
        }

        public IValidationResult Component(Component component)
        {
            return ShipFitCheck.Component(component);
        }

        public IValidationResult Account(Account account)
        {
            return AccountCheck.Account(account);
        }

        public IValidationResult AccessCode(AccessCode accessCode)
        {
            return AccountCheck.AccessCode(accessCode);
        }

        public IValidationResult SettingProfile(SettingProfile settingProfile)
        {
            return AccountCheck.SettingProfile(settingProfile);
        }

        public IValidationResult NotificationRecipient(NotificationRecipient notificationRecipient)
        {
            return AccountCheck.NotificationRecipient(notificationRecipient);
        }

        public IValidationResult Doctrine(Doctrine doctrine)
        {
            return ShipFitCheck.Doctrine(doctrine);
        }
    }
}
