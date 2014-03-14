namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;

    public class KnowledgeBaseListViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
    }
}