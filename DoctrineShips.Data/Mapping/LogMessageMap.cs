namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Tools;

    public class LogMessageMap : EntityTypeConfiguration<LogMessage>
    {
        public LogMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.LogMessageId);

            // Properties
            this.Property(t => t.LogMessageId)
                .IsRequired();

            this.Property(t => t.Type)
                .IsRequired();

            this.Property(t => t.Source)
                .IsRequired();

            this.Property(t => t.Message)
                .IsRequired();

            this.Property(t => t.Level)
                .IsRequired();

            this.Property(t => t.DateLogged)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("LogMessages");
            this.Property(t => t.LogMessageId).HasColumnName("LogMessageId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Source).HasColumnName("Source");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.DateLogged).HasColumnName("DateLogged");
        }
    }
}
