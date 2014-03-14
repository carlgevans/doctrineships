namespace DoctrineShips.Web.Filters
{
    using System;
    using System.Web.Mvc;

    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectResult("/");
                return;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}