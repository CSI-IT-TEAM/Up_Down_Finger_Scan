using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadPhoto {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            
        }

        const string HUBIC = "user id = HUBICVJ; password = HUBICVJ; data source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 211.54.128.21)(PORT = 1521)))(CONNECT_DATA = (SID = HUBICVJ)(SERVER = DEDICATED))); pooling = false;";

        private void btnDownload_Click(object sender, EventArgs e) {
            DownloadImages();
        }

        private void DownloadImages() {
            // Lấy danh sách EMPID từ TextBox, loại bỏ khoảng trắng, tách theo dấu phẩy, xuống dòng, hoặc dấu cách
            var empIds = txtEmpIds.Text
                .Split(new[] { ',', '\n', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(id => $"'{id.Trim()}'");
            string empIdList = string.Join(",", empIds);

            using (OracleConnection connection = new OracleConnection(HUBIC)) {
                connection.Open();
                string sql = $@"
                            SELECT LOWER(FILE_NAME) AS FILE_NAME, PHOTO
                            FROM PW_HR_PHOTO_T
                            WHERE SERVICE_ID = 'VJ'
                              AND EMPID IN ({empIdList})
                        ";
                using (OracleCommand command = new OracleCommand(sql, connection)) {
                    using (DataTable dt = new DataTable()) {
                        (new OracleDataAdapter(command)).Fill(dt);
                        SaveImage(dt);
                    }
                }
            }
        }

        //get image from dt and save to local, save to local run program folder
        private void SaveImage(DataTable dt) {
            // Get the path to the "img" folder in the application's running directory
            string imgFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img");
            // Create the folder if it doesn't exist
            if (!System.IO.Directory.Exists(imgFolder)) {
                System.IO.Directory.CreateDirectory(imgFolder);
            }

            foreach (DataRow row in dt.Rows) {
                try {
                    string fileName = row["FILE_NAME"].ToString().Trim();
                    string filePath = System.IO.Path.Combine(imgFolder, fileName);
                    byte[] imageData = (byte[])row["PHOTO"];
                    System.IO.File.WriteAllBytes(filePath, imageData);
                } catch (Exception ex) {
                    // Optionally log or handle the exception
                }
            }
        }


    }
}
