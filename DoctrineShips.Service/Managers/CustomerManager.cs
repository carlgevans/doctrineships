namespace DoctrineShips.Service.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Validation;
    using EveData;
    using Tools;

    /// <summary>
    /// A class dealing with Doctrine Ships customers.
    /// </summary>
    internal sealed class CustomerManager
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;
        private readonly IEveDataSource eveDataSource;
        private readonly IDoctrineShipsValidation doctrineShipsValidation;
        private readonly ISystemLogger logger;

        /// <summary>
        /// Initialises a new instance of a Customer Manager.
        /// </summary>
        /// <param name="doctrineShipsRepository">An IDoctrineShipsRepository instance.</param>
        /// <param name="eveDataSource">An IEveDataSource instance.</param>
        /// <param name="doctrineShipsValidation">An IDoctrineShipsValidation instance.</param>
        /// <param name="logger">An ISystemLogger logger instance.</param>
        internal CustomerManager(IDoctrineShipsRepository doctrineShipsRepository, IEveDataSource eveDataSource, IDoctrineShipsValidation doctrineShipsValidation, ISystemLogger logger)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
            this.eveDataSource = eveDataSource;
            this.doctrineShipsValidation = doctrineShipsValidation;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches and returns a Doctrine Ships customer.
        /// </summary>
        /// <param name="customerId">The id of the customer for which a customer object should be returned.</param>
        /// <returns>A customer object.</returns>
        internal Customer GetCustomer(int customerId)
        {
            var customer = this.doctrineShipsRepository.GetCustomer(customerId);
            
            if (customer == null)
            {
                customer = new Customer()
                {
                    CustomerId = 0,
                    IsCorp = true,
                    Name = "Unknown",
                    ImageUrl = "http://image.eveonline.com/Corporation/1_128.png"
                };
            }

            return customer;
        }

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships corporation customers.
        /// </summary>
        /// <returns>A list of customer objects.</returns>
        internal IEnumerable<Customer> GetCorporationCustomers()
        {
            return this.doctrineShipsRepository.GetCorporationCustomers();
        }

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships customers.
        /// </summary>
        /// <returns>A list of customer objects.</returns>
        internal IEnumerable<Customer> GetCustomers()
        {
            return this.doctrineShipsRepository.GetCustomers();
        }

        /// <summary>
        /// <para>Adds a customer.</para>
        /// </summary>
        /// <param name="name">The name of the new customer. This must be identical to the in-game name.</param>
        /// <param name="type">Is this customer a corporation, an alliance or an individual character?</param>
        /// <returns>Returns a validation result object.</returns>
        internal IValidationResult AddCustomer(string name, int type)
        {
            int customerId = 0;
            IValidationResult validationResult = new ValidationResult();

            // Does the customer name successfully resolve to an id? 
            customerId = eveDataSource.GetCharacterId(name);

            if (customerId != 0)
            {
                // Populate a new customer object.
                Customer newCustomer = new Customer();

                // Populate the remaining properties.
                newCustomer.CustomerId = customerId;
                newCustomer.Name = name;
                newCustomer.LastRefresh = DateTime.UtcNow;

                switch (type)
                {
                    case 1:
                        newCustomer.ImageUrl = eveDataSource.GetCorporationLogoUrl(newCustomer.CustomerId);
                        newCustomer.IsCorp = true;
                        break;
                    case 2:
                        newCustomer.ImageUrl = eveDataSource.GetAllianceLogoUrl(newCustomer.CustomerId);
                        newCustomer.IsCorp = true;
                        break;
                    case 3:
                        newCustomer.ImageUrl = eveDataSource.GetCharacterPortraitUrl(newCustomer.CustomerId);
                        newCustomer.IsCorp = false;
                        break;
                }
           
                // Validate the new customer.
                validationResult = this.doctrineShipsValidation.Customer(newCustomer);
                if (validationResult.IsValid == true)
                {
                    // Add the new customer and log the event.
                    this.doctrineShipsRepository.CreateCustomer(newCustomer);
                    this.doctrineShipsRepository.Save();
                    logger.LogMessage("Customer '" + newCustomer.Name + "' Successfully Added.", 2, "Message", MethodBase.GetCurrentMethod().Name);
                }
            }
            else
            {
                validationResult.AddError("CustomerName.Valid", "An invalid customer name was entered.");
            }

            return validationResult;
        }

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <param name="customerId">The customer Id being deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        internal bool DeleteCustomer(int customerId)
        {
            Customer customer = this.doctrineShipsRepository.GetCustomer(customerId);

            if (customer != null)
            {
                // Delete the customer and log the event.
                this.doctrineShipsRepository.DeleteCustomer(customer.CustomerId);
                this.doctrineShipsRepository.Save();
                logger.LogMessage("Customer '" + customer.Name + "' Successfully Deleted.", 1, "Message", MethodBase.GetCurrentMethod().Name);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a list of contracts for a given corporation.
        /// </summary>
        /// <param name="customerId">The id of the customer for which contracts should be returned.</param>
        /// <returns>A list of customer contract objects.</returns>
        internal IEnumerable<Contract> GetCustomerContracts(int customerId)
        {
            return this.doctrineShipsRepository.GetAssigneeContracts(customerId);
        }
    }
}
