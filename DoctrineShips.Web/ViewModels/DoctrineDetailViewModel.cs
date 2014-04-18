namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;
    
    public class DoctrineDetailViewModel
    {
        public Doctrine Doctrine { get; set; }
        public SettingProfile SettingProfile { get; set; }
    }
}