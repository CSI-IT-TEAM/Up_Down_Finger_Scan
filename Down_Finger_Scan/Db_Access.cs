using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using ATTN.Model;
using ATTN.Classlib;
using System.Collections.Generic;

namespace ATTN {
    class Db_Access {
        public void Download(string argNameDb, string strLocal, string strSever) {
            Lib.ClassLib.WriteLog?.Invoke($"Start {argNameDb}: {DateTime.Now}", false);

            Connection connect = new Connection();
            OleDbConnection conectAccess = connect.Access(strLocal);
            if (conectAccess == null) return;
            OracleConnection connecOra = connect.Oracle(strSever);
            if (connecOra == null) return;

            GetDataDb db = new GetDataDb();
            try {
                //Get Data from Oracle
                Lib.ClassLib.WriteLog?.Invoke("Get Data from Oracle", false);
                DataSet dsData = db.GetData(connecOra, argNameDb);
                if (dsData == null || dsData.Tables.Count < 2) {
                    Lib.ClassLib.WriteLog?.Invoke("\tOracle Get Data Error", true);
                    return;
                }
                DataTable dtData = dsData.Tables[0];
                DataTable dtDept = dsData.Tables[1];
                Lib.ClassLib.WriteLog?.Invoke($"\tOracle Data: {dtData.Rows.Count}\n Oracle Dept:{dtDept.Rows.Count}", false);

                //Get Sql for Excute Local DB
                Lib.ClassLib.WriteLog?.Invoke("Get Sql for Excute Local DB", false);
                DataTable dtSqlText = db.GetSqlExcuteLocal(connecOra, argNameDb);
                if (dtSqlText == null) {
                    Lib.ClassLib.WriteLog?.Invoke("\tOracle Get Sql Error", true);
                    return;
                }
                List<SqlText> sqlList = dtSqlText.ToList<SqlText>();

                BeforeAndAfterInsertEmp(conectAccess, sqlList, "BEF");
                InsertDeptToAccess(conectAccess, sqlList, dtDept);
                InsertAndUpdateEmp(conectAccess, sqlList, dtData);
                BeforeAndAfterInsertEmp(conectAccess, sqlList, "AFT");
            } catch (Exception ex) {
                Lib.ClassLib.WriteLog?.Invoke(ex.Message, true);
            } finally {
                connecOra.Close();
                conectAccess.Close();
                Lib.ClassLib.WriteLog?.Invoke($"Finish {argNameDb}: {DateTime.Now}", false);
            }           
        }

        private void InsertAndUpdateEmp(OleDbConnection conectAccess, List<SqlText> sqlList, DataTable dtData) {
            Lib.ClassLib.WriteLog?.Invoke("Update and Insert Start", false);

            //Get Employee in Local
            Lib.ClassLib.WriteLog?.Invoke("\tGet Employee in Local", false);
            SqlText sqlString = sqlList.Find(col => col.SUB_CODE == "LOC_EMP");
            DataTable dtEmpLocal = new DataTable();
            using (OleDbDataAdapter da = new OleDbDataAdapter(sqlString.SQL_TEXT, conectAccess)) {
                da.Fill(dtEmpLocal);
            }

            //Sql Insert Local
            Lib.ClassLib.WriteLog?.Invoke("\tGet Sql Insert Local", false);
            sqlString = sqlList.Find(col =>col.SUB_CODE == "INS_EMP");
            string cmdTextInsert = sqlString.SQL_TEXT;

            //Sql Update Local
            Lib.ClassLib.WriteLog?.Invoke("\tGet Sql Update Local", false);
            sqlString = sqlList.Find(col => col.SUB_CODE == "UPD_EMP");
            string cmdTextUpdate = sqlString.SQL_TEXT;

            var sqlEmpList = sqlList.Where(col => col.SUB_CODE.StartsWith("EMP"));
            int rowNum = 0;
            int totalRow = dtData.Rows.Count;

            foreach (DataRow empRow in dtData.Rows) {
                string empNo = empRow["EMP_NO"].ToString();
                if (empNo == "98040018") {
                }
                object[] empArray = empRow.ItemArray;
                try {
                    string cmdText;
                    string status;
                    if (dtEmpLocal.Select($"ID = '{empNo}'").Count() == 0) {
                        cmdText = string.Format(cmdTextInsert, empArray);
                        status = "Insert";
                    } else {
                        cmdText = string.Format(cmdTextUpdate, empArray);
                        status = "Update";
                    }

                    //Exec Update and Insert Data
                    using (OleDbCommand cmd = new OleDbCommand()) {
                        cmd.Connection = conectAccess;
                        cmd.CommandText = cmdText;
                        //table Tuser
                        if (cmd.ExecuteNonQuery() == 0) {
                            Lib.ClassLib.WriteLog?.Invoke($"\tCan not {status} {empNo}", false);
                        }
                        //Other table
                        foreach (SqlText sql in sqlEmpList) {
                            string sqlExec = string.Format(sql.SQL_TEXT, empArray);
                            cmd.CommandText = sqlExec;
                            if ( string.IsNullOrEmpty(sql.COL_CHECK)) {
                                if (cmd.ExecuteNonQuery() == 0) {
                                    Lib.ClassLib.WriteLog?.Invoke($"\tError: {sqlExec} ", false);
                                }
                            } else if (empRow[sql.COL_CHECK].ToString().Trim() != "") {
                                if (cmd.ExecuteNonQuery() == 0) {
                                    Lib.ClassLib.WriteLog?.Invoke($"\tError: {sqlExec} ", false);
                                }
                            }
                        }
                    }
                    rowNum++;
                } catch (Exception ex) {
                    Lib.ClassLib.WriteLog?.Invoke($"\t{empNo}: {ex.Message}", true);
                }
                string statusText = $"Download: {rowNum}/{totalRow}";
                Lib.ClassLib.WriteStatus?.Invoke(statusText);
            }
        }

        private void InsertDeptToAccess(OleDbConnection conn, List<SqlText> sqlList, DataTable dtDept) {
            SqlText sqlText = sqlList.Find(x => x.SUB_CODE == "INS_DEP");
            if (sqlText == null) return;
            foreach (DataRow row in dtDept.Rows) {
                try {
                    string cmdText = string.Format(sqlText.SQL_TEXT, row.ItemArray);
                    using (OleDbCommand cmd = new OleDbCommand(cmdText, conn)) {
                        if (cmd.ExecuteNonQuery() == 0) {
                            Lib.ClassLib.WriteLog?.Invoke($"Error: {cmdText}", true);
                        }
                    }
                } catch (Exception ex) {
                    Lib.ClassLib.WriteLog?.Invoke($"InsertDeptToAccess: {ex.Message}", true);
                }
            }
        }

        private void BeforeAndAfterInsertEmp(OleDbConnection conectAccess, List<SqlText> sqlList, string type) {
            string text = type == "BEF" ? "Before" : "After";
            Lib.ClassLib.WriteLog?.Invoke($"Exec {text} Insert Data", false);
            using (OleDbCommand cmd = new OleDbCommand()) {
                cmd.Connection = conectAccess;
                var sqlBefore = sqlList.Where(x =>x.SUB_CODE.StartsWith(type)).ToList();
                foreach (SqlText sql in sqlBefore) {
                    cmd.CommandText = sql.SQL_TEXT;
                    Lib.ClassLib.WriteLog?.Invoke($"\t{sql.SQL_TEXT}: {cmd.ExecuteNonQuery()}", false);
                }
            }
        }
    }
}
