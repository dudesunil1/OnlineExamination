using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Controllers
{
    public class StudentController : Controller
    {
        StudentService objStudentService = new StudentService();
        StudentService objstudentservices = new StudentService();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(StudentMasterModel objstudlogin)
        {
            try
            {
                

                    bool isLogin = objstudentservices.Login(objstudlogin.Stud_UserName, objstudlogin.Stud_Password);
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