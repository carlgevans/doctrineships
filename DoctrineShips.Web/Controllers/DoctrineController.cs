namespace DoctrineShips.Web.Controllers
{
    using System.Web.Mvc;
    using System.Web.UI;
    using DevTrends.MvcDonutCaching;
    using DoctrineShips.Service;
    using DoctrineShips.Web.ViewModels;
    using Tools;

    [Authorize]
    public class DoctrineController : Controller
    {
        private readonly IDoctrineShipsServices doctrineShipsServices;

        public DoctrineController(IDoctrineShipsServices doctrineShipsServices)
        {
            this.doctrineShipsServices = doctrineShipsServices;
        }

        [DonutOutputCache(Duration = 300, VaryByCustom = "Account", VaryByParam = "*", Location = OutputCacheLocation.Server)]
        public ActionResult Detail(string doctrineId)
        {
            // Cleanse the passed doctrine id string to prevent XSS.
            int cleanDoctrineId = Conversion.StringToInt32(Server.HtmlEncode(doctrineId));

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Instantiate a new view model to populate the view.
            DoctrineDetailViewModel viewModel = new DoctrineDetailViewModel();

            // Populate the view model with the doctrine.
            viewModel.Doctrine = this.doctrineShipsServices.GetDoctrineDetail(accountId, cleanDoctrineId);

            // Get the setting profile for the current account for contract availability thresholds.
            viewModel.SettingProfile = this.doctrineShipsServices.GetAccountSettingProfile(accountId);

            return View(viewModel);
        }
    }
}
