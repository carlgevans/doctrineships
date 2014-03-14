namespace DoctrineShips.Entities
{
    using System;
    using GenericRepository;

    /// <summary>
    /// A Doctrine Ships sales agent. An agent can be a corporation or an indivudual character.
    /// </summary>
    public class SalesAgent : EntityBase
    {
        public int SalesAgentId { get; set; }
        public int AccountId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public int ApiId { get; set; }
        public string ApiKey { get; set; }
        public bool IsCorp { get; set; }
        public bool IsActive { get; set; }
        public int ContractsAvailable { get; set; }
        public DateTime LastForce { get; set; }
        public DateTime LastContractRefresh { get; set; }
        public virtual Account Account { get; set; }
    }
}
