namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;
    
    public class DoctrineMap : EntityTypeConfiguration<Doctrine>
    {
        public DoctrineMap()
        {
            // Primary Key
            this.HasKey(t => t.DoctrineId);

            // Properties
            this.Property(t => t.DoctrineId)
                .IsRequired();

            this.Property(t => t.AccountId)
                .IsRequired();

            this.Property(t => t.Name)
                .IsRequired();

            this.Property(t => t.Description)
                .IsRequired();

            this.Property(t => t.ImageUrl)
                .IsRequired();

            this.Property(t => t.IsOfficial)
                .IsRequired();

            this.Property(t => t.IsDormant)
                .IsRequired();

            this.Property(t => t.LastUpdate)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Doctrines");
            this.Property(t => t.DoctrineId).HasColumnName("DoctrineId");
            this.Property(t => t.AccountId).HasColumnName("AccountId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.ImageUrl).HasColumnName("ImageUrl");
            this.Property(t => t.IsOfficial).HasColumnName("IsOfficial");
            this.Property(t => t.IsDormant).HasColumnName("IsDormant");
            this.Property(t => t.LastUpdate).HasColumnName("LastUpdate");

            // Relationships
            this.HasMany(t => t.DoctrineShipFits)
                .WithRequired(x => x.Doctrine)
                .HasForeignKey(x => x.DoctrineId)
                .WillCascadeOnDelete(true);
        }
    }
}
