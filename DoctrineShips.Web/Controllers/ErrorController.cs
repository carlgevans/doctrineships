namespace DoctrineShips.Web.Controllers
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    using Tools;

    public class ErrorController : Controller
    {
        private readonly ISystemLogger logger;

        public ErrorController(ISystemLogger logger)
        {
            this.logger = logger;
        }

        public ActionResult Error(int statusCode, Exception e)
        {
            // Ignore '404 Not Found' Errors.
            if (statusCode != 404)
            {
                // Log any other status codes & exception.
                logger.LogMessage("Status Code: " + statusCode + " Exception: " + e.ToString(), 0, "Exception", MethodBase.GetCurrentMethod().Name);
            }

            Response.StatusCode = statusCode;
            return View();
        }
    }
}
