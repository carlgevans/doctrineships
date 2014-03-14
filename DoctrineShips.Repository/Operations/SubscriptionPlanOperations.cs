namespace DoctrineShips.Repository.Operations
{
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using GenericRepository;

    internal sealed class SubscriptionPlanOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal SubscriptionPlanOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteSubscriptionPlan(int subscriptionPlanId)
        {
            this.unitOfWork.Repository<SubscriptionPlan>().Delete(subscriptionPlanId);
        }

        internal void UpdateSubscriptionPlan(SubscriptionPlan subscriptionPlan)
        {
            subscriptionPlan.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<SubscriptionPlan>().Update(subscriptionPlan);
        }

        internal SubscriptionPlan AddSubscriptionPlan(SubscriptionPlan subscriptionPlan)
        {
            this.unitOfWork.Repository<SubscriptionPlan>().Insert(subscriptionPlan);
            return subscriptionPlan;
        }

        internal SubscriptionPlan CreateSubscriptionPlan(SubscriptionPlan subscriptionPlan)
        {
            subscriptionPlan.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<SubscriptionPlan>().Insert(subscriptionPlan);
            return subscriptionPlan;
        }

        internal SubscriptionPlan GetSubscriptionPlan(int subscriptionPlanId)
        {
            return this.unitOfWork.Repository<SubscriptionPlan>().Find(subscriptionPlanId);
        }

        internal SubscriptionPlan GetSubscriptionPlanReadOnly(int subscriptionPlanId)
        {
            return this.unitOfWork.Repository<SubscriptionPlan>()
                                .Query()
                                .TrackingOff()
                                .Filter(x => x.SubscriptionPlanId == subscriptionPlanId)
                                .Get()
                                .FirstOrDefault();
        }

        internal IEnumerable<SubscriptionPlan> GetSubscriptionPlans()
        {
            var subscriptionPlans = this.unitOfWork.Repository<SubscriptionPlan>()
                              .Query()
                              .Get()
                              .OrderBy(x => x.SubscriptionPlanId)
                              .ToList();

            return subscriptionPlans;
        }
    }
}