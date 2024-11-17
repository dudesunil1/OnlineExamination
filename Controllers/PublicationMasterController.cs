using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Controllers
{
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