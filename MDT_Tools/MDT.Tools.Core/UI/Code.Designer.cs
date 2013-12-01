namespace MDT.Tools.Core.UI
{
    partial class Code
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Code));
            this.tbCode = new ICSharpCode.TextEditor.TextEditorControl();
            this.SuspendLayout();
            // 
            // tbCode
            // 
            this.tbCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCode.Encoding = ((System.Text.Encoding)(resources.GetObject("tbCode.Encoding")));
            this.tbCode.IndentStyle = ICSharpCode.TextEditor.Document.IndentStyle.None;
            this.tbCode.LineViewerStyle = ICSharpCode.TextEditor.Document.LineViewerStyle.FullRow;
            this.tbCode.Location = new System.Drawing.Point(0, 0);
            this.tbCode.Name = "tbCode";
            this.tbCode.ShowInvalidLines = false;
            this.tbCode.Size = new System.Drawing.Size(519, 486);
            this.tbCode.TabIndex = 4;
            // 
            // Code
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 486);
            this.Controls.Add(this.tbCode);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "Code";
            this.ResumeLayout(false);

        }

        #endregion

        public ICSharpCode.TextEditor.TextEditorControl tbCode;



    }
}
