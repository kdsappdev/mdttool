using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DockSample;
using Lextm.SharpSnmpLib;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.Plugin.WindowsPlugin;
using MDT.Tools.Core.UI;
using MDT.Tools.UI;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.Core.Utils;
using MDT.Tools.Core.Resources;
using System.IO;
using System.Reflection;
using Timer = System.Windows.Forms.Timer;

namespace MDT.Tools
{
    public partial class MainForm : Form, IForm
    {
        private VSToolStripExtender vS2012ToolStripExtender1;
        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();
        private readonly ToolStripRenderer _vs2012ToolStripRenderer = new VS2012ToolStripRenderer();
        private readonly ToolStripRenderer _vs2013ToolStripRenderer = new Vs2013ToolStripRenderer();
        private AutoHideStripSkin _autoHideStripSkin=new AutoHideStripSkin();
        private DockPaneStripSkin _dockPaneStripSkin=new DockPaneStripSkin();
        private string vsVersion = ConfigurationSettings.AppSettings["VsVersion"];
        private VSToolStripExtender.VsVersion version = VSToolStripExtender.VsVersion.Vs2005;
        public MainForm()
        {
           
            InitializeComponent();
            SetSplashScreen();
            Control.CheckForIllegalCrossThreadCalls = false;
            ThreadPool.RegisterWaitForSingleObject(Program.ewh, OnProgramStarted, null, -1, false);
            
            Panel.Skin.AutoHideStripSkin = _autoHideStripSkin;
            vS2012ToolStripExtender1 = new VSToolStripExtender(components);
            vS2012ToolStripExtender1.DefaultRenderer = _toolStripProfessionalRenderer;
            vS2012ToolStripExtender1.VS2012Renderer = _vs2012ToolStripRenderer;
            vS2012ToolStripExtender1.VS2013Renderer = _vs2013ToolStripRenderer;
            this.Panel.Theme = new VS2012LightTheme();
            switch (vsVersion)
            {
                case "2012":
                    this.Panel.Theme = new VS2012LightTheme();
                    version = VSToolStripExtender.VsVersion.Vs2012;
                    break;
                case "2013":
                    this.Panel.Theme = new VS2013BlueTheme();
                    version = VSToolStripExtender.VsVersion.Vs2013;
                    break;
                default:
                    this.Panel.Theme = new VS2005Theme();
                    version = VSToolStripExtender.VsVersion.Vs2005;
                    break;
            }
            this.EnableVSRenderer(version);
            Initialize();
            
        }

       

        private void EnableVSRenderer(VSToolStripExtender.VsVersion version)
        {
           
            vS2012ToolStripExtender1.SetStyle(this.mainMenu, version);
            vS2012ToolStripExtender1.SetStyle(this.mainTool, version);
            vS2012ToolStripExtender1.SetStyle(this.statusBar, version);
        }
        private void OnProgramStarted(object o, bool timedout)
        {
            showMaxForm();
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            
            _pluginManager.Loading();
            
        }

        private void TsmiAboutClick(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog();
        }

        #region Initialize

        private bool _userClosing = false;
        private bool _isSaveWorkSpace = false;
        
        private void Initialize()
        {
            Text = System.Configuration.ConfigurationSettings.AppSettings["App"];
            bool.TryParse(System.Configuration.ConfigurationSettings.AppSettings["UserClosing"], out _userClosing);
            bool.TryParse(System.Configuration.ConfigurationSettings.AppSettings["IsSaveWorkSpace"], out _isSaveWorkSpace);

            
            _pluginUtils = new PluginUtils();

            PluginManagers pms = new PluginManagers();
            pms.RunTimeConfigPath = System.Configuration.ConfigurationSettings.AppSettings["RunTimeConfigPath"];
            pms.PublicKey = System.Configuration.ConfigurationSettings.AppSettings["PublicKey"];
            _pluginManager = pms;
            _pluginManager.Application = this;
            _pluginManager.Init();
            Text = Text + string.Format(" 版本:V{0}(build:{1})", ReflectionHelper.GetVersion(GetType().Assembly), ReflectionHelper.GetPe32Time(GetType().Assembly).ToString("yyyyMMdd"));
            Icon = Resources.Ico;
            //((System.Reflection.AssemblyDescriptionAttribute)System.Reflection.AssemblyDescriptionAttribute.GetCustomAttribute(this.GetType().Assembly,
            //typeof(System.Reflection.AssemblyDescriptionAttribute))).Description
            notifyIcon1.Text = Text;
            notifyIcon1.Icon = Icon;
            if (_isSaveWorkSpace)
            {
                ReStoreWorkSpace();
            }


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

        public void Exit()
        {
            _pluginManager.Unloading();
            SaveWorkSpace();

            Process.GetCurrentProcess().Kill();
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
            Exit();

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
                showMaxForm();
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        private void showMaxForm()
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                this.BringToFront();
                this.Activate();
                this.Focus();
                this.Show();
            }
            catch (Exception ex)
            {
                
                LogHelper.Error(ex);
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
        private void SaveWorkSpace()
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
        private void ReStoreWorkSpace()
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
            string[] strs = str.Split(new char[] { ',' });
            IDockContent dc = Assembly.Load(strs[1]).CreateInstance(strs[0]) as IDockContent;
            return dc;
        }
        #endregion

        #region SplashScreen
      
        internal SplashScreen _splashScreen;
        private void SetSplashScreen()
        {

            Opacity = 0;
            string splashPic = ConfigurationSettings.AppSettings["SplashPic"];
            string splashHeightStr = ConfigurationSettings.AppSettings["SplashHeight"];
            string splashWidthStr = ConfigurationSettings.AppSettings["SplashWidth"];
 
            _splashScreen = new SplashScreen(Image.FromFile(splashPic, true));
            int splashHeight;
            int splashWidth;
            int.TryParse(splashHeightStr, out splashHeight);
            int.TryParse(splashWidthStr, out splashWidth);
            _splashScreen.Width = splashWidth;
            _splashScreen.Height = splashHeight;

           
           
            
           
            Timer _timer = new Timer();
            _timer.Tick += (sender, e) =>
            {
                _timer.Enabled = false;

                _splashScreen.Visible = false;
                _splashScreen.StopAnimate();
                Opacity = 100;
                showMaxForm();
            };
            
            _timer.Interval = 4000;
            _timer.Enabled = true;


            Timer _timer2 = new Timer();
            _timer2.Tick += (sender, e) =>
            {
                _splashScreen.Visible = true;
                _splashScreen.TopMost = true;
                _timer2.Enabled = false;
                
            };
            _timer2.Interval = 10;
            _timer2.Enabled = true;
            
            
        }

       
        #endregion

    }
}
