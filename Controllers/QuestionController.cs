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
    [CheckSessionRole]
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

       


        public ActionResult Create()
        {
            try
            {
                QuestionMasterViewModel objQuestion = new QuestionMasterViewModel();
                LoadData();
                return View(objQuestion);
            }
            catch (Exception)
            {
                // Log the exception if you have logging configured
                TempData["MessageModel"] = MessageModel.Error("An error occurred while loading the page. Please try again.");
                return RedirectToAction("Index");
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                QuestionMasterViewModel objQuestion = objQuestionService.GetQuestionsById(id).FirstOrDefault();
                if (objQuestion == null)
                {
                    TempData["MessageModel"] = MessageModel.Error("Question not found.");
                    return RedirectToAction("Index");
                }
                LoadData();
                return View(objQuestion);
            }
            catch (Exception)
            {
                TempData["MessageModel"] = MessageModel.Error("An error occurred while loading the question details.");
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                QuestionMasterViewModel objQuestion = objQuestionService.GetQuestionsById(id).FirstOrDefault();
                if (objQuestion == null)
                {
                    TempData["MessageModel"] = MessageModel.Error("Question not found.");
                    return RedirectToAction("Index");
                }
                LoadData();
                return View(objQuestion);
            }
            catch (Exception)
            {
                TempData["MessageModel"] = MessageModel.Error("An error occurred while loading the question for editing.");
                return RedirectToAction("Index");
            }
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



        public ActionResult Delete(int id)
        {
            try
            {
               
                    if (id > 0)
                    {
                        var result = objQuestionService.Delete(id);

                        if (result)
                        {
                            TempData["MessageModel"] = MessageModel.Success("Question deleted successfully!");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["MessageModel"] = MessageModel.Error("An error occurred while deleting the question.");
                        }
                    }
                    else
                    {
                        TempData["MessageModel"] = MessageModel.Error("Invalid question ID.");
                    }

                    LoadData();
                    return RedirectToAction("Index");
                
            }
            catch
            {
                TempData["MessageModel"] = MessageModel.Error("An unexpected error occurred.");
                LoadData();
                return RedirectToAction("Index");
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
                        
                        if (res != null && res.Ques_Id > 0)
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
                            TempData["MessageModel"] = MessageModel.Error("An error occurred while saving the question. Please ensure the stored procedure 'SP_QuestionMaster_Insert' exists in your database.");

                        }
                        
                    }
                    else
                    {
                        TempData["MessageModel"] = MessageModel.Error("Please fill all required fields correctly.");
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
            try
            {
                ViewBag.SubjectList = MasterService.GetSubjects() ?? new List<SelectListItem>();
                ViewBag.ClassList = MasterService.GetClass() ?? new List<SelectListItem>();
                ViewBag.PublicationList = MasterService.GetPublications() ?? new List<SelectListItem>();
                ViewBag.TopicList = MasterService.GetTopics() ?? new List<SelectListItem>();
                ViewData["ApiKey"] = apiKeyService?.GetAPIKey() ?? "";
            }
            catch (Exception)
            {
                // Log the exception if you have logging configured
                ViewBag.SubjectList = new List<SelectListItem>();
                ViewBag.ClassList = new List<SelectListItem>();
                ViewBag.PublicationList = new List<SelectListItem>();
                ViewBag.TopicList = new List<SelectListItem>();
                ViewData["ApiKey"] = "";
            }
        }

        [HttpPost]
        public JsonResult InActivateKey(string apiKey)
        {
            try
            {
                string responseMessage = $"Key Inactivated Successfully";
                apiKeyService.InActivateKey(apiKey);
                return Json(new { success = true, message = responseMessage });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while inactivating the key." });
            }
        }
    }
}