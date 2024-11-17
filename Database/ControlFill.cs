using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System;
using Web.SunTechDB;
namespace Web.SunTechDB
{
    public class ControlFill : clsSunDAL, IDisposable
    {
        //static HIMS_BLL.WebGlobalBLL objWebGlobal = new HIMS_BLL.WebGlobalBLL();

        #region --------------------------------- Constructor ------------------------------------------
        public ControlFill()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        ~ControlFill()
        {
            //
            // TODO: Add distructor logic here
            //
            Dispose();
        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }
        #endregion

        #region GridView

        /// <summary>
        /// Fills the GridView with the Data of the Given DataTable
        /// </summary>
        /// <param name="gv">GridView control to be filled</param>
        /// <param name="dt">DataTable having the data to fill in the GridView</param>
        public static void FillGridView(GridView gv, DataTable dt)
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    gv.DataSource = dt;
                    gv.DataBind();
                }
            }
        }


        /// <summary>
        /// Fills the GridView with the Data of the Given DataSet
        /// </summary>
        /// <param name="gv">GridView control to be filled</param>
        /// <param name="ds">Dataset having the data to fill in the GridView</param>
        public static void FillGridView(GridView gv, DataSet ds)
        {
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gv.DataSource = ds.Tables[0].DefaultView;
                    gv.DataBind();
                }
            }
        }

        /// <summary>
        /// Fills the GridView with the Data of the Given DataTable
        /// </summary>
        /// <param name="gv">GridView control to be filled</param>
        /// <param name="dt">DataTable having the data to fill in the GridView</param>
        public static void FillGridView(GridView gv, string ProcedureName)
        {
            try
            {
                DataSet ds = ControlFill.FillDataSet(ProcedureName);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gv.DataSource = ds.Tables[0].DefaultView;
                        gv.DataBind();
                    }
                    else
                    {
                        ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                        gv.DataSource = ds.Tables[0].DefaultView;
                        gv.DataBind();
                    }
                }
            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
            }

        }

        #endregion

        #region DropDownList


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="pProcedureName"></param>
        public static void FillDropDownList(DropDownList ddl, string pProcedureName, Hashtable pValues)
        {
            using (clsSunDAL objDAL = new clsSunDAL())
            {
                DataTable dt = clsSunDAL.FillDataTable(pProcedureName, pValues);
                ddl.Items.Clear();
                if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    ddl.DataSource = dt;
                    ddl.DataTextField = dt.Columns[1].ColumnName;
                    ddl.DataValueField = dt.Columns[0].ColumnName;
                    ddl.DataBind();
                }
                ListItem li = new ListItem("--Select--", "0");
                ddl.Items.Insert(0, li);
            }
        }
        public static void FillDropDownList(string pProcedureName, DropDownList ddl)
        {
            using (clsSunDAL objDAL = new clsSunDAL())
            {
                DataTable dt = clsSunDAL.FillDataTable(pProcedureName);
                ddl.Items.Clear();
                if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    ddl.DataSource = dt;
                    ddl.DataTextField = dt.Columns[1].ColumnName;
                    ddl.DataValueField = dt.Columns[0].ColumnName;
                    ddl.DataBind();
                }
                ListItem li = new ListItem("--Select--", "0");
                ddl.Items.Insert(0, li);
            }
        }
        /// <summary>
        /// Fills the DropDownList with the required data
        /// </summary>
        /// <param name="ddl">DropDownList control to be filled</param>
        /// <param name="dt">DataTable from which data is filled into DropDownList</param>
        public static void FillDropDownList(DropDownList ddl, DataTable dt)
        {
            if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
            {
                ddl.DataSource = dt;
                ddl.DataTextField = dt.Columns[1].ColumnName;
                ddl.DataValueField = dt.Columns[0].ColumnName;
                ddl.DataBind();
            }
            ListItem li = new ListItem("--Select--", "0");
            ddl.Items.Insert(0, li);
        }
        /// <summary>
        /// Fills the DropDownList with the required data
        /// </summary>
        /// <param name="ddl">DropDownList control to be filled</param>
        /// <param name="ds">DataSet from which data is filled into DropDownList</param>
        public static void FillDropDownList(DropDownList ddl, DataSet ds)
        {
            if (ds != null && ds.Tables[0].Columns.Count > 1 && ds.Tables[0].Rows.Count > 0)
            {
                ddl.DataSource = ds;
                ddl.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                ddl.DataBind();
            }
            ListItem li = new ListItem("--Select--", "0");
            ddl.Items.Insert(0, li);
        }
        #endregion
        private static DataTable DefaultTable
        {
            get
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("No");
                dt.Columns.Add("Name");
                //dt.Columns[0].ColumnName = "No";
                //dt.Columns[1].ColumnName = "Name";
                DataRow Dr = dt.NewRow();
                Dr[0] = 0;
                Dr[1] = "---No Data Found---";
                dt.Rows.Add(Dr);
                return dt;
            }
        }
        #region FillSelect
        public static void FillSelect(HtmlSelect ddl, string pProcedureName)
        {
            using (clsSunDAL objDAL = new clsSunDAL())
            {
                DataTable dt = clsSunDAL.FillDataTable(pProcedureName);
                if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    ddl.DataSource = dt;
                    ddl.DataTextField = dt.Columns[1].ColumnName;
                    //ddl.DataValueField = dt.Columns[1].ColumnName;
                    ddl.DataBind();
                }
                ListItem li = new ListItem("--Select--", "0");
                ddl.Items.Insert(0, li);
            }
        }

        public static void FillSelect(HtmlSelect ddl, string pProcedureName, Hashtable pValues)
        {
            using (clsSunDAL objDAL = new clsSunDAL())
            {
                DataTable dt = clsSunDAL.FillDataTable(pProcedureName, pValues);
                if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    ddl.DataSource = dt;
                    ddl.DataTextField = dt.Columns[1].ColumnName;
                    ddl.DataValueField = dt.Columns[0].ColumnName;
                    ddl.DataBind();
                }
                ListItem li = new ListItem("--Select--", "0");
                ddl.Items.Insert(0, li);
            }
        }
        public static void FillSelect(HtmlSelect ddl, DataTable dt)
        {
            if (dt != null && dt.Columns.Count > 1)
            {
                ddl.DataSource = dt;
                ddl.DataTextField = dt.Columns[1].ColumnName;
                ddl.DataValueField = dt.Columns[0].ColumnName;
                ddl.DataBind();
            }
            ListItem li = new ListItem("--Select--", "0");
            ddl.Items.Insert(0, li);
        }
        public static void FillSelect(HtmlSelect ddl, DataSet ds)
        {
            if (ds != null && ds.Tables[0].Columns.Count > 1 && ds.Tables[0].Rows.Count > 0)
            {
                ddl.DataSource = ds;
                ddl.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                ddl.DataValueField = ds.Tables[0].Columns[1].ColumnName;
                ddl.DataBind();
            }
            ListItem li = new ListItem("--Select---", "0");
            ddl.Items.Insert(0, li);
        }
        #endregion

        #region FillListBox
        public static void FillListBox(ListBox lst, string pProcedureName)
        {
            using (clsSunDAL objDAL = new clsSunDAL())
            {
                DataTable dt = clsSunDAL.FillDataTable(pProcedureName);
                if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    lst.DataSource = dt;
                    lst.DataTextField = dt.Columns[1].ColumnName;
                    lst.DataValueField = dt.Columns[0].ColumnName;
                    lst.DataBind();
                }
            }
        }
        public static void FillListBox(ListBox lst, string pProcedureName, Hashtable pValues)
        {
            using (clsSunDAL objDAL = new clsSunDAL())
            {
                DataTable dt = clsSunDAL.FillDataTable(pProcedureName, pValues);
                if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    lst.DataSource = dt;
                    lst.DataTextField = dt.Columns[1].ColumnName;
                    lst.DataValueField = dt.Columns[0].ColumnName;
                    lst.DataBind();
                }
            }
        }

        public static void FillListBox(ListBox lst, DataTable dt)
        {
            if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
            {
                lst.DataSource = dt;
                lst.DataTextField = dt.Columns[1].ColumnName;
                lst.DataValueField = dt.Columns[0].ColumnName;
                lst.DataBind();
            }
        }
        public static void FillListBox(ListBox lst, DataSet ds)
        {
            if (ds != null && ds.Tables[0].Columns.Count > 1 && ds.Tables[0].Rows.Count > 0)
            {
                lst.DataSource = ds;
                lst.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                lst.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                lst.DataBind();
            }
        }
        #endregion

        #region FillCheckList
        public static void FillCheckList(CheckBoxList chkList, string pProcedureName)
        {
            using (clsSunDAL objDAL = new clsSunDAL())
            {
                DataTable dt = clsSunDAL.FillDataTable(pProcedureName);
                if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    chkList.DataSource = dt;
                    chkList.DataTextField = dt.Columns[1].ColumnName;
                    chkList.DataValueField = dt.Columns[0].ColumnName;
                    chkList.DataBind();
                }
            }
        }
        public static void FillCheckList(CheckBoxList chkList, string pProcedureName, Hashtable pValues)
        {
            using (clsSunDAL objDAL = new clsSunDAL())
            {
                DataTable dt = clsSunDAL.FillDataTable(pProcedureName, pValues);
                if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    chkList.DataSource = dt;
                    chkList.DataTextField = dt.Columns[1].ColumnName;
                    chkList.DataValueField = dt.Columns[0].ColumnName;
                    chkList.DataBind();
                }
            }
        }

        public static void FillCheckList(CheckBoxList chkList, DataTable dt)
        {
            if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
            {
                chkList.DataSource = dt;
                chkList.DataTextField = dt.Columns[1].ColumnName;
                chkList.DataValueField = dt.Columns[0].ColumnName;
                chkList.DataBind();
            }
        }
        public static void FillCheckList(CheckBoxList chkList, DataSet ds)
        {
            if (ds != null && ds.Tables[0].Columns.Count > 1 && ds.Tables[0].Rows.Count > 0)
            {
                chkList.DataSource = ds;
                chkList.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                chkList.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                chkList.DataBind();
            }
        }
        #endregion

        #region Fill Radio Button List
        /// <summary>
        /// Fills the RdoList with the required data by DG On 25thAug09
        /// </summary>
        /// <param name="ddl">RadioButtonList control to be filled</param>
        /// <param name="ds">DataSet from which data is filled into RadioButtonList</param>
        public static void FillRdoList(RadioButtonList rdoList, string pProcedureName)
        {
            using (clsSunDAL objDAL = new clsSunDAL())
            {
                DataTable dt = clsSunDAL.FillDataTable(pProcedureName);
                if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    rdoList.DataSource = dt;
                    rdoList.DataTextField = dt.Columns[1].ColumnName;
                    rdoList.DataValueField = dt.Columns[0].ColumnName;
                    rdoList.DataBind();
                }
            }
        }
        public static void FillRdoList(RadioButtonList rdoList, string pProcedureName, Hashtable pValues)
        {
            using (clsSunDAL objDAL = new clsSunDAL())
            {
                DataTable dt = clsSunDAL.FillDataTable(pProcedureName, pValues);
                if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    rdoList.DataSource = dt;
                    rdoList.DataTextField = dt.Columns[1].ColumnName;
                    rdoList.DataValueField = dt.Columns[0].ColumnName;
                    rdoList.DataBind();
                }
            }
        }
        public static void FillRdoList(RadioButtonList rdoList, DataTable dt)
        {
            if (dt != null && dt.Columns.Count > 1 && dt.Rows.Count > 0)
            {
                rdoList.DataSource = dt;
                rdoList.DataTextField = dt.Columns[1].ColumnName;
                rdoList.DataValueField = dt.Columns[0].ColumnName;
                rdoList.DataBind();
            }
        }
        public static void FillRdoList(RadioButtonList rdoList, DataSet ds)
        {
            if (ds != null && ds.Tables[0].Columns.Count > 1 && ds.Tables[0].Rows.Count > 0)
            {
                rdoList.DataSource = ds;
                rdoList.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                rdoList.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                rdoList.DataBind();
            }
        }
        #endregion

        #region Others
        //Fill Drop-Down with Default Blood Group
        public static void FillBloodGroup(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add("");
            ddl.Items.Add("A-Positive");
            ddl.Items.Add("A-Negative");
            ddl.Items.Add("B-Positive");
            ddl.Items.Add("B-Negative");
            ddl.Items.Add("O-Positive");
            ddl.Items.Add("O-Negative");
            ddl.Items.Add("AB-Positive");
            ddl.Items.Add("AB-Negative");
        }
        public static void FillBirthPeriod(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add("");
            ddl.Items.Add("Day(s)");
            ddl.Items.Add("Month(s)");
            ddl.Items.Add("Year(s)");
        }
        public static void FillYear(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add("--Select--");
            for (int i = 1999; i < 3000; i++)
            {
                ddl.Items.Add(i.ToString());
            }
        }
        #endregion






    }
}