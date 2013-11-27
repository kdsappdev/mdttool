using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.Core.Utils
{
   public class EncodingHelper
    {
       public static string ConvertEncoder(Encoding originalEncoding, Encoding targetEncoding, string str)
       {
           string temp = str;
           
           if(originalEncoding!=null&&targetEncoding!=null&&!string.IsNullOrEmpty(temp))
           {
               Byte[] b = originalEncoding.GetBytes(temp);
               temp = targetEncoding.GetString(b);
           }
           return temp;
       }
    }
}
