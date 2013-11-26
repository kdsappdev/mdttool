using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.Csharp_ModelGen.Plugin.Model
{
    public class CsharpModelGenConfig
    {
       public string NameSpace { get; set; }
       public string OutPut { get; set; }
       public string TableFilter { get; set; }
       public bool IsShowGenCode { get; set; }
    }
}
