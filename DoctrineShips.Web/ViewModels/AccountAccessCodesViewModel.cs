namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using DoctrineShips.Entities;

    public class AccountAccessCodesViewModel
    {
        public IEnumerable<AccessCode> AccessCodes { get; set; }

        // Form Data. 
        public int[] RemoveList { get; set; }
        public ICollection<SelectListItem> Roles { get; set; }

        [Display(Name = "Role")]
        public int SelectedRole { get; set; }

        [Required(ErrorMessage = "You must enter a description.")]
        [Display(Name = "Description")]
        [StringLength(30, ErrorMessage = "The description field must be shorter than 30 characters.")]
        public string Description { get; set; }
        
        public AccountAccessCodesViewModel()
        {
            // Populate the roles dropdown list.
            Roles = new List<SelectListItem>();

            Roles.Add(new SelectListItem
            {
                Text = "User",
                Value = "1",
                Selected = true
            });

            Roles.Add(new SelectListItem
            {
                Text = "Account Admin",
                Value = "2"
            });
        }
    }
}