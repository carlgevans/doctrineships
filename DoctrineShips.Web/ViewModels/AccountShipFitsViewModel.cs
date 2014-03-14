namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;
    
    public class AccountShipFitsViewModel
    {
        public IEnumerable<ShipFit> ShipFits { get; set; }

        // Form Data. 
        public int[] RemoveList { get; set; }
    }
}