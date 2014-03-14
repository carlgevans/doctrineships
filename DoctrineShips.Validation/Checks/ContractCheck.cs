namespace DoctrineShips.Validation.Checks
{
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Validation.Entities;
    using EveData.Entities;

    internal sealed class ContractCheck
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;

        internal ContractCheck(IDoctrineShipsRepository doctrineShipsRepository)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
        }

        internal IValidationResult Contract(Contract contract)
        {
            IValidationResult validationResult = new ValidationResult();

            // Is the contract id valid?
            if (contract.ContractId <= 0 || contract.ContractId > long.MaxValue)
            {
                validationResult.AddError("ContractId.Range", "ContractId can not be less than or equal to 0. Also, the upper limit cannot exceed the max value of the long data type.");
            }

            // Does the contract already exist in the database?
            if (this.doctrineShipsRepository.GetContract(contract.ContractId) != null)
            {
                validationResult.AddError("ContractId.Exists", "ContractId already exists in the database.");
            }

            // Does the ship fit exist in the database?
            if (this.doctrineShipsRepository.GetShipFit(contract.ShipFitId) == null)
            {
               validationResult.AddError("ShipFitId.Exists", "ShipFitId does not exist in the database.");
            }

            return validationResult;
        }

        internal IValidationResult ShipFitContract(IEnumerable<ShipFitComponent> shipFitComponents, IEnumerable<IEveDataContractItem> contractItems)
        {
            IValidationResult validationResult = new ValidationResult();

            IEnumerable<ShipFitComponent> compressedShipFitComponents = new List<ShipFitComponent>();
            IEnumerable<ShipFitComponent> compressedContractItems = new List<ShipFitComponent>();

            // Compress the ship fit components list, removing duplicates but adding the quantities.
            if (shipFitComponents != null)
            {
                compressedShipFitComponents = shipFitComponents
                        .OrderBy(o => o.ComponentId)
                        .GroupBy(u => u.ComponentId)
                        .Select(x => new ShipFitComponent()
                        {
                            ComponentId = x.Key,
                            Quantity = x.Sum(p => p.Quantity)
                        });
            }

            // Compress the contract items components list, removing duplicates but adding the quantities.
            if (contractItems != null)
            {
                compressedContractItems = contractItems
                    .OrderBy(o => o.TypeId)
                    .GroupBy(u => u.TypeId)
                    .Select(x => new ShipFitComponent()
                    {
                        ComponentId = x.Key,
                        Quantity = x.Sum(p => p.Quantity)
                    });
            }
            
            // Are the two lists of components identical?
            if (!Enumerable.SequenceEqual<ShipFitComponent>(compressedShipFitComponents, compressedContractItems, new ComponentComparer()))
            {
                validationResult.AddError("ShipFitContract.Match", "The contract and the ship fit are not identical.");
            }

            return validationResult;
        }
    }
}
