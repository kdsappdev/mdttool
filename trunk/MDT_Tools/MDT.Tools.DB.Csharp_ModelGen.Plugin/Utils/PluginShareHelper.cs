using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.Csharp_Model.Plugin.Utils
{
    public class PluginShareHelper
    {
        public static int DBPluginKey = 1;
        public static string DBPlugin_DBCurrentCheckTable =  "DBCurrentCheckTable";
        public static string DBPlugin_DBCurrentDBAllTable = "DBCurrentDBAllTable";
        public static string DBPlugin_DBCurrentDBAllTablesColumns = "DBCurrentDBAllTablesColumns";
        public static string DBPlugin_DBCurrentDBViews = "DBCurrentDBViews";
        public static string DBPlugin_DBCurrentDBTablesPrimaryKeys = "DBCurrentDBTablesPrimaryKeys";

        //共享变量
        public static string DBPlugin_DBCurrentDBName = "DBCurrentDBName";
        public static string DBPlugin_DBCurrentDBConnectionString = "DBCurrentDBConnectionString";
        public static string DBPlugin_DBCurrentDBType = "DBCurrentDBType";

          //共享常量
        public static string DBPlugin_DBtable = "DBtable";
        public static string DBPlugin_DBtablesColumns = "DBtablesColumns";
        public static string DBPlugin_DBviews = "DBviews";
        public static string DBPlugin_DBtablesPrimaryKeys = "DBtablesPrimaryKeys";

        

        //共享控件
        public static string DBPlugin_tsslMessage = "tsslMessage";//状态栏label

        public static string DBPlugin_tspbLoadDBProgress = "tspbLoadDBProgress";// 状态栏进度条

        public static string DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0 = "BroadCast_CheckTableNumberIsGreaterThan0";//广播：选中table个数是否大于0

    }
}
