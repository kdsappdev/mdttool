using System;
using System.Collections.Generic;
using System.Text;
using MDT.Tools.Core.Log;

namespace MDT.Tools.Core.Utils
{
   public class LogHelper
    {
       static ILog log=new ConsoleLog();
       public  static  void Debug(string str)
       {
           log.Debug(str);
       }
       public static void Error(Exception ex)
       {
           log.Error(ex);;
       }
    }
}
