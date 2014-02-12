using System;
using System.Data;
using System.IO;
using System.Text;
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

        public static DataSet WriteXml(DataSet ds,string OriginalEncoding, string TargetEncoding)
        {
            WriteXml(ds);
            DataSet temp=new DataSet();
             foreach (DataTable dt in ds.Tables)
             {
                 string fileName = dt.TableName;
                 foreach (string str in FileHelper.FileNameNotAllowed)
                 {
                     fileName = fileName.Replace(str, "");
                 }

                 string path = SaveDBDataPath + fileName + ".data";
                 string s = File.ReadAllText(path);
                 if (!string.IsNullOrEmpty(OriginalEncoding) && !string.IsNullOrEmpty(TargetEncoding))
                     s = EncodingHelper.ConvertEncoder(Encoding.GetEncoding(OriginalEncoding), Encoding.GetEncoding(TargetEncoding), s);
                 File.WriteAllText(path, s);
                 ReadXml(temp, fileName);
             
             }
            return temp;
        }

        public static void WriteXml(DataSet ds)
        {

            if (ds != null)
            {
                try
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        string fileName = dt.TableName;
                        foreach (string str in FileHelper.FileNameNotAllowed)
                        {
                            fileName = fileName.Replace(str, "");
                        }
                        string path = SaveDBDataPath + fileName + ".data";
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
                string fileName = dbConfigName + dataType;
                ReadXml(ds, fileName);
            }
            return ds;
        }
        public static DataSet ReadXml(DataSet ds, string fileName)
        {
            try
            {
                var dt = new DataTable();
                
                foreach (string str in FileHelper.FileNameNotAllowed)
                {
                    fileName = fileName.Replace(str, "");
                }
                string path = SaveDBDataPath + fileName + ".data";
                FileHelper.CreateDirectory(path);
                dt.ReadXml(path);
                ds.Tables.Add(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ds;
        }

        public static bool IsExist(string dbConfigName, string dataType)
        {
            bool status = false;
            if (!string.IsNullOrEmpty(dbConfigName) && !string.IsNullOrEmpty(dataType))
            {
                string fileName = dbConfigName + dataType;
                foreach (string str in FileHelper.FileNameNotAllowed)
                {
                    fileName = fileName.Replace(str, "");
                }
                string path = SaveDBDataPath + fileName + ".data";
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
