namespace DoctrineShips.Entities
{
    using System;
    using System.Collections.Generic;
    using GenericRepository;
    
    /// <summary>
    /// A Doctrine Ships fit.
    /// </summary>
    public class ShipFit : EntityBase
    {
        public int ShipFitId { get; set; }
        public int AccountId { get; set; }
        public int HullId { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public string RenderImageUrl { get; set; }
        public string Hull { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int ContractsAvailable { get; set; }
        public bool IsMonitored { get; set; }
        public double FitPackagedVolume { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public double ShippingCost { get; set; }
        public double ContractReward { get; set; }
        public double BuyOrderProfit { get; set; }
        public double SellOrderProfit { get; set; }
        public string FittingString { get; set; }
        public string FittingHash { get; set; }
        public string Notes { get; set; }
        public DateTime LastPriceRefresh { get; set; }
        public virtual ICollection<ShipFitComponent> ShipFitComponents { get; set; }
        public virtual Account Account { get; set; }
    }
}
