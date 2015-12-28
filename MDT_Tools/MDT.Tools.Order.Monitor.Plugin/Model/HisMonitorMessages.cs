using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Order.Monitor.Plugin.Model
{
    [Serializable]
    public class HisMonitorMessages
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public List<MonitorMessage> Data=new List<MonitorMessage>();
    }
}
