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

    public class ApiKeyService
    {

        public string GetAPIKey()
        {
            try
            {
                
                DataTable dt = ControlFill.FillDataTable("Sp_API_GetActiveKey");
                if (dt != null && dt.Rows.Count > 0)
                {
                    
                    return dt.Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }

            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public void InActivateKey(string ApiKey)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@ApiKey", ApiKey);
                  clsSunDAL.ExecuteDMLQuery("Sp_API_InActivateKey", hashTable);
                 
                

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                 
            }
        }

        public void ActivateAllKey( )
        {
            string _errMsg;
            try
            {

              
               
                clsSunDAL.ExecuteDMLQuery("Sp_API_ActivateAllKey");



            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;

            }
        }
    }
}