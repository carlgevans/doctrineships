namespace DoctrineShips.Validation.Checks
{
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;

    internal sealed class ShipFitCheck
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;

        internal ShipFitCheck(IDoctrineShipsRepository doctrineShipsRepository)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
        }

        internal IValidationResult ShipFit(ShipFit shipFit)
        {
            IValidationResult validationResult = new ValidationResult();

            // Check that the ship fit has a valid account id.
            if (shipFit.AccountId < 0 || shipFit.AccountId > int.MaxValue)
            {
                validationResult.AddError("AccountId.Range_" + shipFit.ShipFitId.ToString(), "AccountId can not be less than or equal to 0. Also, the upper limit cannot exceed the max value of the int data type.");
            }

            return validationResult;
        }

        internal IValidationResult ShipFitComponent(ShipFitComponent shipFitComponent)
        {
            IValidationResult validationResult = new ValidationResult();

            // Check that the ship fit component has a valid quantity.
            if (shipFitComponent.Quantity <= 0 || shipFitComponent.Quantity > long.MaxValue)
            {
                validationResult.AddError("Quantity.Range_" + shipFitComponent.ShipFitComponentId.ToString(), "Quantity can not be less than or equal to 0. Also, the upper limit cannot exceed the max value of the long data type.");
            }

            return validationResult;
        }

        internal IValidationResult Component(Component component)
        {
            IValidationResult validationResult = new ValidationResult();

            // Is the component id valid?
            if (component.ComponentId <= 0 || component.ComponentId > int.MaxValue)
            {
                validationResult.AddError("ComponentId.Range_" + component.ComponentId.ToString(), "ComponentId can not be less than or equal to 0. Also, the upper limit cannot exceed the max value of the int data type.");
            }

            // Does the component already exist in the database?
            if (this.doctrineShipsRepository.GetComponent(component.ComponentId) != null)
            {
                validationResult.AddError("ComponentId.Exists_" + component.ComponentId.ToString(), "ComponentId already exists in the database.");
            }

            return validationResult;
        }
    }
}
