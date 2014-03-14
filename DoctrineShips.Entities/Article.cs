namespace DoctrineShips.Entities
{
    using GenericRepository;
    using System;

    /// <summary>
    /// A Doctrine Ships article.
    /// </summary>
    public class Article : EntityBase
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public bool IsUnlisted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
