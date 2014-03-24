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
        public int[] RemoveList { get; set; }
        public int AccountId { get; set; }

        [Required(ErrorMessage = "You must enter a description.")]
        [Display(Name = "Description")]
        [StringLength(30, ErrorMessage = "The description field must be shorter than 30 characters.")]
        public string Description { get; set; }
    }
}