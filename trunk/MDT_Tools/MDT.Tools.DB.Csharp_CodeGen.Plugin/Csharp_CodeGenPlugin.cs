using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.DB.Common;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Gen;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Utils;
using MDT.Tools.DB.Csharp_ModelGen.Plugin.UI;
using PluginShareHelper = MDT.Tools.DB.Csharp_CodeGen.Plugin.Utils.PluginShareHelper;


namespace MDT.Tools.DB.Csharp_CodeGen.Plugin
{
    public class Csharp_CodeGenPlugin : DBSubPlugin
    {
        #region 插件信息

        private int _tag = 3;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 13; }
        }

        public override string PluginName
        {
            get { return "Csharp代码生成插件"; }
        }

        public override string Description
        {
            get { return "根据数据库表结构生成Csharp_Model代码类."; }
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
        Csharp_CodeGenConfigUI cmcUI=new Csharp_CodeGenConfigUI();
        private void AddConfig()
        {
            var tabControl = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_TapControl) as TabControl;
            TabPage page=new TabPage("Csharp项目配置");
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

                MessageBox.Show("保存失败["+ex.Message+"]", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        #endregion

        #region 增加上下文菜单
       
        private readonly ToolStripMenuItem _tsiModelGen = new ToolStripMenuItem();
        private readonly ToolStripMenuItem _tsiDALGen = new ToolStripMenuItem();
        private readonly ToolStripMenuItem _tsiBLLGen = new ToolStripMenuItem();
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
                _tsiGen.Text = "Csharp代码生成";
                _tsiModelGen.Text = "Model代码生成";
                _tsiDALGen.Text = "DALWebService代码生成";
                _tsiBLLGen.Text = "BLL&GUI代码生成";
                _tsispringConfigGen.Text = "Spring配置生成";
                _tsiGen.Enabled = false;
                _tsiGen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                _tsiGen.DropDownItems.AddRange(new[] { _tsiModelGen, _tsiDALGen,_tsiBLLGen, _tsispringConfigGen });
                 _tsiModelGen.Click +=(_tsiModelGen_Click);
                 _tsiDALGen.Click += (_tsiDALGen_Click);
                 _tsiBLLGen.Click += (_tsiBLLGen_Click);
                 _tsispringConfigGen.Click += (_tsispringConfigGen_Click);
                 Application.MainContextMenu.Items.Add(_tsiGen);
            }
        }

        void _tsiBLLGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => GenBLLAndGUI(drTable));
        }
        void _tsispringConfigGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => GenCsharpSpringConfig(drTable));
        }

        void _tsiDALGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => GenDALServer(drTable));
        }

        void _tsiModelGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => Gen(drTable));
        }
        private void Gen(DataRow[] drTable)
        {
            var gen = new GenCsharpModel();
            gen.cmc = IniConfigHelper.ReadCsharpModelGenConfig();
            process(drTable, gen);
        }
        private void GenCsharpSpringConfig(DataRow[] drTable)
        {
            var gen = new GenCsharpSpringConfig();
            gen.cmc = IniConfigHelper.ReadCsharpModelGenConfig();
            process(drTable,gen);
        }
        private void GenDALServer(DataRow[] drTable)
        {
            var gen = new GenCsharpDALWebService ();
            gen.cmc = IniConfigHelper.ReadCsharpModelGenConfig();
            process(drTable, gen);
        }
        private void GenBLLAndGUI(DataRow[] drTable)
        {
            var gen = new GenCsharpBLLAndGUI();
            gen.cmc = IniConfigHelper.ReadCsharpModelGenConfig();
            process(drTable, gen);
        }
        #endregion
    }
}
