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

        [Required(ErrorMessage = "You must enter a fit name.")]
        [DisplayName("Fit Name")]
        [StringLength(100, ErrorMessage = "The fit name field must be shorter than 100 characters.")]
        public string Name { get; set; }

        [DisplayName("Role")]
        [StringLength(50, ErrorMessage = "The role field must be shorter than 50 characters.")]
        public string Role { get; set; }

        [DisplayName("Notes")]
        [StringLength(10000, ErrorMessage = "The notes field must be shorter than 10,000 characters.")]
        public string Notes { get; set; }
    }
}