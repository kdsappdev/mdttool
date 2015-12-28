using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Server.Monitor.Plugin.ZedGraph.Model
{
    [Serializable]
    public class TSvrTopicRef
    {
        public string M_Key { get; set; }
        public string M_Topic { get; set; }
        public string Server_key { get; set; }
        public string Topic { get; set; }
        public decimal M_Value { get; set; }
        public string M_Warnlevel { get; set; }
        public string M_Type { get; set; }
        public string M_Desc { get; set; }
        public string M_Updatetime { get; set; }
    }
}
