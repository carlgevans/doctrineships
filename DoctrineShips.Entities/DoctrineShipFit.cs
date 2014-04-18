namespace DoctrineShips.Entities
{
    using System;
    using System.Collections.Generic;
    using GenericRepository;
    
    /// <summary>
    /// A Doctrine Ships doctrine ship fit list.
    /// </summary>
    public class DoctrineShipFit : EntityBase
    {
        public int DoctrineShipFitId { get; set; }
        public int DoctrineId { get; set; }
        public int ShipFitId { get; set; }
        public virtual Doctrine Doctrine { get; set; }
        public virtual ShipFit ShipFit { get; set; }
    }
}
