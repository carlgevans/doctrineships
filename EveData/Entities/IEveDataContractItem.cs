namespace EveData.Entities
{
    using System;

    public interface IEveDataContractItem
    {
        long RecordId { get; set; }
        int TypeId { get; set; }
        long Quantity { get; set; }
        int RawQuantity { get; set; }
        int Singleton { get; set; }
        int Included { get; set; }
    }
}
