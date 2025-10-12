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
    public class AdminController : Controller
    {
        AdminService objAdminService = new AdminService();
        
        [HttpGet]
        public ActionResult Login()
        {
            // Check if admin is already logged in
            string userRole = Session["UserRole"] as string;
            var userData = Session["UserData"];
            
            if (userRole == "ADMIN" && userData != null)
            {
                // Admin is already logged in, redirect to dashboard
                return RedirectToAction("Index", "Admin");
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminLoginViewModel objAdminLogin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isLogin = objAdminService.Login(objAdminLogin.UserName, objAdminLogin.Password);
                    if (isLogin)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }
                return View(objAdminLogin);
            }
            catch
            {
                return View();
            }
        }
        

        public ActionResult Logout()
        {
            string userRole = Session["UserRole"] as string;
            
            // Clear all session data
            Session.Clear();
            Session.Abandon();
            
            // Sign out from forms authentication
            FormsAuthentication.SignOut();
            
            if (userRole == "ADMIN")
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "Home");
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
