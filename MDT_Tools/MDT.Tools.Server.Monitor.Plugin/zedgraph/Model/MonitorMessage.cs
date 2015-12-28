using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Server.Monitor.Plugin.ZedGraph.Model
{
    public class MonitorMessage
    {
        public string MDesc { get; set; }
        public string MKey { get; set; }
        public string MTopic { get; set; }
        public string MType { get; set; }
        public string MUpdatetime { get; set; }
        public int MValue { get; set; }
        public string MWarnlevel { get; set; }
    }
}
