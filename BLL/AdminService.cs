using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.SunTechDB;

namespace OnlineExamination.BLL
{
    public class AdminService
    {
        public bool Login(string userName,string password) {
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("@Comp_UserName", userName);
                hash.Add("@Comp_Password", password);
                DataTable dtData = ControlFill.FillDataTable("Sp_Company_Login", hash);
                if (dtData != null && dtData.Rows.Count > 0)
                {
                    HttpContext.Current.Session["UserData"] = dtData;
                    HttpContext.Current.Session["UserRole"] = "ADMIN";
                    return true;
                    //Response.Redirect("Dashboard.aspx", false);
                }
                else
                {
                    return false;
                    //lblError.Text = "Please enter a valid username and password.";
                    //lblError.Visible = true;
                }

            }
            catch (Exception Ex)
            {
                return false;

                //Response.Write(Ex.Message);
            }
        }
    }
}