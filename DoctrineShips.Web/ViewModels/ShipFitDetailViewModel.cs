namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;
    
    public class ShipFitDetailViewModel
    {
        public ShipFit ShipFit { get; set; }
        public IEnumerable<IEnumerable<ShipFitComponent>> ShipFitComponents { get; set; }
        public SettingProfile SettingProfile { get; set; }
    }
}