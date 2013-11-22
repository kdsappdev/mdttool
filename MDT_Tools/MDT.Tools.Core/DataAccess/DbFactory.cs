using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
namespace DNCCFrameWork.DataAccess
{
    public class DbFactory
    {       
       
        //private static readonly string ConnectionString = "Data Source=202.196.65.59;Initial Catalog=DNCC;User ID=DNCC;Password=DNCC";
        //private  string DbProviderString = ConfigurationManager.AppSettings["DbProvider"].ToString();
        private string DbProviderString = string.Empty;
        //private static readonly string DbProviderString = "SqlHelper";
        public  IDbHelper IDbHelper = null;

        #region 构造函数
        public DbFactory(string connectionStringName)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            BaseDbHelper baseHelper = (BaseDbHelper)Assembly.Load("MDT.Tools.Core").CreateInstance("DNCCFrameWork.DataAccess.DbProvider." + DbProviderString);
            baseHelper.ConnectionString = ConnectionString;
            IDbHelper = baseHelper;
        }
        public DbFactory(string connectionString, string dbProvider)
        {
            string ConnectionString = connectionString;
            //ConnectionString = "Data Source=.;Initial Catalog=LoadTest;Integrated Security=True";
            BaseDbHelper baseHelper = (BaseDbHelper)Assembly.Load("MDT.Tools.Core").CreateInstance("DNCCFrameWork.DataAccess.DbProvider." + dbProvider);
            baseHelper.ConnectionString = ConnectionString;
            IDbHelper = baseHelper;
        }

        #endregion
    }
}
