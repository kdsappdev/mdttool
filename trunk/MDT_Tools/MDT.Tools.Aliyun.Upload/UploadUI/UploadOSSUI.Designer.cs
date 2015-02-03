namespace MDT.Tools.Aliyun.Upload.Plugin.UploadUI
{
    partial class UploadOSSUI
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
            this.tlpPanel = new System.Windows.Forms.TableLayoutPanel();
            this.teaccessId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.teaccesskey = new System.Windows.Forms.TextBox();
            this.tefileName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbBucket = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tefile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnfile = new System.Windows.Forms.Button();
            this.rbUpFile = new System.Windows.Forms.RadioButton();
            this.rbUpfolder = new System.Windows.Forms.RadioButton();
            this.btnClear = new System.Windows.Forms.Button();
            this.tsTool = new System.Windows.Forms.ToolStrip();
            this.tsbConnect = new System.Windows.Forms.ToolStripButton();
            this.tsbupload = new System.Windows.Forms.ToolStripButton();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.teDetail = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tlpPanel.SuspendLayout();
            this.tsTool.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpPanel
            // 
            this.tlpPanel.ColumnCount = 7;
            this.tlpPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.tlpPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tlpPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tlpPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tlpPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPanel.Controls.Add(this.teaccessId, 1, 1);
            this.tlpPanel.Controls.Add(this.label2, 3, 1);
            this.tlpPanel.Controls.Add(this.teaccesskey, 4, 1);
            this.tlpPanel.Controls.Add(this.tefileName, 4, 3);
            this.tlpPanel.Controls.Add(this.label5, 3, 3);
            this.tlpPanel.Controls.Add(this.cbBucket, 1, 2);
            this.tlpPanel.Controls.Add(this.label4, 0, 2);
            this.tlpPanel.Controls.Add(this.label3, 0, 3);
            this.tlpPanel.Controls.Add(this.tefile, 1, 3);
            this.tlpPanel.Controls.Add(this.label1, 0, 1);
            this.tlpPanel.Controls.Add(this.btnfile, 2, 3);
            this.tlpPanel.Controls.Add(this.rbUpFile, 4, 2);
            this.tlpPanel.Controls.Add(this.rbUpfolder, 5, 2);
            this.tlpPanel.Controls.Add(this.btnClear, 3, 2);
            this.tlpPanel.Controls.Add(this.tsTool, 0, 0);
            this.tlpPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPanel.Location = new System.Drawing.Point(0, 0);
            this.tlpPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPanel.Name = "tlpPanel";
            this.tlpPanel.RowCount = 5;
            this.tlpPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPanel.Size = new System.Drawing.Size(656, 129);
            this.tlpPanel.TabIndex = 0;
            // 
            // teaccessId
            // 
            this.tlpPanel.SetColumnSpan(this.teaccessId, 2);
            this.teaccessId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.teaccessId.Location = new System.Drawing.Point(108, 35);
            this.teaccessId.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.teaccessId.Name = "teaccessId";
            this.teaccessId.Size = new System.Drawing.Size(189, 21);
            this.teaccessId.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(336, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Access key：";
            // 
            // teaccesskey
            // 
            this.tlpPanel.SetColumnSpan(this.teaccesskey, 2);
            this.teaccesskey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.teaccesskey.Location = new System.Drawing.Point(419, 35);
            this.teaccesskey.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.teaccesskey.Name = "teaccesskey";
            this.teaccesskey.Size = new System.Drawing.Size(198, 21);
            this.teaccesskey.TabIndex = 4;
            // 
            // tefileName
            // 
            this.tlpPanel.SetColumnSpan(this.tefileName, 2);
            this.tefileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tefileName.Location = new System.Drawing.Point(419, 95);
            this.tefileName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.tefileName.Name = "tefileName";
            this.tefileName.Size = new System.Drawing.Size(198, 21);
            this.tefileName.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(324, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "上传文件名称：";
            // 
            // cbBucket
            // 
            this.tlpPanel.SetColumnSpan(this.cbBucket, 2);
            this.cbBucket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBucket.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBucket.FormattingEnabled = true;
            this.cbBucket.Location = new System.Drawing.Point(108, 65);
            this.cbBucket.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cbBucket.Name = "cbBucket";
            this.cbBucket.Size = new System.Drawing.Size(189, 20);
            this.cbBucket.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Bucket：";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "文件路径：";
            // 
            // tefile
            // 
            this.tefile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tefile.Location = new System.Drawing.Point(108, 95);
            this.tefile.Margin = new System.Windows.Forms.Padding(3, 5, 0, 0);
            this.tefile.Name = "tefile";
            this.tefile.ReadOnly = true;
            this.tefile.Size = new System.Drawing.Size(147, 21);
            this.tefile.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Access key Id：";
            // 
            // btnfile
            // 
            this.btnfile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnfile.FlatAppearance.BorderSize = 0;
            this.btnfile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.btnfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnfile.Location = new System.Drawing.Point(255, 93);
            this.btnfile.Margin = new System.Windows.Forms.Padding(0);
            this.btnfile.Name = "btnfile";
            this.btnfile.Size = new System.Drawing.Size(45, 23);
            this.btnfile.TabIndex = 8;
            this.btnfile.Text = "浏览";
            this.btnfile.UseVisualStyleBackColor = true;
            this.btnfile.MouseLeave += new System.EventHandler(this.btnfile_MouseLeave);
            this.btnfile.Click += new System.EventHandler(this.button2_Click);
            this.btnfile.MouseEnter += new System.EventHandler(this.btnfile_MouseEnter);
            // 
            // rbUpFile
            // 
            this.rbUpFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rbUpFile.AutoSize = true;
            this.rbUpFile.Checked = true;
            this.rbUpFile.Location = new System.Drawing.Point(419, 67);
            this.rbUpFile.Name = "rbUpFile";
            this.rbUpFile.Size = new System.Drawing.Size(71, 16);
            this.rbUpFile.TabIndex = 17;
            this.rbUpFile.TabStop = true;
            this.rbUpFile.Text = "文件上传";
            this.rbUpFile.UseVisualStyleBackColor = true;
            this.rbUpFile.CheckedChanged += new System.EventHandler(this.rbUpFile_CheckedChanged);
            // 
            // rbUpfolder
            // 
            this.rbUpfolder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rbUpfolder.AutoSize = true;
            this.rbUpfolder.Location = new System.Drawing.Point(506, 67);
            this.rbUpfolder.Name = "rbUpfolder";
            this.rbUpfolder.Size = new System.Drawing.Size(83, 16);
            this.rbUpfolder.TabIndex = 18;
            this.rbUpfolder.TabStop = true;
            this.rbUpfolder.Text = "文件夹上传";
            this.rbUpfolder.UseVisualStyleBackColor = true;
            this.rbUpfolder.CheckedChanged += new System.EventHandler(this.rbUpfolder_CheckedChanged);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(323, 63);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(69, 24);
            this.btnClear.TabIndex = 19;
            this.btnClear.Text = "清 除";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.MouseLeave += new System.EventHandler(this.btnClear_MouseLeave);
            this.btnClear.MouseEnter += new System.EventHandler(this.btnClear_MouseEnter);
            // 
            // tsTool
            // 
            this.tlpPanel.SetColumnSpan(this.tsTool, 7);
            this.tsTool.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tsTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbConnect,
            this.tsbupload,
            this.tsbClear});
            this.tsTool.Location = new System.Drawing.Point(0, 5);
            this.tsTool.Name = "tsTool";
            this.tsTool.Size = new System.Drawing.Size(656, 25);
            this.tsTool.TabIndex = 20;
            // 
            // tsbConnect
            // 
            this.tsbConnect.Image = global::MDT.Tools.Aliyun.Upload.Plugin.Properties.Resources._02934;
            this.tsbConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConnect.Name = "tsbConnect";
            this.tsbConnect.Size = new System.Drawing.Size(56, 22);
            this.tsbConnect.Text = "连 接";
            this.tsbConnect.Click += new System.EventHandler(this.tsbConnect_Click);
            // 
            // tsbupload
            // 
            this.tsbupload.Image = global::MDT.Tools.Aliyun.Upload.Plugin.Properties.Resources._02787;
            this.tsbupload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbupload.Name = "tsbupload";
            this.tsbupload.Size = new System.Drawing.Size(56, 22);
            this.tsbupload.Text = "上 传";
            this.tsbupload.Click += new System.EventHandler(this.tsbupload_Click);
            // 
            // tsbClear
            // 
            this.tsbClear.Image = global::MDT.Tools.Aliyun.Upload.Plugin.Properties.Resources._00108;
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(56, 22);
            this.tsbClear.Text = "清 除";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // teDetail
            // 
            this.teDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.teDetail.Location = new System.Drawing.Point(0, 0);
            this.teDetail.Multiline = true;
            this.teDetail.Name = "teDetail";
            this.teDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.teDetail.Size = new System.Drawing.Size(656, 223);
            this.teDetail.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tlpPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.teDetail);
            this.splitContainer1.Size = new System.Drawing.Size(656, 356);
            this.splitContainer1.SplitterDistance = 129;
            this.splitContainer1.TabIndex = 2;
            // 
            // UploadOSSUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 356);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UploadOSSUI";
            this.Load += new System.EventHandler(this.UploadOSSUI_Load);
            this.tlpPanel.ResumeLayout(false);
            this.tlpPanel.PerformLayout();
            this.tsTool.ResumeLayout(false);
            this.tsTool.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox teaccessId;
        private System.Windows.Forms.TextBox teaccesskey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tefile;
        private System.Windows.Forms.ComboBox cbBucket;
        private System.Windows.Forms.TextBox teDetail;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tefileName;
        private System.Windows.Forms.Button btnfile;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RadioButton rbUpFile;
        private System.Windows.Forms.RadioButton rbUpfolder;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ToolStrip tsTool;
        private System.Windows.Forms.ToolStripButton tsbConnect;
        private System.Windows.Forms.ToolStripButton tsbupload;
        private System.Windows.Forms.ToolStripButton tsbClear;
    }
}
