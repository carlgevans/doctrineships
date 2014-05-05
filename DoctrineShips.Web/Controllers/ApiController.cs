namespace DoctrineShips.Web.Controllers
{
    using System;
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

        public ActionResult ShipFitDetail(string id)
        {
            try
            {
                // Cleanse the passed ship fit id string to prevent XSS.
                int cleanShipFitId = Conversion.StringToInt32(Server.HtmlEncode(id));

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
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = "Ship Fit Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { error = "An Error Occured" }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult DoctrineDetail(string id)
        {
            try
            {
                // Cleanse the passed doctrine id string to prevent XSS.
                int cleanDoctrineId = Conversion.StringToInt32(Server.HtmlEncode(id));

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
                        IsDormant = doctrine.IsDormant,
                        LastUpdate = doctrine.LastUpdate
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = "Doctrine Not Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { error = "An Error Occured" }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult AccountShipFits()
        {
            try
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

                if (accountShipFitList != null && accountShipFitList.Count() != 0)
                {
                    return Json(accountShipFitList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = "No Account Ship Fits Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { error = "An Error Occured" }, JsonRequestBehavior.AllowGet);
            }

            
        }

        public ActionResult DoctrineShipFits(string id)
        {
            try
            {
                // Cleanse the passed doctrine id string to prevent XSS.
                int cleanDoctrineId = Conversion.StringToInt32(Server.HtmlEncode(id));

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

                if (doctrineShipFitList != null && doctrineShipFitList.Count() != 0)
                {
                    return Json(doctrineShipFitList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { error = "No Doctrine Ship Fits Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { error = "An Error Occured" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EftFittingString(string id)
        {
            try
            {
                // Cleanse the passed ship fit id string to prevent XSS.
                int cleanShipFitId = Conversion.StringToInt32(Server.HtmlEncode(id));

                // Convert the currently logged-in account id to an integer.
                int accountId = Conversion.StringToInt32(User.Identity.Name);

                if (cleanShipFitId != 0)
                {
                    // Fetch the ship fit. If the user is not authorised to view it, null will be returned.
                    string eftFittingString = this.doctrineShipsServices.GetEftFittingString(cleanShipFitId, accountId);

                    if (!String.IsNullOrEmpty(eftFittingString))
                    {
                        return Content(eftFittingString);
                    }
                }

                return Content("Ship Fit Not Found Or Not Authorised");
            }
            catch (System.Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("An Error Occured");
            }
        }
    }
}
