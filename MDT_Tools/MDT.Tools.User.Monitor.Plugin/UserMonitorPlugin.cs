using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.DB.Common;
using MDT;
namespace MDT.Tools.User.Monitor.Plugin
{
    public class OrderPlugin : AbstractPlugin
    {
        protected readonly ToolStripMenuItem _tsiGen = new ToolStripMenuItem();
        protected delegate void Simple();

        #region 插件信息

        private int _tag = 20001;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 20001; }
        }

        public override string PluginName
        {
            get { return "用户监控"; }
        }

        public override string Description
        {
            get { return "监控在线人数"; }
        }

        public override string Author
        {
            get { return "赵明明"; }
        }

        #endregion

        #region




        protected override void load()
        {
            AddContextMenu();
        }


        #region 增加上下文菜单
        protected void AddContextMenu()
        {
            if (Application.MainContextMenu.InvokeRequired)
            {
                var s = new Simple(AddContextMenu);
                Application.MainContextMenu.Invoke(s, null);
            }
            else
            {
                foreach (ToolStripMenuItem v in Application.MainMenu.Items)
                {
                    if (v.Text == "工具(&T)")
                    {
                        v.DropDownItems.Add(_tsiGen);
                        _tsiGen.Text = "用户监控";
                        _tsiGen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        _tsiGen.Click += new EventHandler(_tsiGen_Click);
                        break;
                    }

                }


            }
        }

        private void _tsiGen_Click(object sender, EventArgs e)
        {

            var form = new Form1() { Text = _tsiGen.Text };

            form.Show(Application.Panel);

        }

        #endregion


        #endregion














     






    }
}