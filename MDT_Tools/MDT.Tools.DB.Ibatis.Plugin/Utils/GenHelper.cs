using System;
using System.Globalization;
using System.Threading;

namespace MDT.Tools.DB.Ibatis.Plugin.Utils
{
    public class GenHelper
    {
        private readonly TextInfo _textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;

        public string GetNow()
        {
            return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
       
        public string GetField(string str)
        {
            string retval = str.ToLower();
            string[] temps = str.Split(new char[] { '_' });
            if (temps.Length > 1)
            {
                for (int i = 0; i < temps.Length; i++)
                {
                    if (i == 0)
                    {
                        retval = temps[0].ToLower();
                    }
                    else
                    {
                        retval += _textInfo.ToTitleCase(temps[i].ToLower());
                    }
                }
            }
            return retval;
        }

        public string GetProperty(string str)
        {
            string retval = "";
            string[] temps = str.Split(new char[] { '_' });
            if (temps.Length > 1)
            {
                for (int i = 0; i < temps.Length; i++)
                {
                    retval += _textInfo.ToTitleCase(temps[i].ToLower());
                }
            }
            else
            {
                retval = _textInfo.ToTitleCase(str.ToLower());
            }
            return retval;
        }
        internal bool hasPrimaryKey = false;
        public bool HasPrimaryKey(string tableName)
        {

            return hasPrimaryKey;
        }

        internal string getGeneratedKeyField = "";
        public string GetGeneratedKeyField(string tableName)
        {
            return getGeneratedKeyField;
        }

        public string GetGeneratedKeyName(string tableName)
        {
            string temp = "";
            if (ModelConfigHelper.IsExist(tableName))
            {
                temp = ModelConfigHelper.GetModelConfig(tableName).GeneratedKey.Column;
            }
            return temp;
        }
    }
}