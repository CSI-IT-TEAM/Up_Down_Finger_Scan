using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;


namespace Lib
{
    class ClassLib
    {
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


        



    }
}
