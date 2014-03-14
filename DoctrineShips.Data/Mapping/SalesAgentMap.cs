namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;
    
    public class SalesAgentMap : EntityTypeConfiguration<SalesAgent>
    {
        public SalesAgentMap()
        {
            // Primary Key
            this.HasKey(t => t.SalesAgentId);

            // Properties
            this.Property(t => t.SalesAgentId)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AccountId)
                .IsRequired();

            this.Property(t => t.ImageUrl)
                .IsRequired();

            this.Property(t => t.Name)
                .IsRequired();

            this.Property(t => t.ApiId)
                .IsRequired();

            this.Property(t => t.ApiKey)
                .IsRequired();

            this.Property(t => t.IsCorp)
                .IsRequired();

            this.Property(t => t.IsActive)
                .IsRequired();

            this.Property(t => t.ContractsAvailable)
                .IsRequired();

            this.Property(t => t.LastForce)
                .IsRequired();

            this.Property(t => t.LastContractRefresh)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("SalesAgents");
            this.Property(t => t.SalesAgentId).HasColumnName("SalesAgentId");
            this.Property(t => t.AccountId).HasColumnName("AccountId");
            this.Property(t => t.ImageUrl).HasColumnName("ImageUrl");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ApiId).HasColumnName("ApiId");
            this.Property(t => t.ApiKey).HasColumnName("ApiKey");
            this.Property(t => t.IsCorp).HasColumnName("IsCorp");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.ContractsAvailable).HasColumnName("ContractsAvailable");
            this.Property(t => t.LastForce).HasColumnName("LastForce");
            this.Property(t => t.LastContractRefresh).HasColumnName("LastContractRefresh");
        }
    }
}
