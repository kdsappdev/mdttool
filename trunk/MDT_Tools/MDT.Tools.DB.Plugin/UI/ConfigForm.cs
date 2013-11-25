using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MDT.Tools.DB.Plugin.UI
{
    internal partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            tvDBConfig.ExpandAll();
        }
    }
}
