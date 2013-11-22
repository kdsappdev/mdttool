using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.Plugin.Model
{
    internal class DbConfigInfo
    {
        private string dbServerName = "";

        public string DbServerName
        {
            get { return dbServerName; }
            set { dbServerName = value; }
        }
        private string dbUserName = "";

        public string DbUserName
        {
            get { return dbUserName; }
            set { dbUserName = value; }
        }
        private string dbUserPwd = "";

        public string DbUserPwd
        {
            get { return dbUserPwd; }
            set { dbUserPwd = value; }
        }
        private string dbType = "";

        public string DbType
        {
            get { return dbType; }
            set { dbType = value; }
        }
        private string dbConfigName = "";

        public string DbConfigName
        {
            get { return dbConfigName; }
            set { dbConfigName = value; }
        }
        public string ConnectionString
        {
            get 
            {
                return string.Format("\"Data Source ={0}; User Id ={1}; Password ={2}\"", dbServerName, dbUserName, dbUserPwd);
            }
            set
            {
                string temp = value;
                if (!string.IsNullOrEmpty(temp))
                {
                    string[] temps = temp.Split(new char[] { '=', ';' },StringSplitOptions.RemoveEmptyEntries);
                    if (temps.Length.Equals(6))
                    {
                        dbServerName = temps[1];
                        dbUserName = temps[3];
                        dbUserPwd = temps[5];
                    }
                }
            }
        }
    }
}
