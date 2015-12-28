using System;
using System.Collections.Generic;
using MDT.Tools.Eventlog.Monitor.Plugin.Model;

namespace MDT.Tools.Eventlog.Monitor.Plugin
{
 
        [Serializable]
        public class HistoryMsg
        {
            public string Code { get; set; }
            public string Msg { get; set; }
            public List<EventMessage> Data = new List<EventMessage>();
        
    }
}
