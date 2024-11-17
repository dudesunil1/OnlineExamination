using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.Controllers
{
    public class GroupMasterController : Controller
    {
        GroupService objGroupService = new GroupService();

        public ActionResult Index()
        {
            List<GroupViewModel> objGroups = objGroupService.GetGroups();
            return View(objGroups);
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
