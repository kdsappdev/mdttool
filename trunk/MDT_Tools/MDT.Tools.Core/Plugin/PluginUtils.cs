using System.Collections.Generic;

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
        private readonly IDictionary<string, object> _dicUtil = new Dictionary<string, object>();

        #region ע�Ṳ����Ϣ
        public void RegisterObject(string name, object obj)
        {
            lock (_dicUtil)
            {
                if (_dicUtil.ContainsKey(name))
                {
                    Remove(name);
                }

                _dicUtil.Add(name, obj);
            }
        }
        #endregion

        #region ��ȡ������Ϣ
        public object GetObject(string name)
        {
            lock (_dicUtil)
            {
                if (_dicUtil.ContainsKey(name))
                {
                    return _dicUtil[name];
                }
                return null;
            }
        }
        #endregion

        #region �Ƴ�������Ϣ
        public  void Remove(string name)
        {
            lock (_dicUtil)
            {
                if (_dicUtil.ContainsKey(name))
                {
                    _dicUtil.Remove(name);
                }
            }
        }
        #endregion

        #region Clear
        public  void Clear()
        {
            lock (_dicUtil)
            {
                _dicUtil.Clear();
            }
        }
        #endregion
    }
}
