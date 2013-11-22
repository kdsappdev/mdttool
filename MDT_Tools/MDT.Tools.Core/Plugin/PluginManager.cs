using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MDT.Tools.Core.Plugin.WindowsPlugin;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Core.Plugin
{
    /// <summary>
    /// PluginManager
    /// 
    /// 插件管理默认实现  用于加载/卸载，管理各种插件
    /// 
    /// 修改纪录
    ///   
    ///         2010.8.8 版本：1.0 孔德帅 添加
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    ///        <name>孔德帅</name>
    ///        <date>2010.8.8</date>
    /// </author> 
    /// </summary>
    public class PluginManager : IPluginManager
    {
        #region 属性
        private IForm _application = null;
        private IDictionary<int, IPlugin> _dicPlugin = new Dictionary<int, IPlugin>();
        private bool _copyToMemory = true;
        public IForm Application
        {
            get
            {
                return _application;
            }
            set
            {
                _application = value;
            }
        }
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
            set
            {

            }
        }
        private string pluginSign;
        public string PluginSign
        {
            set { pluginSign = value; }
        }
        #endregion

        #region 构造函数
        public PluginManager(IForm application)
        {
            Application = application;
            this.PluginChanged += delegate { };
        }
        public PluginManager()
        {
            this.PluginChanged += delegate { };
        }
        #endregion

        #region 方法 加载 卸载 插件

        #region 加载插件
        public void LoadDefault()
        {
            LoadAllPlugins(AppDomain.CurrentDomain.BaseDirectory, false, this.pluginSign);
        }
        public void LoadDefault(string pluginSign)
        {
            this.pluginSign = pluginSign;
            LoadAllPlugins(AppDomain.CurrentDomain.BaseDirectory, true, this.pluginSign);
        }
        public void LoadAllPlugins(string pluginFolderPath, bool searchChildFolder, string pluginSign)
        {
            TypeLoadConfig config = new TypeLoadConfig(CopyToMemory, false, pluginSign);
            IList<Type> pluginTypeList = ReflectionHelper.LoadDerivedType(typeof(IPlugin), pluginFolderPath, searchChildFolder, config);
            List<IPlugin> pluginList = new List<IPlugin>();
            for (int i = 0; i < pluginTypeList.Count; i++)
            {
                IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginTypeList[i]);
                pluginList.Add(plugin);
            }
            pluginList.Sort(new PluginComparer());
            for (int i = 0; i < pluginList.Count; i++)
            {
                IPlugin plugin = pluginList[i];
                if (this._dicPlugin.ContainsKey(plugin.PluginKey))
                {
                    this._dicPlugin.Remove(plugin.PluginKey);
                }
                this._dicPlugin.Add(plugin.PluginKey, plugin);
                plugin.Application = this.Application;
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

        #region 卸载插件
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
                    catch
                    { }
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
                    catch
                    { }
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

        #region 启动指定插件
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

        #region 停止指定插件
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

        #region 通过插件Key获取指定插件
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

        #region 是否包含指定插件
        public bool ContainsPlugin(int pluginKey)
        {
            return this._dicPlugin.ContainsKey(pluginKey);
        }
        #endregion

        #endregion

        #region 事件 插件改变后出发
        public event PluginChanged PluginChanged;
        #endregion

        #region 字典集合到List转换
        private IList<IPlugin> dicToIlist(IDictionary<int, IPlugin> dic)
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

        #region 插件排序

        #endregion
    }
}
