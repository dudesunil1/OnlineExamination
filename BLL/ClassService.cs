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
    public class ClassService
    {
        public List<ClassViewModel> GetClass()
        {
            try
            {
                Hashtable hash = new Hashtable();

                DataTable dt = ControlFill.FillDataTable("SP_ClassMaster_Select", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<ClassViewModel> list = ConversionFunctions.DataTableToList<ClassViewModel>(dt);
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