using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using MDT.Tools.Core.Plugin.WindowsPlugin;

namespace MDT.Tools.Core.Plugin
{
    /// <summary>
    /// IPlugin
    /// 
    /// ����ӿڣ����в����ʵ������ӿ�
    /// 
    /// �޸ļ�¼
    ///   
    ///         2010.8.8 �汾��1.0 �׵�˧ ���
    /// 
    /// �汾��1.0
    /// 
    /// <author>
    ///        <name>�׵�˧</name>
    ///        <date>2010.8.8</date>
    /// </author> 
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Application Ӧ�ó����ṩ�ķ���ӿڣ����ڽ����ע�ᵽӦ�ó�����
        /// </summary>
        IForm Application { get;set;}

        /// <summary>
        /// OnLoading �������ڻص��������������ϱ����á����Դ�PluginUtils�ǻ�ȡ��Ӧ�ô��ݵĲ�������ʼ�����
        /// </summary>
        void OnLoading();

        /// <summary>
        /// BeforeTerminating �������ڻص���ж�ز��ǰ����
        /// </summary>
        void BeforeTerminating();

        int Tag { get;set;}
        /// <summary>
        /// Enabled ����Ƿ�����
        /// </summary>
        bool Enabled { get;set;}

        /// <summary>
        /// PluginKey ����ؼ��֣���ͬ�Ĳ����Key�ǲ�һ����
        /// </summary>
        int PluginKey { get;}

        /// <summary>
        /// ServiceName ����ṩ�ķ��������	
        /// </summary>
        string PluginName { get; }

        /// <summary>
        /// Description �����������Ϣ	
        /// </summary>
        string Description { get;}

        /// <summary>
        /// Version ����汾
        /// </summary>
        string Version { get;}

        /// <summary>
        /// Author ����
        /// </summary>
        string Author { get;}

        void onNotify(string name,object o);
    }

    public class PluginHelper
    {
        public const string PluginSign1 = "Plugin.dll"; //���еĲ������"Plugins.dll"��β

        #region �ֵ伯�ϵ�Listת��
        public static List<IPlugin> DicToIlist(IEnumerable<KeyValuePair<int, IPlugin>> dic)
        {
            List<IPlugin> pluginList = new List<IPlugin>();
            foreach (KeyValuePair<int, IPlugin> kvp in dic)
            {
                if (kvp.Value != null)
                {
                    pluginList.Add(kvp.Value);
                }
            }
            return pluginList;
        }
        #endregion

 
    }
    public class PluginComparer : IComparer<IPlugin>
    {
        public int Compare(IPlugin x, IPlugin y)
        {
            int result=0;
            if (x.Tag > y.Tag)
            {
                result = 1;
            }
            if (x.Tag < y.Tag)
            {
                result = -1;
            }
            if (x.Tag == y.Tag)
            {
                result = 0;
            }
            return result;
        }
    }

    public class PluginComparer2 : IComparer<IPlugin>
    {
        public int Compare(IPlugin x, IPlugin y)
        {
            int result = 0;
            if (x.Tag < y.Tag)
            {
                result = 1;
            }
            if (x.Tag < y.Tag)
            {
                result = -1;
            }
            if (x.Tag == y.Tag)
            {
                result = 0;
            }
            return result;
        }
    }
}
