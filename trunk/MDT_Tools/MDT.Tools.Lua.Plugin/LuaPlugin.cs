using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.Lua;
using System.IO;
using MDT.Tools.Lua.Plugin.Utils;
using MDT.Tools.Core.Utils;
namespace MDT.Tools.Lua.Plugin
{
    public class LuaPlugin : AbstractPlugin
    {
        #region 插件信息

        private int _tag = 31;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 31; }
        }

        public override string PluginName
        {
            get { return "lua脚本插件引擎"; }
        }

        public override string Description
        {
            get { return "lua脚本插件引擎,可以编写Lua脚本来增加插件"; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }
        protected override void load()
        {
            laodLuaScript();
            loadLua();
        }
        protected override void unload()
        {
            unloadLua();
        }
        public override void onNotify(string name, object o)
        {
            onNotifyLua(name, o);
        }

        #endregion

        #region lua Function

        #region 加载脚本文件
        private string luaScriptPostfix = "plugin.lua";
        Dictionary<int, ILuaEngine> luaEngines = new Dictionary<int, ILuaEngine>();
        private void laodLuaScript()
        {
            loadLuaScriptAllfolders(FilePathHelper.LuaScriptPath);
        }
        private void loadLuaScriptAllfolders(string luaScriptPath)
        {
            loadLuaScriptFile(luaScriptPath);
            string[] folders = Directory.GetDirectories(luaScriptPath);
            foreach (string nextFolder in folders)
            {
                loadLuaScriptAllfolders(nextFolder);
            }
        }
        private void loadLuaScriptFile(string luaScriptPath)
        {
            string[] files = Directory.GetFiles(luaScriptPath);
            foreach (string fileName in files)
            {
                if (fileName.EndsWith(luaScriptPostfix))
                {
                    try
                    {
                        ILuaEngine luaEngine = LuaHelper.CreateLuaEngine();
                        luaEngine.BindLuaFunctions(this);
                        LogHelper.Debug(luaEngine.ToString());
                        luaEngine.DoFile(fileName);
                        object[] luaPa = luaEngine.Invoke("init");
                        if (luaPa != null && luaPa.Length == 5)
                        {
                            int luaPluginKey = 0;

                            if (int.TryParse(luaPa[1] + "", out luaPluginKey) && !luaEngines.ContainsKey(luaPluginKey))
                            {
                                luaEngines.Add(luaPluginKey, luaEngine);
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
        #endregion

        public void loadLua()
        {
            foreach (ILuaEngine luaEngine in luaEngines.Values)
            {
                luaEngine.Invoke("load");
            }
        }

        public void unloadLua()
        {
            foreach (ILuaEngine luaEngine in luaEngines.Values)
            {
                luaEngine.Invoke("unload");
            }
        }

        public void onNotifyLua(string name, object o)
        {
            if (luaTopics.ContainsKey(name))
            {
                List<int> subscribers = luaTopics[name];
                foreach (int luaPluginKey in subscribers)
                {
                    if (luaEngines.ContainsKey(luaPluginKey))
                    {
                        luaEngines[luaPluginKey].Invoke("onNotify", name, o);
                    }
                }

            }
        }
        [AttrLuaFunc("registerObject", "注册插件之间共享的信息", "信息key", "信息内容")]
        public new void registerObject(string name, object obj)
        {
            base.registerObject(name, obj);
        }
        [AttrLuaFunc("getObject", "获取插件之间共享的信息", "信息key", "信息内容")]
        public new object getObject(int pluginKey, string name)
        {
            return base.getObject(pluginKey, name);
        }
        [AttrLuaFunc("removeObject", "移除插件之间共享的信息", "信息key")]
        public void removeObject(string name)
        {
            base.remove(name);
        }
        Dictionary<string, List<int>> luaTopics = new Dictionary<string, List<int>>();
        [AttrLuaFunc("unsubscribe", "退订插件之间时时改变的信息", "信息key", "Lua插件的key")]
        public void unsubscribe(string name, int luaPluginKey)
        {

            if (luaTopics.ContainsKey(name))
            {
                List<int> subscribers = luaTopics[name];
                if (subscribers.Contains(luaPluginKey))
                {
                    subscribers.Remove(luaPluginKey);
                }
                if (subscribers.Count == 0)//没有lua插件订阅
                {
                    unsubscribe(name, this);
                }
            }


        }
        [AttrLuaFunc("subscribe", "订阅插件之间时时改变的信息", "信息key", "Lua插件的key")]
        public void subscribe(string name, int luaPluginKey)
        {

            List<int> subscribers = null;
            if (luaTopics.ContainsKey(name))//获取该主题对应的订阅者
            {
                subscribers = luaTopics[name];
            }
            else
            {
                subscribers = new List<int>();
                luaTopics.Add(name, subscribers);
                subscribe(name, this);//初次订阅
            }
            if (!subscribers.Contains(luaPluginKey))
            {
                subscribers.Add(luaPluginKey);
            }
        }
        [AttrLuaFunc("broadcast", "广播插件之间时时改变的信息", "信息key", "信息内容")]
        public new void broadcast(string name, object o)
        {
            base.broadcast(name, o);
        }
        [AttrLuaFunc("getPluginShareKey", "获取插件共享的key", "信息key", "Lua插件的key")]
        public string getPluginShareKey(string name, int pluginKey)
        {
            return getPluginShareKey(pluginKey, name);
        }

        #region 创建界面
        [AttrLuaFunc("getApplication", "获取IApplication")]
        public IApplication getApplication()
        {
            return Application;
        }
        #endregion

        #region log
        [AttrLuaFunc("debug", "调试日志记录", "日志内容")]
        public void debug(object str)
        {
            LogHelper.Debug(str + "");
        }
        [AttrLuaFunc("warn", "警告日志记录", "日志内容")]
        public void warn(object str)
        {
            LogHelper.Warn(str + "");
        }
        [AttrLuaFunc("error", "错误日志记录", "日志内容")]
        public void error(object ex)
        {
            LogHelper.Error(new Exception(ex + ""));
        }
        #endregion

        #region DataRow
        [AttrLuaFunc("getDataRowValue", "获取DataRowValue", "DataRow", "列名")]
        public object getDataRowValue(System.Data.DataRow dr, string columnName)
        {
            object value= dr[columnName];
            if (value == DBNull.Value)
            {
                value = "";
            }
            return value;
        }
        [AttrLuaFunc("setDataRowValue", "设置DataRowValue", "DataRow", "列名", "列value")]
        public void setDataRowValue(System.Data.DataRow dr, string columnName, object value)
        {
            dr[columnName] = value;
        }
        #endregion

        #region dataTable
        [AttrLuaFunc("getDistinctDataTable", "获取getDistinctDataTable", "DataTable","是否Distinct", "列名")]
        public System.Data.DataTable getDistinctDataTable(System.Data.DataView dv,bool isDistinct, LuaInterface.LuaTable columnNames)
        {
            int count = columnNames.Values.Count;//dt.Merge(dt,true,System.Data.MissingSchemaAction)
            string[] strs = new string[count];
            columnNames.Values.CopyTo(strs, 0);
            System.Data.DataTable temp= dv.ToTable(isDistinct, strs);//temp.Select("")
            return temp;
        }
        #endregion

        #region dataGridView
        [AttrLuaFunc("getDataGridViewRowCellValue", "获取getDataGridViewRowCellValue", "DataGridView", "rowIndex", "columnIndex")]
        
        public object getDataGridViewRowCellValue(DataGridView dgv, int rowIndex, int columnIndex)
        {
            object value = dgv.Rows[rowIndex].Cells[columnIndex].Value;
            return value;
        }
        #endregion

        #endregion

    }
}
