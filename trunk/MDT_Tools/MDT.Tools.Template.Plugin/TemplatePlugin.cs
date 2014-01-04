using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.DB.Common;
using MDT.Tools.Template.Plugin.Gen;
using MDT.Tools.Template.Plugin.Model;
using MDT.Tools.Template.Plugin.Utils;

namespace MDT.Tools.Template.Plugin
{
    public class TemplatePlugin : DBSubPlugin
    {
        #region 插件信息

        private int _tag = 7;

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

        #region 增加上下文菜单

        
        private delegate void Simple();
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
                    base.AddContextMenu();
                    _tsiGen.Text = "自定义模板";
                    _tsiGen.Enabled = false;
                    foreach (var template in templateConfig.TemplateParas)
                    {
                        var tsmiTemplate = new ToolStripMenuItem();
                        tsmiTemplate.Text = template.MenuName;
                        tsmiTemplate.Tag = template;
                        _tsiGen.DropDownItems.AddRange(new[] {tsmiTemplate});
                        tsmiTemplate.Click += new EventHandler(tsmiTemplate_Click);
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
                    var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        var gen = new GenTemplate();
                        gen.TemplateParas = templateParas;
                        process(drTable, gen);
                    });
                }
            }
        }
        #endregion
    }
}
