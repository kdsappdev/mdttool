using System;
using System.Diagnostics;
using System.Windows.Forms;
using KnightsWarriorAutoupdater;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.Plugin.WindowsPlugin;
using MDT.Tools.Core.UI;
using MDT.Tools.UI;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.Core.Utils;
using MDT.Tools.Core.Resources;
using System.IO;
using System.Reflection;
namespace MDT.Tools
{
    public partial class MainForm : Form, IForm
    {
        
        public MainForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            Initialize();
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            _pluginManager.Loading();
            System.Threading.ThreadPool.QueueUserWorkItem(o =>
                                                              {
                                                                  #region

                                                                   
                                                                  IAutoUpdater autoUpdater = new AutoUpdater();
                                                                  bool isUpdate = false;
                                                                  try
                                                                  {
                                                                      isUpdate = autoUpdater.IsUpdate();
                                                                      if (isUpdate)
                                                                      {
                                                                          DialogResult dr = MessageBox.Show(this,
                                                                                                            "检查到有新版，是否升级?",
                                                                                                            "提示",
                                                                                                            MessageBoxButtons
                                                                                                                .YesNo,
                                                                                                            MessageBoxIcon.
                                                                                                                Information);
                                                                          if (dr == DialogResult.Yes)
                                                                          {
                                                                              Process.Start(
                                                                                  System.Configuration.ConfigurationSettings
                                                                                      .
                                                                                      AppSettings["AutoUpdate"], "true");
                                                                              TsmiExitClick(null, null);
                                                                          }
                                                                      }
                                                                  }
                                                                  catch
                                                                  {
                                                                       
                                                                  }

                                                                  #endregion                                                                  
                                                                 
                                                              });

        }

        private void TsmiAboutClick(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog();
        }

        #region Initialize

        private bool _userClosing = false;
        private void Initialize()
        {
            Text = System.Configuration.ConfigurationSettings.AppSettings["App"];
            bool.TryParse(System.Configuration.ConfigurationSettings.AppSettings["UserClosing"], out _userClosing);
            //reStoreWorkSpace();
            _pluginUtils = new PluginUtils();
            _pluginManager = new PluginManager(this);
            _pluginManager.LoadDefault(PluginHelper.PluginSign1);
            Text = Text + string.Format(" Beta版本:V{0}(build{1})", ReflectionHelper.GetVersion(this.GetType().Assembly), ReflectionHelper.GetPe32Time(this.GetType().Assembly.Location).ToString("yyyyMMdd"));
            Icon = Resources.Ico;
            //((System.Reflection.AssemblyDescriptionAttribute)System.Reflection.AssemblyDescriptionAttribute.GetCustomAttribute(this.GetType().Assembly,
            //typeof(System.Reflection.AssemblyDescriptionAttribute))).Description
            notifyIcon1.Text = Text;
            notifyIcon1.Icon = Icon;

            
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

        

        public ContextMenuStrip MainContextMenu
        {
            get { return mainContextMenu; }
        }
        public void RegisterObject(string name, object obj)
        {
            _pluginUtils.RegisterObject(name, obj);
        }

        public object GetObject(string name)
        {
            object o = _pluginUtils.GetObject(name);
            return o;
        }

        public void Remove(string name)
        {
            _pluginUtils.Remove(name);
        }
        private readonly PluginBroadCast _pluginBroadCast = new PluginBroadCast();

        public void Subscribe(string name, IPlugin plugin)
        {
            _pluginBroadCast.Subscribe(name, plugin);
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

        private void TsmiExitClick(object sender, EventArgs e)
        {
            _pluginManager.Unloading();
            saveWorkSpace();

            Application.Exit();
        }
        #endregion

        #region notifyIcon
        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (_userClosing)
            {
                if (this.WindowState != FormWindowState.Minimized && e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    this.Hide();
                    this.WindowState = FormWindowState.Minimized;
                    notifyIcon1.ShowBalloonTip(3000, "程序最小化提示",
                                               "图标已经缩小到托盘，打开窗口请双击图标即可。",
                                               ToolTipIcon.Info);
                }
            }
            else
            {
                TsmiExitClick(null, null);
            }
        }
        private void MainForm_Move(object sender, EventArgs e)
        {
            //if (this == null)
            //{
            //    return;
            //}

            ////最小化到托盘的时候显示图标提示信息
            //if (this.WindowState == FormWindowState.Minimized)
            //{
            //    this.Hide();
            //    notifyIcon1.ShowBalloonTip(3000, "程序最小化提示",
            //        "图标已经缩小到托盘，打开窗口请双击图标即可。",
            //        ToolTipIcon.Info);
            //}
        }

        private void MainForm_MaximizedBoundsChanged(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            showForm();
        }
        private void showForm()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;
                this.Show();
                this.BringToFront();
                this.Activate();
                this.Focus();
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        private void NotifyIcon1Click(object sender, EventArgs e)
        {
            showForm();
        }

        #endregion

        #region 关闭所有文档
        private void tsmiCloseAllDocument_Click(object sender, EventArgs e)
        {
            IDockContent[] documents = Panel.DocumentsToArray();
            foreach (var v in documents)
            {
                v.DockHandler.Close();
            }
        }
        #endregion

        #region 工作区
        private void saveWorkSpace()
        {
            try
            {
                string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "./control/workspace.config");

                Panel.SaveAsXml(configFile);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        private void reStoreWorkSpace()
        {
            try
            {
                DeserializeDockContent ddc = new DeserializeDockContent(GetContentFromPersistString);
                string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "./control/workspace.config");
                if (File.Exists(configFile))
                {
                    Panel.LoadFromXml(configFile, ddc);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        private IDockContent GetContentFromPersistString(string str)
        {          
            string[] strs = str.Split(new char[] { ','});
            IDockContent dc = Assembly.Load(strs[1]).CreateInstance(strs[0]) as IDockContent;
            return dc;             
        }
        #endregion

    }
}
