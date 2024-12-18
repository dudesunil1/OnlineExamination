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
    public class TestResultService
    {
        public List<TestResultModel> GetAllResults()
        {
            return GetTestResults(0, 0);

        }
        public List<TestResultModel> GetTestResults(int TestId = 0, int StudentId = 0)
        {
            try
            {
                Hashtable hash = new Hashtable();

                hash.Add("@TestId", TestId);
                hash.Add("@StudentId", StudentId);
               
                DataTable dt = ControlFill.FillDataTable("SP_GetTestResults", hash);
                if (dt != null && dt.Rows.Count > 0)
                {


                    List<TestResultModel> list = ConversionFunctions.DataTableToList<TestResultModel>(dt);
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


    }
}