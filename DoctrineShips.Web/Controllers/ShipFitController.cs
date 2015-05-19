﻿namespace DoctrineShips.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using DoctrineShips.Service;
    using DoctrineShips.Web.Filters;
    using DoctrineShips.Web.ViewModels;
    using Tools;

    [Authorize]
    public class ShipFitController : Controller
    {
        private readonly IDoctrineShipsServices doctrineShipsServices;

        public ShipFitController(IDoctrineShipsServices doctrineShipsServices)
        {
            this.doctrineShipsServices = doctrineShipsServices;
        }

        public ActionResult List()
        {
            // Instantiate a new view model to populate the view.
            ShipFitListViewModel viewModel = new ShipFitListViewModel();

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Get a list of all ship fits for the current account.
            viewModel.ShipFits = this.doctrineShipsServices.GetShipFitList(accountId);

            // Get the setting profile for the current account for contract availability thresholds.
            viewModel.SettingProfile = this.doctrineShipsServices.GetAccountSettingProfile(accountId);

            return View(viewModel);
        }

        public ActionResult Doctrines(string shipFitId)
        {
            // Instantiate a new view model to populate the view.
            ShipFitDetailViewModel viewModel = new ShipFitDetailViewModel();

            // Cleanse the passed ship fit id string to prevent XSS.
            int cleanShipFitId = Conversion.StringToInt32(Server.HtmlEncode(shipFitId));

            // Get a list of all ship fits for the current ship fit.
            viewModel.ShipFitDoctrines = this.doctrineShipsServices.GetDoctrinesByShipFit(cleanShipFitId);

            return View(viewModel);
        }

        
        public ActionResult Detail(string shipFitId)
        {
            // Cleanse the passed ship fit id string to prevent XSS.
            int cleanShipFitId = Conversion.StringToInt32(Server.HtmlEncode(shipFitId));

            ViewBag.ShipFitId = cleanShipFitId;

            return View();
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult DetailResult(string shipFitId)
        {
            // Cleanse the passed ship fit id string to prevent XSS.
            int cleanShipFitId = Conversion.StringToInt32(Server.HtmlEncode(shipFitId));

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Instantiate a new view model to populate the view.
            ShipFitDetailViewModel viewModel = new ShipFitDetailViewModel();

            // Populate the view model with the ship fit.
            viewModel.ShipFit = this.doctrineShipsServices.GetShipFitDetail(cleanShipFitId, accountId);

            // Get the setting profile for the current account for contract availability thresholds.
            viewModel.SettingProfile = this.doctrineShipsServices.GetAccountSettingProfile(accountId);

            if (viewModel.ShipFit != null)
            {
                // Group the list by slot type.
                viewModel.ShipFitComponents = viewModel.ShipFit.ShipFitComponents
                                              .OrderBy(o => o.Component.Name)
                                              .GroupBy(u => u.SlotType)
                                              .OrderBy(x => x.First().SlotType)
                                              .Select(grp => grp.ToList())
                                              .ToList();

                viewModel.ShipFitDoctrines = this.doctrineShipsServices.GetDoctrinesByShipFit(cleanShipFitId);
                return PartialView("_DetailResult", viewModel);
            }
            else
            {
                return PartialView("_DetailResult");
            }
        }
    }
}
