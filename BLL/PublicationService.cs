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
   
    public class PublicationService
    {
       
        public List<PublicationViewModel> GetPublications()
        {
            try
            {
                return GetPublications(0);

            }
            catch (Exception Ex)
            {
                return null;
            }
        }


        public List<PublicationViewModel> GetPublications(int PubId = 0)
        {
            try
            {
                Hashtable hash = new Hashtable();

                hash.Add("@Pub_Id", PubId);
                 DataTable dt = ControlFill.FillDataTable("Sp_PublicationMaster_Select", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<PublicationViewModel> list = ConversionFunctions.DataTableToList<PublicationViewModel>(dt);
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

        public List<PublicationViewModel> GetPublicationsById(int pub)
        {
            return GetPublications(pub);

        }
        public PublicationViewModel Add(PublicationViewModel objPublication) 
        {
            string _errMsg;
            try
            {
               
                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Pub_Name", objPublication.Pub_Name);
                DataTable dt = clsSunDAL.FillDataTable("SP_PublicationMaster_Insert", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objPublication.Pub_Id = dt.Rows[0]["Pub_ID"].ToInt();
                    return objPublication;
                }
                return null;

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                return null;
            }
        }

       


        public PublicationViewModel Update(PublicationViewModel objPublication)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Pub_Name", objPublication.Pub_Name);
                hashTable.Add("@Pub_id", objPublication.Pub_Id);
                bool IsUpdate = clsSunDAL.ExecuteDMLQuery("SP_PublicationMaster_Update", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (IsUpdate)
                {

                    return objPublication;
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