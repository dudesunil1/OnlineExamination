using OnlineExamination.BLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.SunTechDB;

namespace OnlineExamination.Models
{
    public class StudentTestService
    {

        public List<TestStudent> GetstudetTest(int id)
        {
            try
            {
                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Stud_Id", id);

                DataTable dt = ControlFill.FillDataTable("SP_GetTestDetails", hashTable);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<TestStudent> list = ConversionFunctions.DataTableToList<TestStudent>(dt);
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

        public List<TestSubjectDetailsModel> GetTestSubjectDetails(int TS_Id)
        {
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("@TS_Id", TS_Id);
                DataTable dt = ControlFill.FillDataTable("SP_GetQuestionsPerSubject", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<TestSubjectDetailsModel> list = ConversionFunctions.DataTableToList<TestSubjectDetailsModel>(dt);
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