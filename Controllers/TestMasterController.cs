using Newtonsoft.Json;
using OnlineExamination.BLL;
using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
                        // Add TestMaster
                        TestMasterModel res = objtestService.AddTestMaster(objTest);

                        if (res.Test_Id > 0)
                        {
                           
                            ViewBag.TestId = res.Test_Id;
                            ViewBag.ActiveTab = "question-details";

                            LoadData();
                            return View(res);
                        }
                        else
                        {
                            return RedirectToAction("Create");
                        }
                    }
                    LoadData();
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




