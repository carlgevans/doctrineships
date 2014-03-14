namespace DoctrineShips.Web.ViewModels
{
    using DoctrineShips.Entities;
    using System.Collections.Generic;

    public class SearchCustomerContractsStationViewModel
    {
        public Customer Customer { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
    }
}