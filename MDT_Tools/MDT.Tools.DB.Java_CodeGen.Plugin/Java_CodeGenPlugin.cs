using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;

using MDT.Tools.DB.Java_CodeGen.Plugin.Utils;
using MDT.Tools.DB.Java_CodeGen.Plugin.UI;
using MDT.Tools.DB.Java_CodeGen.Plugin.Gen;
using MDT.Tools.DB.Common;

namespace MDT.Tools.DB.Java_CodeGen.Plugin
{
    public class Java_CodeGenPlugin : DBSubPlugin
    {
        #region 插件信息

        private int _tag = 4;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 14; }
        }

        public override string PluginName
        {
            get { return "Java代码生成插件"; }
        }

        public override string Description
        {
            get { return "根据数据库表结构生成Java bs,ws代码类."; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }

        #endregion

        #region load,unload
        protected override void load()
        {
            AddConfig();
            base.load();
        }
         
        #endregion

        #region 增加配置
        Java_CodeGenConfigUI cmcUI = new Java_CodeGenConfigUI();
        private void AddConfig()
        {
            var tabControl = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_TapControl) as TabControl;
            TabPage page = new TabPage("Java项目配置");
            cmcUI.Dock = DockStyle.Fill;
            page.Controls.Add(cmcUI);
            tabControl.Controls.Add(page);
            var button = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_BtnSave) as Button;
            button.Click += button_Click;
        }

        void button_Click(object sender, EventArgs e)
        {
            try
            {
                cmcUI.Save();
            }
            catch (Exception ex)
            {

                MessageBox.Show("保存失败[" + ex.Message + "]", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion

        #region 增加上下文菜单
         
        private readonly ToolStripMenuItem _tsibsGen = new ToolStripMenuItem();
        private readonly ToolStripMenuItem _tsiwsGen = new ToolStripMenuItem();
        private readonly ToolStripMenuItem _tsispringConfigGen = new ToolStripMenuItem();
       
        protected override void AddContextMenu()
        {
            if (Application.MainContextMenu.InvokeRequired)
            {
                var s = new Simple(AddContextMenu);
                Application.MainContextMenu.Invoke(s, null);
            }
            else
            {
                base.AddContextMenu();
                _tsiGen.Text = "Java代码生成";
                _tsibsGen.Text = "BS代码生成";
                _tsiwsGen.Text = "WS代码生成";
                _tsispringConfigGen.Text = "Spring配置生成";
                _tsiGen.Enabled = false;
                 
                _tsiGen.DropDownItems.AddRange(new[] { _tsibsGen, _tsiwsGen, _tsispringConfigGen });
                _tsibsGen.Click += (_tsibsGen_Click);
                _tsiwsGen.Click += (_tsiwsGen_Click);
                _tsispringConfigGen.Click += (_tsispringConfigGen_Click);
               
            }
        }

        void _tsispringConfigGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => GenJavaSpringConfig(drTable));
        }

        void _tsibsGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => GenbsServer(drTable));
        }

        void _tsiwsGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => GenwsService(drTable));
        }

        private void GenbsServer(DataRow[] drTable)
        {
            var gen = new GenJavaBS();
            
            gen.cmc = IniConfigHelper.ReadCsharpModelGenConfig();
            process(drTable, gen);             
        }

        private void GenJavaSpringConfig(DataRow[] drTable)
        {
            var gen = new GenJavaSpringConfig();
            gen.cmc = IniConfigHelper.ReadCsharpModelGenConfig();
            process(drTable, gen);  
        }
        private void GenwsService(DataRow[] drTable)
        {
            var gen = new GenJavaWS();
            gen.cmc = IniConfigHelper.ReadCsharpModelGenConfig();
            process(drTable, gen);  
        }

        #endregion
    }
}
