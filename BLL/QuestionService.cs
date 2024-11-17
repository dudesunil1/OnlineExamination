
using OnlineExamination.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System;
using Web.SunTechDB;
using SunTech.Util;

namespace OnlineExamination.BLL
{

    public class QuestionMasterService
    {
        public List<QuestionMasterViewModel> GetAllQuestions()
        {
            return GetQuestions(0, 0, 0);

        }
        public List<QuestionMasterViewModel> GetQuestionsById(int QuestionId)
        {
            return GetQuestions(QuestionId, 0, 0);

        }
        public List<QuestionMasterViewModel> GetQuestionsBySubjectId(int SubjectId)
        {
            return GetQuestions(0, SubjectId, 0);

        }
        public List<QuestionMasterViewModel> GetQuestionsByTopicId(int TopicId)
        {
            return GetQuestions(0, 0, TopicId);

        }

        public List<QuestionMasterViewModel> GetQuestions(int QuestionId = 0, int SubjectId = 0, int TopicId = 0)
        {
            try
            {
                Hashtable hash = new Hashtable();

                hash.Add("QuestionId", QuestionId);
                hash.Add("SubjectId", SubjectId);
                hash.Add("TopicId", TopicId);

                DataTable dt = ControlFill.FillDataTable("Sel_QuestionMaster_Select", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<QuestionMasterViewModel> list = ConversionFunctions.DataTableToList<QuestionMasterViewModel>(dt);
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

        public QuestionMasterViewModel Add(QuestionMasterViewModel objQuestion)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Ques_SubId", objQuestion.Ques_SubId);

                hashTable.Add("@Ques_TopId", objQuestion.Ques_TopId);

                hashTable.Add("@Ques_PubId", objQuestion.Ques_PubId);

                hashTable.Add("@Ques_Mark", objQuestion.Ques_Mark);

                hashTable.Add("@Ques_JEEMark", objQuestion.Ques_JEEMark);

                hashTable.Add("@Ques_Negative", objQuestion.Ques_Negative);

                hashTable.Add("@Ques_Question", objQuestion.Ques_Question);

                hashTable.Add("@Ques_Answer", objQuestion.Ques_Answer);

                hashTable.Add("@Ques_OptionB", objQuestion.Ques_OptionB);

                hashTable.Add("@Ques_OptionC", objQuestion.Ques_OptionC);

                hashTable.Add("@Ques_OptionD", objQuestion.Ques_OptionD);

                hashTable.Add("@Ques_SolutionDetails", objQuestion.Ques_SolutionDetails);

                DataTable dt = clsSunDAL.FillDataTable("SP_QuestionMaster_Insert", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objQuestion.Ques_Id = dt.Rows[0]["Ques_Id"].ToInt();
                    return objQuestion;
                }
                return null;

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                return null;
            }
        }

        public QuestionMasterViewModel Update(QuestionMasterViewModel objQuestion)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();

                hashTable.Add("@Ques_Id", objQuestion.Ques_Id);
                hashTable.Add("@Ques_SubId", objQuestion.Ques_SubId);

                hashTable.Add("@Ques_TopId", objQuestion.Ques_TopId);

                hashTable.Add("@Ques_PubId", objQuestion.Ques_PubId);

                hashTable.Add("@Ques_Mark", objQuestion.Ques_Mark);

                hashTable.Add("@Ques_JEEMark", objQuestion.Ques_JEEMark);

                hashTable.Add("@Ques_Negative", objQuestion.Ques_Negative);

                hashTable.Add("@Ques_Question", objQuestion.Ques_Question);

                hashTable.Add("@Ques_Answer", objQuestion.Ques_Answer);

                hashTable.Add("@Ques_OptionB", objQuestion.Ques_OptionB);

                hashTable.Add("@Ques_OptionC", objQuestion.Ques_OptionC);

                hashTable.Add("@Ques_OptionD", objQuestion.Ques_OptionD);

                hashTable.Add("@Ques_SolutionDetails", objQuestion.Ques_SolutionDetails);

                bool IsUpdate = clsSunDAL.ExecuteDMLQuery("SP_QuestionMaster_Update", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (IsUpdate)
                {

                    return objQuestion;
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