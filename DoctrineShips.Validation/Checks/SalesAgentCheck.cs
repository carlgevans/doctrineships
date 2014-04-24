namespace DoctrineShips.Validation.Checks
{
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
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

            // Define the bitwise masks for both character and corporation api keys.
            const int charContractMask = 67108864;
            const int corpContractMask = 8388608;
            int checkResult = 0;

            // Perform a bitwise AND to determine if contract access is available on the api key.
            if (apiKeyInfo.Type == EveDataApiKeyType.Character || apiKeyInfo.Type == EveDataApiKeyType.Account)
            {
                checkResult = apiKeyInfo.AccessMask & charContractMask;

                if (checkResult == 0)
                {
                    validationResult.AddError("ApiKey.AccessMask", "A character api key must have 'Contracts' access enabled.");
                }
            }
            else if (apiKeyInfo.Type == EveDataApiKeyType.Corporation)
            {
                checkResult = apiKeyInfo.AccessMask & corpContractMask;

                if (checkResult == 0)
                {
                    validationResult.AddError("ApiKey.AccessMask", "A corporation api key must have 'Contracts' access enabled.");
                }
            }

            return validationResult;
        }
    }
}
