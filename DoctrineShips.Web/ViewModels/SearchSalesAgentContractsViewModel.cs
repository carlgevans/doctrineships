namespace DoctrineShips.Web.ViewModels
{
    using DoctrineShips.Entities;
    using System.Collections.Generic;

    public class SearchSalesAgentContractsViewModel
    {
        public SalesAgent SalesAgent { get; set; }
        public IEnumerable<IEnumerable<Contract>> Contracts { get; set; }
    }
}