using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;

namespace  MDT.Tools.DB.SetComment.Plugin.Utils
{
    internal class FilePathHelper
    {
        private FilePathHelper()
        { }
        public static readonly string SystemConfig = Application.StartupPath + "\\plugin\\MDT.Tools.DB.SetTableAndTableColumnComment.Plugin\\config.ini";
        public static readonly string ExportCsharpModelPath = Application.StartupPath + "\\plugin\\MDT.Tools.DB.SetTableAndTableColumnComment.Plugin\\data\\";
        public static readonly string SaveDBDataPath = Application.StartupPath + "\\plugin\\MDT.Tools.DB.Plugin\\data\\";


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
    }
}
