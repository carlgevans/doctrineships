namespace EveData.Entities
{
    using System;

    public sealed class EveDataContract : IEveDataContract
    {
        public long AcceptorId { get; set; }
        public long AssigneeId { get; set; }
        public EveDataContractAvailability Availability { get; set; }
        public double Buyout { get; set; }
        public double Collateral { get; set; }
        public long ContractId { get; set; }
        public DateTime DateAccepted { get; set; }
        public DateTime DateCompleted { get; set; }
        public DateTime DateExpired { get; set; }
        public DateTime DateIssued { get; set; }
        public int EndStationId { get; set; }
        public bool ForCorp { get; set; }
        public long IssuerCorpId { get; set; }
        public long IssuerId { get; set; }
        public int NumDays { get; set; }
        public double Price { get; set; }
        public double Reward { get; set; }
        public int StartStationId { get; set; }
        public EveDataContractStatus Status { get; set; }
        public string Title { get; set; }
        public EveDataContractType Type { get; set; }
        public double Volume { get; set; }
    }
}
