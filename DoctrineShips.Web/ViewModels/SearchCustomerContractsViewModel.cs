namespace DoctrineShips.Web.ViewModels
{
    using DoctrineShips.Entities;
    using System.Collections.Generic;

    public class SearchCustomerContractsViewModel
    {
        public Customer Customer { get; set; }
        public IEnumerable<IEnumerable<Contract>> Contracts { get; set; }
    }
}