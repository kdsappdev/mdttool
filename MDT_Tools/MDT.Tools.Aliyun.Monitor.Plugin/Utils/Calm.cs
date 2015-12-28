using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MDT.Tools.Aliyun.Monitor.Plugin.Utils
{
    public  class Calm
    {
        static int job = 0;
        public static string getName()
        {
            job++;
            return "NAME" + job;
        }

        static int i = 0;
        public static int getInt()
        {
            i++;
            return i;
        }

       

      
    }
}
