using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.Core.Log
{
    class ConsoleLog:ILog
    {
       public void Debug(string str)
       {
           Console.WriteLine(str);
       }
       public void Warn(string str)
       {
           Console.WriteLine(str);
       }
       public void Error(Exception ex)
       {
           Console.WriteLine(ex.Message);
       }
    }
}
