using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.Csharp_CodeGen.Plugin.Model
{
    public class CsharpCodeGenConfig
    {
       public string ModelNameSpace { get; set; }
       public string DALNameSpace { get; set; }
       public string IDALNameSpace { get; set; }
       public string BLLNameSpace { get; set; }
       public string PluginName { get; set; }
       public string OutPut { get; set; }
       public string TableFilter { get; set; }
       public bool IsShowGenCode { get; set; }
       public bool IsShowComment { get; set; }
       public string CodeRule { get; set; }
       public string Ibatis { get; set; }
    }
}
