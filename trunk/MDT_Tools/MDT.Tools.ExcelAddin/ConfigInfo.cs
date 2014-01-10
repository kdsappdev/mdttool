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




        private string serverName = "";
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }
        private string host = "";
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        private string port = "";
        public string Port
        {
            get { return port; }
            set { port = value; }
        }

        private string protocal = "";
        public string Protocal
        {
            get { return protocal; }
            set { protocal = value; }
        }

        private string topic;
        public string Topic
        {
            get { return topic; }
            set { topic = value; }
        }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }



        private string refreshTimeCCB;
        public string RefreshTimeCCB
        {
            get { return refreshTimeCCB; }
            set { refreshTimeCCB = value; }
        }

        private string refreshTimeBOC;
        public string RefreshTimeBOC
        {
            get { return refreshTimeBOC; }
            set { refreshTimeBOC = value; }
        }

        private string refreshTimePSBC;
        public string RefreshTimePSBC
        {
            get { return refreshTimePSBC; }
            set { refreshTimePSBC = value; }
        }

        #endregion



        private string shost = "";
        private int sport = 0;
        private bool enableSsl = false;
        private string userName = "";
        private string userPwd = "";
        private string subject;


        public string SHost
        {
            get { return shost; }
            set { shost = value; }
        }

        public int SPort
        {
            get { return sport; }
            set { sport = value; }
        }

        public bool EnableSsl
        {
            get { return enableSsl; }
            set { enableSsl = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string UserPwd
        {
            get { return userPwd; }
            set { userPwd = value; }
        }
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }


        private string from = "";
        private string to = "";
        private string cc = "";
        

        public string From
        {
            get { return from; }
            set { from = value; }
        }
        public string To
        {
            get { return to; }
            set { to = value; }
        }
        public string Cc
        {
            get { return cc; }
            set { cc = value; }
        }


    }
}
