using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.Core.Resources;
namespace MDT.Tools.DB.SetTableAndTableColumnComment.Plugin.UI
{
    public partial class tableInfoForm : DockContent
    {
        public tableInfoForm()
        {
            InitializeComponent();
            btnExecute.Image = Resources.start;
        }
    }
}
