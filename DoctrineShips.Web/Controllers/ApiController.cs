namespace DoctrineShips.Web.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using DoctrineShips.Service;
    using DoctrineShips.Web.Filters;
    using Tools;

    [Authorize]
    public class ApiController : Controller
    {
        private readonly IDoctrineShipsServices doctrineShipsServices;

        public ApiController(IDoctrineShipsServices doctrineShipsServices)
        {
            this.doctrineShipsServices = doctrineShipsServices;
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult GetShipFitDetail(string shipFitId)
        {
            // Cleanse the passed ship fit id string to prevent XSS.
            int cleanShipFitId = Conversion.StringToInt32(Server.HtmlEncode(shipFitId));

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Fetch the ship fit for the json response.
            var shipFit = this.doctrineShipsServices.GetShipFitDetail(cleanShipFitId, accountId);

            if (shipFit != null)
            {
                return Json(new
                    {
                        ShipFitId = shipFit.ShipFitId,
                        HullId = shipFit.HullId,
                        ThumbnailImageUrl = shipFit.ThumbnailImageUrl,
                        RenderImageUrl = shipFit.RenderImageUrl,
                        Hull = shipFit.Hull,
                        Name = shipFit.Name,
                        Role = shipFit.Role,
                        ContractsAvailable = shipFit.ContractsAvailable,
                        IsMonitored = shipFit.IsMonitored,
                        FitPackagedVolume = shipFit.FitPackagedVolume,
                        BuyPrice = shipFit.BuyPrice,
                        SellPrice = shipFit.SellPrice,
                        ShippingCost = shipFit.ShippingCost,
                        ContractReward = shipFit.ContractReward,
                        BuyOrderProfit = shipFit.BuyOrderProfit,
                        SellOrderProfit = shipFit.SellOrderProfit,
                        FittingString = shipFit.FittingString,
                        FittingHash = shipFit.FittingHash,
                        LastPriceRefresh = shipFit.LastPriceRefresh,
                        Notes = shipFit.Notes
                    });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Ship Fit Not Found");
            }
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult GetDoctrineDetail(string doctrineId)
        {
            // Cleanse the passed doctrine id string to prevent XSS.
            int cleanDoctrineId = Conversion.StringToInt32(Server.HtmlEncode(doctrineId));

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Fetch the doctrine for the json response.
            var doctrine = this.doctrineShipsServices.GetDoctrineDetail(accountId, cleanDoctrineId);

            if (doctrine != null)
            {
                return Json(new
                    {
                        DoctrineId = doctrine.DoctrineId,
                        Name = doctrine.Name,
                        Description = doctrine.Description,
                        ImageUrl = doctrine.ImageUrl,
                        IsOfficial = doctrine.IsOfficial,
                        LastUpdate = doctrine.LastUpdate
                    });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Doctrine Not Found");
            }
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult GetAccountShipFits()
        {
            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Fetch a list of all ship fits for the account, sorted by name.
            var accountShipFitList = this.doctrineShipsServices.GetShipFitList(accountId)
                                         .OrderBy(x => x.Name)
                                         .Select(x => new
                                         {
                                             ShipFitId = x.ShipFitId,
                                             HullId = x.HullId,
                                             ThumbnailImageUrl = x.ThumbnailImageUrl,
                                             RenderImageUrl = x.RenderImageUrl,
                                             Hull = x.Hull,
                                             Name = x.Name,
                                             Role = x.Role,
                                             ContractsAvailable = x.ContractsAvailable,
                                             IsMonitored = x.IsMonitored,
                                             FitPackagedVolume = x.FitPackagedVolume,
                                             BuyPrice = x.BuyPrice,
                                             SellPrice = x.SellPrice,
                                             ShippingCost = x.ShippingCost,
                                             ContractReward = x.ContractReward,
                                             BuyOrderProfit = x.BuyOrderProfit,
                                             SellOrderProfit = x.SellOrderProfit,
                                             FittingString = x.FittingString,
                                             FittingHash = x.FittingHash,
                                             LastPriceRefresh = x.LastPriceRefresh,
                                             Notes = x.Notes
                                         })
                                         .ToList();

            if (accountShipFitList != null)
            {
                return Json(accountShipFitList);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("No Account Ship Fits Found");
            }
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult GetDoctrineShipFits(string doctrineId)
        {
            // Cleanse the passed doctrine id string to prevent XSS.
            int cleanDoctrineId = Conversion.StringToInt32(Server.HtmlEncode(doctrineId));

            // Convert the currently logged-in account id to an integer.
            int accountId = Conversion.StringToInt32(User.Identity.Name);

            // Fetch the doctrine ship fit list for the json response.
            var doctrineShipFitList = this.doctrineShipsServices.GetDoctrineDetail(accountId, cleanDoctrineId).DoctrineShipFits
                                          .Select(x => new
                                          {
                                              ShipFitId = x.ShipFit.ShipFitId,
                                              HullId = x.ShipFit.HullId,
                                              ThumbnailImageUrl = x.ShipFit.ThumbnailImageUrl,
                                              RenderImageUrl = x.ShipFit.RenderImageUrl,
                                              Hull = x.ShipFit.Hull,
                                              Name = x.ShipFit.Name,
                                              Role = x.ShipFit.Role,
                                              ContractsAvailable = x.ShipFit.ContractsAvailable,
                                              IsMonitored = x.ShipFit.IsMonitored,
                                              FitPackagedVolume = x.ShipFit.FitPackagedVolume,
                                              BuyPrice = x.ShipFit.BuyPrice,
                                              SellPrice = x.ShipFit.SellPrice,
                                              ShippingCost = x.ShipFit.ShippingCost,
                                              ContractReward = x.ShipFit.ContractReward,
                                              BuyOrderProfit = x.ShipFit.BuyOrderProfit,
                                              SellOrderProfit = x.ShipFit.SellOrderProfit,
                                              FittingString = x.ShipFit.FittingString,
                                              FittingHash = x.ShipFit.FittingHash,
                                              LastPriceRefresh = x.ShipFit.LastPriceRefresh,
                                              Notes = x.ShipFit.Notes
                                          })                                            
                                          .ToList();

            if (doctrineShipFitList != null)
            {
                return Json(doctrineShipFitList);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("No Doctrine Ship Fits Found");
            }
        }
    }
}
