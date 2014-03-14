namespace DoctrineShips.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using DevTrends.MvcDonutCaching;
    using DoctrineShips.Service;
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
    }
}
