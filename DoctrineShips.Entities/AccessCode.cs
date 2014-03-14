namespace DoctrineShips.Entities
{
    using System;
    using System.Collections.Generic;
    using GenericRepository;
    
    /// <summary>
    /// A Doctrine Ships access code with role.
    /// </summary>
    public class AccessCode : EntityBase
    {
        public int AccessCodeId { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }
        public string Salt { get; set; }
        public string Key { get; set; }
        public Role Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastLogin { get; set; }
        public virtual Account Account { get; set; }
    }
}
