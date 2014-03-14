namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;
    
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.CustomerId);

            // Properties
            this.Property(t => t.CustomerId)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired();

            this.Property(t => t.ImageUrl)
                .IsRequired();

            this.Property(t => t.IsCorp)
                .IsRequired();

            this.Property(t => t.LastRefresh)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Customers");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ImageUrl).HasColumnName("ImageUrl");
            this.Property(t => t.IsCorp).HasColumnName("IsCorp");
            this.Property(t => t.LastRefresh).HasColumnName("LastRefresh");
        }
    }
}
