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
using System.Web.Security;
using Web.SunTechDB;

namespace OnlineExamination.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string userRole = Session["UserRole"] as string;
            var userData = Session["UserData"];
            int studid = 0;  
            if (Session["StudentId"] != null)
            {
                studid = Convert.ToInt32(Session["StudentId"]);
            }


            if (string.IsNullOrEmpty(userRole) || userData == null)
            {
                return RedirectToAction("Login", "Student");
            }

            if (userRole == "STUDENT")
            {
                ViewBag.UserRole = "STUDENT";
                ViewBag.UserData = userData;
                StudentService objStudentService = new StudentService();
                StudentDashboardViewModel dashboardData = objStudentService.GetStudentDashboardData(studid);
                return View("~/Views/Dashboard/StudentDashboard.cshtml", dashboardData);
            }
            else if (userRole == "ADMIN")
            {
                ViewBag.UserRole = "ADMIN";
                ViewBag.UserData = userData;
                AdminService adminService = new AdminService();
                DashboardStats Objadmin = adminService.AdminDashboard().FirstOrDefault();
                return View("~/Views/Dashboard/AdminDashboard.cshtml", Objadmin);

                
            }
            else
            {
                return RedirectToAction("Login", "Student");
            }
            
        }






        public ActionResult Logout()
        {
            string userRole = Session["UserRole"] as string;
            if (userRole == "ADMIN")
            {

                Session.Clear();
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Session.Clear();
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Login");

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