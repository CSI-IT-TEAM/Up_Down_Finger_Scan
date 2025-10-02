using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UploadPostgreSQL {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            Load += Form1_Load;
            cmdStart.Click += CmdStart_Click;
            tmrLoad.Tick += TmrLoad_Tick;
        }

        private void TmrLoad_Tick(object sender, EventArgs e) {
            UploadData();
        }

        private void CmdStart_Click(object sender, EventArgs e) {
            
        }

        private async void Form1_Load(object sender, EventArgs e) {
            await Task.Delay(4000);
            UploadData();
            tmrLoad.Start();
        }

        


        private void UploadData() {
            string connectString = "Server = 172.30.80.83\\SQLEXPRESS;Database = UNIS;User Id = sa;Password = system;";
            using (SqlConnection sqlCon = new SqlConnection(connectString)) {
                sqlCon.Open();
                DataTable dtSQL = GetDataFromSQL(sqlCon);
                InsertDatatoPostgre(dtSQL, sqlCon);
            }
        }
        

        private void InsertDatatoPostgre(DataTable data, SqlConnection sqlCon) {
            string cs = "Host=203.228.118.39;Port=5432;Database=postgres;Username=dmdev;Password=dmdev;";
            string strUser = System.Net.Dns.GetHostName();
            string strIP = GetLocalIPv4();
            using (var conn = new NpgsqlConnection(cs)) {
                conn.Open();
                var sql = @"
                        INSERT INTO ""REO"".dmhr_punch_in
                        (company, punch_date, machine_id, emp_no, punch_time, add_user_id, add_dttm, add_ip)
                        VALUES (@company, @punch_date, @machine_id, @emp_no, @punch_time, @add_user_id, @add_dttm, @add_ip)
                        RETURNING 1;";
                using (var cmd = new NpgsqlCommand(sql, conn)) {
                    var pCompany = cmd.Parameters.Add("@company", NpgsqlDbType.Text);
                    var pPunchDate = cmd.Parameters.Add("@punch_date", NpgsqlDbType.Date);        
                    var pMachineId = cmd.Parameters.Add("@machine_id", NpgsqlDbType.Text);
                    var pEmpNo = cmd.Parameters.Add("@emp_no", NpgsqlDbType.Text);
                    var pPunchTime = cmd.Parameters.Add("@punch_time", NpgsqlDbType.Timestamp);   
                    var pAddUserId = cmd.Parameters.Add("@add_user_id", NpgsqlDbType.Text);
                    var pAddDttm = cmd.Parameters.Add("@add_dttm", NpgsqlDbType.Timestamp);   
                    var pAddIp = cmd.Parameters.Add("@add_ip", NpgsqlDbType.Text);        
                    cmd.Prepare();

                    int iRow = 0, total = data.Rows.Count;
                    foreach (DataRow row in data.Rows) {
                        // Gán giá trị
                        pCompany.Value = "VJ";
                        pPunchDate.Value = DateTime.ParseExact(
                            row["C_Date"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture
                        ).Date;

                        pMachineId.Value = row["L_TID"].ToString();
                        pEmpNo.Value = row["C_UNIQUE"].ToString();

                        pPunchTime.Value = DateTime.ParseExact(
                            row["C_Date"].ToString() + row["C_Time"].ToString(),
                            "yyyyMMddHHmmss", CultureInfo.InvariantCulture
                        );

                        pAddUserId.Value = strUser;
                        pAddDttm.Value = DateTime.Now;        // hoặc DateTime.UtcNow
                        pAddIp.Value = strIP;

                        cmd.ExecuteNonQuery();    
                        UpdateStatus(row, sqlCon);
                        lblStatus.Text = $"Upload: {++iRow}/{total}";
                    }
                }

            }
        }

        private DataTable GetDataFromSQL(SqlConnection sqlCon) {
            DataTable dt = new DataTable();
                string strSql = "  SELECT A.C_Date,  A.C_Time,  A.L_TID,  A.L_UID, A.L_Mode,  A.C_Unique, B.L_RegServer, A.L_MatchingType  " +
                                    "    FROM TENTER A, TUSER B " +
                                    "   WHERE A.APPLY_YN2 = 'N' " +
                                    "     AND A.L_UID = B.L_ID " +
                                    "     AND A.L_Result in (0, 33) ";
                using (SqlDataAdapter da = new SqlDataAdapter(strSql, sqlCon)) {
                    da.Fill(dt);
                }
            
            return dt;
        }

        private int UpdateStatus(DataRow row, SqlConnection sqlCon) {
            try {
                string strSql = "  UPDATE TENTER  " +
                        "     SET APPLY_YN2 = 'Y' " +
                        "   WHERE APPLY_YN2 = 'N' " +
                        "     AND C_DATE   = '" + row["C_DATE"].ToString() + "' " +
                        "     AND C_TIME   = '" + row["C_TIME"].ToString() + "' " +
                        "     AND L_TID    = " + row["L_TID"].ToString() + " " +
                        "     AND L_UID    = " + row["L_UID"].ToString() + " " +
                        "     AND C_UNIQUE = '" + row["C_UNIQUE"].ToString() + "' " +
                        "     AND L_MODE   = " + row["L_Mode"].ToString() + " ";
                using (SqlCommand cmd = new SqlCommand(strSql, sqlCon)) {
                    return cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                Debug.Write(ex.ToString());
                return 0;
            }
            
        }

        private  string GetLocalIPv4() {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces()) {
                // Bỏ qua card loopback, tắt hoặc card ảo
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet) {
                    var ipProps = ni.GetIPProperties();

                    foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses) {
                        if (addr.Address.AddressFamily == AddressFamily.InterNetwork) // IPv4
                        {
                            return addr.Address.ToString();
                        }
                    }
                }
            }
            return null;
        }
    }
}
