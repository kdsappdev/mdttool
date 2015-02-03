using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.Java_CodeGen.Plugin.Model
{
    public class JavaCodeGenConfig
    {
       public string BSPackage { get; set; }
       public string WSPackage { get; set; }
       public string OutPut { get; set; }
       public string TableFilter { get; set; }
       public bool IsShowGenCode { get; set; }
       public string CodeRule { get; set; }
       public string Ibatis { get; set; }

       public bool IsDelete { get; set; }
       public string DisplayName { get; set; }
       public string Id { get; set; }
    }
}
