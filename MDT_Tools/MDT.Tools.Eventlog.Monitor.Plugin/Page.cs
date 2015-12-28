using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Eventlog.Monitor.Plugin
{
  
        [Serializable]
        public class Page
        {
            public string Code { get; set; }
            public string Msg { get; set; }
            public int Data{ get; set; }

        
    }
}
