using System;

namespace Messages.Domain
{
    /// <summary>
    /// Represents a synchronization message for domain events.
    /// </summary>
    public class SynchMessage
    {
        public required string MessageID { get; set; }
        public required short EventType { get; set; }
        public required string ID { get; init; }
        public string? CorpID { get; init; }
        public bool IsUpdatePrevious { get; init; } = false;
        public DateTime? FromDate { get; init; }
        public DateTime? ToDate { get; init; }
        public string? CloneID { get; init; }
        public long UrlID { get; init; }
        public string? ClientName { get; init; }
       
        public required string TransferConnectionString { get; set; }
    }
    
}
