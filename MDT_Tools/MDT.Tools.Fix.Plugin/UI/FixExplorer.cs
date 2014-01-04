using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.UI;

namespace MDT.Tools.Fix.Plugin.UI
{
    public partial class FixExplorer : ToolWindow
    {
        public FixExplorer()
        {
            InitializeComponent();
        }

        private void _tvFix_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void _tvFix_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cmsSubPlugin.Show(_tvFix, e.Location);

            }
        }
         private void LoadFixInfo()
        {
            //解析FixXML
            //递归创建树形结构
            _tvFix.ExpandAll();
        }
        private void FixExplorer_Load(object sender, EventArgs e)
        {
            LoadFixInfo();
        }
    }
}
