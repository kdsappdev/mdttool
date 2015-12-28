using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.Csharp_CodeGen.Plugin.Utils
{
   public class CodeGenRuleHelper
   {
       public const string Ibatis = "ibatis";
       //Object
       public const string DALServer = "DALServer";
       public const string DALServerSummary = "数据访问服务";
       public const string IDALServerSummary = "数据访问服务接口";

       //BLL
       
       public const string BLLService = "BLLService";
       public const string BLLServerSummary = "业务层";
       public const string IBLLServerSummary = "业务接口";
       //Spring
       public const string Object = "Object.xml";

       //GUI
       public const string GUI = "GUI.cs";
       public const string resxChs = "GUI.zh-CHS.resx";
       public const string resx = "GUI.resx";
   }
}
