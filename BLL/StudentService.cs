using OnlineExamination.Models;
using SunTech.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Web.SunTechDB;

namespace OnlineExamination.BLL
{
    public class StudentService
    {
        public List<StudentMasterModel> GetStudent()
        {
            try
            {
                Hashtable hash = new Hashtable();
                DataTable dt = ControlFill.FillDataTable("SP_StudentMaster_Select", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<StudentMasterModel> list = ConversionFunctions.DataTableToList<StudentMasterModel>(dt);
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

        public List<StudentMasterModel> GetStudentById(int id)
        {
            try
            {
                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Stud_Id", id);

                DataTable dt = ControlFill.FillDataTable("SP_StudentMaster_Select", hashTable);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<StudentMasterModel> list = ConversionFunctions.DataTableToList<StudentMasterModel>(dt);
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


        public StudentMasterModel Add(StudentMasterModel objStudent, string extension)
        {
            string _errMsg;
            try
            {

               

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Stud_Name", objStudent.Stud_Name);
                hashTable.Add("@Stud_Mobile", objStudent.Stud_Mobile);
                hashTable.Add("@Stud_DOB", objStudent.Stud_DOB);
                hashTable.Add("@Stud_Gender", objStudent.Stud_Gender);
                hashTable.Add("@Stud_UserName", objStudent.Stud_UserName);
                hashTable.Add("@Stud_Password", objStudent.Stud_Password);
                hashTable.Add("@Stud_AdmissionDate", objStudent.Stud_AdmissionDate);
                hashTable.Add("@Stud_Group", objStudent.Stud_Group);
                hashTable.Add("@Stud_Class", objStudent.Stud_Class);
                hashTable.Add("@Stud_Photo", extension);


             //   hashTable.Add("@Stud_IsActive", objStudent.Stud_IsActive);
                DataTable dt = clsSunDAL.FillDataTable("SP_StudentMaster_Insert", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objStudent.Stud_Id = dt.Rows[0]["Stud_Id"].ToInt();

                    return objStudent;
                }
                return null;

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                return null;
            }
        }


        public StudentMasterModel Update(StudentMasterModel objStudent,string extension)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Stud_Id", objStudent.Stud_Id);
                hashTable.Add("@Stud_Name", objStudent.Stud_Name);
                hashTable.Add("@Stud_Mobile", objStudent.Stud_Mobile);
                hashTable.Add("@Stud_DOB", objStudent.Stud_DOB);
                hashTable.Add("@Stud_Gender", objStudent.Stud_Gender);
                hashTable.Add("@Stud_UserName", objStudent.Stud_UserName);
                hashTable.Add("@Stud_Password", objStudent.Stud_Password);
                hashTable.Add("@Stud_AdmissionDate", objStudent.Stud_AdmissionDate);
                hashTable.Add("@Stud_Group", objStudent.Stud_Group);
                hashTable.Add("@Stud_Photo", extension);
                hashTable.Add("@Stud_Class", objStudent.Stud_Class);
                //   hashTable.Add("@Stud_IsActive", objStudent.Stud_IsActive);
                DataTable dt = clsSunDAL.FillDataTable("SP_StudentMaster_Update", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objStudent.Stud_Id = dt.Rows[0]["Stud_Id"].ToInt();
                    return objStudent;
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