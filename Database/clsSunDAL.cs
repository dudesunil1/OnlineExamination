using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System;
using System.Configuration;

namespace   Web.SunTechDB
{
    public class clsSunDAL : IDisposable
    {


        #region --------------------------------- Constructor ------------------------------------------

        public clsSunDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        ~clsSunDAL()
        {
            //
            // TODO: Add destructor logic here
            //
            Dispose();
        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        #endregion

        #region ------------------------------- Private Connection -------------------------------------

        /// <summary>
        /// OPEN THE SPECIFIED CONNECTION
        /// </summary>
        /// <returns></returns>
        public static bool OpenConnection(ref SqlConnection _connection)
        {
            try
            {
                //SetConnectionString();
                if (_connection.State != ConnectionState.Open)
                {
                    _connection = new SqlConnection();
                      _connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyDbContext"].ConnectionString;
                    //// _connection.ConnectionString += ";MultipleActiveResultSets=True;connect timeout=300;";

                    _connection.Open();
                }

                return true;
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                String strException = ex.Message;
                return false;
            }
        }



        /// <summary>
        /// CLOSE THE CONNECTION IF OPENED
        /// </summary>
        protected static void CloseConnection(ref SqlConnection _connection)
        {
            try
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
            catch (Exception e)
            {
                String str;
                str = e.Message;
            }
        }

        #endregion

        #region --------------------------------- Private Methods ---------------------------------
        public static string _errMsg = string.Empty;
        public override string ToString()
        {
            return _errMsg;
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
        /// Fill Data Set
        /// </summary>
        /// <param name="pProcedureName">Procudure Name</param>
        /// <returns>Data Set</returns>

        public static DataSet FillDataSet(string pProcedureName)
        {
            DataSet _ds = null;
            DataTable _dt = null;
            SqlConnection _connection = new SqlConnection();
            SqlCommand _sqlCmd = null;

            SqlDataReader _reader = null;
            SqlDataAdapter _adapter = null;
            try
            {

                if (OpenConnection(ref _connection))
                {
                    _sqlCmd = new SqlCommand(pProcedureName, _connection);
                    _sqlCmd.CommandType = CommandType.StoredProcedure;
                    _adapter = new SqlDataAdapter(_sqlCmd);
                    _ds = new DataSet();
                    _adapter.Fill(_ds);
                }
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                //Response.Write(ex.Message);
            }
            finally
            {
                _sqlCmd.Dispose();
                _adapter.Dispose();
                CloseConnection(ref _connection);
            }
            return _ds;
        }

        /// <summary>
        /// Fill Data Set
        /// </summary>
        /// <param name="pProcedureName"></param>
        /// <param name="pValues"></param>
        /// <returns>Data Set</returns>

        public static DataSet FillDataSet(string pProcedureName, Hashtable pValues)
        {
            DataSet _ds = null;
            DataTable _dt = null;
            SqlConnection _connection = new SqlConnection();
            SqlCommand _sqlCmd = null;

            SqlDataReader _reader = null;
            SqlDataAdapter _adapter = null;
            try
            {
                if (OpenConnection(ref _connection))
                {
                    _sqlCmd = new SqlCommand(pProcedureName, _connection);
                    _sqlCmd.CommandTimeout = 300;
                    _sqlCmd.CommandType = CommandType.StoredProcedure;
                    IDictionaryEnumerator _enumerator = pValues.GetEnumerator();
                    while (_enumerator.MoveNext())
                    {
                        _sqlCmd.Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                    }
                    _adapter = new SqlDataAdapter(_sqlCmd);
                    _ds = new DataSet();
                    _adapter.Fill(_ds);
                }
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                //Response.Write(ex.Message);
            }
            finally
            {
                _sqlCmd.Dispose();
                _adapter.Dispose();
                CloseConnection(ref _connection);
            }
            return _ds;
        }

        /// <summary>
        /// Fill Data Table 
        /// </summary> 
        /// <param name="pProcedureName">Procedure Name</param>
        /// <returns>Data Table</returns>

        public static DataTable FillDataTable(string pProcedureName)
        {
            DataTable _dt = null;
            SqlConnection _connection = new SqlConnection();
            SqlCommand _sqlCmd = null;

            SqlDataAdapter _adapter = null;
            try
            {
                if (OpenConnection(ref _connection))
                {
                    _sqlCmd = new SqlCommand(pProcedureName, _connection);
                    _sqlCmd.CommandTimeout = 300;
                    _sqlCmd.CommandType = CommandType.StoredProcedure;
                    _adapter = new SqlDataAdapter(_sqlCmd);
                    _dt = new DataTable();
                    _adapter.Fill(_dt);
                }
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }
            finally
            {
                _sqlCmd.Dispose();
                _adapter.Dispose();
                CloseConnection(ref _connection);
            }
            return _dt;
        }

        /// <summary>
        /// Fill Data Table 
        /// </summary> 
        /// <param name="pProcedureName">Procedure Name</param>
        /// <param name="pValues">Hash Table</param>
        /// <returns>Data Table</returns>

        public static DataTable FillDataTable(string pProcedureName, Hashtable pValues)
        {
            DataTable _dt = null;
            SqlConnection _connection = new SqlConnection();
            SqlCommand _sqlCmd = null;

            SqlDataAdapter _adapter = null;
            try
            {

                if (OpenConnection(ref _connection))
                {
                    _sqlCmd = new SqlCommand(pProcedureName, _connection);
                    _sqlCmd.CommandType = CommandType.StoredProcedure;
                    IDictionaryEnumerator _enumerator = pValues.GetEnumerator();
                    while (_enumerator.MoveNext())
                    {
                        _sqlCmd.Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                    }
                    _sqlCmd.CommandTimeout = 300;
                    _adapter = new SqlDataAdapter(_sqlCmd);
                    _dt = new DataTable();
                    _adapter.Fill(_dt);
                }
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                //Response.Write(ex.Message);
            }
            finally
            {
                _sqlCmd.Dispose();
                _adapter.Dispose();
                CloseConnection(ref _connection);
            }
            return _dt;
        }

        /// <summary>
        /// Fill Data Table 
        /// </summary> 
        /// <param name="pProcedureName">Procedure Name</param>
        /// <param name="pValues">Hash Table</param>
        /// <returns>Data Table</returns>

        public static DataTable FillTable(string tableName)
        {
            DataTable _dt = null;
            SqlConnection _connection = new SqlConnection();
            SqlCommand _sqlCmd = null;

            SqlDataAdapter _adapter = null;
            try
            {

                if (OpenConnection(ref _connection))
                {
                    _sqlCmd = new SqlCommand("Select * from " + tableName, _connection);
                    _sqlCmd.CommandTimeout = 300;
                    _sqlCmd.CommandType = CommandType.Text;
                    _adapter = new SqlDataAdapter(_sqlCmd);
                    _dt = new DataTable();
                    _adapter.Fill(_dt);
                }
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                //Response.Write(ex.Message);
            }
            finally
            {
                _sqlCmd.Dispose();
                _adapter.Dispose();
                CloseConnection(ref _connection);
            }
            return _dt;
        }






        /// <summary>
        /// It Executes The Insert/Update/Delete Query.
        /// </summary>
        /// <param name="pProcedureName">Procedure Name</param>
        /// <returns>It Returns Flase for The Exception Else true </returns>

        public static bool ExecuteDMLQuery(string pProcedureName)
        {

            SqlConnection _connection = new SqlConnection();
            SqlCommand _sqlCmd = null;


            try
            {

                if (OpenConnection(ref _connection))
                {
                    _sqlCmd = new SqlCommand(pProcedureName, _connection);
                    _sqlCmd.CommandTimeout = 300;
                    _sqlCmd.CommandType = CommandType.StoredProcedure;
                    _sqlCmd.ExecuteNonQuery();
                    return true;
                }
                return false;
            }
            catch (SqlException sx)
            {
                SetMessage(sx);
                return false;
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                return false;
            }
            finally
            {
                _sqlCmd.Dispose();
                CloseConnection(ref _connection);
            }
        }



        /// <summary>
        /// It Executes The Insert/Update/Delete Query.
        /// </summary>
        /// <param name="pProcedureName"> Procedure Name</param>
        /// <param name="pValues">Hashtable</param>
        /// <returns>It Returns Flase for The Exception Else true </returns>

        public static bool ExecuteDMLQuery(string pProcedureName, Hashtable pValues)
        {
            SqlConnection _connection = new SqlConnection();
            SqlCommand _sqlCmd = null;
            try
            {

                if (OpenConnection(ref _connection))
                {
                    _sqlCmd = new SqlCommand(pProcedureName, _connection);
                    _sqlCmd.CommandType = CommandType.StoredProcedure;
                    _sqlCmd.CommandTimeout = 300;
                    IDictionaryEnumerator _enumerator = pValues.GetEnumerator();

                    while (_enumerator.MoveNext())
                    {
                        if (_enumerator.Key.ToString().Equals("@Image"))
                        {
                            SqlParameter pImage = new SqlParameter();
                            pImage.SqlDbType = SqlDbType.Binary;
                            pImage.ParameterName = _enumerator.Key.ToString();
                            pImage.Value = _enumerator.Value;
                            _sqlCmd.Parameters.Add(pImage);
                        }
                        else
                        {
                            _sqlCmd.Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                        }
                    }
                    //SqlParameter outParameter = new SqlParameter("@Msg", SqlDbType.VarChar, 255); //New Update
                    //outParameter.Direction = ParameterDirection.InputOutput;
                    //outParameter.Value = "";
                    //_sqlCmd.Parameters.Add(outParameter);
                    _sqlCmd.ExecuteNonQuery();
                    //if (_sqlCmd.Parameters["@Msg"].Value.ToInt() == 1)
                    //return true;
                    //else if (_sqlCmd.Parameters["@Msg"].Value != string.Empty)
                    //{
                    //    _ReturnString = _sqlCmd.Parameters["@Msg"].Value.ToString();
                    return true;
                    //}
                    //else
                    //    return false;
                }
                return false;
            }
            catch (SqlException sx)
            {
                SetMessage(sx);
                return false;
            }

            catch (Exception ex)
            {


                _errMsg = ex.Message;
                return false;
            }
            finally
            {
                _sqlCmd.Dispose();
                CloseConnection(ref _connection);
            }
        }




        /// <summary>
        /// Transaction with sql command and bulk copy of datatable
        /// </summary>
        /// <param name="TableList">Array Of DataTable </param>
        /// <param name="pProcedureName">Array Of Procedure Name</param>
        /// /// <param name="pValues">Array Of Hash Table</param>
        /// <returns>boolean </returns>





        /// <summary>
        /// Transaction with sql command and bulk copy of datatable
        /// </summary>
        /// <param name="TableList">Array Of DataTable </param>
        /// <param name="pProcedureName">Array Of Procedure Name</param>
        /// /// <param name="pValues">Array Of Hash Table</param>
        /// <returns>boolean </returns>

        public static bool ExecuteTransact(string[] pProcedureName, Hashtable[] pValues, DataTable[] TableList)
        {
            SqlConnection _connection = new SqlConnection();
            SqlTransaction _transaction = null;
            bool _transCheck = false;
            SqlCommand[] sqlCommand = new SqlCommand[pProcedureName.Length];
            try
            {

                if (OpenConnection(ref _connection))
                {
                    // Start a local transaction.
                    _transaction = _connection.BeginTransaction();
                    // Must assign both transaction object and connection
                    // to Command object for a pending local transaction
                    SqlBulkCopy blkCpy = new SqlBulkCopy(_connection, SqlBulkCopyOptions.Default, _transaction);

                    for (int i = 0; i < pProcedureName.Length; i++)
                    {
                        if (pProcedureName[i] != null && pProcedureName[i] != "")
                        {
                            sqlCommand[i] = new SqlCommand(pProcedureName[i], _connection);
                            sqlCommand[i].Transaction = _transaction;
                            sqlCommand[i].CommandType = CommandType.StoredProcedure;
                            if (pValues[i] != null)
                            {
                                IDictionaryEnumerator _enumerator = pValues[i].GetEnumerator();
                                while (_enumerator.MoveNext())
                                {
                                    sqlCommand[i].Parameters.AddWithValue(_enumerator.Key.ToString(), _enumerator.Value);
                                }
                            }

                            sqlCommand[i].ExecuteNonQuery();
                        }
                    }
                    if (TableList != null)
                        for (int t = 0; t < TableList.Length; t++)
                        {
                            if (TableList[t] != null && TableList[t].Rows.Count >= 1)
                            {
                                blkCpy.DestinationTableName = TableList[t].TableName;
                                blkCpy.WriteToServer(TableList[t]);
                            }
                        }
                    _transaction.Commit();
                    _transCheck = true;
                }
            }
            catch (SqlException sx)
            {
                SetMessage(sx);
                _transaction.Rollback();
                _transCheck = false;
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
                _transaction.Rollback();
                _transCheck = false;
            }
            finally
            {
                CloseConnection(ref _connection);
            }
            return _transCheck;
        }
        public static bool ExecuteTransact(string[] pProcedureName, Hashtable[] pValues, DataTable TableList)
        {

            DataTable[] tableList = { TableList };
            return ExecuteTransact(pProcedureName, pValues, tableList);
        }
        #endregion

        #region ------------------------------------IDisposable Members--------------------------

        void IDisposable.Dispose()
        {

        }

        #endregion

        private static void SetMessage(SqlException ex)
        {
            if (ex.Number == 547)
            {
                _errMsg = " Record is Used In Another Form. \n Please Check Referance in Another Forms.";
            }
            else if (ex.Number == 2627)
            {
                _errMsg = "Record Already Exist..\n Please Enter Another No.";
            }
            else
                _errMsg = ex.Message;
        }
        //public static string DMLOperation(TranMode _trnMode)
        //{
        //    if (_trnMode == TranMode.StrModeNew)
        //    {
        //        return "NEW";
        //    }
        //    else if (_trnMode == TranMode.StrModeChange)
        //    {
        //        return "EDIT";
        //    }
        //    else if (_trnMode == TranMode.StrModeDelete)
        //    {
        //        return "DELETE";
        //    }
        //    else
        //    {
        //        return "None";
        //    }


        //}

    }
}