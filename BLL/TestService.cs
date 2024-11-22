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
    public class TestService
    {
        public List<TestType> GetTestType()
        {
            try
            {
                Hashtable hash = new Hashtable();

                DataTable dt = ControlFill.FillDataTable("Sel_TestType_Select", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<TestType> list = ConversionFunctions.DataTableToList<TestType>(dt);
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

        public TestMasterModel AddTestMaster(TestMasterModel objTest)
        {
            string _errMsg;
            try
            {
                //TimeSpan testDuration = TimeSpan.FromMinutes(objTest.Test_Duration); // model.Test_Duration is the int (in minutes)

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Test_Name", objTest.Test_Name);
               // hashTable.Add("@Test_GrpId", objTest.Test_GrpId);
                hashTable.Add("@Test_Duration", objTest.Test_Duration); 
                hashTable.Add("@Test_Mark", objTest.Test_Mark);
                hashTable.Add("@Test_StartTime", objTest.Test_StartTime);
                hashTable.Add("@Test_EndTime", objTest.Test_EndTime);
                hashTable.Add("@Test_TypeId", objTest.Test_TypeId);

               
                DataTable dt = clsSunDAL.FillDataTable("Sp_TestMaster_Insert", hashTable);
                _errMsg = clsSunDAL._errMsg;

               
                if (dt != null && dt.Rows.Count > 0)
                {
                   
                    objTest.Test_Id = Convert.ToInt32(dt.Rows[0]["Test_Id"]);
                    return objTest;  
                }

                return null;  

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                return null;  // Return null if an error occurs
            }
        }



        public TestQuestion addQuestiontest(TestQuestion objTest)
        {
            string _errMsg;
            try
            {
                //TimeSpan testDuration = TimeSpan.FromMinutes(objTest.Test_Duration); // model.Test_Duration is the int (in minutes)

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@TQ_QuesId", objTest.TQ_QuesId);
               hashTable.Add("@TQ_TestId", objTest.TQ_TestId);


                DataTable dt = clsSunDAL.FillDataTable("Sp_Insert_TestQuestion", hashTable);
                _errMsg = clsSunDAL._errMsg;


                if (dt != null && dt.Rows.Count > 0)
                {

                    objTest.TQ_Id = Convert.ToInt32(dt.Rows[0]["TQ_Id"]);
                    return objTest;
                }

                return null;

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                return null;  // Return null if an error occurs
            }
        }

    }
}