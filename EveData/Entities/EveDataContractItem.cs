namespace EveData.Entities
{
    using System;

    public sealed class EveDataContractItem : IEveDataContractItem
    {
        public long RecordId { get; set; }
        public int TypeId { get; set; }
        public long Quantity { get; set; }
        public int RawQuantity { get; set; }
        public int Singleton { get; set; }
        public int Included { get; set; }
    }
}
