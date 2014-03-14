namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;

    public class ShipFitMap : EntityTypeConfiguration<ShipFit>
    {
        public ShipFitMap()
        {
            // Primary Key
            this.HasKey(t => t.ShipFitId);
            
            // Properties
            this.Property(t => t.ShipFitId)
                .IsRequired();

            this.Property(t => t.AccountId)
                .IsRequired();

            this.Property(t => t.HullId)
                .IsRequired();

            this.Property(t => t.ThumbnailImageUrl)
                .IsRequired();

            this.Property(t => t.RenderImageUrl)
                .IsRequired();

            this.Property(t => t.Hull)
                .IsRequired();

            this.Property(t => t.Name)
                .IsRequired();

            this.Property(t => t.Role)
                .IsRequired();

            this.Property(t => t.ContractsAvailable)
                .IsRequired();

            this.Property(t => t.IsMonitored)
                .IsRequired();

            this.Property(t => t.FitPackagedVolume)
                .IsRequired();

            this.Property(t => t.BuyPrice)
                .IsRequired();

            this.Property(t => t.SellPrice)
                .IsRequired();

            this.Property(t => t.ShippingCost)
                .IsRequired();

            this.Property(t => t.ContractReward)
                .IsRequired();

            this.Property(t => t.BuyOrderProfit)
                .IsRequired();

            this.Property(t => t.SellOrderProfit)
                .IsRequired();

            this.Property(t => t.FittingString)
                .IsRequired();

            this.Property(t => t.LastPriceRefresh)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ShipFits");
            this.Property(t => t.ShipFitId).HasColumnName("ShipFitId");
            this.Property(t => t.AccountId).HasColumnName("AccountId");
            this.Property(t => t.HullId).HasColumnName("HullId");
            this.Property(t => t.ThumbnailImageUrl).HasColumnName("ThumbnailUrl");
            this.Property(t => t.RenderImageUrl).HasColumnName("RenderImageUrl");
            this.Property(t => t.Hull).HasColumnName("Hull");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Role).HasColumnName("Role");
            this.Property(t => t.ContractsAvailable).HasColumnName("ContractsAvailable");
            this.Property(t => t.IsMonitored).HasColumnName("IsMonitored");
            this.Property(t => t.FitPackagedVolume).HasColumnName("FitPackagedVolume");
            this.Property(t => t.BuyPrice).HasColumnName("BuyPrice");
            this.Property(t => t.SellPrice).HasColumnName("SellPrice");
            this.Property(t => t.ShippingCost).HasColumnName("ShippingCost");
            this.Property(t => t.ContractReward).HasColumnName("ContractReward");
            this.Property(t => t.BuyOrderProfit).HasColumnName("BuyOrderProfit");
            this.Property(t => t.SellOrderProfit).HasColumnName("SellOrderProfit");
            this.Property(t => t.FittingString).HasColumnName("FittingString");
            this.Property(t => t.LastPriceRefresh).HasColumnName("LastPriceRefresh");

            // Relationships
            this.HasMany(t => t.ShipFitComponents)
                .WithRequired(x => x.ShipFit)
                .HasForeignKey(x => x.ShipFitId)
                .WillCascadeOnDelete(true);
        }
    }
}
