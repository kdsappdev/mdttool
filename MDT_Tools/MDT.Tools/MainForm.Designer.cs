namespace MDT.Tools
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.mainTool = new System.Windows.Forms.ToolStrip();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.mainContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifiyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBack = new System.Windows.Forms.ToolStripMenuItem();
            this.DockPanelWeifenLuo = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.notifiyIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(971, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
           
            // 
            // mainTool
            // 
            this.mainTool.Location = new System.Drawing.Point(0, 24);
            this.mainTool.Name = "mainTool";
            this.mainTool.Size = new System.Drawing.Size(971, 25);
            this.mainTool.TabIndex = 2;
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 637);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(971, 22);
            this.statusBar.TabIndex = 3;
            this.statusBar.Text = "statusStrip1";
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            this.imageList.Images.SetKeyName(6, "");
            this.imageList.Images.SetKeyName(7, "");
            this.imageList.Images.SetKeyName(8, "");
            // 
            // mainContextMenu
            // 
            this.mainContextMenu.Name = "mainContextMenu";
            this.mainContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.notifiyIconContextMenu;
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // notifiyIconContextMenu
            // 
            this.notifiyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExit,
            this.tsmiBack});
            this.notifiyIconContextMenu.Name = "notifiyIconContextMenu";
            this.notifiyIconContextMenu.Size = new System.Drawing.Size(117, 48);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(116, 22);
            this.tsmiExit.Text = "退出(&E)";
            this.tsmiExit.Click += new System.EventHandler(this.TsmiExitClick);
            // 
            // tsmiBack
            // 
            this.tsmiBack.Name = "tsmiBack";
            this.tsmiBack.Size = new System.Drawing.Size(116, 22);
            this.tsmiBack.Text = "返回(&B)";
            this.tsmiBack.Click += new System.EventHandler(this.NotifyIcon1Click);
            // 
            // DockPanelWeifenLuo
            // 
            this.DockPanelWeifenLuo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DockPanelWeifenLuo.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.DockPanelWeifenLuo.Location = new System.Drawing.Point(0, 49);
            this.DockPanelWeifenLuo.Name = "DockPanelWeifenLuo";
            this.DockPanelWeifenLuo.Size = new System.Drawing.Size(971, 588);
            this.DockPanelWeifenLuo.TabIndex = 5;
            // 
            // dockPanel1
            // 
            this.dockPanel1.DockBackColor = System.Drawing.SystemColors.Control;
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(200, 100);
            this.dockPanel1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 659);
            this.Controls.Add(this.DockPanelWeifenLuo);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.mainTool);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
           
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MDT";
            
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.MaximizedBoundsChanged += new System.EventHandler(this.MainForm_MaximizedBoundsChanged);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.notifiyIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStrip mainTool;
        private System.Windows.Forms.StatusStrip statusBar;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private WeifenLuo.WinFormsUI.Docking.DockPanel DockPanelWeifenLuo;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip mainContextMenu;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip notifiyIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiBack;
      
    }
}

