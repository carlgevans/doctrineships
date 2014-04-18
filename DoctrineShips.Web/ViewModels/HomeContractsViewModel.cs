namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;

    public class HomeContractsViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
    }
}