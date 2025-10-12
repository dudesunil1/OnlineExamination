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
            string studentId = HttpContext.Current.Session["StudentId"] as string;

            // If userRole or userData is null or empty, redirect to login
            if (string.IsNullOrEmpty(userRole) || userData == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "Login" })
                );
            }
            else
            {
                // If session data exists, set ViewBag data for easy access in actions
                filterContext.Controller.ViewBag.UserRole = userRole;
                filterContext.Controller.ViewBag.UserData = userData;
                filterContext.Controller.ViewBag.StudentId = studentId;
                
                // Also set TempData for easy access in actions without ViewBag
                filterContext.Controller.TempData["UserRole"] = userRole;
                filterContext.Controller.TempData["UserData"] = userData;
                filterContext.Controller.TempData["StudentId"] = studentId;
            }

            base.OnActionExecuting(filterContext);
        }
    }

    // Specific attribute for student-only actions
    public class StudentAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if user is logged in and is a student
            string userRole = HttpContext.Current.Session["UserRole"] as string;
            string studentId = HttpContext.Current.Session["StudentId"] as string;

            // Must be a student with valid student ID
            if (userRole != "STUDENT" || string.IsNullOrEmpty(studentId))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "Login" })
                );
                return;
            }

            // Set ViewBag and TempData for easy access
            filterContext.Controller.ViewBag.UserRole = userRole;
            filterContext.Controller.ViewBag.StudentId = studentId;
            filterContext.Controller.ViewBag.UserData = HttpContext.Current.Session["UserData"];
            
            filterContext.Controller.TempData["UserRole"] = userRole;
            filterContext.Controller.TempData["StudentId"] = studentId;
            filterContext.Controller.TempData["UserData"] = HttpContext.Current.Session["UserData"];

            base.OnActionExecuting(filterContext);
        }
    }

    // Helper class for easy access to current user information
    public static class CurrentUser
    {
        public static string GetStudentId()
        {
            return HttpContext.Current.Session["StudentId"] as string;
        }

        public static string GetUserRole()
        {
            return HttpContext.Current.Session["UserRole"] as string;
        }

        public static object GetUserData()
        {
            return HttpContext.Current.Session["UserData"];
        }

        public static bool IsStudent()
        {
            return GetUserRole() == "STUDENT" && !string.IsNullOrEmpty(GetStudentId());
        }

        public static bool IsAdmin()
        {
            return GetUserRole() == "ADMIN";
        }

        public static int GetStudentIdAsInt()
        {
            string studentId = GetStudentId();
            int.TryParse(studentId, out int result);
            return result;
        }
    }

}