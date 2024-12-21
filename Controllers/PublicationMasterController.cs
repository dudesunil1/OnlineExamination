using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Controllers
{
    [CheckSessionRole]
    public class PublicationMasterController : Controller
    {
        PublicationService objPublicationService = new PublicationService();
     
        public ActionResult Index()
        {
           List<PublicationViewModel> objPublications =   objPublicationService.GetPublications();
            return View(objPublications);
        }
        public ActionResult Create()
        {
            PublicationViewModel objPublication = new PublicationViewModel();
            return View(objPublication);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            PublicationViewModel objPublication = objPublicationService.GetPublicationsById(id).FirstOrDefault();

            if (objPublication == null)
            {
                return HttpNotFound();
            }

            return View(objPublication);
        }


        public ActionResult Edit(PublicationViewModel objpublication, string Action)
        {
            try
            {

                if (Action == "Cancel")
                {
                    return RedirectToAction("Index");
                }
                else if (Action == "Back To List")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        PublicationViewModel res = objPublicationService.Update(objpublication);

                        if (res.Pub_Id > 0)
                        {
                            TempData["MessageModel"] = MessageModel.Success("Publication saved successfully!");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["MessageModel"] = MessageModel.Error("An error occurred while saving the Publication.");

                        }

                    }
                    
                    return View(objpublication);
                }
            }
            catch
            {
                TempData["MessageModel"] = MessageModel.Error("An unexpected error occurred.");
                 return View(objpublication); // Return view with the model on error
            }
        }


        [HttpPost]
        public ActionResult Create(PublicationViewModel objPublication, string Action)
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
                        PublicationViewModel res = objPublicationService.Add(objPublication);
                        if (res.Pub_Id > 0)
                        {
                            return RedirectToAction("Index");
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

    }
}