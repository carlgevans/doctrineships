namespace DoctrineShips.Repository.Operations
{
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using GenericRepository;

    internal sealed class NotificationRecipientOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal NotificationRecipientOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteNotificationRecipient(int notificationRecipientId)
        {
            this.unitOfWork.Repository<NotificationRecipient>().Delete(notificationRecipientId);
        }

        internal void UpdateNotificationRecipient(NotificationRecipient notificationRecipient)
        {
            notificationRecipient.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<NotificationRecipient>().Update(notificationRecipient);
        }

        internal NotificationRecipient AddNotificationRecipient(NotificationRecipient notificationRecipient)
        {
            this.unitOfWork.Repository<NotificationRecipient>().Insert(notificationRecipient);
            return notificationRecipient;
        }

        internal NotificationRecipient CreateNotificationRecipient(NotificationRecipient notificationRecipient)
        {
            notificationRecipient.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<NotificationRecipient>().Insert(notificationRecipient);
            return notificationRecipient;
        }

        internal NotificationRecipient GetNotificationRecipient(int notificationRecipientId)
        {
            return this.unitOfWork.Repository<NotificationRecipient>().Find(notificationRecipientId);
        }

        internal NotificationRecipient GetNotificationRecipientReadOnly(int notificationRecipientId)
        {
            return this.unitOfWork.Repository<NotificationRecipient>()
                                            .Query()
                                            .TrackingOff()
                                            .Filter(x => x.NotificationRecipientId == notificationRecipientId)
                                            .Get()
                                            .FirstOrDefault();
        }
        
        internal IEnumerable<NotificationRecipient> GetNotificationRecipients(int accountId)
        {
            var notificationRecipients = this.unitOfWork.Repository<NotificationRecipient>()
                              .Query()
                              .Filter(x => x.AccountId == accountId)
                              .Get()
                              .OrderBy(x => x.Description)
                              .ToList();

            return notificationRecipients;
        }
    }
}