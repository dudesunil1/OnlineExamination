using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Controllers
{
    public class TopicMasterController : Controller
    {

        TopicService objTopicService = new TopicService();

        public ActionResult Index()
        {    

            List<TopicShowViewModel> objTopics = objTopicService.GetTopics();
            return View(objTopics);
        }

        
       

        

        public ActionResult Create()
        {            
            
            ViewBag.SubjectList =MasterService.GetSubjects();
            ViewBag.ClassList = MasterService.GetClass();

          

            TopicViewModel objTopic = new TopicViewModel();
            return View(objTopic);
        }

        [HttpPost]
        public ActionResult Create(TopicViewModel objTopic, string Action)
        {
            try
            {
                ViewBag.SubjectList = MasterService.GetSubjects();
                ViewBag.ClassList = MasterService.GetClass();
                if (Action == "Cancel")
                {
                    objTopic = new TopicViewModel();
                    return View(objTopic);
                }
                else if (Action == "Back To List")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        TopicViewModel res = objTopicService.Add(objTopic);
                        if (res.Top_Id > 0)
                        {
                            TempData["MessageModel"] = MessageModel.Success("Topid saved successfully!");
                        }
                        else
                        {
                            TempData["MessageModel"] = MessageModel.Error("An error occurred while saving the topic.");
                        }
                    }
                   

                    return View(objTopic);
                }
            }
            catch
            {
                TempData["MessageModel"] = MessageModel.Error("An unexpected error occurred.");
                return View(objTopic);
            }
        }

    }

}
