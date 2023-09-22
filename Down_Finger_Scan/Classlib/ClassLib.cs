using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace Lib
{
    class ClassLib
    {

        public static Action<string, bool> WriteLog;

        public static Action<string> WriteStatus;

        // public static string _strConnection;
        public static void writeToLog(string str)
        {
            try
            {
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\log";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                using (FileStream fs = new FileStream(path + "\\log" + DateTime.Now.ToString("yyyyMMdd") + ".log", FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sw.WriteLine(str);
                }
            }
            catch
            { }

        }

        public static DataTable ReadXML(string file, string name)
        {
            DataTable table = new DataTable(name);
            try
            {
                DataSet lstNode = new DataSet();
                lstNode.ReadXml(file);
                
                table = lstNode.Tables[name];
                return table;
            }
            catch (Exception ex)
            {
                writeToLog(file + "/ReadXML  :   " + ex.ToString());
                return table;
            }
        }

        public static DataSet ReadXML(string file)
        {
            try
            {
                DataSet lstNode = new DataSet();
                lstNode.ReadXml(file);
                return lstNode;
            }
            catch (Exception ex)
            {
                writeToLog(file + "/ReadXML  :   " + ex.ToString());
                return null;
            }
        }


        public static string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }


        public static string[] DataRowToArray(DataRow dr) {
            int totalColumn = dr.ItemArray.Length;
            string[] strArray = new string[totalColumn];
            for (int i = 0; i < totalColumn; i++) {
                strArray[i] = dr[totalColumn].ToString();
            }
            return strArray;
        }


    }
}
