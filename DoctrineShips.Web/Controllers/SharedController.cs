namespace DoctrineShips.Web.Controllers
{
    using System.Web.Mvc;
    using DoctrineShips.Web.ViewModels;
    using DoctrineShips.Service;
    using Tools;

    public class SharedController : Controller
    {
        private readonly IDoctrineShipsServices doctrineShipsServices;

        public SharedController(IDoctrineShipsServices doctrineShipsServices)
        {
            this.doctrineShipsServices = doctrineShipsServices;
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            SharedMenuViewModel viewModel = new SharedMenuViewModel();

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            viewModel.Account = this.doctrineShipsServices.GetAccount(accountId);

            return PartialView("_Menu", viewModel);
        }
    }
}
