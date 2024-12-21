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
    public class GroupMasterController : Controller
    {
        GroupService objGroupService = new GroupService();

        public ActionResult Index()
        {
           
            List<GroupViewModel> objGroups = objGroupService.GetGroups();
            return View(objGroups);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            GroupViewModel objGroups = objGroupService.GetGroupsById(id).FirstOrDefault();

            if (objGroups == null)
            {
                return HttpNotFound();
            }

            return View(objGroups);
        }


        public ActionResult Edit(GroupViewModel objGroups, string Action)
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
                        GroupViewModel res = objGroupService.Update(objGroups);

                        if (res.Grp_Id > 0)
                        {
                            TempData["MessageModel"] = MessageModel.Success("Group saved successfully!");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["MessageModel"] = MessageModel.Error("An error occurred while saving the Group.");

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
            GroupViewModel objGroup = new GroupViewModel();
            return View(objGroup);
        }

        [HttpPost]
        public ActionResult Create(GroupViewModel objGroup, string Action)
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
                        GroupViewModel res = objGroupService.Add(objGroup);
                        if (res.Grp_Id > 0)
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
