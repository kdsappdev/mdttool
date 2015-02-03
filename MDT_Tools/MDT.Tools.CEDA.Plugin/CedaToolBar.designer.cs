namespace MDT.Tools.CEDA.Plugin
{
    partial class CedaToolBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CedaToolBar));
            this.tbtnSubscribe = new System.Windows.Forms.ToolStripButton();
            this.tbtnUnsubscribe = new System.Windows.Forms.ToolStripButton();
            this.tbtnClear = new System.Windows.Forms.ToolStripButton();
            this.tbtnJson = new System.Windows.Forms.ToolStripButton();
            this.tsToolBar = new System.Windows.Forms.ToolStrip();
            this.tsToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbtnSubscribe
            // 
            this.tbtnSubscribe.Image = ((System.Drawing.Image)(resources.GetObject("tbtnSubscribe.Image")));
            this.tbtnSubscribe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnSubscribe.Name = "tbtnSubscribe";
            this.tbtnSubscribe.Size = new System.Drawing.Size(49, 22);
            this.tbtnSubscribe.Text = "订阅";
            // 
            // tbtnUnsubscribe
            // 
            this.tbtnUnsubscribe.Image = ((System.Drawing.Image)(resources.GetObject("tbtnUnsubscribe.Image")));
            this.tbtnUnsubscribe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnUnsubscribe.Name = "tbtnUnsubscribe";
            this.tbtnUnsubscribe.Size = new System.Drawing.Size(49, 22);
            this.tbtnUnsubscribe.Text = "停止";
            // 
            // tbtnClear
            // 
            this.tbtnClear.Image = ((System.Drawing.Image)(resources.GetObject("tbtnClear.Image")));
            this.tbtnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnClear.Name = "tbtnClear";
            this.tbtnClear.Size = new System.Drawing.Size(49, 22);
            this.tbtnClear.Text = "清除";
            // 
            // tbtnJson
            // 
            this.tbtnJson.Image = ((System.Drawing.Image)(resources.GetObject("tbtnJson.Image")));
            this.tbtnJson.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnJson.Name = "tbtnJson";
            this.tbtnJson.Size = new System.Drawing.Size(61, 22);
            this.tbtnJson.Text = "格式化";
            // 
            // tsToolBar
            // 
            this.tsToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbtnSubscribe,
            this.tbtnUnsubscribe,
            this.tbtnClear,
            this.tbtnJson});
            this.tsToolBar.Location = new System.Drawing.Point(0, 0);
            this.tsToolBar.Name = "tsToolBar";
            this.tsToolBar.Size = new System.Drawing.Size(565, 25);
            this.tsToolBar.TabIndex = 0;
            // 
            // CedaToolBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tsToolBar);
            this.Name = "CedaToolBar";
            this.Size = new System.Drawing.Size(565, 29);
            this.tsToolBar.ResumeLayout(false);
            this.tsToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton tbtnSubscribe;
        private System.Windows.Forms.ToolStripButton tbtnUnsubscribe;
        private System.Windows.Forms.ToolStripButton tbtnClear;
        private System.Windows.Forms.ToolStripButton tbtnJson;
        private System.Windows.Forms.ToolStrip tsToolBar;

    }
}
