namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;
    
    public class ShipFitListViewModel
    {
        public IEnumerable<ShipFit> ShipFits { get; set; }
        public SettingProfile SettingProfile { get; set; }
    }
}