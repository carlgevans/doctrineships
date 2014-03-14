namespace DoctrineShips.Entities
{
    using System;
    using GenericRepository;
    
    /// <summary>
    /// A Doctrine Ships contract.
    /// </summary>
    public class Contract : EntityBase
    {
        public long ContractId { get; set; }             
        public int StartStationId { get; set; }      
        public int SolarSystemId { get; set; }
        public long AssigneeId { get; set; }          
        public string StartStationName { get; set; } 
        public string SolarSystemName { get; set; }
        public ContractStatus Status { get; set; }
        public ContractType Type { get; set; }            
        public ContractAvailability Availability { get; set; }
        public string Title { get; set; }
        public bool ForCorp { get; set; }      
        public double Price { get; set; }               
        public double Volume { get; set; }              
        public DateTime DateIssued { get; set; }        
        public DateTime DateExpired { get; set; }
        public long IssuerCorpId { get; set; }
        public long IssuerId { get; set; }
        public int ShipFitId { get; set; }
        public bool IsValid { get; set; }
        public virtual ShipFit ShipFit { get; set; }
    }
}
