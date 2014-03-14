namespace DoctrineShips.Entities
{
    using System;
    using GenericRepository;
    
    /// <summary>
    /// A Doctrine Ships shortened url.
    /// </summary>
    public class ShortUrl : EntityBase
    {
        public string ShortUrlId { get; set; }
        public string LongUrl { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
