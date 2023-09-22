using System;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ATTN
{
    class DownService
    {
        public string _strLocalDB = "";
        public string _strSeverDB = "";

        [Obsolete]
        public bool checkConnectOracle(String arg_conOra)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(arg_conOra))
                {
                    connection.Open();

                    using (OracleDataAdapter da = new OracleDataAdapter("SELECT SYSDATE FROM DUAL", connection))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt != null && dt.Rows.Count > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.ClassLib.writeToLog("getDataLocal()  :   " + ex.ToString());
                return false;
            }
        }

        public bool checkConnectAccess( string strConnection)
        {
            try
            {
                using (OleDbConnection con_ora = new OleDbConnection(strConnection))
                {
                    con_ora.Open();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Lib.ClassLib.writeToLog("getDataLocal()  :   " + ex.ToString());
                return false;
            }
        }

        public bool checkConnectSql(string strConnection)
        {
            try
            {
                using (SqlConnection con_ora = new SqlConnection(strConnection))
                {
                    con_ora.Open();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Lib.ClassLib.writeToLog("getDataLocal()  :   " + ex.ToString());
                return false;
            }
        }

        public DataTable getDataUnis(string strConnection)
        {
            string strSql = "";
            strSql = strSql + "  SELECT COUNT(*)  ";
            strSql = strSql + "    FROM TENTER A, TUSER B ";
            strSql = strSql + "   WHERE A.APPLY_YN = 'N' ";
            strSql = strSql + "     AND A.L_UID = B.L_ID ";

            try
            {
                using (OleDbConnection con = new OleDbConnection(strConnection))
                {
                    con.Open();
                    using (OleDbDataAdapter da = new OleDbDataAdapter(strSql, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.ClassLib.writeToLog("getDataLocal()  :   " + ex.ToString());
                
                return null;
            }
        }

        #region SQL
        public DataTable getDataSQL(string strSql, SqlConnection con, string argErr)
        {
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(strSql, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                
            }
            catch (Exception ex)
            {
                Lib.ClassLib.writeToLog(argErr + ": " + ex.ToString());
                return null;
            }
        }

        public int execSql(string strSql, SqlConnection con, string argErr)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(strSql, con))
                {
                    return cmd.ExecuteNonQuery();                 
                }
            }
            catch (Exception ex)
            {
                Lib.ClassLib.writeToLog(argErr + ": " + ex.ToString());
                return 0;
            }
        }

        [Obsolete]
        public int execOra(string strSql, OracleConnection con, string argErr)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand(strSql, con))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Lib.ClassLib.writeToLog(argErr + ": " + ex.ToString());
                return 0;
            }
        }

        #endregion

        //public static DataTable checkConnectAccess(String str, string strConnection)
        //{
        //    try
        //    {
        //        using (OleDbConnection con_ora = new OleDbConnection(strConnection))
        //        {
        //            con_ora.Open();
        //            using (OleDbDataAdapter da = new OleDbDataAdapter(str, con_ora))
        //            {

        //                DataTable dt = new DataTable();
        //                da.Fill(dt);
        //                return dt;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Lib.ClassLib.writeToLog("getDataLocal()  :   " + ex.ToString());
        //        return null;
        //    }
        //}

        /*
            
        */

        //[Obsolete]
        //private DataSet getDataLocal()
        //{
        //    try
        //    {
        //        using (OracleConnection connection = new OracleConnection(Lib.ClassLib.HUBICVJConnectionString))
        //        {
        //            connection.Open();

        //            OracleCommand command = new OracleCommand("PKG_RPT_HRM_PAYROLL.SELECT_TEST", connection);
        //            command.CommandType = CommandType.StoredProcedure;

        //            command.Parameters.Add("ARG_DATE", OracleType.Char).Value = "";
        //            command.Parameters.Add("OUT_CURSOR", OracleType.Cursor);
        //            command.Parameters.Add("OUT_CURSOR2", OracleType.Cursor);

        //            command.Parameters["ARG_DATE"].Direction = ParameterDirection.Input;
        //            command.Parameters["OUT_CURSOR"].Direction = ParameterDirection.Output;
        //            command.Parameters["OUT_CURSOR2"].Direction = ParameterDirection.Output;

        //            using (DataSet ds = new DataSet())
        //            {
        //                (new OracleDataAdapter(command)).Fill(ds);
        //                return ds;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Lib.ClassLib.writeToLog("getDataLocal()  :   " + ex.ToString());
        //        return null;
        //    }
        //}
    }
}
