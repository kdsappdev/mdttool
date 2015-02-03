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

       public bool IsDelete { get; set; }
       public string Id { get; set; }

        #region dll路径
       public string DALDllName { get; set; }
       public string BLLDllName { get; set; }
        #endregion

        /// <summary>
        /// 唯一标识展示名
        /// </summary>
       public string DisplayName { get; set; }
    }
}
