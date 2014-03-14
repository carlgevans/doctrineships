namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;
    
    public class ArticleMap : EntityTypeConfiguration<Article>
    {
        public ArticleMap()
        {
            // Primary Key
            this.HasKey(t => t.ArticleId);

            // Properties
            this.Property(t => t.ArticleId)
                .IsRequired();

            this.Property(t => t.Title)
                .IsRequired();

            this.Property(t => t.Summary)
                .IsRequired();

            this.Property(t => t.Content)
                .IsRequired();

            this.Property(t => t.Author)
                .IsRequired();

            this.Property(t => t.IsUnlisted)
                .IsRequired();

            this.Property(t => t.DateCreated)
                .IsRequired();

            this.Property(t => t.LastUpdated)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Articles");
            this.Property(t => t.ArticleId).HasColumnName("ArticleId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Summary).HasColumnName("Summary");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Author).HasColumnName("Author");
            this.Property(t => t.IsUnlisted).HasColumnName("IsUnlisted");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            this.Property(t => t.LastUpdated).HasColumnName("LastUpdated");
        }
    }
}
