namespace DoctrineShips.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                  "Auth",
                  "Auth/{accountId}/{key}/{secondKey}",
                  new { controller = "Account", action = "Authenticate", accountId = string.Empty, key = string.Empty, secondKey = string.Empty });

            routes.MapRoute(
                  "A",
                  "A/{accountId}/{key}/{secondKey}",
                  new { controller = "Account", action = "Authenticate", accountId = string.Empty, key = string.Empty, secondKey = string.Empty });

            routes.MapRoute(
                  "S",
                  "S/{shortUrlId}",
                  new { controller = "Tools", action = "ShortUrlRedirect", shortUrlId = string.Empty });

            routes.MapRoute(
                  "CustomerContracts",
                  "Search/Cust/{customerId}",
                  new { controller = "Search", action = "CustomerContracts", customerId = string.Empty });

            routes.MapRoute(
                  "CustomerContractsStation",
                  "Search/CustStation/{stationId}/{customerId}",
                  new { controller = "Search", action = "CustomerContractsStation", stationId = string.Empty, customerId = string.Empty });

            routes.MapRoute(
                  "SalesAgentContracts",
                  "Search/Agent/{salesAgentId}",
                  new { controller = "Search", action = "SalesAgentContracts", salesAgentId = string.Empty });

            routes.MapRoute(
                  "ShipFitContracts",
                  "Search/ShipFit/{shipFitId}",
                  new { controller = "Search", action = "ShipFitContracts", shipFitId = string.Empty });

            routes.MapRoute(
                  "ShipFitContractsStation",
                  "Search/ShipFitStation/{stationId}/{shipFitId}",
                  new { controller = "Search", action = "ShipFitContractsStation", stationId = string.Empty, shipFitId = string.Empty });

            routes.MapRoute(
                  "RefreshContracts",
                  "Task/RefreshContracts/{key}/{force}/{batchSize}",
                  new { controller = "Task", action = "RefreshContracts", key = string.Empty, force = string.Empty, batchSize = string.Empty });

            routes.MapRoute(
                  "DailyMaintenance",
                  "Task/DailyMaintenance/{key}",
                  new { controller = "Task", action = "DailyMaintenance", key = string.Empty });

            routes.MapRoute(
                  "HourlyMaintenance",
                  "Task/HourlyMaintenance/{key}",
                  new { controller = "Task", action = "HourlyMaintenance", key = string.Empty });

            routes.MapRoute(
                  "RefreshShipFits",
                  "Task/RefreshShipFits/{key}",
                  new { controller = "Task", action = "RefreshShipFits", key = string.Empty });

            routes.MapRoute(
                  "ShipFitDetail",
                  "ShipFit/Detail/{shipFitId}",
                  new { controller = "ShipFit", action = "Detail", shipFitId = string.Empty });

            routes.MapRoute(
                  "DoctrineDetail",
                  "Doctrine/Detail/{doctrineId}",
                  new { controller = "Doctrine", action = "Detail", doctrineId = string.Empty });

            routes.MapRoute(
                  "ShipFitDetailResult",
                  "ShipFit/DetailResult/{shipFitId}",
                  new { controller = "ShipFit", action = "DetailResult", shipFitId = string.Empty });

            routes.MapRoute(
                  name: "Default",
                  url: "{controller}/{action}/{id}",
                  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}