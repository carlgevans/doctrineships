namespace DoctrineShips.Web.ViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class SalesAgentRegistrationViewModel
    {
        [Required(ErrorMessage = "The EVE Api Key Id is required.")]
        [DisplayName("EVE Api Key Id")]
        public int ApiId { get; set; }

        [Required(ErrorMessage = "The EVE Api vCode is required.")]
        [DisplayName("EVE Api vCode")]
        public string ApiKey { get; set; }        
    }
}