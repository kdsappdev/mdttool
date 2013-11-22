using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.Core.Plugin
{
    /// <summary>
    /// PluginUtils
    /// 
    /// 插件应用程序交互，用于宿主应用程序向插件传递必要的参数信息
    /// 
    /// 修改纪录
    ///   
    ///         2010.8.9 版本：1.0 孔德帅 添加
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    ///        <name>孔德帅</name>
    ///        <date>2010.8.9</date>
    /// </author> 
    /// </summary>
    public class PluginUtils
    {
        private  IDictionary<string, object> DicUtil = new Dictionary<string, object>();

        #region 注册共享信息
        public void RegisterObject(string name, object obj)
        {
            lock (DicUtil)
            {
                if (DicUtil.ContainsKey(name))
                {
                    Remove(name);
                }

                DicUtil.Add(name, obj);
            }
        }
        #endregion

        #region 获取共享信息
        public object GetObject(string name)
        {
            lock (DicUtil)
            {
                if (DicUtil.ContainsKey(name))
                {
                    return DicUtil[name];
                }
                return null;
            }
        }
        #endregion

        #region 移除共享信息
        public  void Remove(string name)
        {
            lock (DicUtil)
            {
                if (DicUtil.ContainsKey(name))
                {
                    DicUtil.Remove(name);
                }
            }
        }
        #endregion

        #region Clear
        public  void Clear()
        {
            lock (DicUtil)
            {
                DicUtil.Clear();
            }
        }
        #endregion
    }
}
