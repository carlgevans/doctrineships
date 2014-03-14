namespace Tools
{
    using GenericRepository;
    using System;

    /// <summary>
    /// A system logger message.
    /// </summary>
    public class LogMessage : EntityBase
    {
        public int LogMessageId { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public int Level { get; set; }
        public DateTime DateLogged { get; set; }
    }
}
