using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using PagedList;

namespace OnlineExamination.Controllers
{
    public class StudentController : Controller
    {
        StudentService objStudentService = new StudentService();
        
        StudentTestService objTestService = new StudentTestService();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(StudentMasterModel objstudlogin)
        {
            try
            {

                
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


        [HttpGet]
        public ActionResult TestList()
        {
            string studId = Session["StudentId"] as string;

            if (string.IsNullOrEmpty(studId))
            {
                return RedirectToAction("Login", "Student"); // Redirect to login if not found
            }

            int studentId = int.Parse(studId);

            List<TestStudent> objtestStudent = objTestService.GetstudetTest(studentId);


            if (objtestStudent == null)
            {
                return HttpNotFound();
            }

            return View(objtestStudent);
        }
        [HttpGet]
        public ActionResult ShowTestQuestion(int id, int? page)
        {
            QuestionMasterService objTestService = new QuestionMasterService();
            string studId = Session["StudentId"] as string;

            if (string.IsNullOrEmpty(studId))
            {
                return RedirectToAction("Login", "Student"); // Redirect to login if not found
            }

            int studentId = int.Parse(studId);

            List<TestQuestionViewModel> objtestquestion = objTestService.GetTestQuestions(id);
            int pageSize = 1; // Show one question per page
            int pageNumber = (page ?? 1); // Default to page 1 if no page is specified

            // Pass the paginated result to the view
            

            if (objtestquestion == null)
            {
                return HttpNotFound();
            }

            return View(objtestquestion.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult TestDetails(int id)
        {
            string studId = Session["StudentId"] as string;

            if (string.IsNullOrEmpty(studId))
            {
                return RedirectToAction("Login", "Student"); // Redirect to login if not found
            }

            
            TestSubjectDetailsModel objtestStudent = objTestService.GetTestSubjectDetails(id).FirstOrDefault();

            if (objtestStudent == null)
            {
                return HttpNotFound();
            }

            return View(objtestStudent);
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