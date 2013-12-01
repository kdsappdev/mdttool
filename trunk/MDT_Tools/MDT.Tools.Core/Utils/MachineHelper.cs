using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace MDT.Tools.Core.Utils
{
   public class MachineHelper
    {
       private static Mutex mutex = null;
       public static bool CheckProcessIsMultiple(string name)
       {            
           bool newMutexCreated = false;
           try
           {
               string mutexName = "Global\\" + name;
               mutex = new Mutex(false, mutexName, out newMutexCreated);
           }
           catch
           {
 
           }
           return !newMutexCreated;
       }
    }
}
