namespace EveData.Entities
{
    using System;

    public sealed class EveDataWalletEntry : IEveDataWalletEntry
    {
        public double Amount { get; set; }
        public int ArgId1 { get; set; }
        public string ArgName1 { get; set; }
        public double Balance { get; set; }
        public DateTime Date { get; set; }
        public int OwnerId1 { get; set; }
        public int OwnerId2 { get; set; }
        public string OwnerName1 { get; set; }
        public string OwnerName2 { get; set; }
        public string Reason { get; set; }
        public long RefId { get; set; }
        public int RefTypeId { get; set; }
        public string TaxAmount { get; set; }
        public string TaxReceiverId { get; set; }
    }
}
