using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.DB.DocGen.Plugin.Gen;
using MDT.Tools.DB.DocGen.Plugin.Utils;
using MDT.Tools.DB.Common;
namespace MDT.Tools.DB.DocGen.Plugin
{
    public class DocGenPlugin : DBSubPlugin
    {
        #region 插件信息

        private int _tag = 2;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 11; }
        }

        public override string PluginName
        {
            get { return "数据字典生成插件"; }
        }

        public override string Description
        {
            get { return "根据数据库表结构生成doc文档."; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }

        #endregion

        #region

      



        

        #region 增加上下文菜单
        
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
                _tsiGen.Click += TsiDocGenClick;
                _tsiGen.Text = "生成数据库文档";                
            }
        }

        void TsiDocGenClick(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

            ThreadPool.QueueUserWorkItem(o => GenDBWord(drTable));
        }
        private void GenDBWord(DataRow[] drTable)
        {
            var gen = new GenDbWord();
            process(drTable, gen);
        }
        #endregion

        
        #endregion
    }
}
