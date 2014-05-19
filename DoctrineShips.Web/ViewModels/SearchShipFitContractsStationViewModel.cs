namespace DoctrineShips.Web.ViewModels
{
    using DoctrineShips.Entities;
    using System.Collections.Generic;

    public class SearchShipFitContractsStationViewModel
    {
        public ShipFit ShipFit { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
    }
}