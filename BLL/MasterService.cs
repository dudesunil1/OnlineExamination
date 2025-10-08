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
            try
            {
                ClassService classService = new ClassService();
                List<ClassViewModel> objClass = classService.GetClass();
                if (objClass != null && objClass.Count > 0)
                {
                    return objClass.Select(s => new SelectListItem
                    {
                        Value = s.ID.ToString(),  // Use appropriate property for Value
                        Text = s.Name             // Use appropriate property for Text
                    }).ToList();
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging configured
                return new List<SelectListItem>();
            }
        }
        public static List<SelectListItem> GetGroups()
        {
            try
            {
                GroupService groupService = new GroupService();
                List<GroupViewModel> objgroup = groupService.GetGroups();
                if (objgroup != null && objgroup.Count > 0)
                {
                    return objgroup.Select(s => new SelectListItem
                    {
                        Value = s.Grp_Id.ToString(),  // Use appropriate property for Value
                        Text = s.Grp_Name             // Use appropriate property for Text
                    }).ToList();
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }

        public static List<SelectListItem> GetTest()
        {
            try
            {
                TestService testService = new TestService();
                List<TestMasterModel> objgroup = testService.GetTests();
                if (objgroup != null && objgroup.Count > 0)
                {
                    return objgroup.Select(s => new SelectListItem
                    {
                        Value = s.Test_Id.ToString(),  // Use appropriate property for Value
                        Text = s.Test_Name             // Use appropriate property for Text
                    }).ToList();
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }

        public static List<SelectListItem> GetSubjects()
        {
            try
            {
                SubjectService subjectService = new SubjectService();
                List<SubjectsViewModel> objSubjects = subjectService.GetSubjects();
                if (objSubjects != null && objSubjects.Count > 0)
                {
                    return objSubjects.Select(s => new SelectListItem
                    {
                        Value = s.Sub_Id.ToString(),  // Use appropriate property for Value
                        Text = s.Sub_Name             // Use appropriate property for Text
                    }).ToList();
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }
        public static List<SelectListItem> GetTestType()
        {
            try
            {
                TestService testService = new TestService();
                List<TestType> objtest = testService.GetTestType();
                if (objtest != null && objtest.Count > 0)
                {
                    return objtest.Select(s => new SelectListItem
                    {
                        Value = s.TT_Id.ToString(),  // Use appropriate property for Value
                        Text = s.TT_Name             // Use appropriate property for Text
                    }).ToList();
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }
        public static List<SelectListItem> GetPublications()
        {
            try
            {
                PublicationService publicationService = new PublicationService();
                List<PublicationViewModel> objSubjects = publicationService.GetPublications();
                if (objSubjects != null && objSubjects.Count > 0)
                {
                    return objSubjects.Select(s => new SelectListItem
                    {
                        Value = s.Pub_Id.ToString(),  // Use appropriate property for Value
                        Text = s.Pub_Name             // Use appropriate property for Text
                    }).ToList();
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }
        public static List<SelectListItem> GetTopics()
        {
            try
            {
                TopicService topicService = new TopicService();
                List<TopicShowViewModel> objTopics = topicService.GetTopics();
                if (objTopics != null && objTopics.Count > 0)
                {
                    return objTopics.Select(s => new SelectListItem
                    {
                        Value = s.Top_Id.ToString(),  // Use appropriate property for Value
                        Text = s.Top_Name             // Use appropriate property for Text
                    }).ToList();
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }


    }
}