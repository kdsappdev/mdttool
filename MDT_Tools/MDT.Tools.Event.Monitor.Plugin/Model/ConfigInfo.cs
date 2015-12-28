using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Event.Monitor.Plugin.Model
{
    public class ConfigInfo
    {
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
        private string eventMonitorName;

        public string EventMonitorName
        {
            get { return eventMonitorName; }
            set { eventMonitorName = value; }
        }
    }
    public class Event_ConfigInfo
    {
        
       
        public bool Show { get; set; }
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
        private string serId;

        public string SerId
        {
            get { return serId; }
            set { serId = value; }
        }

       
    }
}
