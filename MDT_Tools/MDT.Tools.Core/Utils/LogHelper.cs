using System;
using MDT.Tools.Core.Log;

namespace MDT.Tools.Core.Utils
{
   public class LogHelper
    {
       static readonly ILog Log=new ConsoleLog();
       public  static  void Debug(string str)
       {
           Log.Debug(str);
       }
       public static void Error(Exception ex)
       {
           Log.Error(ex);
       }
    }
}
