using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.Plugin.WindowsPlugin;
using MDT.Tools.Core.UI;
using MDT.Tools.UI;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools
{
    public partial class MainForm : Form, IForm
    {
        Explorer explorer = new Explorer();

        public MainForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            explorer.Show(DockPanelWeifenLuo, DockState.DockLeft);
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog();
        }

        #region Initialize
        private void Initialize()
        {
            notifyIcon1.Text = this.Text;
            notifyIcon1.Icon = this.Icon;
            pluginUtils=new PluginUtils();
            pluginManager = new PluginManager(this);
            pluginManager.LoadDefault(PluginHelper.PluginSign1);
        }

        #endregion

        #region IForm

        private IPluginManager pluginManager;
        private PluginUtils pluginUtils;
        public ToolStrip MainTool
        {
            get { return this.mainTool; }
        }

        public IPluginManager PluginManager
        {
            get { return this.pluginManager; }
        }

        public MenuStrip MainMenu
        {
            get { return this.mainMenu; }
        }

        public StatusStrip StatusBar
        {
            get { return this.statusBar; }
        }

        public DockPanel Panel
        {
            get { return this.DockPanelWeifenLuo; }
        }

        public Explorer Explorer
        {
            get { return this.explorer; }
        }

        public ContextMenuStrip MainContextMenu
        {
            get { return this.mainContextMenu; }
        }
        public void RegisterObject(string name, object obj)
        {
            pluginUtils.RegisterObject(name,obj);
        }

        public object GetObject(string name)
        {
            object o=pluginUtils.GetObject(name);
            return o;
        }

        public void Remove(string name)
        {
            pluginUtils.Remove(name);
        }
        private PluginBroadCast pluginBroadCast=new PluginBroadCast();
        
        public void Subscribe(string name, IPlugin plugin)
        {
           pluginBroadCast.Subscribe(name,plugin);
        }
        public void Unsubscribe(string name, IPlugin plugin)
        {
            pluginBroadCast.Unsubscribe(name, plugin);
        }
        public void BroadCast(string name, object o)
        {
            pluginBroadCast.BroadCast(name, o);
        }
        #endregion

        #region 退出
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.ShowInTaskbar = false;
                this.Hide();
            }
        }
        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region notifyIcon

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
        }

        #endregion
    }
}
