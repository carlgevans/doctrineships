namespace DoctrineShips.Entities
{
    public enum ContractAvailability
    {
        Public = 0,
        Private = 1,
    }

    public enum ContractStatus
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

    public enum ContractType
    {
        ItemExchange = 0,
        Courier = 1,
        Auction = 2,
        Loan = 3,
    }

    public enum SlotType
    {
        Other = 0,
        Hull = 1,
        High = 2,
        Medium = 3,
        Low = 4,
        Rig = 5,
        Subsystem = 6,
        Drone = 7,
        Cargo = 8,
    }

    public enum Role
    {
        None = 0,
        User = 1,
        Admin = 2,
        SiteAdmin = 3,
    }

    public enum NotificationMethod
    {
        DirectMessage = 0,
        Tweet = 1,
    }
}