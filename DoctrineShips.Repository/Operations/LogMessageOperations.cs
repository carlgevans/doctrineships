namespace DoctrineShips.Repository.Operations
{
    using System.Collections.Generic;
    using System.Linq;
    using GenericRepository;
    using Tools;
    using System;

    internal sealed class LogMessageOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal LogMessageOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal LogMessage AddLogMessage(LogMessage logMessage)
        {
            this.unitOfWork.Repository<LogMessage>().Insert(logMessage);
            return logMessage;
        }

        internal LogMessage GetLogMessage(int logMessageId)
        {
            return this.unitOfWork.Repository<LogMessage>().Find(logMessageId);
        }

        internal IEnumerable<LogMessage> GetLogMessages(TimeSpan logPeriod)
        {
            DateTime dateNow = DateTime.UtcNow;
            DateTime oldestDate = dateNow - logPeriod;

            var logMessages = this.unitOfWork.Repository<LogMessage>()
                  .Query()
                  .Filter(x => x.DateLogged > oldestDate)
                  .Get()
                  .OrderByDescending(x => x.DateLogged)
                  .ToList();

            return logMessages;
        }

        internal void DeleteLogsOlderThanDate(DateTime olderThanDate)
        {
            IEnumerable<LogMessage> logMessages = null;

            logMessages = this.unitOfWork.Repository<LogMessage>()
                      .Query()
                      .Filter(x => x.DateLogged < olderThanDate)
                      .Get()
                      .ToList();

            foreach (var item in logMessages)
            {
                this.unitOfWork.Repository<LogMessage>().Delete(item);
            }
        }

        internal void ClearLog()
        {
            IEnumerable<LogMessage> logMessages = null;

            logMessages = this.unitOfWork.Repository<LogMessage>()
                      .Query()
                      .Get()
                      .ToList();

            foreach (var item in logMessages)
            {
                this.unitOfWork.Repository<LogMessage>().Delete(item);
            }
        }
    }
}