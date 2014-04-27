namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;
    
    public class AccessCodeMap : EntityTypeConfiguration<AccessCode>
    {
        public AccessCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.AccessCodeId);

            // Properties
            this.Property(t => t.AccessCodeId)
                .IsRequired();

            this.Property(t => t.AccountId)
                .IsRequired();

            this.Property(t => t.Description)
                .IsRequired();

            this.Property(t => t.Salt)
                .IsRequired();

            this.Property(t => t.Key)
                .IsRequired();

            this.Property(t => t.Role)
                .IsRequired();

            this.Property(t => t.IsActive)
                .IsRequired();

            this.Property(t => t.LastLogin)
                .IsRequired();
            
            this.Property(t => t.Data)
                .IsRequired();
            
            this.Property(t => t.DateCreated)
                .IsRequired();

            this.Property(t => t.DateExpires)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("AccessCodes");
            this.Property(t => t.AccessCodeId).HasColumnName("AccessCodeId");
            this.Property(t => t.AccountId).HasColumnName("AccountId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Salt).HasColumnName("Salt");
            this.Property(t => t.Key).HasColumnName("Key");
            this.Property(t => t.Role).HasColumnName("Role");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.LastLogin).HasColumnName("LastLogin");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            this.Property(t => t.DateExpires).HasColumnName("DateExpires");
        }
    }
}
