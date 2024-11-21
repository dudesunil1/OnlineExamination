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
    public class GroupDetailsService
    {
        public List<GroupDetailsShowViewModel> GetGroups()
        {
            return GetGroupDetails(0,0,0);

        }

        public List<GroupDetailsShowViewModel> GetGroupsById(int GDID)
        {
            return GetGroupDetails(GDID, 0, 0);

        }
        public GroupDetailsViewModel Update(GroupDetailsViewModel objGroup)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@GD_Id", objGroup.GD_Id);
                hashTable.Add("@GD_GrpId", objGroup.GD_GrpId);
                hashTable.Add("@GD_SubId", objGroup.GD_SubId);
                bool IsUpdate = clsSunDAL.ExecuteDMLQuery("Sp_GroupDetails_Update", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (IsUpdate)
                {

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


        public GroupDetailsViewModel Add(GroupDetailsViewModel objGroupDetail)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@GD_GrpId", objGroupDetail.GD_GrpId);
                hashTable.Add("@GD_SubId", objGroupDetail.GD_SubId);
                 DataTable dt = clsSunDAL.FillDataTable("Sp_GroupDetails_Insert", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objGroupDetail.GD_Id = dt.Rows[0]["GD_Id"].ToInt();
                    return objGroupDetail;
                }
                return null;

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                return null;
            }
        }
        private List<GroupDetailsShowViewModel> GetGroupDetails(int GdId = 0, int GrouppId = 0, int SubjectId = 0)
        {
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("@GD_Id", GdId);

                hash.Add("@GD_GrpId", SubjectId);
                hash.Add("@GD_SubId", GrouppId);


                DataTable dt = ControlFill.FillDataTable("SP_GroupDetails_Select", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<GroupDetailsShowViewModel> list = ConversionFunctions.DataTableToList<GroupDetailsShowViewModel>(dt);
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