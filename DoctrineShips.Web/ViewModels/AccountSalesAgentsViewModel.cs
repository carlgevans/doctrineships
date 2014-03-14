namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using DoctrineShips.Entities;
    
    public class AccountSalesAgentsViewModel
    {
        public IEnumerable<SalesAgent> SalesAgents { get; set; }
        
        // Form Data. 
        public int[] RemoveList { get; set; }

        [Required(ErrorMessage = "The EVE Api Key Id is required.")]
        [DisplayName("EVE Api Key Id")]
        public int ApiId { get; set; }

        [Required(ErrorMessage = "The EVE Api vCode is required.")]
        [DisplayName("EVE Api vCode")]
        public string ApiKey { get; set; }        
    }
}