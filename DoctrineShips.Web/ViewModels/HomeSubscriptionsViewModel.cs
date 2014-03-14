namespace DoctrineShips.Web.ViewModels
{
    using System.Collections.Generic;
    using DoctrineShips.Entities;

    public class HomeSubscriptionsViewModel
    {
        public IEnumerable<SubscriptionPlan> SubscriptionPlans { get; set; }
    }
}