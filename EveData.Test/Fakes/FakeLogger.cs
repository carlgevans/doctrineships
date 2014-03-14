using Tools;

namespace EveData.Test.Fakes
{
    public class FakeLogger : ISystemLogger
    {
        public void LogMessage(string logMessage, int logLevel, string type = "Message", string source = "Unknown")
        {
            System.Diagnostics.Debug.WriteLine(logMessage);
        }
    }
}
