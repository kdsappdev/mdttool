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
        public static  DataSet ConvertDataSet(DataSet ds, string OriginalEncoding, string TargetEncoding)
        {
            if (!string.IsNullOrEmpty(OriginalEncoding) && !string.IsNullOrEmpty(TargetEncoding))
            {
                foreach (DataTable dt in ds.Tables)
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (!string.IsNullOrEmpty(row[column.ColumnName] + ""))
                            {
                                row[column.ColumnName] = convertType(column.DataType,
                                                                     EncodingHelper.ConvertEncoder(
                                                                         Encoding.GetEncoding(OriginalEncoding),
                                                                         Encoding.GetEncoding(TargetEncoding),
                                                                         row[column.ColumnName] + ""));
                            }
                        }
                    }
                }
            }
            return ds;
        }

        public static DataSet WriteXml(DataSet ds, string OriginalEncoding, string TargetEncoding)
        {
            DataSet temp = ds;
            ds=ConvertDataSet(ds, OriginalEncoding, TargetEncoding);
            
            WriteXml(ds);

            return temp;
        }
        private static object convertType(Type t, string str)
        {
            object o = str;
            try
            {

                if (t == typeof(decimal))
                {
                    o = decimal.Parse(str);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(new Exception(string.Format("str:{0},type:{1}",str,t.Name),ex));
            }
            return o;
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
                LogHelper.Error(ex);
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
