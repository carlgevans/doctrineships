namespace DoctrineShips.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using DoctrineShips.Entities;

    public class SiteAccountsViewModel
    {
        public IEnumerable<Account> Accounts { get; set; }

        // Form Data. 
        public ICollection<SelectListItem> SubscriptionPlans { get; set; }
        public int[] RemoveList { get; set; }
        public int AccountId { get; set; }

        [Required(ErrorMessage = "You must enter a description.")]
        [Display(Name = "Description")]
        [StringLength(30, ErrorMessage = "The description field must be shorter than 30 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "A Subscription Plan is required.")]
        [DisplayName("Subscription Plan")]
        [Range(0, Int32.MaxValue, ErrorMessage = "That does not look like a valid subscription plan id.")]
        public int SubscriptionPlanId { get; set; }
    }
}