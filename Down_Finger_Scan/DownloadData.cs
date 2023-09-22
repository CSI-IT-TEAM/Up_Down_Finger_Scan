using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//using Oracle.DataAccess.Client;


namespace ATTN {
    public partial class DownloadData : Form {
        public DownloadData() {
            InitializeComponent();
            Lib.ClassLib.WriteLog += WriteLog;
            Lib.ClassLib.WriteStatus += WriteStatus;
            InitTimerAuto();
        }
        System.Windows.Forms.Timer tmrAuto = new System.Windows.Forms.Timer();
        DownService _db = new DownService();
        public static string TypeData = "VJ";
        public static string User = System.Net.Dns.GetHostName();
        public static string Ip = Lib.ClassLib.GetLocalIPAddress();

        private async void AutoDownload() {
            DataSet ds = Lib.ClassLib.ReadXML(Application.StartupPath + "\\configATTN.XML");
            string processKill = ds.Tables["config"].Rows[0]["ProcessKill"].ToString();
            KillProcess(processKill);

            DataTable dt = (DataTable)cboLocal.DataSource;
            string strSever = cboSever.SelectedValue.ToString().Trim().Replace("\n", "").Replace("\t", "");
            foreach (DataRow row in dt.Rows) {
                
                string strLocal = row["CONNECT"].ToString();
                string nameDb = row["NAME"].ToString();
                string typeDb = row["TYPEBD"].ToString();
                await Task.Run(() => DownLoad(strSever, strLocal, nameDb, typeDb));
                //if (typeDb == "SQL") {
                //    Db_SQL sqlDb = new Db_SQL();
                //    await Task.Run(() => sqlDb.Download(nameDb, strLocal, strSever));
                //} else {
                //    Db_Access unisAccess = new Db_Access();
                //    await Task.Run(() => unisAccess.Download(nameDb, strLocal, strSever));
                //}
                
            }
        }

        private void DownLoad(string strSever, string strLocal, string nameDb, string typeDb) {
            //string strSever = cboSever.SelectedValue.ToString().Trim().Replace("\n", "").Replace("\t", "");
            //string strLocal = row["CONNECT"].ToString();
            //string nameDb = row["NAME"].ToString();
            //string typeDb = row["TYPEBD"].ToString();
            if (typeDb == "SQL") {
                Db_SQL sqlDb = new Db_SQL();
                sqlDb.Download(nameDb, strLocal, strSever);
            } else {
                Db_Access unisAccess = new Db_Access();
                unisAccess.Download(nameDb, strLocal, strSever);
            }
        }

        private void FrmUpload_Load(object sender, EventArgs e) {
            DataSet ds = Lib.ClassLib.ReadXML(Application.StartupPath + "\\configATTN.XML");
            SetCbo(cboLocal, ds.Tables["DB_LOCAL"]);
            SetCbo(cboSever, ds.Tables["DB_SERVER"]);
            TypeData = ds.Tables["config"].Rows[0]["DATA"].ToString();
            
        }

        private async void ManualDownload() {
            DataTable dt = (DataTable)cboLocal.DataSource;
            int selectedIndex = cboLocal.SelectedIndex;
            string strSever = cboSever.SelectedValue.ToString().Trim().Replace("\n", "").Replace("\t", "");
            string strLocal = dt.Rows[selectedIndex]["CONNECT"].ToString();
            string nameDb = dt.Rows[selectedIndex]["NAME"].ToString();
            string typeDb = dt.Rows[selectedIndex]["TYPEBD"].ToString();
            await Task.Run(() => DownLoad(strSever, strLocal, nameDb, typeDb));
        }

        private void CmdStart_Click(object sender, EventArgs e) {
            ManualDownload();
        }

        private void KillProcess(string argProcessName) {
            foreach (Process process in Process.GetProcessesByName(argProcessName)) {
                process.Kill();
                WriteLog($"Kill Process: {argProcessName}");
            }
        }

        private void WriteLog(string argText, bool isErr = false) {
            txtLog.BeginInvoke(new Action(() => {
                txtLog.Text += argText + "\r\n";
                txtLog.SelectionStart = txtLog.TextLength;
                txtLog.ScrollToCaret();
                txtLog.Refresh();
            }));
        }

        private void WriteStatus(string argText) {
            lblStatus.BeginInvoke(new Action(() => {
                lblStatus.Text = argText;
            }));
        }

        private void CmdCheckDb_Click(object sender, EventArgs e) {
            Classlib.Connection connect = new Classlib.Connection();
            bool bLocal = connect.CanConnectAccess(cboLocal.SelectedValue.ToString());
            string severConnectStr = cboSever.SelectedValue.ToString().Trim().Replace("\n", "").Replace("\t", "");
            bool bSever = connect.CanConnectOracle(severConnectStr);
            if (bLocal && bSever)
                WriteLog("Connect Success");
        }

        private void SetCbo(ComboBox argCbo, DataTable dt) {
            argCbo.DataSource = dt;
            argCbo.DisplayMember = "NAME";
            argCbo.ValueMember = "CONNECT";
            argCbo.SelectedIndex = 0;
        }

        private void chkAuto_CheckedChanged(object sender, EventArgs e) {
            dtpRunTime.Enabled = !chkAuto.Checked;
        }



        #region Timer
        private void InitTimerAuto() {
            tmrAuto.Interval = 1000;
            tmrAuto.Tick += TmrAuto_Tick;
            tmrAuto.Start();
        }

        bool isRunning = false;
        private void TmrAuto_Tick(object sender, EventArgs e) {
            if (!chkAuto.Checked) return;
            if (DateTime.Now.Hour != dtpRunTime.Value.Hour ||
                DateTime.Now.Minute != dtpRunTime.Value.Minute) {
                isRunning = false;
                return;
            }
            if (isRunning) return;
            isRunning = true;
            AutoDownload();

        }

        #endregion Timer

        #region SQL
        [Obsolete]
        private void DownloadSQL() {
            try {
                WriteLog(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "--->Start Download");

                string strSever = cboSever.SelectedValue.ToString();
                string strLocal = cboLocal.SelectedValue.ToString();
                string strUser = System.Net.Dns.GetHostName();
                string strIP = System.Net.Dns.GetHostEntry(strUser).AddressList.GetValue(1).ToString();
                string strSql = "";
                string[] arrVar;
                DataTable dtSql1 = new DataTable()
                    ;
                DataTable dtSql2 = new DataTable();
                DataTable dtDept = new DataTable();
                DataTable dtSever = new DataTable();

                using (SqlConnection conSql = new SqlConnection(strLocal)) {
                    conSql.Open();

                    // DataTable dt1 = _db.getDataSQL("select ATID, CODE, NAME, CARDNUMBER, DEPARTMENTCODE, STATUS, PRIVILEGEONDEVICE, ENABLEONDEVICE, CMDCODE from tblEmployeeNew", conSql, "");
                    using (OracleConnection conOra = new OracleConnection(strSever)) {
                        conOra.Open();
                        using (OracleCommand command = new OracleCommand("PW_TL_SCAN_MC_DOWN_SQL_S", conOra)) {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add("ARG_DATA", OracleType.VarChar).Value = TypeData;
                            command.Parameters.Add("ARG_NAME_DB", OracleType.VarChar).Value = cboLocal.Text;
                            command.Parameters.Add("ARG_IP", OracleType.VarChar).Value = strIP;
                            command.Parameters.Add("ARG_USER", OracleType.VarChar).Value = strUser;
                            command.Parameters.Add("OUT_CURSOR", OracleType.Cursor);
                            command.Parameters.Add("OUT_CURSOR2", OracleType.Cursor);

                            command.Parameters["ARG_DATA"].Direction = ParameterDirection.Input;
                            command.Parameters["ARG_NAME_DB"].Direction = ParameterDirection.Input;
                            command.Parameters["ARG_IP"].Direction = ParameterDirection.Input;
                            command.Parameters["ARG_USER"].Direction = ParameterDirection.Input;
                            command.Parameters["OUT_CURSOR"].Direction = ParameterDirection.Output;
                            command.Parameters["OUT_CURSOR2"].Direction = ParameterDirection.Output;

                            DataSet ds = new DataSet();
                            (new OracleDataAdapter(command)).Fill(ds);
                            dtSql1 = ds.Tables[0];
                            dtSql2 = ds.Tables[1];
                        }

                        DataTable dtLocal = _db.getDataSQL(dtSql1.Rows[0]["SQL_TEXT1"].ToString(), conSql, "Get Emp in SQL DB");
                        int iCol = dtLocal.Columns.Count;
                        arrVar = new string[iCol + 1];

                        _db.execOra(string.Format(dtSql1.Rows[0]["SQL_TEXT2"].ToString(), strUser + "|" + strIP), conOra, "Insert Sql to Ora");

                        strSql = dtSql1.Rows[0]["SQL_TEXT3"].ToString();

                        WriteLog("  " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "-->Begin Ora HE_EM_RF ");
                        foreach (DataRow row in dtLocal.Rows) {
                            for (int i = 0; i < iCol; i++)
                                arrVar[i] = row[i].ToString();
                            arrVar[iCol] = strUser + "|" + strIP;
                            if (_db.execOra(string.Format(strSql, arrVar), conOra, "Insert Sql to Ora") == 0) {
                                WriteLog(" Insert Sql to Ora: " + row[0]);
                            }
                        }
                        WriteLog("  " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "-->End Ora HE_EM_RF ");


                        //Get data Oracle
                        WriteLog("  " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "-->Begin get data Ora");
                        using (OracleCommand command = new OracleCommand("PW_TL_SCAN_MC_DOWNLOAD_S", conOra)) {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add("ARG_DATA", OracleType.VarChar).Value = TypeData;
                            command.Parameters.Add("ARG_NAME_DB", OracleType.VarChar).Value = cboLocal.Text;
                            command.Parameters.Add("ARG_IP", OracleType.VarChar).Value = strIP;
                            command.Parameters.Add("ARG_USER", OracleType.VarChar).Value = strUser;
                            command.Parameters.Add("OUT_CURSOR", OracleType.Cursor);
                            command.Parameters.Add("OUT_CURSOR2", OracleType.Cursor);

                            command.Parameters["ARG_DATA"].Direction = ParameterDirection.Input;
                            command.Parameters["ARG_NAME_DB"].Direction = ParameterDirection.Input;
                            command.Parameters["ARG_IP"].Direction = ParameterDirection.Input;
                            command.Parameters["ARG_USER"].Direction = ParameterDirection.Input;
                            command.Parameters["OUT_CURSOR"].Direction = ParameterDirection.Output;
                            command.Parameters["OUT_CURSOR2"].Direction = ParameterDirection.Output;

                            DataSet ds = new DataSet();
                            (new OracleDataAdapter(command)).Fill(ds);
                            dtSever = ds.Tables[0];
                            dtDept = ds.Tables[1];
                        }
                        WriteLog("  " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "-->End get data Ora");

                        //insert emp
                        WriteLog("  " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "-->Begin insert emp");
                        foreach (DataRow row in dtSever.Rows) {
                            if (row["EMP_NO"].ToString() == "19120069") { }
                            _db.execSql(row["SQL_CODE"].ToString(), conSql, row["SQL_CODE"].ToString());
                        }
                        WriteLog("  " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "-->End insert emp");

                        //Insert Dept
                        WriteLog("  " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "-->Begin insert dept");
                        foreach (DataRow row in dtDept.Rows) {
                            _db.execSql(row["SQL_CODE"].ToString(), conSql, row["SQL_CODE"].ToString());
                        }
                        WriteLog("  " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "-->End insert dept");
                    }

                }
            } catch (Exception ex) {
                WriteLog("  Error !!!");
                Lib.ClassLib.writeToLog("Download SQL : " + ex.ToString());
            } finally {
                cmdStart.Enabled = true;
                WriteLog(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "--->Finish Download ");
            }
        }

        #endregion


        




        
    }
}
