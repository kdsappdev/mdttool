namespace MDT.Tools.Fix.Plugin.UI
{
    partial class FixExplorer
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Header");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Messages");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Components");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Fields");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Trailer");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Fix 4.4", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FixExplorer));
            this._tvFix = new System.Windows.Forms.TreeView();
            this.cmsSubPlugin = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // _tvFix
            // 
            this._tvFix.CheckBoxes = true;
            this._tvFix.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tvFix.Location = new System.Drawing.Point(0, 0);
            this._tvFix.Name = "_tvFix";
            treeNode1.Name = "tnHeader";
            treeNode1.Text = "Header";
            treeNode2.Name = "tnMessages";
            treeNode2.Text = "Messages";
            treeNode3.Name = "tnComponents";
            treeNode3.Text = "Components";
            treeNode4.Name = "tnFields";
            treeNode4.Text = "Fields";
            treeNode5.Name = "tnTrailer";
            treeNode5.Text = "Trailer";
            treeNode6.Name = "Fix";
            treeNode6.Text = "Fix 4.4";
            this._tvFix.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this._tvFix.Size = new System.Drawing.Size(203, 347);
            this._tvFix.TabIndex = 0;
            this._tvFix.MouseClick += new System.Windows.Forms.MouseEventHandler(this._tvFix_MouseClick);
            this._tvFix.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._tvFix_AfterSelect);
            // 
            // cmsSubPlugin
            // 
            this.cmsSubPlugin.Name = "cmsSubPlugin";
            this.cmsSubPlugin.Size = new System.Drawing.Size(61, 4);
            // 
            // FixExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 347);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this._tvFix);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FixExplorer";
            this.Load += new System.EventHandler(this.FixExplorer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView _tvFix;
        private System.Windows.Forms.ContextMenuStrip cmsSubPlugin;
    }
}
