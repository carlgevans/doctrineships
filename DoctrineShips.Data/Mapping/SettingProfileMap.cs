namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;

    public class SettingProfileMap : EntityTypeConfiguration<SettingProfile>
    {
        public SettingProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.SettingProfileId);

            // Properties
            this.Property(t => t.SettingProfileId)
                .IsRequired();

            this.Property(t => t.AccountId)
                .IsRequired();

            this.Property(t => t.BuyStationId)
                .IsRequired();

            this.Property(t => t.SellStationId)
                .IsRequired();

            this.Property(t => t.BrokerPercentage)
                .IsRequired();

            this.Property(t => t.SalesTaxPercentage)
                .IsRequired();

            this.Property(t => t.ContractMarkupPercentage)
                .IsRequired();

            this.Property(t => t.ContractBrokerFee)
                .IsRequired();

            this.Property(t => t.ShippingCostPerM3)
                .IsRequired();

            this.Property(t => t.TwitterHandle)
                .IsRequired();

            this.Property(t => t.AlertThreshold)
                .IsRequired();

            this.Property(t => t.ShortUrlExpiryHours)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("SettingProfiles");
            this.Property(t => t.SettingProfileId).HasColumnName("SettingProfileId");
            this.Property(t => t.AccountId).HasColumnName("AccountId");
            this.Property(t => t.BuyStationId).HasColumnName("BuyStationId");
            this.Property(t => t.SellStationId).HasColumnName("SellStationId");
            this.Property(t => t.BrokerPercentage).HasColumnName("BrokerPercentage");
            this.Property(t => t.SalesTaxPercentage).HasColumnName("SalesTaxPercentage");
            this.Property(t => t.ContractMarkupPercentage).HasColumnName("ContractMarkupPercentage");
            this.Property(t => t.ContractBrokerFee).HasColumnName("ContractBrokerFee");
            this.Property(t => t.ShippingCostPerM3).HasColumnName("ShippingCostPerM3");
            this.Property(t => t.TwitterHandle).HasColumnName("TwitterHandle");
            this.Property(t => t.AlertThreshold).HasColumnName("AlertThreshold");
            this.Property(t => t.ShortUrlExpiryHours).HasColumnName("ShortUrlExpiryHours");
        }
    }
}
