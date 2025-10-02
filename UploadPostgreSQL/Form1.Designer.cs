namespace UploadPostgreSQL {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.cmdCheckDb = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboSever = new System.Windows.Forms.ComboBox();
            this.cboLocal = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tmrLoad = new System.Windows.Forms.Timer();
            this.cmdStart = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.txtLog, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 77);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(413, 226);
            this.tableLayoutPanel1.TabIndex = 16;
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
            this.txtLog.Size = new System.Drawing.Size(407, 220);
            this.txtLog.TabIndex = 8;
            // 
            // cmdCheckDb
            // 
            this.cmdCheckDb.Font = new System.Drawing.Font("Calibri", 12F);
            this.cmdCheckDb.Location = new System.Drawing.Point(196, 2);
            this.cmdCheckDb.Name = "cmdCheckDb";
            this.cmdCheckDb.Size = new System.Drawing.Size(80, 33);
            this.cmdCheckDb.TabIndex = 15;
            this.cmdCheckDb.Text = "Check DB";
            this.cmdCheckDb.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(8, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 23);
            this.label2.TabIndex = 14;
            this.label2.Text = "Sever";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "Local";
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
            this.cboSever.TabIndex = 12;
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
            this.cboLocal.TabIndex = 11;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Calibri", 12F);
            this.lblStatus.Location = new System.Drawing.Point(201, 41);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(59, 19);
            this.lblStatus.TabIndex = 17;
            this.lblStatus.Text = "Upload:";
            // 
            // tmrLoad
            // 
            this.tmrLoad.Interval = 60000;
            // 
            // cmdStart
            // 
            this.cmdStart.Font = new System.Drawing.Font("Calibri", 12F);
            this.cmdStart.Location = new System.Drawing.Point(295, 1);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(53, 33);
            this.cmdStart.TabIndex = 18;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 303);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cmdCheckDb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboSever);
            this.Controls.Add(this.cboLocal);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cmdStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button cmdCheckDb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboSever;
        private System.Windows.Forms.ComboBox cboLocal;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer tmrLoad;
        private System.Windows.Forms.Button cmdStart;
    }
}

