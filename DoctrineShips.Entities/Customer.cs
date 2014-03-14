namespace DoctrineShips.Entities
{
    using System;
    using GenericRepository;

    /// <summary>
    /// A Doctrine Ships customer. A customer can be a corporation or an indivudual character.
    /// </summary>
    public class Customer : EntityBase
    {
        public int CustomerId { get; set; }                         
        public string Name { get; set; }       
        public string ImageUrl { get; set; }
        public bool IsCorp { get; set; }
        public DateTime LastRefresh { get; set; } 
    }
}
