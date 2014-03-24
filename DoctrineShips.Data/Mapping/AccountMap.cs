namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;
    
    public class AccountMap : EntityTypeConfiguration<Account>
    {
        public AccountMap()
        {
            // Primary Key
            this.HasKey(t => t.AccountId);

            // Properties
            this.Property(t => t.AccountId)
                .IsRequired();

            this.Property(t => t.Description)
                .IsRequired();

            this.Property(t => t.SettingProfileId)
                .IsRequired();

            this.Property(t => t.IsActive)
                .IsRequired();

            this.Property(t => t.DateCreated)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Accounts");
            this.Property(t => t.AccountId).HasColumnName("AccountId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.SettingProfileId).HasColumnName("SettingProfileId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");

            // Relationships
            this.HasRequired(t => t.SettingProfile)
                .WithMany()
                .HasForeignKey(x => x.SettingProfileId)
                .WillCascadeOnDelete(false);

            this.HasMany(t => t.AccessCodes)
                .WithRequired(x => x.Account)
                .HasForeignKey(x => x.AccountId)
                .WillCascadeOnDelete(false);

            this.HasMany(t => t.ShipFits)
                .WithRequired(x => x.Account)
                .HasForeignKey(x => x.AccountId)
                .WillCascadeOnDelete(false);

            this.HasMany(t => t.SalesAgents)
                .WithRequired(x => x.Account)
                .HasForeignKey(x => x.AccountId)
                .WillCascadeOnDelete(false);

            this.HasMany(t => t.NotificationRecipients)
                .WithRequired(x => x.Account)
                .HasForeignKey(x => x.AccountId)
                .WillCascadeOnDelete(false);
        }
    }
}
