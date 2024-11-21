using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExamination.BLL
{
    public class MasterService
    {
        public static List<SelectListItem> GetClass()
        {
            ClassService classService = new ClassService();
            List<ClassViewModel> objClass = classService.GetClass();
            if (objClass.Count > 0)
            {
                return objClass.Select(s => new SelectListItem
                {
                    Value = s.ID.ToString(),  // Use appropriate property for Value
                    Text = s.Name             // Use appropriate property for Text
                }).ToList();

            }
            else
            {
                return null;
            }
        }
        public static List<SelectListItem> GetGroups()
        {
            GroupService groupService = new GroupService();
            List<GroupViewModel> objClass = groupService.GetGroups();
            if (objClass.Count > 0)
            {
                return objClass.Select(s => new SelectListItem
                {
                    Value = s.Grp_Id.ToString(),  // Use appropriate property for Value
                    Text = s.Grp_Name             // Use appropriate property for Text
                }).ToList();

            }
            else
            {
                return null;
            }
        }
        public static List<SelectListItem> GetSubjects()
        {
            SubjectService subjectService = new SubjectService();
            List<SubjectsViewModel> objSubjects = subjectService.GetSubjects();
            if (objSubjects.Count > 0)
            {
                return objSubjects.Select(s => new SelectListItem
                {
                    Value = s.Sub_Id.ToString(),  // Use appropriate property for Value
                    Text = s.Sub_Name             // Use appropriate property for Text
                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public static List<SelectListItem> GetTestType()
        {
            TestService testService = new TestService();
            List<TestType> objtest = testService.GetTestType();
            if (objtest.Count > 0)
            {
                return objtest.Select(s => new SelectListItem
                {
                    Value = s.TT_Id.ToString(),  // Use appropriate property for Value
                    Text = s.TT_Name             // Use appropriate property for Text
                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public static List<SelectListItem> GetPublications()
        {
            PublicationService publicationService = new PublicationService();
            List<PublicationViewModel> objSubjects = publicationService.GetPublications();
            if (objSubjects.Count > 0)
            {
                return objSubjects.Select(s => new SelectListItem
                {
                    Value = s.Pub_Id.ToString(),  // Use appropriate property for Value
                    Text = s.Pub_Name             // Use appropriate property for Text
                }).ToList();
            }
            else
            {
                return null;
            }
        }
        public static List<SelectListItem> GetTopics()
        {
            TopicService topicService = new TopicService();
            List<TopicShowViewModel> objTopics = topicService.GetTopics();
            if (objTopics.Count > 0)
            {
                return objTopics.Select(s => new SelectListItem
                {
                    Value = s.Top_Id.ToString(),  // Use appropriate property for Value
                    Text = s.Top_Name             // Use appropriate property for Text
                }).ToList();
            }
            else
            {
                return null;
            }
        }


    }
}