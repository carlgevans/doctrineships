namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;
    
    public class ShipFitComponentMap : EntityTypeConfiguration<ShipFitComponent>
    {
        public ShipFitComponentMap()
        {
            // Primary Key
            this.HasKey(t => t.ShipFitComponentId);

            // Properties
            this.Property(t => t.ShipFitComponentId)
                .IsRequired();

            this.Property(t => t.ShipFitId)
                .IsRequired();

            this.Property(t => t.ComponentId)
                .IsRequired();

            this.Property(t => t.BuyPrice)
                .IsRequired();

            this.Property(t => t.SellPrice)
                .IsRequired();

            this.Property(t => t.Quantity)
                .IsRequired();

            this.Property(t => t.SlotType)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ShipFitComponents");
            this.Property(t => t.ShipFitComponentId).HasColumnName("ShipFitComponentId");
            this.Property(t => t.ShipFitId).HasColumnName("ShipFitId");
            this.Property(t => t.ComponentId).HasColumnName("ComponentId");
            this.Property(t => t.BuyPrice).HasColumnName("BuyPrice");
            this.Property(t => t.SellPrice).HasColumnName("SellPrice");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.SlotType).HasColumnName("SlotType");

            this.HasRequired(t => t.Component)
                .WithMany()
                .HasForeignKey(x => x.ComponentId)
                .WillCascadeOnDelete(false);
        }
    }
}
