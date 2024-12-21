using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExamination
{
    using System.Web.Mvc;

    public class CheckSessionRoleAttribute : ActionFilterAttribute
    {
        // This method is called before the action is executed
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if session data exists
            string userRole = HttpContext.Current.Session["UserRole"] as string;
            var userData = HttpContext.Current.Session["UserData"];

            // If userRole or userData is null or empty, redirect to login
            if (string.IsNullOrEmpty(userRole) || userData == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Student", action = "Login" })
                );
            }
            else
            {
                // If session data exists, set ViewBag data
                filterContext.Controller.ViewBag.UserRole = userRole;
                filterContext.Controller.ViewBag.UserData = userData;
            }

            base.OnActionExecuting(filterContext);
        }
    }

}