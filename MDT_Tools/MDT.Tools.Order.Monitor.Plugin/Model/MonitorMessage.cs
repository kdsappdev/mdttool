using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Order.Monitor.Plugin.Model
{
    public class MonitorMessage
    {
        public string date { get; set; }
        public string time { get; set; }
        public string type { get; set; }
        public string symbol{ get; set; }
        public int num { get; set; }
        public double amount { get; set; }
        public string memo { get; set; }
    }
}
