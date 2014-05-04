namespace DoctrineShips.Web
{
    using System;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using DoctrineShips.Data;
    using DoctrineShips.Repository;
    using DoctrineShips.Web.Controllers;
    using GenericRepository;
    using Tools;
    
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Set a global application brand.
            Application["Brand"] = WebConfigurationManager.AppSettings["Brand"] ?? string.Empty;
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                string[] roles = authTicket.UserData.Split(new Char[] { ',' });

                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);
                Context.User = userPrincipal;
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Server.ClearError();

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Error");
            routeData.Values.Add("exception", exception);

            if (exception.GetType() == typeof(HttpException))
            {
                routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
            }
            else
            {
                routeData.Values.Add("statusCode", 500);
            }

            IController controller = new ErrorController(new SystemLogger(new DoctrineShipsRepository(new UnitOfWork(new DoctrineShipsContext()))));
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            Response.End();
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            return arg.ToLower() == "account" ? "Account=" + User.Identity.Name : "Account=Anonymous";
        }
    }
}
