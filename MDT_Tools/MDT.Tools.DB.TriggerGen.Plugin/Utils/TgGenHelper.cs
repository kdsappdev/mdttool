using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.DB.Common;

namespace MDT.Tools.DB.TriggerGen.Plugin.Utils
{
     public  class TgGenHelper
    {
         public string getCurrentTime()
         {
             return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
         }

         public string getColumnType(ColumnInfo cInfo,string head)
         {
             string result = "";
             switch (cInfo.DataType)
             {
                 case "DATE":
                     result = "TO_CHAR(" + head + cInfo.Name + ",'YYYY-MM-DD HH24:MI:SS')";
                     break;
                 case "CLOB":
                     result = "TO_CHAR(" + head + cInfo.Name + ")";
                     break;
                 default:
                     result = head + cInfo.Name;
                     break;
             }

             return result;

             //string result;
             //string dateType = dr["DATA_TYPE"] as string;
             //switch (dateType)
             //{
             //    case "DATE":
             //        result = "TO_CHAR(" + head + columnName + ",'YYYY-MM-DD HH24:MI:SS')";
             //        break;
             //    case "CLOB":
             //        result = "TO_CHAR(" + head + columnName + ")";
             //        break;
             //    default:
             //        result = head + columnName;
             //        break;
             //}

             //return result;
         }
    }
}
