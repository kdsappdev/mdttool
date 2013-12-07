using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.DB.Common
{
    public class DBFileHelper
    {
        private DBFileHelper()
        { }
        public static readonly string SystemConfig = Application.StartupPath + "\\control\\dbconfig.ini";
        public static readonly string SaveDBDataPath = Application.StartupPath + "\\data\\";


        public static void WriteXml(DataSet ds)
        {

            if (ds != null)
            {
                try
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        string path = SaveDBDataPath + dt.TableName + ".data";
                        FileHelper.CreateDirectory(path);
                        dt.WriteXml(path, XmlWriteMode.WriteSchema);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public static DataSet ReadXml(DataSet ds, string dbConfigName, string dataType)
        {
            if (ds == null)
            {
                ds = new DataSet();
            }
            bool status = IsExist(dbConfigName, dataType);
            if (status)
            {
                try
                {
                    var dt = new DataTable();
                    string path = SaveDBDataPath + dbConfigName + dataType + ".data";
                    FileHelper.CreateDirectory(path);
                    dt.ReadXml(path);
                    ds.Tables.Add(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return ds;
        }
        public static bool IsExist(string dbConfigName, string dataType)
        {
            bool status = false;
            if (!string.IsNullOrEmpty(dbConfigName) && !string.IsNullOrEmpty(dataType))
            {
                string path = SaveDBDataPath + dbConfigName + dataType + ".data";
                FileHelper.CreateDirectory(path);
                if (File.Exists(path))
                {
                    status = true;
                }
            }
            return status;
        }
    }
}
