
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MDT.Tools.Core.Utils;


namespace MDT.Tools.Aliyun.Upload.Plugin.DataMemory
{
    /// <summary>
    /// 记忆功能，数据存储在Data目录中UserData.xml中
    /// </summary>
    internal class UserDataManager
    {

        private Dictionary<String, UserDataObject> _Dic = null;


        /// <summary>
        /// userDataObject.key 要用所在模块名+作用来命名，防止key重复
        /// </summary>
        /// <param name="userDataObject"></param>
        public void SaveUserObject(UserDataObject userDataObject)
        {
           String key =  userDataObject.Key;
            if (_Dic.ContainsKey(key))
            {
                _Dic.Remove(key);
            }
            _Dic.Add(key, userDataObject);
            SaveXml();
        }

        public UserDataObject GetUserObject( string key)
        {
            UserDataObject obj = null;
            //key = location + "^" + userName + "^" + key;
            if (_Dic.ContainsKey(key))
            {
                obj = _Dic[key];
            }
            return obj;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, UserDataObject> kvp in _Dic)
            {
                sb.AppendLine(kvp.Value.ToString());
            }
            return sb.ToString();
        }

        private String GetPath()
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\data";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\UserData.xml";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
               LogHelper.Error(ex);
            }
            return path;
        }

        public UserDataManager()
        {
            _Dic = new Dictionary<string, UserDataObject>();

            LoadXml();
        }

        private Boolean LoadXml()
        {
            _Dic.Clear();
            String path = GetPath();
            try
            {
                UserDataObjects objs = XmlSerialize.DeserializeFile(typeof (UserDataObjects), path) as UserDataObjects;
                if (objs != null)
                {
                    foreach (UserDataObject obj in objs.Lists)
                    {
                        String key = obj.Key;
                        _Dic.Add(key, obj);
                    }
                }
            }
            catch
            {
                
            }
            return true;
        }

        private Boolean SaveXml()
        {
            String path = GetPath();
            UserDataObjects objs = new UserDataObjects();
            foreach (String key in _Dic.Keys)
            {
                objs.Lists.Add(_Dic[key]);
            }

            XmlSerialize.SerializeFile(objs, path);

            _Dic.Clear();

            LoadXml();
            return true;
        }
    }

    #region Model

    #region UserDataObjects
    [Serializable]
    public class UserDataObjects : Object
    {
        public UserDataObjects()
        {
        }

        private List<UserDataObject> _Lists = new List<UserDataObject>();

        public List<UserDataObject> Lists
        {
            get { return _Lists; }
            set { _Lists = value; }
        }

    }
    #endregion

    #region UserDataObject
    [Serializable]
    public class UserDataObject
    {
        //private String _Location = "";

        //public String Location
        //{
        //    get { return _Location; }
        //    set { _Location = value; }
        //}
        //private String _UserName = "";

        //public String UserName
        //{
        //    get { return _UserName; }
        //    set { _UserName = value; }
        //}
        private string key = "";

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private String _Value = "";

        public String Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        //public override string ToString()
        //{
        //    return string.Format("Location:{0},UserName:{1},Key:{2},Value:{3}", Location, UserName, key, Value);
        //}
        public override string ToString()
        {
            return string.Format("Key:{0},Value:{1}",  key, Value);
        }

    }
    #endregion

    #endregion
}
