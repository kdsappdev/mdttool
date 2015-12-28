using System;
using System.Collections.Generic;
using MDT.Tools.Order.Monitor.Plugin.Model;

namespace MDT.Tools.Order.Monitor.Plugin
{
 
        [Serializable]
        public class TimeChange
        {
            public string Code { get; set; }
            public string Msg { get; set; }
            public long period { get; set; }
        
    }
}
