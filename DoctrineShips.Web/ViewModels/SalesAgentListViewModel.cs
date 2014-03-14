namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;

    public class SalesAgentListViewModel
    {
        public IEnumerable<SalesAgent> SalesAgents { get; set; }
    }
}