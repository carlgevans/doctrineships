namespace Tools
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Logs notifications, warnings or errors.
    /// </summary>
    public class SystemLogger : ISystemLogger
    {
        private readonly ISystemLoggerStore logStore;

        public SystemLogger(ISystemLoggerStore logStore)
        {
            this.logStore = logStore;
        }

        /// <summary>
        /// Submits a message to the system logger.
        /// </summary>
        /// <param name="messageToLog">The message to log.</param>
        /// <param name="logLevel">The level of the log message (0 - Error, 1 - Warning, 2 - Notification).</param>
        /// <param name="type">Optional type such as the method name. The default value is 'Message'.</param>
        /// <param name="source">Optional source information such as the method name. The default value is 'Unknown'.</param>
        public void LogMessage(string messageToLog, int logLevel, string type = "Message", string source = "Unknown")
        {
            // Construct log message.
            LogMessage logMessage = new LogMessage() {
                Type = type,
                Message = messageToLog,
                Level = logLevel,
                Source = source,
                DateLogged = DateTime.UtcNow
            };

            // Add the message to the store.
            logStore.AddLogMessage(logMessage);
            logStore.Save();

            // Also output to the debug console.
            System.Diagnostics.Debug.Write("Type: " + logMessage.Type + " - Source: " + logMessage.Source + " - Message: " + logMessage.Message + "\n");
        }
    }
}
       