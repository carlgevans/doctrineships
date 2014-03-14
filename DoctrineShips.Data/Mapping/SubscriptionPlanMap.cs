namespace DoctrineShips.Data.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DoctrineShips.Entities;
    
    public class SubscriptionPlanMap : EntityTypeConfiguration<SubscriptionPlan>
    {
        public SubscriptionPlanMap()
        {
            // Primary Key
            this.HasKey(t => t.SubscriptionPlanId);

            // Properties
            this.Property(t => t.SubscriptionPlanId)
                .IsRequired();

            this.Property(t => t.Name)
                .IsRequired();

            this.Property(t => t.Description)
                .IsRequired();

            this.Property(t => t.SalesAgentLimit)
                .IsRequired();

            this.Property(t => t.PricePerMonth)
                .IsRequired();

            this.Property(t => t.IsHidden)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("SubscriptionPlans");
            this.Property(t => t.SubscriptionPlanId).HasColumnName("SubscriptionPlanId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.SubscriptionPlanId).HasColumnName("SubscriptionPlanId");
            this.Property(t => t.SalesAgentLimit).HasColumnName("SalesAgentLimit");
            this.Property(t => t.PricePerMonth).HasColumnName("PricePerMonth");
            this.Property(t => t.IsHidden).HasColumnName("IsHidden");
        }
    }
}
