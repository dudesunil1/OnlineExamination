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
    public class TestResultController : Controller
    {

        public ActionResult List()
        {
            TestResultService testResultservice = new TestResultService();
            List<TestResultModel> testResults = testResultservice.GetAllResults();
            return View(testResults);
        }


    }
}