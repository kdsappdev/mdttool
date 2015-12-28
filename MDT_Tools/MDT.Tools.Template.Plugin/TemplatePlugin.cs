using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Lua;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Common;
using MDT.Tools.Template.Plugin.Gen;
using MDT.Tools.Template.Plugin.Model;
using MDT.Tools.Template.Plugin.Utils;

namespace MDT.Tools.Template.Plugin
{
    public class TemplatePlugin : DBSubPlugin
    {
        #region 插件信息

        private int _tag = 100;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 17; }
        }

        public override string PluginName
        {
            get { return "模板插件"; }
        }

        public override string Description
        {
            get { return "根据自定义模板生成文件."; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }

        #endregion

        protected override void load()
        {
            subscribe(PluginShareHelper.DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0, this);
            subscribe(MDT.Tools.Fix.Common.Utils.PluginShareHelper.BroadCastCheckFixNumberIsGreaterThan0, this);
            _dbContextMenuStrip = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.CmcSubPlugin) as ContextMenuStrip;
            fixCmcSubPlugin = getObject(MDT.Tools.Fix.Common.Utils.PluginShareHelper.FixPluginKey, MDT.Tools.Fix.Common.Utils.PluginShareHelper.CmsSubPlugin) as ContextMenuStrip;
            AddContextMenu();
            ILuaEngine luaEngine = LuaHelper.CreateLuaEngine();
            luaEngine.BindLuaFunctions(this);
        }
        protected override void unload()
        {
            unsubscribe(PluginShareHelper.DBPlugin_BroadCast_CheckTableNumberIsGreaterThan0, this);
            unsubscribe(MDT.Tools.Fix.Common.Utils.PluginShareHelper.BroadCastCheckFixNumberIsGreaterThan0, this);

        }
        public override void onNotify(string name, object o)
        {
           base.onNotify(name,o);
           if (MDT.Tools.Fix.Common.Utils.PluginShareHelper.BroadCastCheckFixNumberIsGreaterThan0.Equals(name) && o.GetType().IsValueType)
            {
                var flag = (bool)o;
                if(fixCmcSubPlugin!=null)
                {
                    SetFixCmcEnable(flag);
                }
            } 
        }

        protected void SetFixCmcEnable(bool flag)
        {
            if (Application.MainMenu.InvokeRequired)
            {
                var s = new SimpleBool(SetEnable);
                Application.MainMenu.Invoke(s, new object[] { flag });
            }
            else
            {
                fixCmcSubPlugin.Enabled = flag;
            }
        }

        #region 增加上下文菜单

        
       
        
        private ContextMenuStrip fixCmcSubPlugin;
        protected override void AddContextMenu()
        {
            if (Application.MainContextMenu.InvokeRequired)
            {
                var s = new Simple(AddContextMenu);
                Application.MainContextMenu.Invoke(s, null);
            }
            else
            {
                
                TemplateConfig templateConfig = IniConfigHelper.ReadTemplateConfig();
                if (templateConfig.TemplateNum > 0)
                {
                    foreach (var template in templateConfig.TemplateParas)
                    {
                        string[] strs = template.MenuName.Split(new[]{"|"},StringSplitOptions.RemoveEmptyEntries);
                        string menuName = "";
                        var tsmiTemplate = new ToolStripMenuItem();
                        tsmiTemplate.Visible = template.IsMenuVisable;
                        if (template.DataTye == "DB")
                        {
							SetEnable(false);
                            ToolStripMenuItem itemG = null;
                          if(strs.Length>=2)
                          {
                              foreach (ToolStripMenuItem temp in _dbContextMenuStrip.Items)
                              {
                                  if (temp.Text == strs[0])
                                  {
                                      itemG = temp;
                                      break;
                                  }
                              }
                              if (itemG==null)
                              {
                                  itemG=new ToolStripMenuItem();
                                  itemG.Text = strs[0];
                                  _dbContextMenuStrip.Items.Add(itemG);
                              }
                              menuName = strs[1];
                              itemG.DropDownItems.Add(tsmiTemplate);
                          }
                          else
                          {
                              menuName = strs[0];
                              _dbContextMenuStrip.Items.AddRange(new[] { tsmiTemplate });
                          }
                            tsmiTemplate.Text = menuName;
                            tsmiTemplate.Tag = template;
                            tsmiTemplate.Click += new EventHandler(tsmiTemplate_Click);
                        }
                        else if (template.DataTye == "Fix")
                        {
                              
                              if (fixCmcSubPlugin != null)
                              {
                                  SetFixCmcEnable(false);

                                  ToolStripMenuItem itemG = null;
                                  if (strs.Length >= 2)
                                  {
                                      foreach (ToolStripMenuItem temp in fixCmcSubPlugin.Items)
                                      {
                                          if (temp.Text == strs[0])
                                          {
                                              itemG = temp;
                                              break;
                                          }
                                      }
                                      if (itemG == null)
                                      {
                                          itemG = new ToolStripMenuItem();
                                          itemG.Text = strs[0];
                                          fixCmcSubPlugin.Items.Add(itemG);
                                      }
                                      menuName = strs[1];
                                      itemG.DropDownItems.Add(tsmiTemplate);
                                  }
                                  else
                                  {
                                      menuName = strs[0];
                                      fixCmcSubPlugin.Items.AddRange(new[] { tsmiTemplate });
                                  }
                                  tsmiTemplate.Text = menuName;
                                  tsmiTemplate.Tag = template;
                                  tsmiTemplate.Click += new EventHandler(tsmiTemplate_Click);
                              }
                        }
                    }
                }

            }
        }

        void tsmiTemplate_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmiTemplate=sender as ToolStripMenuItem;
            if(tsmiTemplate!=null)
            {
                TemplateParas templateParas = tsmiTemplate.Tag as TemplateParas;
                if(templateParas!=null)
                {
                    if (templateParas.DataTye == "DB")
                    {
                        var drTable =
                            getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as
                            DataRow[];

                        ThreadPool.QueueUserWorkItem(o =>
                                                         {
                                                             var gen = new GenTemplate();
                                                             gen.TemplateParas = templateParas;
                                                             process(drTable, gen);
                                                         });
                    }
                    else if (templateParas.DataTye == "Fix")
                    {
                        var os =
                            getObject(MDT.Tools.Fix.Common.Utils.PluginShareHelper.FixPluginKey, MDT.Tools.Fix.Common.Utils.PluginShareHelper.FixCurrentCheck) as
                            object[];
                        var fieldDics =
                            getObject(MDT.Tools.Fix.Common.Utils.PluginShareHelper.FixPluginKey, MDT.Tools.Fix.Common.Utils.PluginShareHelper.FixFieldDic) as
                            List<Fix.Common.Model.FieldDic>;

                        ThreadPool.QueueUserWorkItem(o =>
                        {
                            var gen = new GenTemplate();
                            gen.TemplateParas = templateParas;
                            gen.FieldDics = fieldDics;
                            getDBShare(gen);
                            gen.process(os);
                        });
                    }
                }

            }
        }
        #endregion

        #region
        [AttrLuaFunc("getDBContextMenu", "获取DB类型的上下文菜单")]
        public ContextMenuStrip getDBContextMenu()
        {
            return _dbContextMenuStrip;
        }
        [AttrLuaFunc("getFixContextMenu", "获取Fix类型的上下文菜单")]
        public ContextMenuStrip getFixContextMenu()
        {
            return fixCmcSubPlugin;
        }
        #endregion
    }
}
