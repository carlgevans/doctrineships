namespace DoctrineShips.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using DevTrends.MvcDonutCaching;
    using DoctrineShips.Service;
    using DoctrineShips.Web.ViewModels;

    public class HomeController : Controller
    {
        private readonly IDoctrineShipsServices doctrineShipsServices;

        public HomeController(IDoctrineShipsServices doctrineShipsServices)
        {
            this.doctrineShipsServices = doctrineShipsServices;
        }

        [DonutOutputCache(Duration = 600, VaryByCustom = "Account", Location = OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            HomeIndexViewModel viewModel = new HomeIndexViewModel();

            viewModel.Customers = this.doctrineShipsServices.GetCorporationCustomers();

            return View(viewModel);
        }
    }
}
