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
    }
}