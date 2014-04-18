namespace DoctrineShips.Web.ViewModels
{
    using DoctrineShips.Entities;
    using System.Collections.Generic;

    public class SearchShipFitContractsViewModel
    {
        public ShipFit ShipFit { get; set; }
        public IEnumerable<IEnumerable<Contract>> Contracts { get; set; }
    }
}