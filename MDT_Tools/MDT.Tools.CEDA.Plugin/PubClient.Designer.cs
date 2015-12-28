namespace MDT.Tools.CEDA.Plugin
{
    partial class PubClient
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PubClient));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.btnSend = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.tbtnJson = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.cbVS = new System.Windows.Forms.CheckBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtTopic = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMsgBody = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRole = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.ckEncryption = new System.Windows.Forms.CheckBox();
            this.ckSSL = new System.Windows.Forms.CheckBox();
            this.txtMsg = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnect,
            this.btnSend,
            this.btnStop,
            this.btnClear,
            this.tbtnJson});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(657, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnConnect
            // 
            this.btnConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnConnect.Image")));
            this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(52, 22);
            this.btnConnect.Text = "连接";
            // 
            // btnSend
            // 
            this.btnSend.Image = ((System.Drawing.Image)(resources.GetObject("btnSend.Image")));
            this.btnSend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(52, 22);
            this.btnSend.Text = "发送";
            // 
            // btnStop
            // 
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(52, 22);
            this.btnStop.Text = "停止";
            // 
            // btnClear
            // 
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(52, 22);
            this.btnClear.Text = "清除";
            // 
            // tbtnJson
            // 
            this.tbtnJson.Image = ((System.Drawing.Image)(resources.GetObject("tbtnJson.Image")));
            this.tbtnJson.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnJson.Name = "tbtnJson";
            this.tbtnJson.Size = new System.Drawing.Size(64, 22);
            this.tbtnJson.Text = "格式化";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 166F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cbType, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbVS, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtHost, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtPort, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtTopic, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtMsgBody, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label7, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.label6, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtRole, 5, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtPwd, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtUserName, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.ckEncryption, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.ckSSL, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(657, 182);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "类型:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "地址:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "端口:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Topic:";
            // 
            // cbType
            // 
            this.cbType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "CEDA",
            "ACS"});
            this.cbType.Location = new System.Drawing.Point(73, 5);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(96, 20);
            this.cbType.TabIndex = 8;
            // 
            // cbVS
            // 
            this.cbVS.AutoSize = true;
            this.cbVS.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cbVS.Location = new System.Drawing.Point(175, 8);
            this.cbVS.Name = "cbVS";
            this.cbVS.Size = new System.Drawing.Size(74, 16);
            this.cbVS.TabIndex = 9;
            this.cbVS.Text = "是否登录";
            this.cbVS.UseVisualStyleBackColor = true;
            // 
            // txtHost
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtHost, 3);
            this.txtHost.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtHost.Location = new System.Drawing.Point(73, 30);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(257, 21);
            this.txtHost.TabIndex = 10;
            // 
            // txtPort
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtPort, 3);
            this.txtPort.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtPort.Location = new System.Drawing.Point(73, 55);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(257, 21);
            this.txtPort.TabIndex = 11;
            // 
            // txtTopic
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtTopic, 3);
            this.txtTopic.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtTopic.Location = new System.Drawing.Point(73, 80);
            this.txtTopic.Name = "txtTopic";
            this.txtTopic.Size = new System.Drawing.Size(257, 21);
            this.txtTopic.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 136);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "报文:";
            // 
            // txtMsgBody
            // 
            this.txtMsgBody.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this.txtMsgBody, 6);
            this.txtMsgBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMsgBody.Location = new System.Drawing.Point(73, 105);
            this.txtMsgBody.Name = "txtMsgBody";
            this.txtMsgBody.Size = new System.Drawing.Size(581, 74);
            this.txtMsgBody.TabIndex = 17;
            this.txtMsgBody.Text = "";
            this.txtMsgBody.TextChanged += new System.EventHandler(this.txtMsgBody_TextChanged);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(365, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "角色:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(365, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "密码:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(353, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "登录名:";
            // 
            // txtRole
            // 
            this.txtRole.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtRole.Location = new System.Drawing.Point(406, 80);
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(160, 21);
            this.txtRole.TabIndex = 15;
            // 
            // txtPwd
            // 
            this.txtPwd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtPwd.Location = new System.Drawing.Point(406, 55);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(160, 21);
            this.txtPwd.TabIndex = 14;
            // 
            // txtUserName
            // 
            this.txtUserName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtUserName.Location = new System.Drawing.Point(406, 30);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(160, 21);
            this.txtUserName.TabIndex = 13;
            // 
            // ckEncryption
            // 
            this.ckEncryption.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ckEncryption.AutoSize = true;
            this.ckEncryption.Location = new System.Drawing.Point(572, 56);
            this.ckEncryption.Name = "ckEncryption";
            this.ckEncryption.Size = new System.Drawing.Size(48, 16);
            this.ckEncryption.TabIndex = 18;
            this.ckEncryption.Text = "加密";
            this.ckEncryption.UseVisualStyleBackColor = true;
            this.ckEncryption.CheckedChanged += new System.EventHandler(this.ckEncryption_CheckedChanged);
            // 
            // ckSSL
            // 
            this.ckSSL.AutoSize = true;
            this.ckSSL.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ckSSL.Location = new System.Drawing.Point(255, 8);
            this.ckSSL.Name = "ckSSL";
            this.ckSSL.Size = new System.Drawing.Size(75, 16);
            this.ckSSL.TabIndex = 19;
            this.ckSSL.Text = "是否SSL";
            this.ckSSL.UseVisualStyleBackColor = true;
            // 
            // txtMsg
            // 
            this.txtMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMsg.Location = new System.Drawing.Point(0, 0);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(657, 203);
            this.txtMsg.TabIndex = 18;
            this.txtMsg.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtMsg);
            this.splitContainer1.Size = new System.Drawing.Size(657, 389);
            this.splitContainer1.SplitterDistance = 182;
            this.splitContainer1.TabIndex = 19;
            // 
            // PubClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 414);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "PubClient";
            this.Text = "PubClient";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.ToolStripButton btnSend;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.CheckBox cbVS;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtTopic;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtRole;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox txtMsgBody;
        private System.Windows.Forms.RichTextBox txtMsg;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton tbtnJson;
        private System.Windows.Forms.CheckBox ckEncryption;
        private System.Windows.Forms.CheckBox ckSSL;
    }
}