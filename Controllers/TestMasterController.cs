using Newtonsoft.Json;
using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

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



            LoadData();

            ViewBag.ActiveTab = "test-details";
            
            return View();
        }

        [HttpPost]
        
        public JsonResult StoreSessionData(List<QuestionPaperData> model)
        {
            SubjectService objSubjectService = new SubjectService();
            TopicService objTopicService = new TopicService();
            QuestionMasterService objQuestionService = new QuestionMasterService();
            TestService objTestService = new TestService();

            // Dummy TestId for example, adjust as needed
            int testId = Session["TestId"] != null ? Convert.ToInt32(Session["TestId"]) : 0;
            List<TestMasterModel> testList = objTestService.GetTests(testId);
            var test = testList.FirstOrDefault(t => t.Test_Id == testId);

            var allSubjectsResponse = new List<object>();

            foreach (var item in model)
            {
                // Prepare subject name from session or service as needed
                var selectedSubjects = Session["SelectedSubjects"] as Dictionary<int, (string Name, int QuestionCount)>;
                string subjectName = "";
                if (selectedSubjects != null && selectedSubjects.Any())
                {
                    // Find current subject name by Id
                    if (selectedSubjects.TryGetValue(item.SubjectId, out var subjectData))
                    {
                        subjectName = $"{subjectData.Name} ({subjectData.QuestionCount})";
                    }
                }

                // Convert lists to comma-separated strings
                string topicIdsStr = string.Join(",", item.TopicIds ?? new List<int>());
                string questionIdsStr = string.Join(",", item.QuestionIds ?? new List<int>());
                string topicNames = item.TopicNames ?? "";

                // Fetch questions based on selected IDs
                List<int> questionIds = questionIdsStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => Convert.ToInt32(id.Trim())).ToList();

                var allQuestions = new List<QuestionMasterViewModel>();

                foreach (int qid in questionIds)
                {
                    var questions = objQuestionService.GetQuestionsById(qid);
                    if (questions != null && questions.Any())
                    {
                        allQuestions.AddRange(questions);
                    }
                }

                var simplifiedQuestions = allQuestions.Select(q => new
                {
                    Id = q.Ques_Id,
                    QuestionText = q.Ques_Question,
                }).ToList();

                DateTime dateTime = test != null ? test.Test_StartTime : DateTime.Now;
                string testdatetime = dateTime.ToString("dd-MM-yyyy HH:mm:ss");

                allSubjectsResponse.Add(new
                {
                    SubjectId = item.SubjectId,
                    subjectName = subjectName,
                    topicIds = topicIdsStr,
                    questionIds = questionIdsStr,
                    testName = test?.Test_Name ?? "",
                    testType = test?.Test_Mark ?? 0,
                    testduration = test?.Test_Duration ?? 0,
                    testdatetime = testdatetime,
                    testMark = test?.Test_Mark ?? 0,
                    topicNames = topicNames,
                    simplifiedQuestions = simplifiedQuestions
                });
            }

            return Json(new
            {
                success = true,
                data = allSubjectsResponse
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Genratepaper(List<int> Id)
        {
            if (Id != null && Id.Count > 0)
            {
                int testId = Convert.ToInt32(Session["TestId"]);

                foreach (int qid in Id)
                {
                    TestQuestion model = new TestQuestion
                    {
                        TQ_TestId = testId,
                        TQ_QuesId = qid
                    };

                    objtestService.addQuestiontest(model);
                }

                TempData["MessageModel"] = MessageModel.Success("Test saved successfully!");
                return RedirectToAction("Create");


            }

            TempData["MessageModel"] = MessageModel.Error("An error occurred while saving the Test.");
            return RedirectToAction("Create");
        }



        private void LoadData()
        {
            ViewBag.ClassList = MasterService.GetClass();
            ViewBag.Testtypelist = MasterService.GetTestType();
            ViewBag.SubjectList = MasterService.GetSubjects();
            ViewBag.SubjectId = null;
           


        }




        [HttpGet]
        public JsonResult GetTopicsBySubjectId(int subjectId)
        {
            try
            {
                if (subjectId <= 0)
                {
                    return Json(new { error = true, message = "Invalid subject ID." }, JsonRequestBehavior.AllowGet);
                }

                TopicService objtopicService = new TopicService();
                var topics = objtopicService.GetTopicsBySubjectId(subjectId); // already created by you

                // Return as JSON
                return Json(topics, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

       

        [HttpGet]
        public ContentResult GetQuestionsByTopicIds(List<int> topicIds)
        {
            var service = new QuestionMasterService();
            var allQuestions = new List<QuestionMasterViewModel>();

            if (topicIds != null && topicIds.Any())
            {
                foreach (int id in topicIds)
                {
                    var questions = service.GetQuestionsByTopicId(id);
                    if (questions != null)
                    {
                        allQuestions.AddRange(questions);
                    }
                }
            }

            if (allQuestions.Any())
            {
                var html = new StringBuilder();

                html.AppendLine("<div style='max-height: 300px; overflow-y: auto;'>");
                html.AppendLine("<table style='width: 100%; border-collapse: collapse; border: 1px solid #ccc;'>");
                html.AppendLine("<thead>");
                html.AppendLine("<tr style='background-color: #f4f4f4;'>");
                html.AppendLine("<th style='padding: 8px; text-align: center;'>Select</th>");
                html.AppendLine("<th style='padding: 8px;'>#</th>");
                html.AppendLine("<th style='padding: 8px;'>Question</th>");
                html.AppendLine("</tr>");
                html.AppendLine("</thead>");
                html.AppendLine("<tbody>");

                int index = 1;
                foreach (var q in allQuestions)
                {
                    html.AppendLine("<tr>");
                    html.AppendLine($"<td style='text-align: center; padding: 8px;'><input type='checkbox' id='q{index}' name='questions' value='{q.Ques_Id}' /></td>");

                    html.AppendLine($"<td style='padding: 8px;'>{index}</td>");
                    html.AppendLine($"<td style='padding: 8px;'><label for='q{index}'>{q.Ques_Question}</label></td>");
                    html.AppendLine("</tr>");
                    index++;
                }

                html.AppendLine("</tbody>");
                html.AppendLine("</table>");
                html.AppendLine("</div>");

                return Content(html.ToString(), "text/html");
            }

            return Content("<p>No questions found.</p>", "text/html");
        }


       
      
       




        [HttpPost]
        public ActionResult Create(TestMasterModel objTest, string Action)
        {
            try
            {
                // Navigation logic
                if (Action == "Cancel")
                {
                    return RedirectToAction("Create");
                }
                else if (Action == "Back To List")
                {
                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {
                    // Save test
                    TestMasterModel res = objtestService.AddTestMaster(objTest);

                    if (res.Test_Id > 0)
                    {
                        LoadData();
                        Session["TestId"] = res.Test_Id;

                        var subjectList = ViewBag.SubjectList as List<SelectListItem>;
                        var selectedSubjects = new Dictionary<int, (string Name, int QuestionCount)>();

                        foreach (string key in Request.Form.AllKeys)
                        {
                            if (key.StartsWith("NoOfQuestions_"))
                            {
                                string subjectIdStr = key.Replace("NoOfQuestions_", "");
                                string questionCountStr = Request.Form[key];

                                if (int.TryParse(subjectIdStr, out int subjectId) &&
                                    int.TryParse(questionCountStr, out int numQuestions) &&
                                    numQuestions > 0)
                                {
                                    // Get subject name from the subject list
                                    string subjectName = subjectList?.FirstOrDefault(s => s.Value == subjectIdStr)?.Text ?? "Unknown";

                                    selectedSubjects[subjectId] = (Name: subjectName, QuestionCount: numQuestions);
                                }
                            }
                        }

                        Session["SelectedSubjects"] = selectedSubjects;
                        ViewBag.TestId = res.Test_Id;
                        ViewBag.ActiveTab = "question-details";

                        return View(res); // Stay in same request to preserve session
                    }
                }


                LoadData();
                return View(objTest);
            }
            catch (Exception ex)
            {
                TempData["MessageModel"] = MessageModel.Error("An unexpected error occurred: " + ex.Message);
                LoadData();
                return View(objTest);
            }
        }

        
      

    }




}




