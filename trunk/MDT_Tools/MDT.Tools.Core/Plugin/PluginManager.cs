using System;
using System.Collections.Generic;
using MDT.Tools.Core.Plugin.WindowsPlugin;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Core.Plugin
{
    /// <summary>
    /// PluginManager
    /// 
    /// �������Ĭ��ʵ��  ���ڼ���/ж�أ�������ֲ��
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
    public class PluginManager : IPluginManager
    {
        #region ����

        private readonly IDictionary<int, IPlugin> _dicPlugin = new Dictionary<int, IPlugin>();
        private bool _copyToMemory = false;
        public IForm Application { get; set; }

        public bool CopyToMemory
        {
            get
            {
                return _copyToMemory;
            }
            set
            {
                _copyToMemory = value;
            }
        }

        public IList<IPlugin> PluginList
        {
            get
            {
                return dicToIlist(_dicPlugin);
            }
        }
        private string _pluginSign;
        public string PluginSign
        {
            set { _pluginSign = value; }
        }
        #endregion

        #region ���캯��
        public PluginManager(IForm application)
        {
            Application = application;
            PluginChanged += delegate { };
        }
        public PluginManager()
        {
            PluginChanged += delegate { };
        }
        #endregion

        #region ���� ���� ж�� ���

        #region ���ز��
        public void LoadDefault()
        {
            LoadAllPlugins(AppDomain.CurrentDomain.BaseDirectory, false, _pluginSign);
        }
        public void LoadDefault(string pluginSign)
        {
            _pluginSign = pluginSign;
            LoadAllPlugins(AppDomain.CurrentDomain.BaseDirectory, true, _pluginSign);
        }
        public void LoadAllPlugins(string pluginFolderPath, bool searchChildFolder, string pluginSign)
        {
            var config = new TypeLoadConfig(CopyToMemory, false, pluginSign);
            IList<Type> pluginTypeList = ReflectionHelper.LoadDerivedType(typeof(IPlugin), pluginFolderPath, searchChildFolder, config);
            var pluginList = new List<IPlugin>();
            for (int i = 0; i < pluginTypeList.Count; i++)
            {
                var plugin = (IPlugin)Activator.CreateInstance(pluginTypeList[i]);
                pluginList.Add(plugin);
            }
            pluginList.Sort(new PluginComparer());
            for (int i = 0; i < pluginList.Count; i++)
            {
                IPlugin plugin = pluginList[i];
                if (_dicPlugin.ContainsKey(plugin.PluginKey))
                {
                    _dicPlugin.Remove(plugin.PluginKey);
                }
                _dicPlugin.Add(plugin.PluginKey, plugin);
                plugin.Application = Application;
                try
                {
                    plugin.OnLoading();
                    PluginChanged();
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }

            }

        }
        #endregion

        #region ж�ز��
        public void Clear()
        {
            if (_dicPlugin.Count > 0)
            {
                foreach (IPlugin plugin in _dicPlugin.Values)
                {
                    try
                    {
                        plugin.BeforeTerminating();
                    }
                    catch (Exception ex)
                    { LogHelper.Error(ex);}
                }
                _dicPlugin.Clear();
                PluginChanged();
            }
        }
        public void DynRemovePlugin(int pluginKey)
        {
            try
            {
                IPlugin plugin = GetPlugin(pluginKey);
                if (plugin != null)
                {
                    plugin.Enabled = false;
                    try
                    {
                        plugin.BeforeTerminating();
                    }
                    catch (Exception ex)
                    { LogHelper.Error(ex);}
                    _dicPlugin.Remove(pluginKey);
                    PluginChanged();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
        #endregion

        #region ����ָ�����
        public void EnablePlugin(int pluginKey)
        {
            try
            {
                IPlugin plugin = GetPlugin(pluginKey);
                if (plugin != null && !plugin.Enabled)
                {
                    plugin.Enabled = true;
                    plugin.OnLoading();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
        #endregion

        #region ָֹͣ�����
        public void DisEnablePlugin(int pluginKey)
        {
            IPlugin plugin = GetPlugin(pluginKey);
            if (plugin != null)
            {
                plugin.Enabled = false;
                plugin.BeforeTerminating();
            }
        }

        #endregion

        #region ͨ�����Key��ȡָ�����
        public IPlugin GetPlugin(int pluginKey)
        {
            IPlugin plugin = null;
            if (_dicPlugin.ContainsKey(pluginKey))
            {
                plugin = _dicPlugin[pluginKey];
            }
            return plugin;
        }
        #endregion

        #region �Ƿ����ָ�����
        public bool ContainsPlugin(int pluginKey)
        {
            return _dicPlugin.ContainsKey(pluginKey);
        }
        #endregion

        #endregion

        #region �¼� ����ı�����
        public event PluginChanged PluginChanged;
        #endregion

        #region �ֵ伯�ϵ�Listת��
        private IList<IPlugin> dicToIlist(IEnumerable<KeyValuePair<int, IPlugin>> dic)
        {
            IList<IPlugin> pluginList = new List<IPlugin>();
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

        #region �������

        #endregion
    }
}
