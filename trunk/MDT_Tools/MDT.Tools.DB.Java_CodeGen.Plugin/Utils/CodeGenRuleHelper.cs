using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.Java_CodeGen.Plugin.Utils
{
   public class CodeGenRuleHelper
   {
       public const string Ibatis = "ibatis";
       //bs
       public const string IBSServer = "Server";
       public const string BSServer = "Server";
       public const string BSSummary = "BS实现";
       public const string IBSSummary = "BS接口";

       //Ibatis
       public const string Example = "Example";
       public const string DAO = "DAO";

       //ws
       public const string IWSService = "";
       public const string WSService = "Service";
       public const string WSSummary = "WS实现";
       public const string IWSSummary = "WS接口";

       //spring config
       public const string SqlMapConfig = "SqlMapConfig.xml";
       public const string DAOContext = "DAOContext.xml";
       public const string WebServiceContext = "WebServiceContext.xml";
   }
}
