using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineExamination.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home (Student Login by default)
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult Login()
        {
            // Check if student is already logged in
            string userRole = Session["UserRole"] as string;
            string studentId = Session["StudentId"] as string;
            
            if (userRole == "STUDENT" && !string.IsNullOrEmpty(studentId))
            {
                // Student is already logged in, redirect to dashboard
                return RedirectToAction("Index", "Student");
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(StudentMasterModel objstudlogin)
        {
            try
            {
                StudentService objStudentService = new StudentService();
                bool isLogin = objStudentService.Login(objstudlogin.Stud_UserName, objstudlogin.Stud_Password);
                if (isLogin)
                {
                    // Redirect to Student Dashboard instead of Admin
                    return RedirectToAction("Index", "Student");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            // Clear all session data
            Session.Clear();
            Session.Abandon();
            
            // Sign out from forms authentication
            FormsAuthentication.SignOut();
            
            // Redirect to login page
            return RedirectToAction("Login", "Home");
        }
    }
}
