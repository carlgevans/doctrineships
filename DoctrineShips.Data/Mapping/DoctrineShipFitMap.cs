namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;
    
    public class DoctrineShipFitMap : EntityTypeConfiguration<DoctrineShipFit>
    {
        public DoctrineShipFitMap()
        {
            // Primary Key
            this.HasKey(t => t.DoctrineShipFitId);

            // Properties
            this.Property(t => t.DoctrineShipFitId)
                .IsRequired();

            this.Property(t => t.DoctrineId)
                .IsRequired();

            this.Property(t => t.ShipFitId)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("DoctrineShipFits");
            this.Property(t => t.DoctrineShipFitId).HasColumnName("DoctrineShipFitId");
            this.Property(t => t.DoctrineId).HasColumnName("DoctrineId");
            this.Property(t => t.ShipFitId).HasColumnName("ShipFitId");

            // Relationships
            this.HasRequired(t => t.ShipFit)
                .WithMany()
                .HasForeignKey(x => x.ShipFitId)
                .WillCascadeOnDelete(false);
        }
    }
}
