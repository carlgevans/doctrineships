namespace DoctrineShips.Entities
{
    using System;
    using GenericRepository;

    /// <summary>
    /// A Doctrine Ships notification recipient.
    /// </summary>
    public class NotificationRecipient : EntityBase
    {
        public int NotificationRecipientId { get; set; }
        public int AccountId { get; set; }
        public string TwitterHandle { get; set; }
        public string Description { get; set; }
        public bool ReceivesDailySummary { get; set; }
        public bool ReceivesAlerts { get; set; }
        public int AlertIntervalHours { get; set; }
        public NotificationMethod Method { get; set; }
        public DateTime LastAlert { get; set; }
        public virtual Account Account { get; set; }
    }
}
