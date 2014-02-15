﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.Utils;
using MDT.Tools.Core.Plugin.WindowsPlugin;

namespace MDT.Tools
{
    internal class PluginManagers : IPluginManager
    {
        List<IPluginManager> pluginManagerList = new List<IPluginManager>();


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
                return PluginHelper.DicToIlist(_dicPlugin);
            }
        }
        private string _pluginSign = ".dll";
        public string PluginSign
        {
            set { _pluginSign = value; }
        }

        public string RunTimeConfigPath { get; set; }
        public string PublicKey { get; set; }
        private RunTimeConfig rtc;
        public void Init()
        {
            rtc = IniConfigHelper.ReadRunTimeConfig(RunTimeConfigPath, PublicKey);
            LoadDefault(_pluginSign);
        }

        public void Loading()
        {
            try
            {
                List<IPlugin> plugins = PluginHelper.DicToIlist(_dicPlugin);
                plugins.Sort(new PluginComparer());
                List<IPlugin> framePlugins = new List<IPlugin>();
                List<IPlugin> functionPlugins = new List<IPlugin>();
                foreach (int framePluginKey in rtc.GetFramePluginKeyList)
                {
                    IPlugin p = GetPlugin(framePluginKey);
                    if (p != null)
                    {
                        plugins.Remove(p);
                        framePlugins.Add(p);
                    }
                    else
                    {
                        LogHelper.Error(new Exception("Frame Plugin Lost,Please Check."));
                        Environment.Exit(0);
                    }
                }
                foreach (int functionPluginKey in rtc.GetFunctionPluginKeyList)
                {
                    IPlugin p = GetPlugin(functionPluginKey);
                    if (p != null)
                    {
                        plugins.Remove(p);
                        functionPlugins.Add(p);
                    }
                }

                foreach (IPlugin plugin in framePlugins)
                {
                    try
                    {
                        plugin.OnLoading();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                        Environment.Exit(0);
                    }
                }

                foreach (IPlugin plugin in plugins)
                {
                    try
                    {
                        plugin.OnLoading();

                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                Environment.Exit(0);
            }
        }

        public void Unloading()
        {
            List<IPlugin> plugins = PluginHelper.DicToIlist(_dicPlugin);
            plugins.Sort(new PluginComparer2());
            List<IPlugin> framePlugins = new List<IPlugin>();
            List<IPlugin> functionPlugins = new List<IPlugin>();
            foreach (int framePluginKey in rtc.GetFramePluginKeyList)
            {
                IPlugin p = GetPlugin(framePluginKey);
                if (p != null)
                {
                    plugins.Remove(p);
                    framePlugins.Add(p);
                }
            }
            foreach (int functionPluginKey in rtc.GetFunctionPluginKeyList)
            {
                IPlugin p = GetPlugin(functionPluginKey);
                if (p != null)
                {
                    plugins.Remove(p);
                    functionPlugins.Add(p);
                }
            }

            
            

           
            foreach (IPlugin plugin in plugins)
            {
                try
                {
                    plugin.BeforeTerminating();
                    
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }
            for (int i = framePlugins.Count - 1; i >= 0; i--)
            {
                try
                {
                    framePlugins[i].BeforeTerminating();
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }
        }

        public void LoadDefault(string pluginSign)
        {

            _pluginSign = pluginSign;
            LoadAllPlugins(AppDomain.CurrentDomain.BaseDirectory, true, _pluginSign);
        }

        public void LoadAllPlugins(string pluginFolderPath, bool searchChildFolder, string pluginSign)
        {
            var config = new TypeLoadConfig(CopyToMemory, false, pluginSign);
            IList<Type> pluginManagerTypeList = ReflectionHelper.LoadDerivedType(typeof(IPluginManager), pluginFolderPath, searchChildFolder, config);
            for (int i = 0; i < pluginManagerTypeList.Count; i++)
            {
                var pluginManager = (IPluginManager)Activator.CreateInstance(pluginManagerTypeList[i]);
                pluginManager.CopyToMemory = CopyToMemory;
                pluginManager.Application = Application;
                pluginManager.Init();
                foreach (var plugin in pluginManager.PluginList)
                {
                    if (_dicPlugin.ContainsKey(plugin.PluginKey))
                    {
                        _dicPlugin.Remove(plugin.PluginKey);
                    }
                    _dicPlugin.Add(plugin.PluginKey, plugin);

                }
                pluginManagerList.Add(pluginManager);

            }


        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void DynRemovePlugin(int pluginKey)
        {
            throw new NotImplementedException();
        }

        public void EnablePlugin(int pluginKey)
        {
            throw new NotImplementedException();
        }

        public void DisEnablePlugin(int pluginKey)
        {
            throw new NotImplementedException();
        }

        public IPlugin GetPlugin(int pluginKey)
        {
            IPlugin plugin=null;
            if (ContainsPlugin(pluginKey))
            {
                plugin = _dicPlugin[pluginKey];
            }
            return plugin;
        }

        public bool ContainsPlugin(int pluginKey)
        {
            return _dicPlugin.ContainsKey(pluginKey);
        }

        public event PluginChanged PluginChanged;
    }
}
