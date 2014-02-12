using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.Common
{
    public class PluginShareHelper
    {
        public const int DBPluginKey = 1;
        public const string DBPlugin_DBCurrentCheckTable = "DBCurrentCheckTable";
        public const string DBPlugin_DBCurrentDBAllTable = "DBCurrentDBAllTable";
        public const string DBPlugin_DBCurrentDBAllTablesColumns = "DBCurrentDBAllTablesColumns";
        public const string DBPlugin_DBCurrentDBViews = "DBCurrentDBViews";
        public const string DBPlugin_DBCurrentDBTablesPrimaryKeys = "DBCurrentDBTablesPrimaryKeys";

        //共享变量
        public const string DBPlugin_DBCurrentDBName = "DBCurrentDBName";
        public const string DBPlugin_DBCurrentDBConnectionString = "DBCurrentDBConnectionString";
        public const string DBPlugin_DBCurrentDBType = "DBCurrentDBType";

          //共享常量
        public const string DBPlugin_DBtable = "DBtable";
        public const string DBPlugin_DBtablesColumns = "DBtablesColumns";
        public const string DBPlugin_DBviews = "DBviews";
        public const string DBPlugin_DBtablesPrimaryKeys = "DBtablesPrimaryKeys";

        public const string DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0 = "BroadCast_CheckTableNumberIsGreaterThan0";//广播：选中table个数是否大于0
        public const string DBPlugin_BroadCast_DBEnable = "BroadCastDBEnable";
        //共享控件
        public const string DBPlugin_tsslMessage = "tsslMessage";//状态栏label
        public const string CmcSubPlugin = "CmcSubPlugin";// 上下文菜单
        public const string DBPlugin_tspbLoadDBProgress = "tspbLoadDBProgress";// 状态栏进度条

       

        public const string DBPlugin_TapControl = "TapControl";// 配置界面
        public const string DBPlugin_BtnSave = "BtnSave";// 保存按钮

        //数据库编码

        public const string DBPlugin_OriginalEncoding = "OriginalEncoding";
        public const string DBPlugin_TargetEncoding = "TargetEncoding";
    }
}
