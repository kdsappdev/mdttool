using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.User.Monitor.Plugin
{

    public class PFS_ConfigInfo
    {
        public int PFSConfigInfoNum { get; set; }
        public int AlertState { get; set; }
        public bool Show { get; set; }
        public string SoundAlert { get; set; }
        private List<ConfigInfo> configInfo = new List<ConfigInfo>();
        public List<ConfigInfo> ConfigInfo
        {
            get { return configInfo; }
            set { configInfo = value; }
        }
    }


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

        private string pFSMonitorName;

        public string PFSMonitorName
        {
            get { return pFSMonitorName; }
            set { pFSMonitorName = value; }
        }

    }
}
