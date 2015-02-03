

namespace MDT.Tools.CEDA.Plugin
{
    partial class SubscribeClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubscribeClient));
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tsToolBar = new System.Windows.Forms.ToolStrip();
            this.tbtnSubscribe = new System.Windows.Forms.ToolStripButton();
            this.tbtnUnsubscribe = new System.Windows.Forms.ToolStripButton();
            this.tbtnClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.SecBtnCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.MinBtnCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMsg = new System.Windows.Forms.RichTextBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTopic = new System.Windows.Forms.TextBox();
            this.cbVS = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtClear = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1SCount = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelMTCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1MaxCount = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtnSub = new System.Windows.Forms.RadioButton();
            this.rbtnSubImage = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.txtRole = new System.Windows.Forms.TextBox();
            this.lcServerName = new System.Windows.Forms.Label();
            this.cbServiceName = new System.Windows.Forms.ComboBox();
            this.ckEncryption = new System.Windows.Forms.CheckBox();
            this.ckSSL = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel3.SuspendLayout();
            this.tsToolBar.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tsToolBar, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(736, 424);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // tsToolBar
            // 
            this.tsToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbtnSubscribe,
            this.tbtnUnsubscribe,
            this.tbtnClear,
            this.toolStripSplitButton1});
            this.tsToolBar.Location = new System.Drawing.Point(0, 0);
            this.tsToolBar.Name = "tsToolBar";
            this.tsToolBar.Size = new System.Drawing.Size(736, 25);
            this.tsToolBar.TabIndex = 3;
            // 
            // tbtnSubscribe
            // 
            this.tbtnSubscribe.Image = ((System.Drawing.Image)(resources.GetObject("tbtnSubscribe.Image")));
            this.tbtnSubscribe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnSubscribe.Name = "tbtnSubscribe";
            this.tbtnSubscribe.Size = new System.Drawing.Size(52, 22);
            this.tbtnSubscribe.Text = "订阅";
            this.tbtnSubscribe.Click += new System.EventHandler(this.tbtnSubscribe_Click);
            // 
            // tbtnUnsubscribe
            // 
            this.tbtnUnsubscribe.Image = ((System.Drawing.Image)(resources.GetObject("tbtnUnsubscribe.Image")));
            this.tbtnUnsubscribe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnUnsubscribe.Name = "tbtnUnsubscribe";
            this.tbtnUnsubscribe.Size = new System.Drawing.Size(52, 22);
            this.tbtnUnsubscribe.Text = "停止";
            this.tbtnUnsubscribe.Click += new System.EventHandler(this.tbtnUnsubscribe_Click);
            // 
            // tbtnClear
            // 
            this.tbtnClear.Image = ((System.Drawing.Image)(resources.GetObject("tbtnClear.Image")));
            this.tbtnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnClear.Name = "tbtnClear";
            this.tbtnClear.Size = new System.Drawing.Size(52, 22);
            this.tbtnClear.Text = "清除";
            this.tbtnClear.Click += new System.EventHandler(this.tbtnClear_Click);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SecBtnCheck,
            this.MinBtnCheck});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(88, 22);
            this.toolStripSplitButton1.Text = "统计查看";
            // 
            // SecBtnCheck
            // 
            this.SecBtnCheck.Name = "SecBtnCheck";
            this.SecBtnCheck.Size = new System.Drawing.Size(136, 22);
            this.SecBtnCheck.Text = "秒单位统计";
            this.SecBtnCheck.Click += new System.EventHandler(this.SecBtnCheck_Click);
            // 
            // MinBtnCheck
            // 
            this.MinBtnCheck.Name = "MinBtnCheck";
            this.MinBtnCheck.Size = new System.Drawing.Size(136, 22);
            this.MinBtnCheck.Text = "分单位统计";
            this.MinBtnCheck.Click += new System.EventHandler(this.MinBtnCheck_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel1, 3);
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 176F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtMsg, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtHost, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPort, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtTopic, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.cbVS, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbType, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtClear, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label11, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label12, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtUserName, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPwd, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.label14, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label13, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtRole, 5, 4);
            this.tableLayoutPanel1.Controls.Add(this.lcServerName, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbServiceName, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.ckEncryption, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.ckSSL, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(730, 393);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "地址:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "端口:";
            // 
            // txtMsg
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtMsg, 7);
            this.txtMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMsg.Location = new System.Drawing.Point(3, 188);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(724, 202);
            this.txtMsg.TabIndex = 4;
            this.txtMsg.Text = "";
            // 
            // txtHost
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtHost, 3);
            this.txtHost.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtHost.Location = new System.Drawing.Point(73, 28);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(270, 21);
            this.txtHost.TabIndex = 5;
            this.txtHost.Text = "192.168.20.2/mq.httpMQTunnel";
            // 
            // txtPort
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtPort, 3);
            this.txtPort.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtPort.Location = new System.Drawing.Point(73, 53);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(270, 21);
            this.txtPort.TabIndex = 6;
            this.txtPort.Text = "29990";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "主题:";
            // 
            // txtTopic
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtTopic, 3);
            this.txtTopic.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtTopic.Location = new System.Drawing.Point(73, 103);
            this.txtTopic.Name = "txtTopic";
            this.txtTopic.Size = new System.Drawing.Size(270, 21);
            this.txtTopic.TabIndex = 8;
            this.txtTopic.Text = "OHLC.*";
            // 
            // cbVS
            // 
            this.cbVS.AutoSize = true;
            this.cbVS.Checked = true;
            this.cbVS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbVS.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cbVS.Location = new System.Drawing.Point(191, 6);
            this.cbVS.Name = "cbVS";
            this.cbVS.Size = new System.Drawing.Size(74, 16);
            this.cbVS.TabIndex = 10;
            this.cbVS.Text = "是否登录";
            this.cbVS.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "类型:";
            // 
            // cbType
            // 
            this.cbType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "ACS",
            "CEDA"});
            this.cbType.Location = new System.Drawing.Point(73, 3);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(112, 20);
            this.cbType.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "显示个数:";
            // 
            // txtClear
            // 
            this.txtClear.AcceptsTab = true;
            this.tableLayoutPanel1.SetColumnSpan(this.txtClear, 3);
            this.txtClear.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtClear.Location = new System.Drawing.Point(73, 128);
            this.txtClear.Name = "txtClear";
            this.txtClear.Size = new System.Drawing.Size(270, 21);
            this.txtClear.TabIndex = 16;
            this.txtClear.Text = "0";
            this.txtClear.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 10;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 7);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.label1SCount, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label10, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelMTCount, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelCount, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label6, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelTime, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label9, 8, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1MaxCount, 9, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 153);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(724, 29);
            this.tableLayoutPanel2.TabIndex = 19;
            // 
            // label1SCount
            // 
            this.label1SCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1SCount.AutoSize = true;
            this.label1SCount.Location = new System.Drawing.Point(227, 8);
            this.label1SCount.Name = "label1SCount";
            this.label1SCount.Size = new System.Drawing.Size(0, 12);
            this.label1SCount.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(279, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 19;
            this.label10.Text = "总平均tps/s:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMTCount
            // 
            this.labelMTCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelMTCount.AutoSize = true;
            this.labelMTCount.Location = new System.Drawing.Point(362, 8);
            this.labelMTCount.Name = "labelMTCount";
            this.labelMTCount.Size = new System.Drawing.Size(0, 12);
            this.labelMTCount.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "总数:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCount
            // 
            this.labelCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(69, 8);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(0, 12);
            this.labelCount.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(415, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "总耗时/s:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTime
            // 
            this.labelTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(480, 8);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(0, 12);
            this.labelTime.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(126, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "最近1秒内tps/s:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(531, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 23;
            this.label9.Text = "峰值tps/s:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1MaxCount
            // 
            this.label1MaxCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1MaxCount.AutoSize = true;
            this.label1MaxCount.Location = new System.Drawing.Point(602, 8);
            this.label1MaxCount.Name = "label1MaxCount";
            this.label1MaxCount.Size = new System.Drawing.Size(0, 12);
            this.label1MaxCount.TabIndex = 24;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(367, 31);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 20;
            this.label11.Text = "登录名:";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(379, 56);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 12);
            this.label12.TabIndex = 21;
            this.label12.Text = "密码:";
            // 
            // txtUserName
            // 
            this.txtUserName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtUserName.Location = new System.Drawing.Point(420, 28);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(170, 21);
            this.txtUserName.TabIndex = 23;
            // 
            // txtPwd
            // 
            this.txtPwd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtPwd.Location = new System.Drawing.Point(420, 53);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(170, 21);
            this.txtPwd.TabIndex = 24;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 81);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 12);
            this.label14.TabIndex = 27;
            this.label14.Text = "订阅方式:";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel4, 3);
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.37705F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.62295F));
            this.tableLayoutPanel4.Controls.Add(this.rbtnSub, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.rbtnSubImage, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(70, 75);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(276, 25);
            this.tableLayoutPanel4.TabIndex = 30;
            // 
            // rbtnSub
            // 
            this.rbtnSub.AutoSize = true;
            this.rbtnSub.Checked = true;
            this.rbtnSub.Location = new System.Drawing.Point(3, 3);
            this.rbtnSub.Name = "rbtnSub";
            this.rbtnSub.Size = new System.Drawing.Size(71, 16);
            this.rbtnSub.TabIndex = 28;
            this.rbtnSub.TabStop = true;
            this.rbtnSub.Text = "普通订阅";
            this.toolTip1.SetToolTip(this.rbtnSub, "直接订阅");
            this.rbtnSub.UseVisualStyleBackColor = true;
            // 
            // rbtnSubImage
            // 
            this.rbtnSubImage.AutoSize = true;
            this.rbtnSubImage.Location = new System.Drawing.Point(92, 3);
            this.rbtnSubImage.Name = "rbtnSubImage";
            this.rbtnSubImage.Size = new System.Drawing.Size(149, 16);
            this.rbtnSubImage.TabIndex = 29;
            this.rbtnSubImage.Text = "新订阅模式(请求+订阅)";
            this.toolTip1.SetToolTip(this.rbtnSubImage, "先请求在订阅，需要后台服务器编写对应请求/响应接口");
            this.rbtnSubImage.UseVisualStyleBackColor = true;
            this.rbtnSubImage.CheckedChanged += new System.EventHandler(this.rbtnSubImage_CheckedChanged);
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(379, 106);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 12);
            this.label13.TabIndex = 22;
            this.label13.Text = "角色:";
            // 
            // txtRole
            // 
            this.txtRole.Location = new System.Drawing.Point(420, 103);
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(170, 21);
            this.txtRole.TabIndex = 25;
            // 
            // lcServerName
            // 
            this.lcServerName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lcServerName.AutoSize = true;
            this.lcServerName.Location = new System.Drawing.Point(367, 81);
            this.lcServerName.Name = "lcServerName";
            this.lcServerName.Size = new System.Drawing.Size(47, 12);
            this.lcServerName.TabIndex = 31;
            this.lcServerName.Text = "服务名:";
            this.lcServerName.Visible = false;
            // 
            // cbServiceName
            // 
            this.cbServiceName.FormattingEnabled = true;
            this.cbServiceName.Location = new System.Drawing.Point(420, 78);
            this.cbServiceName.Name = "cbServiceName";
            this.cbServiceName.Size = new System.Drawing.Size(170, 20);
            this.cbServiceName.TabIndex = 32;
            this.cbServiceName.Visible = false;
            // 
            // ckEncryption
            // 
            this.ckEncryption.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ckEncryption.AutoSize = true;
            this.ckEncryption.Location = new System.Drawing.Point(596, 54);
            this.ckEncryption.Name = "ckEncryption";
            this.ckEncryption.Size = new System.Drawing.Size(48, 16);
            this.ckEncryption.TabIndex = 33;
            this.ckEncryption.Text = "加密";
            this.ckEncryption.UseVisualStyleBackColor = true;
            // 
            // ckSSL
            // 
            this.ckSSL.AutoSize = true;
            this.ckSSL.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ckSSL.Location = new System.Drawing.Point(271, 6);
            this.ckSSL.Name = "ckSSL";
            this.ckSSL.Size = new System.Drawing.Size(72, 16);
            this.ckSSL.TabIndex = 34;
            this.ckSSL.Text = "是否SSL";
            this.ckSSL.UseVisualStyleBackColor = true;
            // 
            // SubscribeClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 424);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "SubscribeClient";
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tsToolBar.ResumeLayout(false);
            this.tsToolBar.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ToolStrip tsToolBar;
        private System.Windows.Forms.ToolStripButton tbtnSubscribe;
        private System.Windows.Forms.ToolStripButton tbtnUnsubscribe;
        private System.Windows.Forms.ToolStripButton tbtnClear;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem SecBtnCheck;
        private System.Windows.Forms.ToolStripMenuItem MinBtnCheck;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtMsg;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTopic;
        private System.Windows.Forms.CheckBox cbVS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtClear;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1SCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelMTCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1MaxCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtRole;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton rbtnSub;
        private System.Windows.Forms.RadioButton rbtnSubImage;
        private System.Windows.Forms.Label lcServerName;
        private System.Windows.Forms.ComboBox cbServiceName;
        private System.Windows.Forms.CheckBox ckEncryption;
        private System.Windows.Forms.CheckBox ckSSL;
    }
}
