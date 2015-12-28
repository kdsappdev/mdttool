using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Server.Monitor.Plugin.Model
{
    public class MSDResult
    {
        public MSD MSD { get; set; }
        public string Code { get; set; }
        public string Msg { get; set; }
    }

    public class MSD
    {
        public List<TSvrinfo> svrinfos = new List<TSvrinfo>();
        public List<TSvrRef> svrrefs = new List<TSvrRef>();
    }

    public class TSvrRef
    {
        public string Servername_From { get; set; }

        public string Servername_To { get; set; }
    }

    public class TSvrinfo
    {
        public string ServiceName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Type { get; set; }
        public string isMainServer { get; set; }
        public string Ref_ServiceName { get; set; }
        public string Remark { get; set; }
    }
}
