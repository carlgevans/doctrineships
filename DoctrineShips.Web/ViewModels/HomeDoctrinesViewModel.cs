namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;

    public class HomeDoctrinesViewModel
    {
        public IEnumerable<Doctrine> Doctrines { get; set; }
    }
}