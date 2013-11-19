using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.UI;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools
{
    public partial class MainForm : Form
    {
        DbExplorer dbExplorer = new DbExplorer();
        private Doc doc = new Doc();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dbExplorer.Show(DockPanelWeifenLuo, DockState.DockLeft);
            //doc.FileName = "New";
            doc.Text = "New";
            doc.Show(DockPanelWeifenLuo);
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog();
        }
    }
}
