using OnlineExamination.Models;
using SunTech.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.SunTechDB;

namespace OnlineExamination.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

                string userRole = Session["UserRole"] as string;
                var userData = Session["UserData"];

                // Check for null session or expired session data
                if (string.IsNullOrEmpty(userRole) || userData == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Dynamically load data based on the role
                if (userRole == "STUDENT")
                {
                    // If student, get student-specific data
                    ViewBag.UserRole = "STUDENT";
                    ViewBag.UserData = userData;  // Send student data to the view
                }
                else if (userRole == "ADMIN")
                {
                    // If admin, get admin-specific data
                    ViewBag.UserRole = "ADMIN";
                    ViewBag.UserData = userData;  // Send admin data to the view
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

                return View();
            
        }

    




    public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}