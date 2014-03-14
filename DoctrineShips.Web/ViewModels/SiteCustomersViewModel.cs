namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.Web.Mvc;
    using DoctrineShips.Entities;

    public class SiteCustomersViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }

        // Form Data. 
        public ICollection<SelectListItem> Types { get; set; }
        public int[] RemoveList { get; set; }

        [Required(ErrorMessage = "A customer name is required.")]
        [DisplayName("Customer Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A Type is required.")]
        [DisplayName("Type")]
        [Range(1, 3, ErrorMessage = "That isn't a valid type.")]
        public int Type { get; set; }

        public SiteCustomersViewModel()
        {
            // Populate the dropdown lists.
            Types = new List<SelectListItem>();

            Types.Add(new SelectListItem
            {
                Text = "Corporation",
                Value = "1",
                Selected = true
            });

            Types.Add(new SelectListItem
            {
                Text = "Alliance",
                Value = "2"
            });

            Types.Add(new SelectListItem
            {
                Text = "Individual",
                Value = "3"
            });
        }
    }
}