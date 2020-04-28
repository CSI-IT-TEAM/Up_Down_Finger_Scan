using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Delete_He_Card_Print
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly string ConString = "user id = HUBICVJ; password = HUBICVJ; data source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 211.54.128.21)(PORT = 1521)))(CONNECT_DATA = (SID = HUBICVJ)(SERVER = DEDICATED)));pooling = true;";

        [Obsolete]
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(ConString))
                {
                    connection.Open();
                    using (OracleCommand command = new OracleCommand("SP_DEL_HE_CARD_PRINT", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if (command.ExecuteNonQuery() == 1)
                            MessageBox.Show("Delete Success");
                        else
                            MessageBox.Show("Delete Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
    
}
