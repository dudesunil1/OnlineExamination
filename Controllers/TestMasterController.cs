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

                            ViewBag.TestId = res.Test_Id;
                            ViewBag.ActiveTab = "question-details";
                            var selectedSubjects = Session["SelectedSubjects"] as Dictionary<string, (bool isChecked, int numQuestions, string subjectName)>;

                            ViewBag.SubjectDropdown = selectedSubjects.Select(subject => new SelectListItem
                            {
                                Value = subject.Key, // Subject ID
                                Text = $"{subject.Value.subjectName} ({subject.Value.numQuestions} questions)" // Display subject name and number of questions
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


        [HttpPost]
        public ActionResult SubmitSelectedQuestions(List<int> selectedQuestions, int TestId)
        {
            // Check if questions are selected
            if (selectedQuestions != null && selectedQuestions.Any())
            {
                // For example, you can save the selected questions to a database, 
                // associate them with the provided TestId, or perform other logic
                foreach (var questionId in selectedQuestions)
                {
                    // Example logic: Save the selected question for the specific test
                    // Assuming you have a method to associate questions with tests
                    bool isSaved = SaveQuestionToTest(questionId, TestId);

                    if (!isSaved)
                    {
                        // If saving fails, return a failure message
                        return Json(new { success = false, message = "An error occurred while saving questions." });
                    }
                }

                // Return a success response
                return Json(new { success = true, message = "Questions submitted successfully!" });
            }
            else
            {
                // Return an error message if no questions are selected
                return Json(new { success = false, message = "No questions selected." });
            }
        }

        
        private bool SaveQuestionToTest(int questionId, int testId)
        {
            try
            {
                // Assuming you have a method to add questions to a test in your database
                // For example, add to a "TestQuestion" table:
                var testQuestion = new TestQuestion
                {
                    TQ_TestId = testId,
                    TQ_QuesId = questionId
                };

                // You can replace this with actual database logic (e.g., using Entity Framework)
                TestService testservice = new TestService();
                testservice.addQuestiontest(testQuestion);
                return true;
            }
            catch (Exception ex)
            {
                // Log the error if needed and return false
                Console.WriteLine(ex.Message);
                return false;
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

        [HttpGet]
        public ActionResult GetTopicsBySubject(int subjectId)
        {
            TopicService topicService = new TopicService();
            // Fetch topics based on subjectId from your service or database
            List<TopicShowViewModel> topics = topicService.GetTopicsBySubjectId(subjectId);

            if (topics != null && topics.Any())
            {
                // If topics are found, return them as JSON
                return Json(topics);
            }

            // If no topics found, return an empty list
            return Json(new List<TopicShowViewModel>());
        }
    }




}




