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
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            tvDBConfig.ExpandAll();
        }
    }
}
