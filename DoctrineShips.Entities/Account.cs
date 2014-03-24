namespace DoctrineShips.Entities
{
    using System;
    using System.Collections.Generic;
    using GenericRepository;
    
    /// <summary>
    /// A Doctrine Ships account.
    /// </summary>
    public class Account : EntityBase
    {
        public int AccountId { get; set; }
        public string Description { get; set; }
        public int SettingProfileId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual SettingProfile SettingProfile { get; set; }
        public virtual ICollection<AccessCode> AccessCodes { get; set; }
        public virtual ICollection<ShipFit> ShipFits { get; set; }
        public virtual ICollection<SalesAgent> SalesAgents { get; set; }
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }
    }
}
