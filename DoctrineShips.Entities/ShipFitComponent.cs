namespace DoctrineShips.Entities
{
    using System;
    using System.Collections.Generic;
    using GenericRepository;
    
    /// <summary>
    /// A Doctrine Ships fit component list.
    /// </summary>
    public class ShipFitComponent : EntityBase
    {
        public int ShipFitComponentId { get; set; }
        public int ShipFitId { get; set; }
        public int ComponentId { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public long Quantity { get; set; }
        public SlotType SlotType { get; set; }
        public virtual ShipFit ShipFit { get; set; }
        public virtual Component Component { get; set; }
    }
}
