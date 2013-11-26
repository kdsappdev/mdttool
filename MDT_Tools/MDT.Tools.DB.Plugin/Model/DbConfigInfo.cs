using System;

namespace MDT.Tools.DB.Plugin.Model
{
    internal class DbConfigInfo
    {
        private string _dbServerName = "";

        public string DbServerName
        {
            get { return _dbServerName; }
            set { _dbServerName = value; }
        }
        private string _dbUserName = "";

        public string DbUserName
        {
            get { return _dbUserName; }
            set { _dbUserName = value; }
        }
        private string _dbUserPwd = "";

        public string DbUserPwd
        {
            get { return _dbUserPwd; }
            set { _dbUserPwd = value; }
        }
        private string _dbType = "";

        public string DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }
        private string _dbConfigName = "";

        public string DbConfigName
        {
            get { return _dbConfigName; }
            set { _dbConfigName = value; }
        }
        public string ConnectionString
        {
            get 
            {
                return string.Format("\"Data Source ={0}; User Id ={1}; Password ={2}\"", _dbServerName, _dbUserName, _dbUserPwd);
            }
            set
            {
                string temp = value;
                if (!string.IsNullOrEmpty(temp))
                {
                    string[] temps = temp.Split(new[] { '=', ';' },StringSplitOptions.RemoveEmptyEntries);
                    if (temps.Length.Equals(6))
                    {
                        _dbServerName = temps[1];
                        _dbUserName = temps[3];
                        _dbUserPwd = temps[5];
                    }
                }
            }
        }
    }
}
