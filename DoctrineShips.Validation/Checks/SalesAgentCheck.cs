namespace DoctrineShips.Validation.Checks
{
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Validation.Entities;
    using EveData.Entities;

    internal sealed class SalesAgentCheck
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;

        internal SalesAgentCheck(IDoctrineShipsRepository doctrineShipsRepository)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
        }

        internal IValidationResult SalesAgent(SalesAgent salesAgent)
        {
            IValidationResult validationResult = new ValidationResult();

            // Is the salesAgent id valid?
            if (salesAgent.SalesAgentId <= 0 || salesAgent.SalesAgentId > int.MaxValue)
            {
                validationResult.AddError("SalesAgentId.Range", "SalesAgentId can not be less than or equal to 0. Also, the upper limit cannot exceed the max value of the int data type.");
            }

            // Does the salesAgent already exist in the database?
            if (this.doctrineShipsRepository.GetSalesAgent(salesAgent.SalesAgentId) != null)
            {
                validationResult.AddError("SalesAgentId.Exists", "SalesAgentId already exists in the database.");
            }

            return validationResult;
        }

        internal IValidationResult ApiKey(IEveDataApiKey apiKeyInfo)
        {
            IValidationResult validationResult = new ValidationResult();

            // Check the access mask of the key is correct.
            if (apiKeyInfo.Type == EveDataApiKeyType.Character || apiKeyInfo.Type == EveDataApiKeyType.Account)
            {
                if (apiKeyInfo.AccessMask != 67108864)
                {
                    validationResult.AddError("ApiKey.AccessMask", "A character api key must have an access mask of 67108864 ('Contracts' Only).");
                }
            }
            else if (apiKeyInfo.Type == EveDataApiKeyType.Corporation)
            {
                if (apiKeyInfo.AccessMask != 8388608)
                {
                    validationResult.AddError("ApiKey.AccessMask", "A corporation api key must have an access mask of 8388608 ('Contracts' Only).");
                }
            }

            return validationResult;
        }
    }
}
