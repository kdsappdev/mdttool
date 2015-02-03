namespace MDT.Tools.Aliyun.Upload.Plugin.Download
{
    partial class DownloadOSSUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadOSSUI));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbConnect = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.teaccesskey = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLarge = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSmaill = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmList = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBackoff = new System.Windows.Forms.Button();
            this.teaccessId = new System.Windows.Forms.TextBox();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 244F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 244F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.teaccesskey, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnBackoff, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.teaccessId, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(828, 329);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.toolStrip1, 5);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbConnect});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(828, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
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
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Access key Id：";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(357, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Access key：";
            // 
            // teaccesskey
            // 
            this.teaccesskey.Location = new System.Drawing.Point(440, 33);
            this.teaccesskey.Name = "teaccesskey";
            this.teaccesskey.Size = new System.Drawing.Size(238, 21);
            this.teaccesskey.TabIndex = 5;
            // 
            // listView1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.listView1, 5);
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(3, 96);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(822, 230);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            this.listView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseMove);
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            this.listView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView1_ItemDrag);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmDetails,
            this.tsmLarge,
            this.tsmSmaill,
            this.tsmList});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 92);
            // 
            // tsmDetails
            // 
            this.tsmDetails.Name = "tsmDetails";
            this.tsmDetails.Size = new System.Drawing.Size(124, 22);
            this.tsmDetails.Text = "详细信息";
            this.tsmDetails.Click += new System.EventHandler(this.tsmDetails_Click);
            // 
            // tsmLarge
            // 
            this.tsmLarge.Name = "tsmLarge";
            this.tsmLarge.Size = new System.Drawing.Size(124, 22);
            this.tsmLarge.Text = "大图标";
            this.tsmLarge.Click += new System.EventHandler(this.tsmLarge_Click);
            // 
            // tsmSmaill
            // 
            this.tsmSmaill.Name = "tsmSmaill";
            this.tsmSmaill.Size = new System.Drawing.Size(124, 22);
            this.tsmSmaill.Text = "小图标";
            this.tsmSmaill.Click += new System.EventHandler(this.tsmSmaill_Click);
            // 
            // tsmList
            // 
            this.tsmList.Name = "tsmList";
            this.tsmList.Size = new System.Drawing.Size(124, 22);
            this.tsmList.Text = "列表";
            this.tsmList.Click += new System.EventHandler(this.tsmList_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            this.imageList1.Images.SetKeyName(1, "00032.ico");
            this.imageList1.Images.SetKeyName(2, "00721.ico");
            this.imageList1.Images.SetKeyName(3, "02787.ico");
            this.imageList1.Images.SetKeyName(4, "02934.ico");
            this.imageList1.Images.SetKeyName(5, "00018.ico");
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(348, 27);
            this.panel1.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 0;
            // 
            // btnBackoff
            // 
            this.btnBackoff.FlatAppearance.BorderSize = 0;
            this.btnBackoff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.btnBackoff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackoff.Image = global::MDT.Tools.Aliyun.Upload.Plugin.Properties.Resources._00128;
            this.btnBackoff.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBackoff.Location = new System.Drawing.Point(357, 63);
            this.btnBackoff.Name = "btnBackoff";
            this.btnBackoff.Size = new System.Drawing.Size(67, 23);
            this.btnBackoff.TabIndex = 11;
            this.btnBackoff.Text = "上一级";
            this.btnBackoff.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBackoff.UseVisualStyleBackColor = true;
            this.btnBackoff.MouseLeave += new System.EventHandler(this.btnBackoff_MouseLeave);
            this.btnBackoff.Click += new System.EventHandler(this.btnBackoff_Click);
            this.btnBackoff.MouseEnter += new System.EventHandler(this.btnBackoff_MouseEnter);
            // 
            // teaccessId
            // 
            this.teaccessId.Location = new System.Drawing.Point(113, 33);
            this.teaccessId.Name = "teaccessId";
            this.teaccessId.Size = new System.Drawing.Size(238, 21);
            this.teaccessId.TabIndex = 3;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "folder.png");
            this.imageList2.Images.SetKeyName(1, "123.png");
            this.imageList2.Images.SetKeyName(2, "00018.ico");
            // 
            // DownloadOSSUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 329);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "DownloadOSSUI";
            this.Load += new System.EventHandler(this.DownloadOSSUI_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox teaccessId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox teaccesskey;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBackoff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmDetails;
        private System.Windows.Forms.ToolStripMenuItem tsmLarge;
        private System.Windows.Forms.ToolStripMenuItem tsmSmaill;
        private System.Windows.Forms.ToolStripMenuItem tsmList;
    }
}
