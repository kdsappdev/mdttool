using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MDT.Tools.SolutionDesign.Plugin.Properties;

namespace MDT.Tools.SolutionDesign.Plugin.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public event Action<string> sendValue;

        private void btnOk_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(teName.Text))
            {
                errorProvider1.SetError(teName, Resources.NotNUll);

                return;
            }

            if (sendValue != null)
                sendValue(teName.Text);

            this.Close();

            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
