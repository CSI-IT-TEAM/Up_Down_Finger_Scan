namespace ATTN
{
    partial class DownloadData {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadData));
            this.cboLocal = new System.Windows.Forms.ComboBox();
            this.cboSever = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdCheckDb = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmdStart = new System.Windows.Forms.Button();
            this.dtpRunTime = new System.Windows.Forms.DateTimePicker();
            this.chkAuto = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboLocal
            // 
            this.cboLocal.BackColor = System.Drawing.Color.White;
            this.cboLocal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboLocal.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold);
            this.cboLocal.FormattingEnabled = true;
            this.cboLocal.Location = new System.Drawing.Point(69, 2);
            this.cboLocal.Name = "cboLocal";
            this.cboLocal.Size = new System.Drawing.Size(121, 31);
            this.cboLocal.TabIndex = 0;
            // 
            // cboSever
            // 
            this.cboSever.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSever.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboSever.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold);
            this.cboSever.FormattingEnabled = true;
            this.cboSever.Location = new System.Drawing.Point(69, 34);
            this.cboSever.Name = "cboSever";
            this.cboSever.Size = new System.Drawing.Size(121, 31);
            this.cboSever.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Local";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(8, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sever";
            // 
            // cmdCheckDb
            // 
            this.cmdCheckDb.Font = new System.Drawing.Font("Calibri", 12F);
            this.cmdCheckDb.Location = new System.Drawing.Point(337, 4);
            this.cmdCheckDb.Name = "cmdCheckDb";
            this.cmdCheckDb.Size = new System.Drawing.Size(79, 33);
            this.cmdCheckDb.TabIndex = 5;
            this.cmdCheckDb.Text = "Check DB";
            this.cmdCheckDb.UseVisualStyleBackColor = true;
            this.cmdCheckDb.Click += new System.EventHandler(this.CmdCheckDb_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.txtLog, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 102);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(428, 226);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // txtLog
            // 
            this.txtLog.AllowDrop = true;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Calibri", 11F);
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(422, 220);
            this.txtLog.TabIndex = 8;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Calibri", 12F);
            this.lblStatus.Location = new System.Drawing.Point(8, 79);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(78, 19);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Download:";
            // 
            // cmdStart
            // 
            this.cmdStart.Font = new System.Drawing.Font("Calibri", 12F);
            this.cmdStart.Location = new System.Drawing.Point(258, 4);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(79, 33);
            this.cmdStart.TabIndex = 9;
            this.cmdStart.Text = "Manual";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.CmdStart_Click);
            // 
            // dtpRunTime
            // 
            this.dtpRunTime.CalendarFont = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpRunTime.CustomFormat = "HH:mm";
            this.dtpRunTime.Enabled = false;
            this.dtpRunTime.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpRunTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpRunTime.Location = new System.Drawing.Point(295, 39);
            this.dtpRunTime.Name = "dtpRunTime";
            this.dtpRunTime.ShowUpDown = true;
            this.dtpRunTime.Size = new System.Drawing.Size(121, 27);
            this.dtpRunTime.TabIndex = 10;
            this.dtpRunTime.Value = new System.DateTime(2023, 5, 20, 10, 0, 0, 0);
            // 
            // chkAuto
            // 
            this.chkAuto.AutoSize = true;
            this.chkAuto.Checked = true;
            this.chkAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAuto.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAuto.Location = new System.Drawing.Point(200, 41);
            this.chkAuto.Name = "chkAuto";
            this.chkAuto.Size = new System.Drawing.Size(93, 23);
            this.chkAuto.TabIndex = 12;
            this.chkAuto.Text = "Auto Run";
            this.chkAuto.UseVisualStyleBackColor = true;
            this.chkAuto.CheckedChanged += new System.EventHandler(this.chkAuto_CheckedChanged);
            // 
            // DownloadData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 328);
            this.Controls.Add(this.chkAuto);
            this.Controls.Add(this.dtpRunTime);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cmdCheckDb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboSever);
            this.Controls.Add(this.cboLocal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DownloadData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Download";
            this.Load += new System.EventHandler(this.FrmUpload_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboLocal;
        private System.Windows.Forms.ComboBox cboSever;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdCheckDb;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.DateTimePicker dtpRunTime;
        private System.Windows.Forms.CheckBox chkAuto;
    }
}

