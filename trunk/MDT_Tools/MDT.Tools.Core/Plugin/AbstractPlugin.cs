using System;
using System.Collections.Generic;
using System.Text;
using MDT.Tools.Core.Plugin.WindowsPlugin;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Core.Plugin
{
    public abstract class AbstractPlugin : IPlugin
    {

        private IForm application;


        public IForm Application
        {
            get { return application; }
            set { application = value; }
        }

        public virtual void OnLoading()
        {

        }

        public virtual void BeforeTerminating()
        {

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

        public void RegisterObject(string name, object obj)
        {
            string key = getPluginShareKey(PluginKey, name);
            application.RegisterObject(key, obj);
        }

        public object GetObject(string name)
        {
            string key = getPluginShareKey(PluginKey, name);
            return application.GetObject(key);
        }

        public void Remove(string name)
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
                return ReflectionHelper.getVersion(type.Assembly);
            }
        }
        public abstract string Author { get; }
    }
}
