using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Controllers
{
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
        public ActionResult StoreSelectedSubjects(string subjectId, bool isChecked, int numQuestions, string subjectName)
        {
            // Retrieve the dictionary of selected subjects from the session
            var selectedSubjects = Session["SelectedSubjects"] as Dictionary<string, (bool isChecked, int numQuestions, string subjectName)>;

            if (selectedSubjects == null)
            {
                selectedSubjects = new Dictionary<string, (bool, int, string)>();
            }

            // Add or update the subject in the dictionary with the subject name, checked state, and number of questions
            selectedSubjects[subjectId] = (isChecked, numQuestions, subjectName);

            // Save the updated dictionary back to the session
            Session["SelectedSubjects"] = selectedSubjects;

            return Json(new { success = true });
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
                        TestMasterModel res = objtestService.AddTestMaster(objTest);
                        if (res.Test_Id > 0)
                        {
                            ViewBag.ActiveTab = "question-details";
                            var selectedSubjects = Session["SelectedSubjects"] as Dictionary<string, (bool isChecked, int numQuestions, string subjectName)>;
                            
                            ViewBag.SubjectDropdown = selectedSubjects.Select(subject => new SelectListItem
                            {
                                Value = subject.Key, // Subject ID
                                Text = $" {subject.Value.subjectName}" // Display both subject ID and name
                            }).ToList();

                            ViewBag.SubjectData = selectedSubjects;
                            LoadData();
                            return View(res);
                            
                        }
                        else
                        {
                            return RedirectToAction("Create");
                        }
                    }
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetQuestionsByTopicId(int topicId)
        {
            // Fetch the questions for the selected topicId
            QuestionMasterService questionService = new QuestionMasterService();
            var questions = questionService.GetQuestionsByTopicId(topicId); // Pass a single topicId

            // Create HTML for the question list
            string htmlContent = string.Empty;

            if (questions != null && questions.Any())
            {
                // Start the container for questions
                htmlContent += "<ul class='question-list'>";

                foreach (var question in questions)
                {
                    // Add each question as a checkbox item in the list
                    htmlContent += $@"
                <li>
                    <input type='checkbox' class='question-checkbox' id='question{question.Ques_Id}' name='SelectedQuestions' value='{question.Ques_Id}'>
                    <label class='form-check-label' for='question{question.Ques_Id}'>
                        {question.Ques_Question}
                    </label>
                </li>";
                }

                htmlContent += "</ul>"; // End the container
            }
            else
            {
                htmlContent = "<p>No questions available for the selected topic.</p>";
            }

            // Return the HTML as JSON
            return Json(new { html = htmlContent }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopicsBySubjectId(int subjectId)
        {
            TopicService topicService = new TopicService();
            var topics = topicService.GetTopicsBySubjectId(subjectId); // Retrieve topics for the subject

            // Create HTML for the checkbox list
            string htmlContent = string.Empty;

            if (topics != null && topics.Any())
            {
                // Start the container for checkboxes
                htmlContent += "<div class='checkbox-list'>";

                foreach (var topic in topics)
                {
                    // Add each topic as a checkbox
                    htmlContent += $@"
                <div class='form-check'>
                    <input type='checkbox' class='topic-checkbox' id='topic{topic.Top_Id}' name='SelectedTopics' value='{topic.Top_Id}'>
                    <label class='form-check-label' for='topic{topic.Top_Id}'>
                        {topic.Top_Name}
                    </label>
                </div>";
                }

                htmlContent += "</div>"; // End the container
            }
            else
            {
                htmlContent = "<p>No topics available for the selected subject.</p>";
            }

            // Return the HTML as JSON
            return Json(new { html = htmlContent }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
            public ActionResult QuestionDetails()
            {
                // Retrieve the selected subjects and their question numbers from the session
                var selectedSubjects = Session["SelectedSubjects"] as Dictionary<string, int>;

                // Initialize if the session is null or empty
                if (selectedSubjects == null)
                {
                    selectedSubjects = new Dictionary<string, int>();
                }

                // Pass the selected subjects to the view as separate values
                ViewBag.SubjectIds = selectedSubjects.Keys.ToList(); // List of Subject IDs
                ViewBag.NumQuestions = selectedSubjects.Values.ToList(); // List of corresponding numQuestions


                return View();
            }

        
    }



}
