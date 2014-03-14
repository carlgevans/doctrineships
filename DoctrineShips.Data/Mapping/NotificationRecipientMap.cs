namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;

    public class NotificationRecipientMap : EntityTypeConfiguration<NotificationRecipient>
    {
        public NotificationRecipientMap()
        {
            // Primary Key
            this.HasKey(t => t.NotificationRecipientId);

            // Properties
            this.Property(t => t.NotificationRecipientId)
                .IsRequired();

            this.Property(t => t.AccountId)
                .IsRequired();

            this.Property(t => t.TwitterHandle)
                .IsRequired();

            this.Property(t => t.Description)
                .IsRequired();

            this.Property(t => t.ReceivesDailySummary)
                .IsRequired();

            this.Property(t => t.ReceivesAlerts)
                .IsRequired();

            this.Property(t => t.AlertIntervalHours)
                .IsRequired();

            this.Property(t => t.Method)
                .IsRequired();

            this.Property(t => t.LastAlert)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("NotificationRecipients");
            this.Property(t => t.NotificationRecipientId).HasColumnName("NotificationRecipientId");
            this.Property(t => t.AccountId).HasColumnName("AccountId");
            this.Property(t => t.TwitterHandle).HasColumnName("TwitterHandle");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.ReceivesDailySummary).HasColumnName("ReceivesDailySummary");
            this.Property(t => t.ReceivesAlerts).HasColumnName("ReceivesAlerts");
            this.Property(t => t.AlertIntervalHours).HasColumnName("AlertIntervalHours");
            this.Property(t => t.Method).HasColumnName("Method");
            this.Property(t => t.LastAlert).HasColumnName("LastAlert");
        }
    }
}
