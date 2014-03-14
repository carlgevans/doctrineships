namespace DoctrineShips.Entities
{
    using System;
    using GenericRepository;
    
    /// <summary>
    /// A Doctrine Ships fit component.
    /// </summary>
    public class Component : EntityBase
    {
        public int ComponentId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public double Volume { get; set; }
    }
}
