using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.SetComment.Plugin.Utils
{
    internal class DBType
    {
        public static string GetDbProviderString(string dbType)
        {
            string str = "";
            switch (dbType)
            {
                case "Oracle":
                    str = "OracleHelper";
                    break;
                case "Sql Server":
                    str = "SqlHelper";
                    break;
            }
            return str;
        }
    }
}
