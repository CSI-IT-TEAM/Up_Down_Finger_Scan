namespace DownloadPhoto {
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
            this.txtEmpIds = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtEmpIds
            // 
            this.txtEmpIds.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtEmpIds.Location = new System.Drawing.Point(145, 0);
            this.txtEmpIds.Multiline = true;
            this.txtEmpIds.Name = "txtEmpIds";
            this.txtEmpIds.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEmpIds.Size = new System.Drawing.Size(229, 355);
            this.txtEmpIds.TabIndex = 0;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(12, 12);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(120, 40);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Download Image";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 355);
            this.Controls.Add(this.txtEmpIds);
            this.Controls.Add(this.btnDownload);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEmpIds;
        private System.Windows.Forms.Button btnDownload;
    }
}

