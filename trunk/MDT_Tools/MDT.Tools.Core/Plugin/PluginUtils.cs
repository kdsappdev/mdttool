using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.Core.Plugin
{
    /// <summary>
    /// PluginUtils
    /// 
    /// ���Ӧ�ó��򽻻�����������Ӧ�ó����������ݱ�Ҫ�Ĳ�����Ϣ
    /// 
    /// �޸ļ�¼
    ///   
    ///         2010.8.9 �汾��1.0 �׵�˧ ���
    /// 
    /// �汾��1.0
    /// 
    /// <author>
    ///        <name>�׵�˧</name>
    ///        <date>2010.8.9</date>
    /// </author> 
    /// </summary>
    public class PluginUtils
    {
        private  IDictionary<string, object> DicUtil = new Dictionary<string, object>();

        #region ע�Ṳ����Ϣ
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

        #region ��ȡ������Ϣ
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

        #region �Ƴ�������Ϣ
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
