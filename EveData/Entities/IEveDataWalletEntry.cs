namespace EveData.Entities
{
    using System;

    public interface IEveDataWalletEntry
    {
        double Amount { get; set; }
        int ArgId1 { get; set; }
        string ArgName1 { get; set; }
        double Balance { get; set; }
        DateTime Date { get; set; }
        int OwnerId1 { get; set; }
        int OwnerId2 { get; set; }
        string OwnerName1 { get; set; }
        string OwnerName2 { get; set; }
        string Reason { get; set; }
        long RefId { get; set; }
        int RefTypeId { get; set; }
        string TaxAmount { get; set; }
        string TaxReceiverId { get; set; }
    }
}
