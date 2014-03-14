namespace Tools
{
    /// <summary>
    /// An interface for a system logger.
    /// </summary>
    public interface ISystemLogger
    {
        /// <summary>
        /// Submits a message to the system logger.
        /// </summary>
        /// <param name="logMessage">The message to log.</param>
        /// <param name="logLevel">The level of the log message (0 - Error, 1 - Warning, 2 - Notification).</param>
        /// <param name="type">Optional type such as the method name. The default value is 'Message'.</param>
        /// <param name="source">Optional source information such as the method name.</param>
        void LogMessage(string logMessage, int logLevel, string type = "Message", string source = "Unknown");
    }
}