using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Controllers
{
    public class GroupDetailController : Controller
    {
        // GET: GroupDetails
       
        GroupDetailsService objgroupdetailsService = new GroupDetailsService();
        [HttpGet]
        public ActionResult Index()
        {

            List<GroupDetailsShowViewModel> objGroups = objgroupdetailsService.GetGroups();
            return View(objGroups);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.SubjectList = MasterService.GetSubjects();
            ViewBag.grouplist = MasterService.GetGroups();
            GroupDetailsShowViewModel objGroups = objgroupdetailsService.GetGroupsById(id).FirstOrDefault();

            if (objGroups == null)
            {
                return HttpNotFound();
            }

            return View(objGroups);
        }


        public ActionResult Edit(GroupDetailsViewModel objGroups, string Action)
        {
            try
            {

                if (Action == "Cancel")
                {
                    //objGroupDetails = new GroupDetailsViewModel();
                    //return View(objGroupDetails);

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
                        GroupDetailsViewModel res = objgroupdetailsService.Update(objGroups);

                        if (res.GD_Id > 0)
                        {
                            TempData["MessageModel"] = MessageModel.Success("Group Details saved successfully!");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["MessageModel"] = MessageModel.Error("An error occurred while saving the Group Details.");

                        }

                    }

                    return View(objGroups);
                }
            }
            catch
            {
                TempData["MessageModel"] = MessageModel.Error("An unexpected error occurred.");
                return View(objGroups); // Return view with the model on error
            }
        }

        public ActionResult Create()
        {

            ViewBag.SubjectList = MasterService.GetSubjects();
            ViewBag.grouplist = MasterService.GetGroups();



            GroupDetailsViewModel objGroupDetails = new GroupDetailsViewModel();
            return View(objGroupDetails);
        }

        [HttpPost]
        public ActionResult Create(GroupDetailsViewModel objGroupDetails, string Action)
        {
            try
            {
                ViewBag.SubjectList = MasterService.GetSubjects();
                ViewBag.grouplist = MasterService.GetGroups();

                if (Action == "Cancel")
                {
                    //objGroupDetails = new GroupDetailsViewModel();
                    //return View(objGroupDetails);

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
                        GroupDetailsViewModel res = objgroupdetailsService.Add(objGroupDetails);
                        if (res.GD_Id > 0)
                        {
                            TempData["MessageModel"] = MessageModel.Success("Group Details saved successfully!");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["MessageModel"] = MessageModel.Error("An error occurred while saving the Group Details.");
                        }
                    }


                    return View(objGroupDetails);
                }
            }
            catch
            {
                TempData["MessageModel"] = MessageModel.Error("An unexpected error occurred.");
                return View(objGroupDetails);
            }
        }

    }
}