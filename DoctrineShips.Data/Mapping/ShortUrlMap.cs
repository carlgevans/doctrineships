namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;

    public class ShortUrlMap : EntityTypeConfiguration<ShortUrl>
    {
        public ShortUrlMap()
        {
            // Primary Key
            this.HasKey(t => t.ShortUrlId);

            // Properties
            this.Property(t => t.ShortUrlId)
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(6)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LongUrl)
                .IsRequired();

            this.Property(t => t.DateCreated)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ShortUrls");
            this.Property(t => t.ShortUrlId).HasColumnName("ShortUrlId");
            this.Property(t => t.LongUrl).HasColumnName("LongUrl");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
        }
    }
}
