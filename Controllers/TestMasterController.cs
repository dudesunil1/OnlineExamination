using Newtonsoft.Json;
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

        public ActionResult GetTopicsBySubjectId(int subjectId, string selectedTopicIds)
        {
            // Create instance of TopicService to retrieve topics
            TopicService topicService = new TopicService();
            var topics = topicService.GetTopicsBySubjectId(subjectId); // Retrieve topics for the selected subject

            // Initialize an empty dictionary to hold selected topic IDs by subject
            Dictionary<int, List<int>> selectedTopicsBySubject = new Dictionary<int, List<int>>();

            // Check if selectedTopicIds is not null or empty
            if (!string.IsNullOrEmpty(selectedTopicIds))
            {
                try
                {
                    // Deserialize the selected topic IDs JSON string into a dictionary
                    selectedTopicsBySubject = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(selectedTopicIds);
                }
                catch (Exception ex)
                {
                    // Log the exception (optional)
                    // For now, just make sure to handle any deserialization errors gracefully.
                    Console.WriteLine("Error deserializing selectedTopicIds: " + ex.Message);
                }
            }

            // Initialize the list to hold the selected topic IDs for this subject
            List<int> selectedIdsForSubject = selectedTopicsBySubject.ContainsKey(subjectId)
                ? selectedTopicsBySubject[subjectId]
                : new List<int>();

            // Create a variable to hold the generated HTML content
            string htmlContent = string.Empty;

            // Check if there are any topics
            if (topics != null && topics.Any())
            {
                // Start the container for the checkboxes
                htmlContent += "<div class='checkbox-list'>";

                foreach (var topic in topics)
                {
                    // Check if the current topic's ID is in the selected list for this subject
                    bool isChecked = selectedIdsForSubject.Contains(topic.Top_Id);

                    // Add a checkbox input and check it if it's in the selected topic IDs
                    htmlContent += $@"
            <div class='form-check'>
                <input type='checkbox' class='topic-checkbox' id='topic{topic.Top_Id}' name='SelectedTopics' value='{topic.Top_Id}' {(isChecked ? "checked" : "")}>
                <label class='form-check-label' for='topic{topic.Top_Id}'>
                    {topic.Top_Name}
                </label>
            </div>";
                }

                // Close the checkbox list container
                htmlContent += "</div>";
            }
            else
            {
                // If no topics are available for the selected subject
                htmlContent = "<p>No topics available for the selected subject.</p>";
            }

            // Return the generated HTML as JSON to be used by the frontend
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
