namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using DoctrineShips.Entities;
    
    public class AccountDoctrinesViewModel
    {
        public IEnumerable<Doctrine> Doctrines { get; set; }
        
        // Form Data.
        public int[] DoctrineShipFitIds { get; set; }
        public int[] RemoveList { get; set; }
        public int AccountId { get; set; }
        public int DoctrineId { get; set; }

        [DisplayName("Doctrine Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }

        [DisplayName("Official?")]
        [Range(0, 1, ErrorMessage = "That isn't a valid value.")]
        public bool IsOfficial { get; set; }

        [DisplayName("Dormant?")]
        [Range(0, 1, ErrorMessage = "That isn't a valid value.")]
        public bool IsDormant { get; set; }
    }
}