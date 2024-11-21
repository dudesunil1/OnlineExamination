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
    public class SubjectService
    {
        public List<SubjectsViewModel> GetSubjects()
        {
            try
            {
                Hashtable hash = new Hashtable();

                DataTable dt = ControlFill.FillDataTable("Sp_SubjectMaster_Select", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<SubjectsViewModel> list = ConversionFunctions.DataTableToList<SubjectsViewModel>(dt);
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
        public List<SubjectsViewModel> GetSubjectsById(int subid)
        {
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("@Sub_Id", subid);
                DataTable dt = ControlFill.FillDataTable("Sp_SubjectMaster_Select", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<SubjectsViewModel> list = ConversionFunctions.DataTableToList<SubjectsViewModel>(dt);
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


        public List<SubjectsViewModel> GetSubByClass(int ClassId)
        {
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("@ClassId", ClassId);

               
                DataTable dt = ControlFill.FillDataTable("GetSubjectsByClassId", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<SubjectsViewModel> list = ConversionFunctions.DataTableToList<SubjectsViewModel>(dt);
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

        public SubjectsViewModel AddSubject(SubjectsViewModel objSubject)
        {
            string _errMsg;
            try
            {
                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Sub_Name", objSubject.Sub_Name);
                 DataTable dt = clsSunDAL.FillDataTable("SP_SubjectMaster_Insert", hashTable);
                 _errMsg = clsSunDAL._errMsg;

                if (dt != null && dt.Rows.Count > 0)
                {
                     objSubject.Sub_Id = dt.Rows[0]["Sub_ID"].ToInt();
                    return objSubject; 
                }

                return null; 

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                return null; 
            }
        }
        public SubjectsViewModel Update(SubjectsViewModel objSubject)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Sub_Id", objSubject.Sub_Id);
                hashTable.Add("@Sub_Name", objSubject.Sub_Name);
                bool IsUpdate = clsSunDAL.ExecuteDMLQuery("SP_SubjectMaster_Update", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (IsUpdate)
                {

                    return objSubject;
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