using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Threading.Tasks;
using System.Threading;
using System.Data.OleDb;

//using Oracle.DataAccess.Client;


namespace ATTN
{
    public partial class FrmUpload : Form
    {
        public FrmUpload()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            
        }

        DownService _db = new DownService();
        Dictionary<string, string> _dtnTypeDb = new Dictionary<string, string>();
        Thread _thrd;
      

        #region Event

        private void FrmUpload_Load(object sender, EventArgs e)
        {
            DataSet ds = Lib.ClassLib.ReadXML(Application.StartupPath + "\\configATTN.XML");
            setCbo(cboLocal, ds.Tables["DB_LOCAL"]);
            setCbo(cboSever, ds.Tables["DB_SERVER"]);
            
            tmrLoad.Interval = Convert.ToInt32(ds.Tables["config"].Rows[0]["timer"].ToString()) * 1000;
            
        }

        private void TmrLoad_Tick(object sender, EventArgs e)
        {
            if (!_thrd.IsAlive)
            {
                RunUpload();
            }          
        }

        private void CmdStart_Click(object sender, EventArgs e)
        {
            RunUpload();

            tmrLoad.Start();
        }

        [Obsolete]
        private void CmdCheckDb_Click(object sender, EventArgs e)
        {
            bool bLocal = checkDB(cboLocal);
            bool bSever = checkDB(cboSever);
            if (bLocal && bSever)
                writeLog("Connect Success");
            else
                writeLog("Local: " + bLocal.ToString() + ";  Sever: " + bSever.ToString());
        }

        private void FrmUpload_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Upload UNIS
        [Obsolete]
        private void uploadUNIS()
        {
            try
            {
                using (OleDbConnection con = new OleDbConnection(cboLocal.SelectedValue.ToString()))
                {
                    con.Open();
                    /*****************
                     * Local
                     *
                     *****************/
                    DataTable dt = new DataTable();

                    string strSql = "";
                    strSql = strSql + "  SELECT A.C_Date,  A.C_Time,  A.L_TID,  A.L_UID, A.L_Mode,  A.C_Unique, B.L_RegServer  ";
                    strSql = strSql + "    FROM TENTER A, TUSER B ";
                    strSql = strSql + "   WHERE A.APPLY_YN = 'N' ";
                    strSql = strSql + "     AND A.L_UID = B.L_ID ";

                    using (OleDbDataAdapter da = new OleDbDataAdapter(strSql, con))
                    {
                        da.Fill(dt);
                    }

                    if (dt == null || dt.Rows.Count == 0) return;
                    
                    /*****************
                     * Sever
                     *
                     *****************/
                    int iRowUpload = 0, iRow = dt.Rows.Count;
                    writeLog(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "--->Upload: " + iRow.ToString());

                    string strUser = System.Net.Dns.GetHostName();
                    string strIP = System.Net.Dns.GetHostEntry(strUser).AddressList.GetValue(1).ToString();
                    using (OracleConnection connection = new OracleConnection(cboSever.SelectedValue.ToString()))
                    {

                        connection.Open();

                        foreach (DataRow row in dt.Rows)
                        {
                            using (OracleCommand command = new OracleCommand("PW_TL_SCAN_MC_UPLOAD_S", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.Add("ARG_SERVICE_ID", OracleType.VarChar).Value = "VJ";
                                command.Parameters.Add("ARG_LOAD_DATE", OracleType.VarChar).Value = row["C_DATE"].ToString();
                                command.Parameters.Add("ARG_FILE_NAME", OracleType.VarChar).Value = "";
                                command.Parameters.Add("ARG_MACHINE_ID", OracleType.VarChar).Value = row["L_TID"].ToString();
                                command.Parameters.Add("ARG_EMP_NO", OracleType.VarChar).Value = row["L_UID"].ToString();
                                command.Parameters.Add("ARG_EMPID", OracleType.VarChar).Value = row["C_UNIQUE"].ToString();
                                command.Parameters.Add("ARG_IO_DIV", OracleType.VarChar).Value = row["L_Mode"].ToString();
                                command.Parameters.Add("ARG_IO_TIME", OracleType.VarChar).Value = row["C_DATE"].ToString() + row["C_TIME"].ToString();
                                command.Parameters.Add("ARG_FIX_TF", OracleType.VarChar).Value = "";
                                command.Parameters.Add("ARG_CHECK_FIL", OracleType.VarChar).Value = "";
                                command.Parameters.Add("ARG_IP", OracleType.VarChar).Value = strIP;
                                command.Parameters.Add("ARG_USER", OracleType.VarChar).Value = strUser;

                                command.Parameters["ARG_SERVICE_ID"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_LOAD_DATE"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_FILE_NAME"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_MACHINE_ID"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_EMP_NO"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_EMPID"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_IO_DIV"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_IO_TIME"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_FIX_TF"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_CHECK_FIL"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_IP"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_USER"].Direction = ParameterDirection.Input;

                                if (command.ExecuteNonQuery().ToString() == "1")
                                {
                                    lblStatus.Text = "Upload: " + (++iRowUpload).ToString() + "/" + iRow.ToString();
                                    /*****************
                                     * Upload Status In Local
                                     *
                                     *****************/
                                    strSql = "";
                                    strSql = strSql + "  UPDATE TENTER A";
                                    strSql = strSql + "     SET A.APPLY_YN = 'Y' ";
                                    strSql = strSql + "   WHERE A.APPLY_YN = 'N' ";
                                    strSql = strSql + "     AND A.C_DATE   = '" + row["C_DATE"].ToString() + "' ";
                                    strSql = strSql + "     AND A.C_TIME   = '" + row["C_TIME"].ToString() + "' ";
                                    strSql = strSql + "     AND A.L_TID    = " + row["L_TID"].ToString() + " ";
                                    strSql = strSql + "     AND A.L_UID    = " + row["L_UID"].ToString() + " ";
                                    strSql = strSql + "     AND A.C_UNIQUE = '" + row["C_UNIQUE"].ToString() + "' ";
                                    strSql = strSql + "     AND A.L_MODE   = " + row["L_Mode"].ToString() + " ";

                                    using (OleDbCommand cmd = new OleDbCommand(strSql, con))
                                    {
                                        try
                                        {
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            writeLog("Update column APPLY_YN --> ERROR : " + row["C_UNIQUE"].ToString());
                                            Lib.ClassLib.writeToLog("Update column APPLY_YN -->ERROR : " + row["C_UNIQUE"].ToString() + "--->" + ex.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    writeLog("Upload to Sever " + (++iRowUpload).ToString() + "/" + iRow.ToString() + ": " + row["C_UNIQUE"].ToString() + "-->ERROR");
                                    //writeError("Upload to Sever -->ERROR : " + row["C_UNIQUE"].ToString());
                                    Lib.ClassLib.writeToLog("Upload to Sever -->ERROR : " + row["C_UNIQUE"].ToString());
                                }
                            }

                        }
                    }
                    lblStatus.Text = "";
                    writeLog(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "--->Finish: " + iRowUpload.ToString());
                }

            }
            catch (Exception ex)
            {
                Lib.ClassLib.writeToLog("uploadUNIS : " + ex.ToString());
            }
        }
        #endregion

        #region Upload Access
        [Obsolete]
        private void uploadACCESS()
        {
            try
            {
                using (OleDbConnection con = new OleDbConnection(cboLocal.SelectedValue.ToString()))
                {
                    con.Open();
                    /*****************
                     * Local
                     *
                     *****************/
                    DataTable dt = new DataTable();

                    string strSql = "";
                    strSql = strSql + "  SELECT A.E_Date,  A.E_Time,  A.g_ID,  A.E_id, A.e_Mode,  A.E_idno  ";
                    strSql = strSql + "    FROM TENTER A, TUSER B ";
                    strSql = strSql + "   WHERE A.APPLY_YN = 'N' ";
                    strSql = strSql + "     AND A.E_id = B.id ";

                    using (OleDbDataAdapter da = new OleDbDataAdapter(strSql, con))
                    {
                        da.Fill(dt);
                    }

                    if (dt == null || dt.Rows.Count == 0) return;

                    /*****************
                     * Sever
                     *
                     *****************/
                    int iRowUpload = 0, iRow = dt.Rows.Count;
                    writeLog(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "--->Upload: " + iRow.ToString());

                    string strUser = System.Net.Dns.GetHostName();
                    string strIP = System.Net.Dns.GetHostEntry(strUser).AddressList.GetValue(1).ToString();
                    using (OracleConnection connection = new OracleConnection(cboSever.SelectedValue.ToString()))
                    {

                        connection.Open();

                        foreach (DataRow row in dt.Rows)
                        {
                            using (OracleCommand command = new OracleCommand("PW_TL_SCAN_MC_UPLOAD_S", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.Add("ARG_SERVICE_ID", OracleType.VarChar).Value = "VJ";
                                command.Parameters.Add("ARG_LOAD_DATE", OracleType.VarChar).Value = row["E_Date"].ToString();
                                command.Parameters.Add("ARG_FILE_NAME", OracleType.VarChar).Value = "";
                                command.Parameters.Add("ARG_MACHINE_ID", OracleType.VarChar).Value = row["g_ID"].ToString();
                                command.Parameters.Add("ARG_EMP_NO", OracleType.VarChar).Value = row["E_id"].ToString();
                                command.Parameters.Add("ARG_EMPID", OracleType.VarChar).Value = row["E_idno"].ToString();
                                command.Parameters.Add("ARG_IO_DIV", OracleType.VarChar).Value = row["e_Mode"].ToString();
                                command.Parameters.Add("ARG_IO_TIME", OracleType.VarChar).Value = row["E_Date"].ToString() + row["E_Time"].ToString();
                                command.Parameters.Add("ARG_FIX_TF", OracleType.VarChar).Value = "";
                                command.Parameters.Add("ARG_CHECK_FIL", OracleType.VarChar).Value = "";
                                command.Parameters.Add("ARG_IP", OracleType.VarChar).Value = strIP;
                                command.Parameters.Add("ARG_USER", OracleType.VarChar).Value = strUser;

                                command.Parameters["ARG_SERVICE_ID"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_LOAD_DATE"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_FILE_NAME"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_MACHINE_ID"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_EMP_NO"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_EMPID"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_IO_DIV"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_IO_TIME"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_FIX_TF"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_CHECK_FIL"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_IP"].Direction = ParameterDirection.Input;
                                command.Parameters["ARG_USER"].Direction = ParameterDirection.Input;

                                if (command.ExecuteNonQuery() == 1)
                                {
                                    lblStatus.Text = "Upload: " + (++iRowUpload).ToString() + "/" + iRow.ToString();
                                    /*****************
                                     * Upload Status In Local
                                     *
                                     *****************/
                                    strSql = "";
                                    strSql = strSql + "  UPDATE TENTER A";
                                    strSql = strSql + "     SET A.APPLY_YN = 'Y' ";
                                    strSql = strSql + "   WHERE A.APPLY_YN = 'N' ";
                                    strSql = strSql + "     AND A.E_Date   = '" + row["E_Date"].ToString() + "' ";
                                    strSql = strSql + "     AND A.E_Time   = '" + row["E_Time"].ToString() + "' ";
                                    strSql = strSql + "     AND A.g_ID     = " + row["g_ID"].ToString() + " ";
                                    strSql = strSql + "     AND A.E_id     = " + row["E_id"].ToString() + " ";
                                    strSql = strSql + "     AND A.E_idno   = '" + row["E_idno"].ToString() + "' ";
                                    strSql = strSql + "     AND A.e_Mode   = '" + row["e_Mode"].ToString() + "' ";

                                    using (OleDbCommand cmd = new OleDbCommand(strSql, con))
                                    {
                                        try
                                        {
                                            cmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            writeLog("Update column APPLY_YN --> ERROR : " + row["E_id"].ToString());
                                            Lib.ClassLib.writeToLog("Update column APPLY_YN -->ERROR : " + row["E_id"].ToString() + "--->" + ex.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    writeLog("Upload to Sever " + (++iRowUpload).ToString() + "/" + iRow.ToString() + ": " + row["E_id"].ToString() + "-->ERROR");
                                    //writeError("Upload to Sever -->ERROR : " + row["C_UNIQUE"].ToString());
                                    Lib.ClassLib.writeToLog("Upload to Sever -->ERROR : " + row["E_id"].ToString());
                                }
                            }

                        }
                    }
                    lblStatus.Text = "";
                    writeLog(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "--->Finish: " + iRowUpload.ToString());
                }

            }
            catch (Exception ex)
            {
                Lib.ClassLib.writeToLog("uploadUNIS : " + ex.ToString());
            }
        }
        #endregion

        #region Upload SQL
        [Obsolete]
        private void uploadSQL()
        { }

        #endregion

       

        #region Proc

        private void RunUpload()
        {
            ThreadStart ts;

            if (cboLocal.Text == "SQL")
            { ts = new ThreadStart(uploadSQL); }
            else if (cboLocal.Text == "ACCESS")
            { ts = new ThreadStart(uploadACCESS); }
            else
                ts = new ThreadStart(uploadUNIS);

            _thrd = new Thread(ts);
            _thrd.Start();
        }

        private void writeLog(string argText)
        {
           
            txtLog.BeginInvoke(new Action(() =>
            {
                txtLog.Text += argText + "\r\n";
                txtLog.SelectionStart = txtLog.TextLength;
                txtLog.ScrollToCaret();
                txtLog.Refresh();
            }));
        }

        //private void writeError(string argText)
        //{

        //    txtLog.BeginInvoke(new Action(() =>
        //    {
        //        txtErr.Text += argText + "\r\n";
        //        txtErr.SelectionStart = txtLog.TextLength;
        //        txtErr.ScrollToCaret();
        //        txtErr.Refresh();
        //    }));
        //}

        private void setCbo(ComboBox argCbo, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _dtnTypeDb.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
            }
            argCbo.DataSource = dt;
            argCbo.DisplayMember = "NAME";
            argCbo.ValueMember = "CONNECT";
            argCbo.SelectedIndex = 0;
           // checkDB(argCbo);
        }

        [Obsolete]
        private bool checkDB(ComboBox argCbo)
        {
            if (_dtnTypeDb[argCbo.Text] == "ACCESS" || _dtnTypeDb[argCbo.Text] == "UNIS")
            {
                return _db.checkConnectAccess(argCbo.SelectedValue.ToString());
            }
            else if (_dtnTypeDb[argCbo.Text] == "SQL")
            {
                return _db.checkConnectSql(argCbo.SelectedValue.ToString());
            }
            else 
            {
                return _db.checkConnectOracle(argCbo.SelectedValue.ToString());
            }
        }


        #endregion

        
    }
}
