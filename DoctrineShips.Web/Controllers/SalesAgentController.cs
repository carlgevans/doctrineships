namespace DoctrineShips.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.UI;
    using DevTrends.MvcDonutCaching;
    using DoctrineShips.Service;
    using DoctrineShips.Validation;
    using DoctrineShips.Web.ViewModels;
    using Tools;
    
    [Authorize]
    public class SalesAgentController : Controller
    {
        private readonly IDoctrineShipsServices doctrineShipsServices;

        public SalesAgentController(IDoctrineShipsServices doctrineShipsServices)
        {
            this.doctrineShipsServices = doctrineShipsServices;
        }

        [DonutOutputCache(Duration = 300, VaryByCustom = "Account", Location = OutputCacheLocation.Server)]
        public ActionResult List()
        {
            // Instantiate a new view model to populate the view.
            SalesAgentListViewModel viewModel = new SalesAgentListViewModel();

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Get a list of all active sales agents for the current account.
            viewModel.SalesAgents = this.doctrineShipsServices.GetSalesAgents(accountId).Where(x => x.IsActive == true);

            return View(viewModel);
        }

        public ActionResult ForceContractRefresh(string salesAgentId)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Clean the controller parameters.
            int cleanSalesAgentId = Conversion.StringToInt32(Server.HtmlEncode(salesAgentId));

            // Force a contract refresh for the passed sales agent.
            this.doctrineShipsServices.ForceContractRefresh(accountId, cleanSalesAgentId);

            return RedirectToAction("List");
        }

        public ActionResult Registration()
        {
            SalesAgentRegistrationViewModel viewModel = new SalesAgentRegistrationViewModel();

            // Set the ViewBag to the TempData status value passed from the Add & Delete methods.
            ViewBag.Status = TempData["Status"];

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<ActionResult> Register(SalesAgentRegistrationViewModel viewModel)
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            if (ModelState.IsValid)
            {
                // Clean the passed api key.
                string cleanApiKey = Conversion.StringToSafeString(Server.HtmlEncode(viewModel.ApiKey));

                IValidationResult validationResult = await this.doctrineShipsServices.AddSalesAgent(viewModel.ApiId, cleanApiKey, accountId);

                // If the validationResult is not valid, something did not validate in the service layer.
                if (validationResult.IsValid)
                {
                    TempData["Status"] = "Your registration as a sales agent was successful.";
                }
                else
                {
                    TempData["Status"] = "Error: Your registration as a sales agent failed, a validation error occured.<br /><b>Error Details: </b>";

                    foreach (var error in validationResult.Errors)
                    {
                        TempData["Status"] += error.Value + "<br />";
                    }
                }

                return RedirectToAction("Registration");
            }
            else
            {
                return View("~/Views/SalesAgent/Registration.cshtml", viewModel);
            }
        }
    }
}
