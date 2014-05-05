namespace DoctrineShips.Service.Entities
{
    using System;
    using GenericRepository;
    using DoctrineShips.Entities;

    /// <summary>
    /// A Doctrine Ships access token.
    /// </summary>
    public class AccessToken : EntityBase
    {
        public int AccountId { get; set; }
        public string Description { get; set; }
        public Role Role { get; set; }
        public string Data { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExpires { get; set; }
    }
}
