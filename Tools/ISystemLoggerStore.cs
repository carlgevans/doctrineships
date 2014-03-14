namespace Tools
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An interface for a system logger store.
    /// </summary>
    public interface ISystemLoggerStore
    {
        LogMessage AddLogMessage(LogMessage logMessage);
        LogMessage GetLogMessage(int logMessageId);
        IEnumerable<LogMessage> GetLogMessages(TimeSpan logPeriod);
        void ClearLog();
        void Save();
    }
}