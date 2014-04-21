namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using DoctrineShips.Entities;
    
    public class AccountShipFitsViewModel
    {
        public IEnumerable<ShipFit> ShipFits { get; set; }

        // Form Data. 
        public int[] RemoveList { get; set; }
        public int ShipFitId { get; set; }
        public int AccountId { get; set; }

        [DisplayName("Fit Name")]
        public string Name { get; set; }

        [DisplayName("Role")]
        public string Role { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }
    }
}