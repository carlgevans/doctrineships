namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using DoctrineShips.Entities;
    
    public class SiteSubscriptionPlansViewModel
    {
        public IEnumerable<SubscriptionPlan> SubscriptionPlans { get; set; }

        // Form Data. 
        public int[] RemoveList { get; set; }

        [Required(ErrorMessage = "A Plan Name is required.")]
        [DisplayName("Plan Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A Description is required.")]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A Sales Agent Limit is required.")]
        [DisplayName("Sales Agent Limit")]
        [Range(1, 500, ErrorMessage = "A sales agent limit must be between 1 and 500.")]
        public int SalesAgentLimit { get; set; }

        [Required(ErrorMessage = "A Price Per Month is required.")]
        [DisplayName("Price Per Month")]
        [Range(0, long.MaxValue, ErrorMessage = "That is not a valid price per month.")]
        public double PricePerMonth { get; set; }

        [DisplayName("Hidden From Public List?")]
        [Range(0, 1, ErrorMessage = "That isn't a valid value.")]
        public bool IsHidden { get; set; }
    }
}