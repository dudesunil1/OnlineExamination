using OnlineExamination.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.SunTechDB;

namespace OnlineExamination.BLL
{
    public class TestStudentQuestionService
    {
        public List<TestStudentQuestionModel> GetTestStudentQuestions(int TestID,int StudentId)
        {
            try
            {
                Hashtable hash = new Hashtable();

                hash.Add("@TestId", TestID);
                hash.Add("@StudentId", StudentId);


                DataTable dt = ControlFill.FillDataTable("SP_GetTestStudentQuestions", hash);
                if (dt != null && dt.Rows.Count > 0)
                {


                    List<TestStudentQuestionModel> list = ConversionFunctions.DataTableToList<TestStudentQuestionModel>(dt);
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
        public TestStudentQuestionModel SubmitAnswer(TestStudentQuestionModel objQuestion)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@STQ_Id", objQuestion.STQ_Id);
                hashTable.Add("@STQ_TestId", objQuestion.STQ_TestId);
                hashTable.Add("@STQ_StudId", objQuestion.STQ_StudId);
                hashTable.Add("@STQ_StudentAns", objQuestion.STQ_StudentAns);


                

                bool IsUpdate = clsSunDAL.ExecuteDMLQuery("SP_TestStudentQuestion_SubmitAnswer", hashTable);
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