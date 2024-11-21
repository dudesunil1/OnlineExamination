using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.BLL
{
    public class MasterController : Controller
    {
        // GET: Master
        public JsonResult GetTopicsBySubjectIDAndClassID(int SubjectID, int ClassID)
        {
            TopicService topicsService = new TopicService();
            List<TopicShowViewModel> objSubSubjects = topicsService.GetTopicsBySubjectClass(SubjectID, ClassID);
            return Json(objSubSubjects, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubjectsByClass(int ClassID)
        {
            SubjectService subjectsService = new SubjectService();
            List<SubjectsViewModel> objSubSubjects = subjectsService.GetSubByClass(ClassID);
            return Json(objSubSubjects, JsonRequestBehavior.AllowGet);
        }
    }
}