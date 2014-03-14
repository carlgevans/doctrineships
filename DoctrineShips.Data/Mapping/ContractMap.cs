namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;
    
    public class ContractMap : EntityTypeConfiguration<Contract>
    {
        public ContractMap()
        {
            // Primary Key
            this.HasKey(t => t.ContractId);

            // Properties
            this.Property(t => t.ContractId)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.StartStationId)
                .IsRequired();

            this.Property(t => t.SolarSystemId)
                .IsRequired();

            this.Property(t => t.AssigneeId)
                .IsRequired();

            this.Property(t => t.StartStationName)
                .IsRequired();

            this.Property(t => t.SolarSystemName)
                .IsRequired();

            this.Property(t => t.Status)
                .IsRequired();

            this.Property(t => t.Type)
                .IsRequired();

            this.Property(t => t.Availability)
                .IsRequired();

            this.Property(t => t.Title)
                .IsRequired();

            this.Property(t => t.ForCorp)
                .IsRequired();

            this.Property(t => t.Price)
                .IsRequired();

            this.Property(t => t.Volume)
                .IsRequired();

            this.Property(t => t.DateIssued)
                .IsRequired();

            this.Property(t => t.DateExpired)
                .IsRequired();

            this.Property(t => t.IssuerCorpId)
                .IsRequired();

            this.Property(t => t.IssuerId)
                .IsRequired();

            this.Property(t => t.ShipFitId)
                .IsRequired();

            this.Property(t => t.IsValid)
               .IsRequired();

            // Table & Column Mappings
            this.ToTable("Contracts");
            this.Property(t => t.ContractId).HasColumnName("ContractId");
            this.Property(t => t.StartStationId).HasColumnName("StartStationId");
            this.Property(t => t.SolarSystemId).HasColumnName("SolarSystemId");
            this.Property(t => t.AssigneeId).HasColumnName("AssigneeId");
            this.Property(t => t.StartStationName).HasColumnName("StartStationName");
            this.Property(t => t.SolarSystemName).HasColumnName("SolarSystemName");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Availability).HasColumnName("Availability");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.ForCorp).HasColumnName("ForCorp");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.DateIssued).HasColumnName("DateIssued");
            this.Property(t => t.DateExpired).HasColumnName("DateExpired");
            this.Property(t => t.IssuerCorpId).HasColumnName("IssuerCorpId");
            this.Property(t => t.IssuerId).HasColumnName("IssuerId");
            this.Property(t => t.ShipFitId).HasColumnName("ShipFitId");
            this.Property(t => t.IsValid).HasColumnName("IsValid");

            // Relationships
            this.HasRequired(t => t.ShipFit)
                .WithMany()
                .HasForeignKey(x => x.ShipFitId)
                .WillCascadeOnDelete(false);
        }
    }
}
