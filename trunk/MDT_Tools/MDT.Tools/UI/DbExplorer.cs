using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace MDT.Tools.UI
{
    public partial class DbExplorer : ToolWindow
    {
        public DbExplorer()
        {
            InitializeComponent();
        }
        protected override void OnRightToLeftLayoutChanged(EventArgs e)
        {
            treeView1.RightToLeftLayout = RightToLeftLayout;
        }
    }
}
