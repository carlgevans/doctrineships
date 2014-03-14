namespace DoctrineShips.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using DoctrineShips.Service;
    using Tools;
    
    public class ToolsController : Controller
    {
        private static IDictionary<string, string> cachedValues_LongUrl = new Dictionary<string, string>();
        private readonly IDoctrineShipsServices doctrineShipsServices;
        private readonly string websiteDomain;
        
        public ToolsController(IDoctrineShipsServices doctrineShipsServices)
        {
            this.doctrineShipsServices = doctrineShipsServices;
            this.websiteDomain = WebConfigurationManager.AppSettings["WebsiteDomain"];
        }

        [HttpPost]
        public ActionResult ShortenUrl(string longUrl)
        {
            string decodedUrl = System.Uri.UnescapeDataString(longUrl);

            // Ensure that the url being shortened is not for another website.
            if (!String.IsNullOrEmpty(decodedUrl) && Url.IsLocalUrl(decodedUrl))
            {
                var shortUrlId = this.doctrineShipsServices.AddShortUrl(decodedUrl);

                return Content(this.websiteDomain + "/S/" + shortUrlId);
            }
            else
            {
                return Content("Operation Failed.");
            }
        }

        public ActionResult ShortUrlRedirect(string shortUrlId)
        {
            string longUrl;

            // Cleanse the passed shortUrlId.
            string cleanShortUrlId = Conversion.StringToSafeString(Server.HtmlEncode(shortUrlId));
            
            // If the short url is cached return the cached long url, otherwise fetch it from the service layer.
            if (!cachedValues_LongUrl.TryGetValue(cleanShortUrlId, out longUrl))
            {
                longUrl = this.doctrineShipsServices.GetLongUrl(cleanShortUrlId);
                cachedValues_LongUrl.Add(shortUrlId, longUrl);
            }

            // If a local and clean short url has been passed, redirect to it.
            if (!String.IsNullOrEmpty(longUrl) && Url.IsLocalUrl(longUrl))
            {
                return Redirect(longUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
