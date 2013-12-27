using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.ExcelAddin
{
    public class ConfigInfo
    {
        #region 字段
        private string header;
        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        private string menuName;
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }

        private List<string> rows = new List<string>();
        public List<string> Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        private string outFileName;
        public string OutFileName
        {
            get { return outFileName; }
            set { outFileName = value; }
        }

        private int rowsCount;
        public int RowsCount
        {
            get { return rowsCount; }
            set { rowsCount = value; }
        }

        private string outPath;

        public string OutPath
        {
            get { return outPath; }
            set { outPath = value; }
        }
        private string vkInterval;

        public string VkInterval
        {
            get { return vkInterval; }
            set { vkInterval = value; }
        }

        private bool logon;

        public bool Logon
        {
            get { return logon; }
            set { logon = value; }
        }

        #endregion
    }
}
