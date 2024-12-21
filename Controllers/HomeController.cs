using OnlineExamination.BLL;
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
                    return RedirectToAction("Login", "Student");
                }

                
                if (userRole == "STUDENT")
                {
                   
                    ViewBag.UserRole = "STUDENT";
                    ViewBag.UserData = userData;
                AdminService objadmindashbord = new AdminService();
                DashboardStats objdashboard = objadmindashbord.AdminDashboard().FirstOrDefault();

                return View(objdashboard);
            }
                else if (userRole == "ADMIN")
                {
                AdminService objadmindashbord = new AdminService();
                DashboardStats objdashboard = objadmindashbord.AdminDashboard().FirstOrDefault();

                ViewBag.UserRole = "ADMIN";
                    ViewBag.UserData = userData;
                return View(objdashboard);
            }
                else
                {
                    return RedirectToAction("Login", "Student");
                }

                
            
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