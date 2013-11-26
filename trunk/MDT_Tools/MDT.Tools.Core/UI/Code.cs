using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools.Core.UI
{
    public partial class Code : DockContent
    {
        public Code()
        {
            InitializeComponent();
        }

        public string CodeContent
        {
            get { return richTextBox1.Text; }
            set { richTextBox1.Text = value; }
        }
    }
}
