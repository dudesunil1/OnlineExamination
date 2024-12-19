using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;

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

        [HttpGet]
        public ActionResult Profile()
        {
            string studId = Session["StudentId"] as string;
            
            if (string.IsNullOrEmpty(studId))
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if not found
            }

            int studentId = int.Parse(studId);

            ViewBag.grouplist = MasterService.GetGroups();
            ViewBag.Classlist = MasterService.GetClass();
            StudentMasterModel objPublication = objStudentService.GetStudentById(studentId).FirstOrDefault();


            if (objPublication == null)
            {
                return HttpNotFound();
            }

            return View(objPublication);
        }


    }
}