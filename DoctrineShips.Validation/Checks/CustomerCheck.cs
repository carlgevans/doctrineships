namespace DoctrineShips.Validation.Checks
{
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;

    internal sealed class CustomerCheck
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;

        internal CustomerCheck(IDoctrineShipsRepository doctrineShipsRepository)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
        }

        internal IValidationResult Customer(Customer customer)
        {
            IValidationResult validationResult = new ValidationResult();

            // Is the customer id valid?
            if (customer.CustomerId <= 0 || customer.CustomerId > int.MaxValue)
            {
                validationResult.AddError("CustomerId.Range", "CustomerId can not be less than or equal to 0. Also, the upper limit cannot exceed the max value of the long data type.");
            }

            // Is the customer name valid?
            if (customer.Name == string.Empty || customer.Name == null)
            {
                validationResult.AddError("CustomerName.Null", "CustomerName can not be empty or null.");
            }

            // Does the customer already exist in the database?
            if (this.doctrineShipsRepository.GetCustomer(customer.CustomerId) != null)
            {
                validationResult.AddError("CustomerId.Exists", "CustomerId already exists in the database.");
            }

            return validationResult;
        }
    }
}
