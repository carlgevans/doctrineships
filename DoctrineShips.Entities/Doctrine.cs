namespace DoctrineShips.Entities
{
    using System;
    using System.Collections.Generic;
    using GenericRepository;

    /// <summary>
    /// A Doctrine Ships doctrine, a group of ship fits that compliment each other.
    /// </summary>
    public class Doctrine : EntityBase
    {
        public int DoctrineId { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsDormant { get; set; }
        public DateTime LastUpdate { get; set; }
        public virtual ICollection<DoctrineShipFit> DoctrineShipFits { get; set; }
        public virtual Account Account { get; set; }
    }
}
