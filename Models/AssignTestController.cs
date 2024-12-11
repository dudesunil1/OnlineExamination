using OnlineExamination.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Models
{
    public class AssignTestController : Controller
    {
        

        StudentService objStudentService = new StudentService();
        [HttpGet]
        public ActionResult Create()
        {
           
            LoadData();
           
            List<StudentMasterModel> objstudent = objStudentService.GetStudent();
            ViewBag.StudentList = objstudent;
            return View(objstudent);
        }


        [HttpPost]
        public ActionResult Submit(int SelectedTestId, List<int> selectedStudentIds)
        {
            // Check if there are any students selected
            if (selectedStudentIds == null || !selectedStudentIds.Any())
            {
                ModelState.AddModelError("", "No students selected.");
                return View(); // Return to the current view with an error message
            }

            // Check if a valid Test ID is provided
            if (SelectedTestId <= 0)
            {
                ModelState.AddModelError("", "Invalid test selected.");
                return View(); // Return to the current view with an error message
            }

            // Call the TestService to assign the test to each selected student one by one
            TestService testService = new TestService();
            foreach (var studentId in selectedStudentIds)
            {
                bool isAssigned = testService.AssignTest(SelectedTestId, studentId);  // Passing one student ID at a time

                // Check if the assignment was successful for the current student
                if (!isAssigned)
                {
                    ModelState.AddModelError("", $"Failed to assign test to student with ID {studentId}.");
                    return View(); // Return to the current view with an error message for this student
                }
            }

            // If all students are successfully assigned, store a success message in TempData
            TempData["SuccessMessage"] = "Test successfully assigned to selected students.";

            // Redirect to the same page for a new assignment
            return RedirectToAction("Create", "AssignTest"); // Assuming Index is the method that renders the assignment page
        }



        private void LoadData()
        {
            ViewBag.TestList = MasterService.GetTest();
            ViewBag.ClassList = MasterService.GetClass(); // This is your service call to get the data
           
        }
    }
}