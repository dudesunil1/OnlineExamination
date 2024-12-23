using Newtonsoft.Json;
using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Controllers
{
    [CheckSessionRole]
    public class TestMasterController : Controller
    {
        TestService objtestService = new TestService();
        public ActionResult Index()
        {
            return View();
        }

        // GET: TestMaster/Create
        public ActionResult Create()
        {

            var selectedSubjects = Session["SelectedSubjects"] as Dictionary<string, (bool isChecked, int numQuestions, string subjectName)>;

            // Initialize if the session is null or empty
            if (selectedSubjects == null || !selectedSubjects.Any())
            {
                // Create a default dropdown with "No subjects available"
                ViewBag.SubjectDropdown = new List<SelectListItem>
    {
        new SelectListItem { Value = "0", Text = "No subjects available", Disabled = true, Selected = true }
    };
            }
            else
            {
                // Populate the dropdown with available subjects from the session
                ViewBag.SubjectDropdown = selectedSubjects.Select(subject => new SelectListItem
                {
                    Value = subject.Key, // Subject ID
                    Text = $" {subject.Value.subjectName}" // Display both subject ID and name
                }).ToList();
            }



            ViewBag.ActiveTab = "test-details";
            LoadData();
            return View();
        }

        private void LoadData()
        {
            ViewBag.ClassList = MasterService.GetClass();
            ViewBag.Testtypelist = MasterService.GetTestType();
            ViewBag.SubjectList = MasterService.GetSubjects();
        }







        [HttpPost]
        public ActionResult Create(TestMasterModel objTest, string Action)
        {
            try
            {
                if (Action == "Cancel")
                {
                    return RedirectToAction("Create");
                }
                else if (Action == "Back To List")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        // Add TestMaster
                        TestMasterModel res = objtestService.AddTestMaster(objTest);

                        if (res.Test_Id > 0)
                        {
                            // After saving, store selected subjects and their question count in session
                            StoreSelectedSubjectsInSession();

                            // Set active tab to "question-details"
                            ViewBag.TestId = res.Test_Id;
                            ViewBag.ActiveTab = "question-details";

                            LoadData(); // Reload data if necessary
                            return View(res);
                        }
                        else
                        {
                            return RedirectToAction("Create");
                        }
                    }
                    LoadData();
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // Method to store selected subjects and their number of questions in session
        private void StoreSelectedSubjectsInSession()
        {
            var selectedSubjects = Request.Form["SelectedSubjects"];
            var subjectQuestions = new Dictionary<string, int>();

            if (selectedSubjects != null)
            {
                foreach (var subjectId in selectedSubjects)
                {
                    // Convert subjectId to string (if not already a string)
                    string subjectKey = subjectId.ToString();

                    // Parse the number of questions for the subject, default to 0 if not valid
                    int questionCount = 0;
                    if (int.TryParse(Request.Form["NoOfQuestions_" + subjectKey], out questionCount))
                    {
                        // Store the subject and its number of questions
                        subjectQuestions[subjectKey] = questionCount;
                    }
                }

                // Store the data in session
                Session["SelectedSubjects"] = subjectQuestions;
            }
        }


    }




}




