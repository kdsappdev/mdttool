using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Order.Monitor.Plugin
{

    public class Event_ConfigInfo
    {
        private List<ConfigInfo> configInfo = new List<ConfigInfo>();
        public List<ConfigInfo> ConfigInfo
        {
            get { return configInfo; }
            set { configInfo = value; }
        }
        private string ip;

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }
        private int port;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        private string id;

        public string svrId
        {
            get { return id; }
            set { id = value; }
        }

        private string topicstr1;

        public string topicStr1
        {
            get { return topicstr1; }
            set { topicstr1 = value; }
        }

        private string topicstr2;

        public string topicStr2
        {
            get { return topicstr2; }
            set { topicstr2 = value; }
        }

        private string topichistorystr1;

        public string topichistoryStr1
        {
            get { return topichistorystr1; }
            set { topichistorystr1 = value; }
        }

        private string topichistorystr2;

        public string topichistoryStr2
        {
            get { return topichistorystr2; }
            set { topichistorystr2 = value; }
        }
    }


    public class ConfigInfo
    {


        private string pFSMonitorName;

        public string PFSMonitorName
        {
            get { return pFSMonitorName; }
            set { pFSMonitorName = value; }
        }

    }
}
