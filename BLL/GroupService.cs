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
   
    public class GroupService
    {       
        public List<GroupViewModel> GetGroups()
        {
            try
            {               
                Hashtable hash = new Hashtable();
                DataTable dt = ControlFill.FillDataTable("Sp_GroupMaster_Select", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<GroupViewModel> list = ConversionFunctions.DataTableToList<GroupViewModel>(dt);
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

        public GroupViewModel Add(GroupViewModel objGroup) 
        {
            string _errMsg;
            try
            {               
                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Grp_Name", objGroup.Grp_Name);
                DataTable dt = clsSunDAL.FillDataTable("SP_GroupMaster_Insert", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objGroup.Grp_Id = dt.Rows[0]["Grp_Id"].ToInt();
                    return objGroup;
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