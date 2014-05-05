namespace DoctrineShips.Entities
{
    using System;
    using GenericRepository;

    /// <summary>
    /// A Doctrine Ships application setting profile.
    /// </summary>
    public class SettingProfile : EntityBase
    {
        public int SettingProfileId { get; set; }
        public int AccountId { get; set; }
        public int BuyStationId { get; set; }
        public int SellStationId { get; set; }
        public double BrokerPercentage { get; set; }
        public double SalesTaxPercentage { get; set; }
        public double ContractMarkupPercentage { get; set; }
        public double ContractBrokerFee { get; set; }
        public double ShippingCostPerM3 { get; set; }
        public string TwitterHandle { get; set; }
        public int AlertThreshold { get; set; }
        public int ShortUrlExpiryHours { get; set; }
    }
}
