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
    [CheckSessionRole]
    public class StudentController : Controller
    {
        StudentService objStudentService = new StudentService();
        
        StudentTestService objTestService = new StudentTestService();
       

      

        [HttpGet]
      
        public ActionResult TestList()
        {
            string studId = Session["StudentId"] as string;

            if (string.IsNullOrEmpty(studId))
            {
                return RedirectToAction("Login", "Login"); // Redirect to login if not found
            }

            int studentId = int.Parse(studId);

            List<TestStudent> objtestStudent = objTestService.GetstudetTest(studentId);



            return View(objtestStudent);
        }
        [HttpGet]
        public ActionResult ShowTestQuestion(int id, int? page)
        {
            QuestionMasterService objTestService = new QuestionMasterService();
            string studId = Session["StudentId"] as string;

            if (string.IsNullOrEmpty(studId))
            {
                return RedirectToAction("Login", "Login"); // Redirect to login if not found
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
            try
            {
                string studId = Session["StudentId"] as string;

                if (string.IsNullOrEmpty(studId))
                {
                    return RedirectToAction("Login", "Login");
                }

                int studentId = int.Parse(studId);

                // First, get the student's test assignment to ensure they have access
                List<TestStudent> studentTests = objTestService.GetstudetTest(studentId);
                TestStudent currentTest = studentTests?.FirstOrDefault(t => t.TS_Id == id);

                if (currentTest == null)
                {
                    TempData["ErrorMessage"] = "Test not found or you don't have access to this test.";
                    return RedirectToAction("TestList");
                }

                // Now get the detailed test information
                var testDetailsList = objTestService.GetTestSubjectDetails(id);

                TestSubjectDetailsModel objtestStudent = null;

                if (testDetailsList != null && testDetailsList.Any())
                {
                    objtestStudent = testDetailsList.FirstOrDefault();
                }

                // If stored procedure didn't return data, create a basic model from TestStudent data
                if (objtestStudent == null)
                {
                    objtestStudent = new TestSubjectDetailsModel
                    {
                        TS_Id = currentTest.TS_Id,
                        TestID = currentTest.TS_TestId,
                        SubjectName = "Test",
                        TestDuration = currentTest.Test_Duration,
                        TestStartTime = currentTest.TS_StartTime,
                        TestEndTime = currentTest.TS_End_Time,
                        TestMark = 0,
                        NumberOfQuestions = 0
                    };
                }
                else
                {
                    // Ensure TestID is populated
                    if (objtestStudent.TestID == 0)
                    {
                        objtestStudent.TestID = currentTest.TS_TestId;
                    }
                }

                return View(objtestStudent);
            }
            catch (Exception ex)
            {
                // Log the detailed error for debugging
                System.Diagnostics.Debug.WriteLine("TestDetails Error: " + ex.ToString());
                string stackTrace = ex.StackTrace != null && ex.StackTrace.Length > 200 
                    ? ex.StackTrace.Substring(0, 200) 
                    : ex.StackTrace ?? "";
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message + " | Stack: " + stackTrace;
                return RedirectToAction("TestList");
            }
        }

        [HttpGet]
        public ActionResult TakeExam(int testId, int question = 1)
        {
            string studId = Session["StudentId"] as string;

            if (string.IsNullOrEmpty(studId))
            {
                return RedirectToAction("Login", "Login");
            }

            int studentId = int.Parse(studId);

            // Verify student has access to this test
            List<TestStudent> studentTests = objTestService.GetstudetTest(studentId);
            TestStudent assignedTest = studentTests?.FirstOrDefault(t => t.TS_TestId == testId);

            if (assignedTest == null)
            {
                TempData["ErrorMessage"] = "You are not authorized to take this test or the test does not exist.";
                return RedirectToAction("TestList");
            }

            // Get exam interface data
            StudentService studentService = new StudentService();
            ExamInterfaceViewModel examData = studentService.GetExamInterfaceData(testId, studentId, question);

            if (examData == null)
            {
                TempData["ErrorMessage"] = "Unable to load exam data. Please try again.";
                return RedirectToAction("TestDetails", new { id = assignedTest.TS_Id });
            }

            return View(examData);
        }

        [HttpPost]
        public JsonResult StartExam(int testId)
        {
            try
            {
                string studId = Session["StudentId"] as string;
                if (string.IsNullOrEmpty(studId))
                {
                    return Json(new { success = false, message = "Student not logged in" });
                }

                int studentId = int.Parse(studId);

                // Initialize exam session
                StudentService studentService = new StudentService();
                bool started = studentService.StartExamSession(testId, studentId);

                return Json(new { success = started });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SaveAnswer(int testId, int questionNumber, string answer, bool markForReview = false)
        {
            try
            {
                string studId = Session["StudentId"] as string;
                if (string.IsNullOrEmpty(studId))
                {
                    return Json(new { success = false, message = "Student not logged in" });
                }

                int studentId = int.Parse(studId);

                StudentService studentService = new StudentService();
                bool saved = studentService.SaveStudentAnswer(testId, studentId, questionNumber, answer, markForReview);

                return Json(new { success = saved });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ClearAnswer(int testId, int questionNumber)
        {
            try
            {
                string studId = Session["StudentId"] as string;
                if (string.IsNullOrEmpty(studId))
                {
                    return Json(new { success = false, message = "Student not logged in" });
                }

                int studentId = int.Parse(studId);

                StudentService studentService = new StudentService();
                bool cleared = studentService.ClearStudentAnswer(testId, studentId, questionNumber);

                return Json(new { success = cleared });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SubmitExam(int testId)
        {
            try
            {
                string studId = Session["StudentId"] as string;
                if (string.IsNullOrEmpty(studId))
                {
                    return Json(new { success = false, message = "Student not logged in" });
                }

                int studentId = int.Parse(studId);

                StudentService studentService = new StudentService();
                bool submitted = studentService.SubmitExam(testId, studentId);

                return Json(new { success = submitted });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult QuestionPaper(int testId)
        {
            string studId = Session["StudentId"] as string;

            if (string.IsNullOrEmpty(studId))
            {
                return RedirectToAction("Login", "Login");
            }

            int studentId = int.Parse(studId);

            // Get all questions for the test
            QuestionMasterService questionService = new QuestionMasterService();
            List<TestQuestionViewModel> questions = questionService.GetTestQuestions(testId);

            if (questions == null)
            {
                return HttpNotFound();
            }

            return View(questions);
        }

    }
}