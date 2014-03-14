namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;
    using Tools;

    public class SiteLogViewModel
    {
        public IEnumerable<LogMessage> LogMessages { get; set; }
    }
}