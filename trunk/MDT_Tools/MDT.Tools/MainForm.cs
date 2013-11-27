using System;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.Plugin.WindowsPlugin;
using MDT.Tools.Core.UI;
using MDT.Tools.UI;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.Core.Utils;
namespace MDT.Tools
{
    public partial class MainForm : Form, IForm
    {
        readonly Explorer _explorer = new Explorer();

        public MainForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;  
            Initialize();
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            _explorer.Show(DockPanelWeifenLuo, DockState.DockLeft);
        }

        private void TsmiAboutClick(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog();
        }

        #region Initialize
        private void Initialize()
        {
           
            Text = Text + string.Format(" Beta版本:{0}(build{1})", ReflectionHelper.GetVersion(this.GetType().Assembly),ReflectionHelper.GetPe32Time(this.GetType().Assembly.Location).ToString("yyyyMMdd"));
            //((System.Reflection.AssemblyDescriptionAttribute)System.Reflection.AssemblyDescriptionAttribute.GetCustomAttribute(this.GetType().Assembly,
//typeof(System.Reflection.AssemblyDescriptionAttribute))).Description
            notifyIcon1.Text = Text;
            notifyIcon1.Icon = Icon;
            _pluginUtils=new PluginUtils();
            _pluginManager = new PluginManager(this);
            _pluginManager.LoadDefault(PluginHelper.PluginSign1);
        }

        #endregion

        #region IForm

        private IPluginManager _pluginManager;
        private PluginUtils _pluginUtils;
        public ToolStrip MainTool
        {
            get { return mainTool; }
        }

        public IPluginManager PluginManager
        {
            get { return _pluginManager; }
        }

        public MenuStrip MainMenu
        {
            get { return mainMenu; }
        }

        public StatusStrip StatusBar
        {
            get { return statusBar; }
        }

        public DockPanel Panel
        {
            get { return DockPanelWeifenLuo; }
        }

        public Explorer Explorer
        {
            get { return _explorer; }
        }

        public ContextMenuStrip MainContextMenu
        {
            get { return mainContextMenu; }
        }
        public void RegisterObject(string name, object obj)
        {
            _pluginUtils.RegisterObject(name,obj);
        }

        public object GetObject(string name)
        {
            object o=_pluginUtils.GetObject(name);
            return o;
        }

        public void Remove(string name)
        {
            _pluginUtils.Remove(name);
        }
        private readonly PluginBroadCast _pluginBroadCast=new PluginBroadCast();
        
        public void Subscribe(string name, IPlugin plugin)
        {
           _pluginBroadCast.Subscribe(name,plugin);
        }
        public void Unsubscribe(string name, IPlugin plugin)
        {
            _pluginBroadCast.Unsubscribe(name, plugin);
        }
        public void BroadCast(string name, object o)
        {
            _pluginBroadCast.BroadCast(name, o);
        }
        #endregion

        #region 退出
        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                ShowInTaskbar = false;
                Hide();
            }
        }
        private void TsmiExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region notifyIcon

        private void NotifyIcon1Click(object sender, EventArgs e)
        {
            Show();
            ShowInTaskbar = true;
        }

        #endregion
    }
}
