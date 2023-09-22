using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATTN.Classlib {
    class Connection {
        public OracleConnection Oracle(string connectionString) {
            try {
                OracleConnection conn = new OracleConnection(connectionString);
                conn.Open();
                return conn;
            } catch (Exception ex) {
                Lib.ClassLib.WriteLog?.Invoke($"\tOracle Connect: {ex.Message}", true);
                return null;
            }
            
        }

        public OleDbConnection Access(string connectionString) {
            try {
                OleDbConnection conn = new OleDbConnection(connectionString);
                conn.Open();
                return conn;
            } catch (Exception ex) {
                Lib.ClassLib.WriteLog?.Invoke($"\tAccess Connect: {ex.Message}", true);
                return null;
            }
        }

        public SqlConnection Sql(string connectionString) {
            try {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                return conn;
            } catch (Exception ex) {
                Lib.ClassLib.WriteLog?.Invoke($"\tAccess Connect: {ex.Message}", true);
                return null;
            }
        }

        public bool CanConnectOracle(string connectionString) {
            try {
                OracleConnection conn = new OracleConnection(connectionString);
                conn.Open();
                conn.Close();
                return true;

            } catch (Exception ex) {
                Lib.ClassLib.WriteLog?.Invoke($"Oracle Connect: {ex.Message}", true);
                return false;
            }
        }

        public bool CanConnectAccess(string connectionString) {
            try {
                OleDbConnection conn = new OleDbConnection(connectionString);
                conn.Open();
                conn.Close();
                return true;
            } catch (Exception ex) {
                Lib.ClassLib.WriteLog?.Invoke($"Access Connect: {ex.Message}", true);
                return false;
            }
        }
    }
}
