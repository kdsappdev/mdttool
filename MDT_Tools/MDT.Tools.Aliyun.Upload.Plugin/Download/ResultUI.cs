using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MDT.Tools.Aliyun.Upload.Plugin.Download
{
    public partial class ResultUI : Form
    {
        public ResultUI()
        {
            InitializeComponent();
        }
        public delegate void ResultUIText(string s); 
        public void setURL(string s)
        {
            if (InvokeRequired)
                Invoke(new ResultUIText(setURL), new object[] { s });
            else
                textBox1.Text = s;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
           Clipboard.SetDataObject(this.textBox1.Text.Trim(), true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
