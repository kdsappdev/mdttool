﻿using System;
using System.Collections.Generic;
using System.Text;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Core.Plugin
{
    public class PluginBroadCast
    {
        Dictionary<string, Dictionary<int, IPlugin>> dicSub = new Dictionary<string, Dictionary<int, IPlugin>>();
        public void Subscribe(string name, IPlugin plugin)
        {
            try
            {
                Dictionary<int, IPlugin> dic;
                if (!dicSub.ContainsKey(name))
                {
                    dic = new Dictionary<int, IPlugin>();
                    dicSub.Add(name, dic);
                }
                else
                {
                    dic = dicSub[name];
                }
                if (!dic.ContainsKey(plugin.PluginKey))
                {
                    dic.Add(plugin.PluginKey, plugin);
                }
            }
            catch (Exception ex)
            {

                LogHelper.Error(ex);
            }

        }
        public void Unsubscribe(string name, IPlugin plugin)
        {
            try
            {
                if (dicSub.ContainsKey(name))
                {
                    Dictionary<int, IPlugin> dic = dicSub[name];
                    if (dic.ContainsKey(plugin.PluginKey))
                    {
                        dic.Remove(plugin.PluginKey);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        public void BroadCast(string name, object o)
        {
            try
            {
                if (dicSub.ContainsKey(name))
                {
                    Dictionary<int, IPlugin> dic = dicSub[name];
                    IPlugin[] plugins = new IPlugin[dic.Count];
                    dic.Values.CopyTo(plugins, 0);
                    for (int i = 0; i < plugins.Length; i++)
                    {
                        if (plugins[i] != null)
                        {
                            plugins[i].onNotify(name, o);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
    }
}
