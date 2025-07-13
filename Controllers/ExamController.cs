using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineExamination.BLL;
using OnlineExamination.Models;
using SunTech.Util;
namespace OnlineExamination.Controllers
{
    public class ExamController : Controller
    {
        TestStudentQuestionService objQuestionService = new TestStudentQuestionService();
        public ActionResult Index(int? testId )
        {


            List<TestStudentQuestionModel> objQuestions = objQuestionService.GetTestStudentQuestions(1, 1);

             
 
            return View(objQuestions);
        }

        [HttpPost]
        public ActionResult Submit(List<TestStudentQuestionModel> question)
        {



            foreach (var q in question)
            {
                  objQuestionService.SubmitAnswer(q);
            }
            return View("Result");
        }

      

    }
}