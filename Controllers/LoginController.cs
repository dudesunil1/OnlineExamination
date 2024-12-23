using OnlineExamination.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Models
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
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
                    return RedirectToAction("Index", "Home");
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


       

    }
}