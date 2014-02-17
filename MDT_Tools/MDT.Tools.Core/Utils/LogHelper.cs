using System;
using MDT.Tools.Core.Log;

namespace MDT.Tools.Core.Utils
{
   public class LogHelper
    {
       static readonly ILog Log=new Log4netLog();
       public  static  void Debug(string str)
       {
           Log.Debug(str);
       }
       public static void Error(Exception ex)
       {
           Log.Error(ex);
       }
       public static void Error(string str)
       {
           Log.Error(str);
       }
       public static void Warn(string str)
       {
           Log.Warn(str);
       }
    }
}
