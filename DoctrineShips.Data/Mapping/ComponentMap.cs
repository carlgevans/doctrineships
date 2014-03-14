namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;
    
    public class ComponentMap : EntityTypeConfiguration<Component>
    {
        public ComponentMap()
        {
            // Primary Key
            this.HasKey(t => t.ComponentId);

            // Properties
            this.Property(t => t.ComponentId)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired();

            this.Property(t => t.ImageUrl)
                .IsRequired();

            this.Property(t => t.Volume)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Components");
            this.Property(t => t.ComponentId).HasColumnName("ComponentId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ImageUrl).HasColumnName("ImageUrl");
            this.Property(t => t.Volume).HasColumnName("Volume");
        }
    }
}
