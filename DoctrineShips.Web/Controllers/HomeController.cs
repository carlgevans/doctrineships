namespace DoctrineShips.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using DevTrends.MvcDonutCaching;
    using DoctrineShips.Service;
    using DoctrineShips.Web.ViewModels;
    using Tools;

    public class HomeController : Controller
    {
        private readonly IDoctrineShipsServices doctrineShipsServices;

        public HomeController(IDoctrineShipsServices doctrineShipsServices)
        {
            this.doctrineShipsServices = doctrineShipsServices;
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Doctrines");
            }
            else
            {
                return RedirectToAction("Contracts");
            }
        }

        [DonutOutputCache(Duration = 600, VaryByCustom = "Account", Location = OutputCacheLocation.Server)]
        [Authorize]
        public ActionResult Doctrines()
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            HomeDoctrinesViewModel viewModel = new HomeDoctrinesViewModel();

            // Fetch a list of doctrines for the current account.
            viewModel.Doctrines = this.doctrineShipsServices.GetDoctrineList(accountId);

            return View(viewModel);
        }

        [DonutOutputCache(Duration = 600, VaryByCustom = "Account", Location = OutputCacheLocation.Server)]
        public ActionResult Contracts()
        {
            HomeContractsViewModel viewModel = new HomeContractsViewModel();

            viewModel.Customers = this.doctrineShipsServices.GetCorporationCustomers();

            return View(viewModel);
        }
    }
}
