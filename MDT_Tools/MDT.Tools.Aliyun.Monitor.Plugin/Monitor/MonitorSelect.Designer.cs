namespace MDT.Tools.Aliyun.Monitor.Plugin.Monitor
{
    partial class MonitorSelect
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gvData = new System.Windows.Forms.DataGridView();
            this.gcSeqNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcMonitorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcBucketName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gcstatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmSee = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.lbSelect = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.btSelect = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.82146F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.55996F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.61858F));
            this.tableLayoutPanel1.Controls.Add(this.gvData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbFileName, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbSelect, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.dtDate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btSelect, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(546, 291);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gvData
            // 
            this.gvData.AllowUserToAddRows = false;
            this.gvData.AllowUserToDeleteRows = false;
            this.gvData.AllowUserToOrderColumns = true;
            this.gvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gcSeqNo,
            this.gcFileName,
            this.gcSize,
            this.gcTime,
            this.gcMonitorName,
            this.gcBucketName,
            this.gcstatus});
            this.tableLayoutPanel1.SetColumnSpan(this.gvData, 7);
            this.gvData.ContextMenuStrip = this.contextMenuStrip1;
            this.gvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvData.Location = new System.Drawing.Point(3, 40);
            this.gvData.Name = "gvData";
            this.gvData.ReadOnly = true;
            this.gvData.RowTemplate.Height = 23;
            this.gvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvData.Size = new System.Drawing.Size(540, 248);
            this.gvData.TabIndex = 6;
            // 
            // gcSeqNo
            // 
            this.gcSeqNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gcSeqNo.DataPropertyName = "SeqNo";
            this.gcSeqNo.HeaderText = "序号";
            this.gcSeqNo.Name = "gcSeqNo";
            this.gcSeqNo.ReadOnly = true;
            this.gcSeqNo.Visible = false;
            // 
            // gcFileName
            // 
            this.gcFileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gcFileName.DataPropertyName = "FileName";
            this.gcFileName.HeaderText = "名称";
            this.gcFileName.Name = "gcFileName";
            this.gcFileName.ReadOnly = true;
            this.gcFileName.Width = 54;
            // 
            // gcSize
            // 
            this.gcSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gcSize.DataPropertyName = "Size";
            this.gcSize.HeaderText = "大小";
            this.gcSize.Name = "gcSize";
            this.gcSize.ReadOnly = true;
            this.gcSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.gcSize.Width = 54;
            // 
            // gcTime
            // 
            this.gcTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gcTime.DataPropertyName = "LastModified";
            this.gcTime.HeaderText = "时间";
            this.gcTime.Name = "gcTime";
            this.gcTime.ReadOnly = true;
            this.gcTime.Width = 54;
            // 
            // gcMonitorName
            // 
            this.gcMonitorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gcMonitorName.DataPropertyName = "MonitorName";
            this.gcMonitorName.HeaderText = "监控文件";
            this.gcMonitorName.Name = "gcMonitorName";
            this.gcMonitorName.ReadOnly = true;
            this.gcMonitorName.Visible = false;
            // 
            // gcBucketName
            // 
            this.gcBucketName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gcBucketName.DataPropertyName = "BucketName";
            this.gcBucketName.HeaderText = "Bucke名称";
            this.gcBucketName.Name = "gcBucketName";
            this.gcBucketName.ReadOnly = true;
            this.gcBucketName.Visible = false;
            // 
            // gcstatus
            // 
            this.gcstatus.DataPropertyName = "Status";
            this.gcstatus.HeaderText = "状态";
            this.gcstatus.Name = "gcstatus";
            this.gcstatus.ReadOnly = true;
            this.gcstatus.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmSee});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // tsmSee
            // 
            this.tsmSee.Name = "tsmSee";
            this.tsmSee.Size = new System.Drawing.Size(100, 22);
            this.tsmSee.Text = "查看";
            this.tsmSee.Click += new System.EventHandler(this.tsmSee_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "日期：";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(136, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "文件名称：";
            // 
            // tbFileName
            // 
            this.tbFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFileName.Location = new System.Drawing.Point(207, 6);
            this.tbFileName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(59, 21);
            this.tbFileName.TabIndex = 3;
            this.tbFileName.TextChanged += new System.EventHandler(this.tbFileName_TextChanged);
            // 
            // lbSelect
            // 
            this.lbSelect.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbSelect.AutoSize = true;
            this.lbSelect.Location = new System.Drawing.Point(381, 12);
            this.lbSelect.Name = "lbSelect";
            this.lbSelect.Size = new System.Drawing.Size(65, 12);
            this.lbSelect.TabIndex = 4;
            this.lbSelect.Text = "查询计数：";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(452, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "label4";
            // 
            // dtDate
            // 
            this.dtDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtDate.Location = new System.Drawing.Point(63, 6);
            this.dtDate.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(38, 21);
            this.dtDate.TabIndex = 7;
            this.dtDate.Value = new System.DateTime(2015, 8, 20, 16, 56, 15, 0);
            // 
            // btSelect
            // 
            this.btSelect.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btSelect.Location = new System.Drawing.Point(272, 7);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(59, 23);
            this.btSelect.TabIndex = 8;
            this.btSelect.Text = "查 询";
            this.btSelect.UseVisualStyleBackColor = true;
            this.btSelect.Click += new System.EventHandler(this.button1_Click);
            // 
            // MonitorSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MonitorSelect";
            this.Size = new System.Drawing.Size(546, 291);
            this.Load += new System.EventHandler(this.MonitorSelect_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Label lbSelect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView gvData;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcSeqNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcMonitorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcBucketName;
        private System.Windows.Forms.DataGridViewTextBoxColumn gcstatus;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.Button btSelect;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmSee;
    }
}
