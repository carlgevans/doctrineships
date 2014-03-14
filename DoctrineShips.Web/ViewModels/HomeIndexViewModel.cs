namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;

    public class HomeIndexViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
    }
}