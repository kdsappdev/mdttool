using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.DB.Common;
namespace MDT.Tools.DB.SetComment.Plugin
{
    public class SetCommentPlugin : DBSubPlugin
    {
        #region 插件信息

        private int _tag = 6;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 16; }
        }

        public override string PluginName
        {
            get { return "设置表及列描述信息插件"; }
        }

        public override string Description
        {
            get { return "根据数据库表结构设置表及列描述信息."; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }

        #endregion       

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
                _tsiGen.Text = "表备注修改";             
                _tsiGen.Click += new EventHandler(_tsiGen_Click);
                
            }
        }

        void _tsiGen_Click(object sender, EventArgs e)
        {
            var drTable = getObject(PluginShareHelper.DBPluginKey, PluginShareHelper.DBPlugin_DBCurrentCheckTable) as DataRow[];

             setComment(drTable);
            
        }
        private void setComment(DataRow[] drTable)
        {
            var sc = new setComment();
             process(drTable, sc);
        }
        
        #endregion
    }
}
