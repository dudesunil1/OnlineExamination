using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Controllers
{
    public class QuestionController : Controller
    {
        QuestionMasterService objQuestionService = new QuestionMasterService();
        TopicService objTopicsService = new TopicService();
        PublicationService objPublicationService = new PublicationService();
        ApiKeyService apiKeyService = new ApiKeyService();
        public ActionResult Index()
        {
            LoadData();
           List<QuestionMasterViewModel> objQuestions = objQuestionService.GetAllQuestions();
            return View(objQuestions);

        }

       


        QuestionMasterViewModel objQuestion;
        public ActionResult Create()
        {
            if(objQuestion == null)
                objQuestion = new QuestionMasterViewModel();
            //ViewBag.SubjectList = GetSubjects();
            //ViewBag.ClassList = GetClass();
            //ViewBag.PublicationList = GetPublications();

            //ViewData["ApiKey"] = apiKeyService.GetAPIKey();
            LoadData();
            return View(objQuestion);
        }

        public ActionResult Details(int id)
        {
            objQuestion= objQuestionService.GetQuestionsById(id).FirstOrDefault();
            LoadData();
            return View(objQuestion);
        }

        public ActionResult Edit(int id)
        {
            objQuestion = objQuestionService.GetQuestionsById(id).FirstOrDefault();
            LoadData();
            return View(objQuestion);
        }

        [HttpPost]
        public ActionResult Edit(QuestionMasterViewModel objQuestion, string Action)
        {
            try
            {
                
                if (Action == "Back To List")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        QuestionMasterViewModel res = objQuestionService.Update(objQuestion);

                        if (res.Ques_Id > 0)
                        {
                            TempData["MessageModel"] = MessageModel.Success("Question saved successfully!");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["MessageModel"] = MessageModel.Error("An error occurred while saving the question.");

                        }

                    }
                    LoadData();
                    return View(objQuestion);
                }
            }
            catch
            {
                TempData["MessageModel"] = MessageModel.Error("An unexpected error occurred.");
                LoadData();
                return View(objQuestion); // Return view with the model on error
            }
        }


        [HttpPost]
        public ActionResult Create(QuestionMasterViewModel objQuestion, string Action)
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
                        QuestionMasterViewModel res = objQuestionService.Add(objQuestion);
                        
                        if (res.Ques_Id > 0)
                        {
                            objQuestion.Ques_Id = 0;
                            objQuestion.Ques_Question =
                                objQuestion.Ques_Answer =
                                objQuestion.Ques_OptionB =
                                objQuestion.Ques_OptionC =
                                objQuestion.Ques_OptionD =
                                objQuestion.Ques_SolutionDetails = "";

                            TempData["MessageModel"] = MessageModel.Success("Question saved successfully!");
                        }
                        else
                        {
                            TempData["MessageModel"] = MessageModel.Error("An error occurred while saving the question.");

                        }
                        
                    }
                    LoadData();
                    return View(objQuestion);
                }
            }
            catch
            {
                TempData["MessageModel"] = MessageModel.Error("An unexpected error occurred.");
                LoadData();
                return View(objQuestion); // Return view with the model on error
            }
        }

        private void LoadData()
        {
            ViewBag.SubjectList =MasterService.GetSubjects();
            ViewBag.ClassList = MasterService.GetClass();
            ViewBag.PublicationList = MasterService.GetPublications();
            ViewBag.TopicList = MasterService.GetTopics();
            ViewData["ApiKey"] = apiKeyService.GetAPIKey();
        }

        [HttpPost]
        public async Task<JsonResult> InActivateKey(string apiKey)
        {
            string responseMessage = $"Key Inactivated Successfully";
            apiKeyService.InActivateKey(apiKey);

            return Json(new { success = true, message = responseMessage });
           
        }
    }
}