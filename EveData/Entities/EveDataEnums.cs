namespace EveData.Entities
{
    public enum EveDataImageType 
    { 
        Character, 
        Corporation, 
        Alliance, 
        InventoryType, 
        Render 
    }

    public enum EveDataContractAvailability
    {
        Public = 0,
        Private = 1,
    }

    public enum EveDataContractStatus
    {
        Outstanding = 0,
        Deleted = 1,
        Completed = 2,
        Failed = 3,
        InProgress = 4,
        CompletedByIssuer = 5,
        CompletedByContractor = 6,
        Cancelled = 7,
        Rejected = 8,
        Reversed = 9,
    }

    public enum EveDataContractType
    {
        ItemExchange = 0,
        Courier = 1,
        Auction = 2,
        Loan = 3,
    }

    public enum EveDataApiKeyType
    {
        Account = 0,
        Character = 1,
        Corporation = 2,
    }
}
