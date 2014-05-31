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

            // Null checks.
            if (shipFit.Notes == null)
            {
                validationResult.AddError("Notes.Null", "Notes cannot be null.");
            }

            if (shipFit.Name == null)
            {
                validationResult.AddError("Name.Null", "Name cannot be null.");
            }

            if (shipFit.Role == null)
            {
                validationResult.AddError("Role.Null", "Role cannot be null.");
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

        internal IValidationResult Doctrine(Doctrine doctrine)
        {
            IValidationResult validationResult = new ValidationResult();

            // Check that the doctrine has a valid account id.
            if (doctrine.AccountId < 0 || doctrine.AccountId > int.MaxValue)
            {
                validationResult.AddError("AccountId.Range", "AccountId can not be less than or equal to 0. Also, the upper limit cannot exceed the max value of the int data type.");
            }

            // Null & empty checks.
            if (doctrine.Name == null || doctrine.Name == string.Empty )
            {
                validationResult.AddError("Name.Null", "Name cannot be null or empty.");
            }

            if (doctrine.Description == null)
            {
                validationResult.AddError("Description.Null", "Description cannot be null.");
            }

            if (doctrine.ImageUrl == null)
            {
                validationResult.AddError("ImageUrl.Null", "ImageUrl cannot be null.");
            }

            if (doctrine.LastUpdate == null)
            {
                validationResult.AddError("LastUpdate.Null", "LastUpdate cannot be null.");
            }

            // Is this a https url?
            if (doctrine.ImageUrl != string.Empty && doctrine.ImageUrl.Length >= 5)
            {
                if (doctrine.ImageUrl.Substring(0, 5) != "https")
                {
                    validationResult.AddError("ImageUrl.Https", "ImageUrl must start with https. Please use an image hosting service such as imgur that supports https.");
                }
            }

            return validationResult;
        }
    }
}
