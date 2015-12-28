namespace Atf.Installer.SetUpCheck
{
    partial class SetupCheckForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupCheckForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnHelpDesk = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.clTip = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.dgvCheckItem = new System.Windows.Forms.DataGridView();
            this.colCheckItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsInstall = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsOk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lcMsg = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckItem)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.dgvCheckItem, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lcMsg, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(724, 551);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 213F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.btnHelpDesk, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCheck, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnClose, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.clTip, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.progressBar1, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(20, 517);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(684, 31);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // btnHelpDesk
            // 
            this.btnHelpDesk.Location = new System.Drawing.Point(613, 3);
            this.btnHelpDesk.Name = "btnHelpDesk";
            this.btnHelpDesk.Size = new System.Drawing.Size(68, 23);
            this.btnHelpDesk.TabIndex = 3;
            this.btnHelpDesk.Text = "技术支持";
            this.btnHelpDesk.UseVisualStyleBackColor = true;
            this.btnHelpDesk.Click += new System.EventHandler(this.btnHelpDesk_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(536, 3);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(71, 23);
            this.btnCheck.TabIndex = 1;
            this.btnCheck.Text = "环境检查";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(459, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(71, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "退出检查";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // clTip
            // 
            this.clTip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clTip.Location = new System.Drawing.Point(3, 0);
            this.clTip.Name = "clTip";
            this.clTip.Size = new System.Drawing.Size(237, 31);
            this.clTip.TabIndex = 3;
            this.clTip.Text = "准备就绪...";
            this.clTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar1.Location = new System.Drawing.Point(246, 6);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(207, 19);
            this.progressBar1.TabIndex = 4;
            this.progressBar1.Tag = "";
            // 
            // dgvCheckItem
            // 
            this.dgvCheckItem.AllowUserToDeleteRows = false;
            this.dgvCheckItem.AllowUserToResizeRows = false;
            this.dgvCheckItem.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvCheckItem.BackgroundColor = System.Drawing.Color.White;
            this.dgvCheckItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCheckItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCheckItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckItemName,
            this.colIsInstall,
            this.colCheckResult,
            this.colIsOk,
            this.colAResult});
            this.dgvCheckItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCheckItem.Location = new System.Drawing.Point(20, 38);
            this.dgvCheckItem.Name = "dgvCheckItem";
            this.dgvCheckItem.ReadOnly = true;
            this.dgvCheckItem.RowHeadersVisible = false;
            this.dgvCheckItem.RowTemplate.Height = 23;
            this.dgvCheckItem.Size = new System.Drawing.Size(684, 473);
            this.dgvCheckItem.TabIndex = 6;
            this.dgvCheckItem.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCheckItem_CellMouseLeave);
            this.dgvCheckItem.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCheckItem_CellMouseEnter);
            this.dgvCheckItem.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvCheckItem_CellPainting);
            this.dgvCheckItem.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCheckItem_CellContentClick);
            // 
            // colCheckItemName
            // 
            this.colCheckItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCheckItemName.DataPropertyName = "Name";
            this.colCheckItemName.HeaderText = "检查项目";
            this.colCheckItemName.Name = "colCheckItemName";
            this.colCheckItemName.ReadOnly = true;
            // 
            // colIsInstall
            // 
            this.colIsInstall.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colIsInstall.DataPropertyName = "IsInstall";
            this.colIsInstall.HeaderText = "是否安装";
            this.colIsInstall.Name = "colIsInstall";
            this.colIsInstall.ReadOnly = true;
            this.colIsInstall.Width = 78;
            // 
            // colCheckResult
            // 
            this.colCheckResult.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCheckResult.DataPropertyName = "CheckResult";
            this.colCheckResult.HeaderText = "检测结果";
            this.colCheckResult.Name = "colCheckResult";
            this.colCheckResult.ReadOnly = true;
            // 
            // colIsOk
            // 
            this.colIsOk.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colIsOk.DataPropertyName = "IsOk";
            this.colIsOk.HeaderText = "是否符合";
            this.colIsOk.Name = "colIsOk";
            this.colIsOk.ReadOnly = true;
            // 
            // colAResult
            // 
            this.colAResult.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAResult.DataPropertyName = "AResult";
            this.colAResult.HeaderText = "建议结果";
            this.colAResult.Name = "colAResult";
            this.colAResult.ReadOnly = true;
            this.colAResult.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // lcMsg
            // 
            this.lcMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcMsg.Location = new System.Drawing.Point(20, 10);
            this.lcMsg.Name = "lcMsg";
            this.lcMsg.Size = new System.Drawing.Size(684, 25);
            this.lcMsg.TabIndex = 3;
            this.lcMsg.Text = "系统环境检查结果:";
            this.lcMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // SetupCheckForm
            // 
            this.AcceptButton = this.btnCheck;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(724, 551);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetupCheckForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统环境检查";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SetupCheckForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckItem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lcMsg;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnHelpDesk;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvCheckItem;
        private System.Windows.Forms.Label clTip;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsInstall;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsOk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAResult;
    }
}