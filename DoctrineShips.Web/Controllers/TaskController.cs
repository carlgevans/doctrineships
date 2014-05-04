namespace DoctrineShips.Web.Controllers
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DoctrineShips.Service;
    using Tools;
    
    public class TaskController : Controller
    {
        private readonly IDoctrineShipsServices doctrineShipsServices;
        private readonly ISystemLogger logger;
        
        public TaskController(IDoctrineShipsServices doctrineShipsServices, ISystemLogger logger)
        {
            this.doctrineShipsServices = doctrineShipsServices;
            this.logger = logger;
        }

        [OutputCache(Duration = 5)]
        public ActionResult RefreshContracts(string key, string force, string batchSize)
        {
            Stopwatch stopWatch = new Stopwatch();

            // Cleanse the passed string parameters to prevent XSS.
            string cleanKey = Server.HtmlEncode(key);
            bool cleanForce = Conversion.StringToBool(Server.HtmlEncode(force), false);
            int cleanBatchSize = Conversion.StringToInt32(Server.HtmlEncode(batchSize), 10);

            if (cleanKey == doctrineShipsServices.Settings.TaskKey)
            {
                // Time the execution of the contract refresh.
                stopWatch.Reset();
                stopWatch.Start();

                // Check sales agents and refresh their contracts if required. Also update various contract counts.
                this.doctrineShipsServices.RefreshContracts(cleanForce, cleanBatchSize);
                
                // Stop the clock.
                stopWatch.Stop();

                logger.LogMessage("Contract Refresh Successful, Time Taken: " + stopWatch.Elapsed, 2, "Message", MethodBase.GetCurrentMethod().Name);
                return Content("Contract Refresh Successful, Time Taken: " + stopWatch.Elapsed);
            }
            else
            {
                logger.LogMessage("Contract Refresh Failed, Invalid Key: " + cleanKey, 1, "Message", MethodBase.GetCurrentMethod().Name);
                return Content("Contract Refresh Failed, Invalid Key");
            }
        }

        [OutputCache(Duration = 5)]
        public async Task<ActionResult> HourlyMaintenance(string key)
        {
            Stopwatch stopWatch = new Stopwatch();

            // Cleanse the passed string parameters to prevent XSS.
            string cleanKey = Server.HtmlEncode(key);

            if (cleanKey == doctrineShipsServices.Settings.TaskKey)
            {
                // Time the execution of the hourly maintenance.
                stopWatch.Reset();
                stopWatch.Start();

                // Run hourly maintenance tasks.
                await this.doctrineShipsServices.HourlyMaintenance();

                // Stop the clock.
                stopWatch.Stop();

                logger.LogMessage("Hourly Maintenance Successful, Time Taken: " + stopWatch.Elapsed, 2, "Message", MethodBase.GetCurrentMethod().Name);
                return Content("Hourly Maintenance Successful, Time Taken: " + stopWatch.Elapsed);
            }
            else
            {
                logger.LogMessage("Hourly Maintenance Failed, Invalid Key: " + cleanKey, 1, "Message", MethodBase.GetCurrentMethod().Name);
                return Content("Hourly Maintenance Failed, Invalid Key");
            }
        }

        [OutputCache(Duration = 5)]
        public async Task<ActionResult> DailyMaintenance(string key)
        {
            Stopwatch stopWatch = new Stopwatch();

            // Cleanse the passed string parameters to prevent XSS.
            string cleanKey = Server.HtmlEncode(key);

            if (cleanKey == doctrineShipsServices.Settings.TaskKey)
            {
                // Time the execution of the daily maintenance.
                stopWatch.Reset();
                stopWatch.Start();

                // Run daily maintenance tasks.
                await this.doctrineShipsServices.DailyMaintenance();

                // Stop the clock.
                stopWatch.Stop();

                logger.LogMessage("Daily Maintenance Successful, Time Taken: " + stopWatch.Elapsed, 2, "Message", MethodBase.GetCurrentMethod().Name);
                return Content("Daily Maintenance Successful, Time Taken: " + stopWatch.Elapsed);
            }
            else
            {
                logger.LogMessage("Daily Maintenance Failed, Invalid Key: " + cleanKey, 1, "Message", MethodBase.GetCurrentMethod().Name);
                return Content("Daily Maintenance Failed, Invalid Key");
            }
        }

        [OutputCache(Duration = 5)]
        public ActionResult RefreshShipFits(string key)
        {
            Stopwatch stopWatch = new Stopwatch();

            // Cleanse the passed string parameters to prevent XSS.
            string cleanKey = Server.HtmlEncode(key);

            if (cleanKey == doctrineShipsServices.Settings.TaskKey)
            {
                // Time the execution of the daily maintenance.
                stopWatch.Reset();
                stopWatch.Start();

                // Refresh all fitting strings.
                this.doctrineShipsServices.RefreshShipFits();

                // Stop the clock.
                stopWatch.Stop();

                logger.LogMessage("Refresh Of All Ship Fits Successful, Time Taken: " + stopWatch.Elapsed, 2, "Message", MethodBase.GetCurrentMethod().Name);
                return Content("Refresh Of All Ship Fits Successful, Time Taken: " + stopWatch.Elapsed);
            }
            else
            {
                logger.LogMessage("Refresh Of All Ship Fits Failed, Invalid Key: " + cleanKey, 1, "Message", MethodBase.GetCurrentMethod().Name);
                return Content("Refresh Of All Ship Fits Failed, Invalid Key");
            }
        }
    }
}
