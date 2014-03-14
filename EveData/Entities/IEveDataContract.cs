namespace EveData.Entities
{
    using System;

    public interface IEveDataContract
    {
        long AcceptorId { get; set; }
        long AssigneeId { get; set; }
        EveDataContractAvailability Availability { get; set; }
        double Buyout { get; set; }
        double Collateral { get; set; }
        long ContractId { get; set; }
        DateTime DateAccepted { get; set; }
        DateTime DateCompleted { get; set; }
        DateTime DateExpired { get; set; }
        DateTime DateIssued { get; set; }
        int EndStationId { get; set; }
        bool ForCorp { get; set; }
        long IssuerCorpId { get; set; }
        long IssuerId { get; set; }
        int NumDays { get; set; }
        double Price { get; set; }
        double Reward { get; set; }
        int StartStationId { get; set; }
        EveDataContractStatus Status { get; set; }
        string Title { get; set; }
        EveDataContractType Type { get; set; }
        double Volume { get; set; }
    }
}
