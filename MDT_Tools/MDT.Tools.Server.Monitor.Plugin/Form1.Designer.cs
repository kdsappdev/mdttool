namespace MDT.Tools.Server.Monitor.Plugin
{
    partial class Form1
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
            this.srControl1 = new MDT.Tools.Server.Monitor.Plugin.SRControl();
            this.SuspendLayout();
            // 
            // srControl1
            // 
            this.srControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.srControl1.Location = new System.Drawing.Point(0, 0);
            this.srControl1.Name = "srControl1";
            this.srControl1.Size = new System.Drawing.Size(636, 548);
            this.srControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 548);
            this.Controls.Add(this.srControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private SRControl srControl1;
    }
}

