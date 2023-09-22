using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATTN.Model;

namespace ATTN {
    class GetDataDb {

        public DataTable GetSqlExcuteLocal(OracleConnection connection, string argName) {

            OracleCommand command = new OracleCommand("PKG_DOWNLOAD_MC_SCAN.GET_SQL", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("ARG_DATA", OracleDbType.Varchar2).Value = "";
            command.Parameters.Add("ARG_NAME_DB", OracleDbType.Varchar2).Value = argName;
            command.Parameters.Add("ARG_IP", OracleDbType.Varchar2).Value = DownloadData.Ip;
            command.Parameters.Add("ARG_USER", OracleDbType.Varchar2).Value = DownloadData.User;
            command.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            using (DataTable dt = new DataTable()) {
                (new OracleDataAdapter(command)).Fill(dt);
                return dt;
            }
        }

        public DataSet GetData(OracleConnection connection, string argName) {

            OracleCommand command = new OracleCommand("PKG_DOWNLOAD_MC_SCAN.GET_DATA", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("ARG_DATA", OracleDbType.Varchar2).Value = DownloadData.TypeData;
            command.Parameters.Add("ARG_NAME_DB", OracleDbType.Varchar2).Value = argName;
            command.Parameters.Add("ARG_IP", OracleDbType.Varchar2).Value = DownloadData.Ip;
            command.Parameters.Add("ARG_USER", OracleDbType.Varchar2).Value = DownloadData.User;
            command.Parameters.Add("OUT_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            command.Parameters.Add("OUT_CURSOR2", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            using (DataSet ds = new DataSet()) {
                (new OracleDataAdapter(command)).Fill(ds);
                return ds;
            }
        }

        public bool SaveEmpInforToOracle(OracleConnection connection, List<EmpLocalInfor> empList) {
            using (OracleTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted)) {
                try {
                    int i = 0;
                    string user = DownloadData.User;
                    string ip = DownloadData.Ip;
                    foreach (EmpLocalInfor emp in empList) {
                        i++;
                        OracleCommand command = new OracleCommand("PKG_DOWNLOAD_MC_SCAN.INSERT_HE_EMP_RF", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;

                        command.Parameters.Add("ARG_EMP_NO", OracleDbType.Varchar2).Value = emp.EMP_NO;
                        command.Parameters.Add("ARG_RF_ID", OracleDbType.Varchar2).Value = emp.RF_ID;
                        command.Parameters.Add("ARG_DEP_CODE", OracleDbType.Varchar2).Value = emp.DEPT_CODE;
                        command.Parameters.Add("ARG_IP", OracleDbType.Varchar2).Value = ip;
                        command.Parameters.Add("ARG_USER", OracleDbType.Varchar2).Value = user;
                        command.Parameters.Add("ARG_ROW", OracleDbType.Varchar2).Value = i;

                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();

                    return true;
                } catch (Exception ex) {
                    transaction.Rollback();
                    Lib.ClassLib.WriteLog?.Invoke($"SaveEmpInforToOracle: {ex.Message}", true);
                    return false;
                }
            }
        }
    }
}
