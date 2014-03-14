namespace DoctrineShips.Repository.Operations
{
    using DoctrineShips.Entities;
    using GenericRepository;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class CustomerOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal CustomerOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteCustomer(int customerId)
        {
            this.unitOfWork.Repository<Customer>().Delete(customerId);
        }

        internal void UpdateCustomer(Customer customer)
        {
            customer.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<Customer>().Update(customer);
        }

        internal Customer AddCustomer(Customer customer)
        {
            this.unitOfWork.Repository<Customer>().Insert(customer);
            return customer;
        }

        internal Customer CreateCustomer(Customer customer)
        {
            customer.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<Customer>().Insert(customer);
            return customer;
        }

        internal Customer GetCustomer(int customerId)
        {
            var customer = this.unitOfWork.Repository<Customer>()
                           .Query()
                           .Filter(x => x.CustomerId == customerId)
                           .Get()
                           .FirstOrDefault();

            return customer;
        }

        internal IEnumerable<Customer> GetCustomers()
        {
            var customers = this.unitOfWork.Repository<Customer>()
                            .Query()
                            .Get()
                            .OrderBy(x => x.Name)
                            .ToList();

            return customers;
        }

        internal IEnumerable<Customer> GetCorporationCustomers()
        {
            var customers = this.unitOfWork.Repository<Customer>()
                            .Query()
                            .Filter(q => q.IsCorp == true)
                            .Get()
                            .OrderBy(x => x.Name)
                            .ToList();

            return customers;
        }

        internal IEnumerable<Customer> GetCharacterCustomers()
        {
            var customers = this.unitOfWork.Repository<Customer>()
                            .Query()
                            .Filter(q => q.IsCorp == false)
                            .Get()
                            .OrderBy(x => x.Name)
                            .ToList();

            return customers;
        }
    }
}