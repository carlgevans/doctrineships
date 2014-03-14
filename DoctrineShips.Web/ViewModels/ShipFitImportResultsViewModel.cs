namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;

    public class ShipFitImportResultsViewModel
    {
        public IEnumerable<ShipFit> ShipFits { get; set; }
    }
}