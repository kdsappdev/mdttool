using System;
using System.Collections.Generic;
using System.Text;
using MDT.Tools.Core.Plugin.WindowsPlugin;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Core.Plugin
{
    public abstract class AbstractPlugin : IPlugin
    {
        protected bool isLoad = false;
        private IForm application;


        public IForm Application
        {
            get { return application; }
            set { application = value; }
        }

        public virtual void OnLoading()
        {
            if (!isLoad)
            {
                load();
            }
        }
        protected virtual void load()
        {
            
        }
        protected virtual void unload()
        {
        }

        public virtual void BeforeTerminating()
        {
            if (isLoad)
            {
                unload();
            }
        }
        public virtual void onNotify(string name, object o)
        {
        }

        protected void subscribe(string name, IPlugin plugin)
        {
            application.Subscribe(name, plugin);
        }
        protected void unsubscribe(string name, IPlugin plugin)
        {
            application.Unsubscribe(name, plugin);
        }
        protected void broadcast(string name, object o)
        {
            application.BroadCast(name, o);
        }

        protected string getPluginShareKey(int pluginKey, string name)
        {
            return string.Format("{0}_{1}", pluginKey, name);
        }

        protected void registerObject(string name, object obj)
        {
            string key = getPluginShareKey(PluginKey, name);
            application.RegisterObject(key, obj);
        }

        protected object getObject(int pluginKey, string name)
        {
            string key = getPluginShareKey(pluginKey, name);
            return application.GetObject(key);
        }

        protected void remove(string name)
        {
            string key = getPluginShareKey(PluginKey, name);
            application.Remove(key);
        }

        public abstract int Tag { get; set; }


        protected bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }



        public abstract int PluginKey
        {
            get;
        }
        public abstract string PluginName { get; }
        public abstract string Description { get; }

        public string Version
        {
            get
            {
                Type type = this.GetType();//+ "Build:(" + ReflectionHelper.GetPe32Time(type.Assembly.Location) + ")"; ;
                return ReflectionHelper.GetVersion(type.Assembly)+ "(build:" + ReflectionHelper.GetPe32Time(type.Assembly).ToString("yyyyMMdd") + ")";
            }
        }
        public abstract string Author { get; }
    }
}
