namespace DoctrineShips.Entities
{
    using GenericRepository;
    
    /// <summary>
    /// A Doctrine Ships subscription plan.
    /// </summary>
    public class SubscriptionPlan : EntityBase
    {
        public int SubscriptionPlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SalesAgentLimit { get; set; }
        public double PricePerMonth { get; set; }   
        public bool IsHidden { get; set; }
    }
}
