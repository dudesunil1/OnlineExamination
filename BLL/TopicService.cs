using OnlineExamination.Models;
using SunTech.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.SunTechDB;

namespace OnlineExamination.BLL
{
    public class TopicService
    {

        public List<TopicShowViewModel> GetTopics()
        {
            return GetTopicsList(0, 0, 0);

        }

        public List<TopicShowViewModel> GetTopicsByClassId(int Classid)
        {
            return GetTopicsList(0, Classid, 0);

        }
        public List<TopicShowViewModel> GetTopicsBySubjectId(int SubjectId)
        {
            return GetTopicsList(0, 0, SubjectId);

        }
        public List<TopicShowViewModel> GetTopicsBySubjectClass(int SubjectId,int ClassId)
        {
            return GetTopicsList(0, ClassId, SubjectId);
        }
        public List<TopicShowViewModel> GetTopicsList(int TopicId=0,int ClassId=0,int SubjectId=0 )
        {
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("@Top_Id", TopicId);

                hash.Add("@Top_SubId", SubjectId);
                hash.Add("@Top_ClassID", ClassId);
                DataTable dt = ControlFill.FillDataTable("SP_TopicMaster_Select", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<TopicShowViewModel> list = ConversionFunctions.DataTableToList<TopicShowViewModel>(dt);
                    return list;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        
      
      

        public TopicViewModel Add(TopicViewModel objTopic)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Top_Name", objTopic.Top_Name);
                hashTable.Add("@Top_SubId", objTopic.Top_SubId);
                hashTable.Add("@Top_ClassId", objTopic.Top_ClassId);

                DataTable dt = clsSunDAL.FillDataTable("SP_TopicMaster_Insert", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objTopic.Top_Id = dt.Rows[0]["Top_Id"].ToInt();
                    return objTopic;
                }
                return null;

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                return null;
            }
        }

        public TopicViewModel Update(TopicViewModel objTopic)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Top_Id", objTopic.Top_Id);
                hashTable.Add("@Top_Name", objTopic.Top_Name);
                hashTable.Add("@Top_SubId", objTopic.Top_SubId);
                hashTable.Add("@Top_ClassId", objTopic.Top_ClassId);
                bool IsUpdate = clsSunDAL.ExecuteDMLQuery("SP_TopicMaster_Update", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (IsUpdate)
                {

                    return objTopic;
                }
                return null;

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                return null;
            }
        }

    }
}