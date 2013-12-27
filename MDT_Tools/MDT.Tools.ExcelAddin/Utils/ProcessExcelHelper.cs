using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
namespace MDT.Tools.ExcelAddin.Utils
{
    public class ProcessExcelHelper
    {

        public static void processExcel(Excel.Application application)
        {
            ConfigInfo configInfo = IniConfigHelper.ReadConfigInfo();//获取配置文件
            Excel.Workbook book = (Excel.Workbook)application.ActiveWorkbook;
            string path = configInfo.OutPath;
            string fileName = configInfo.OutFileName;
            string[] strF = configInfo.OutFileName.Split(new char[] { '{', '}' });
            if (strF.Length == 3)
            {
                fileName = string.Format("{0}{1}{2}", strF[0], DateTime.Now.ToString(strF[1]), strF[2]);
                LogHelper.Info("输出的文件名：" + fileName);
            }
            FileHelper.Delete(path, fileName);
            FileHelper.Write(path, fileName, configInfo.Header, true);
            for (int i = 0; i < configInfo.RowsCount; i++)
            {
                string row = configInfo.Rows[i];
                string[] strs = row.Split(new char[] { ',' });
                foreach (string str in strs)
                {
                    string temp = str.Trim();
                    if (temp.IndexOf("}") > 0)
                    {
                        string[] str1s = temp.Replace("{", "").Replace("}", "").Split(new char[] { '.' });
                        Excel.Worksheet sheet = (Excel.Worksheet)book.Sheets[int.Parse(str1s[0])];
                        Excel.Range source = sheet.get_Range(str1s[1], str1s[1]);
                        string value = source.Value + "";
                        if (str1s.Length >= 3 && !string.IsNullOrEmpty(value))
                        {
                            int dataScale = 0;
                            decimal d = 0;
                            if (int.TryParse(str1s[2], out dataScale) && decimal.TryParse(value, out d))
                            {
                                value = string.Format("{0:N" + dataScale + "}", d);

                            }
                        }
                        row = row.Replace(temp, value);
                    }
                }
                FileHelper.Write(path, fileName, row, true);

            }
        }
    }
}
