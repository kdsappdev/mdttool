namespace MDT.Tools.Event.Monitor.Plugin
{
    partial class Form2
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.levelComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.infoVoiceT = new System.Windows.Forms.ComboBox();
            this.warnVioceT = new System.Windows.Forms.ComboBox();
            this.errorVoiceT = new System.Windows.Forms.ComboBox();
            this.fatalErrorVT = new System.Windows.Forms.ComboBox();
            this.infoVoiceBt = new System.Windows.Forms.Button();
            this.warningVoiceBt = new System.Windows.Forms.Button();
            this.errorVoiceBt = new System.Windows.Forms.Button();
            this.fatalErrorVoiceBt = new System.Windows.Forms.Button();
            this.infoColorPickEdit = new System.Windows.Forms.Button();
            this.warnColorPickEdit = new System.Windows.Forms.Button();
            this.errorColorPickEdit = new System.Windows.Forms.Button();
            this.fatalErrorColorPickEdit = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.Okbt = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.setDef = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.MaximumSize = new System.Drawing.Size(697, 232);
            this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(697, 232);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.38F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(697, 232);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 447F));
            this.tableLayoutPanel2.Controls.Add(this.levelComboBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(691, 37);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // levelComboBox
            // 
            this.levelComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.levelComboBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.levelComboBox.FormattingEnabled = true;
            this.levelComboBox.Items.AddRange(new object[] {
            "信息",
            "警告",
            "错误",
            "严重的错误"});
            this.levelComboBox.Location = new System.Drawing.Point(247, 6);
            this.levelComboBox.Name = "levelComboBox";
            this.levelComboBox.Size = new System.Drawing.Size(128, 24);
            this.levelComboBox.TabIndex = 1;
            this.levelComboBox.TextChanged += new System.EventHandler(this.levelComboBoxEdit_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(121, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "监控事件级别：";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(110, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(476, 168);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "个性化";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.infoVoiceT, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.warnVioceT, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.errorVoiceT, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.fatalErrorVT, 3, 3);
            this.tableLayoutPanel3.Controls.Add(this.infoVoiceBt, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.warningVoiceBt, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.errorVoiceBt, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.fatalErrorVoiceBt, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.infoColorPickEdit, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.warnColorPickEdit, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.errorColorPickEdit, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.fatalErrorColorPickEdit, 1, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(470, 143);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(10, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 21;
            this.label5.Text = "严重的错误：";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(58, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 16);
            this.label4.TabIndex = 20;
            this.label4.Text = "错误：";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(58, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 19;
            this.label3.Text = "警告：";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(58, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "信息：";
            // 
            // infoVoiceT
            // 
            this.infoVoiceT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.infoVoiceT.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.infoVoiceT.FormattingEnabled = true;
            this.infoVoiceT.Items.AddRange(new object[] {
            "响铃一次",
            "循环播放"});
            this.infoVoiceT.Location = new System.Drawing.Point(354, 5);
            this.infoVoiceT.Name = "infoVoiceT";
            this.infoVoiceT.Size = new System.Drawing.Size(113, 24);
            this.infoVoiceT.TabIndex = 14;
            this.infoVoiceT.TextChanged += new System.EventHandler(this.VoiceT_TextChanged);
            // 
            // warnVioceT
            // 
            this.warnVioceT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.warnVioceT.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.warnVioceT.FormattingEnabled = true;
            this.warnVioceT.Items.AddRange(new object[] {
            "响铃一次",
            "循环播放"});
            this.warnVioceT.Location = new System.Drawing.Point(354, 40);
            this.warnVioceT.Name = "warnVioceT";
            this.warnVioceT.Size = new System.Drawing.Size(113, 24);
            this.warnVioceT.TabIndex = 15;
            this.warnVioceT.TextChanged += new System.EventHandler(this.VoiceT_TextChanged);
            // 
            // errorVoiceT
            // 
            this.errorVoiceT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.errorVoiceT.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.errorVoiceT.FormattingEnabled = true;
            this.errorVoiceT.Items.AddRange(new object[] {
            "响铃一次",
            "循环播放"});
            this.errorVoiceT.Location = new System.Drawing.Point(354, 75);
            this.errorVoiceT.Name = "errorVoiceT";
            this.errorVoiceT.Size = new System.Drawing.Size(113, 24);
            this.errorVoiceT.TabIndex = 16;
            this.errorVoiceT.TextChanged += new System.EventHandler(this.VoiceT_TextChanged);
            // 
            // fatalErrorVT
            // 
            this.fatalErrorVT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.fatalErrorVT.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fatalErrorVT.FormattingEnabled = true;
            this.fatalErrorVT.Items.AddRange(new object[] {
            "响铃一次",
            "循环播放"});
            this.fatalErrorVT.Location = new System.Drawing.Point(354, 112);
            this.fatalErrorVT.Name = "fatalErrorVT";
            this.fatalErrorVT.Size = new System.Drawing.Size(113, 24);
            this.fatalErrorVT.TabIndex = 17;
            this.fatalErrorVT.TextChanged += new System.EventHandler(this.VoiceT_TextChanged);
            // 
            // infoVoiceBt
            // 
            this.infoVoiceBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.infoVoiceBt.Location = new System.Drawing.Point(237, 6);
            this.infoVoiceBt.Name = "infoVoiceBt";
            this.infoVoiceBt.Size = new System.Drawing.Size(111, 23);
            this.infoVoiceBt.TabIndex = 22;
            this.infoVoiceBt.Text = "选择声音";
            this.infoVoiceBt.UseVisualStyleBackColor = true;
            this.infoVoiceBt.Click += new System.EventHandler(this.infoVoiceBt_Click);
            // 
            // warningVoiceBt
            // 
            this.warningVoiceBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.warningVoiceBt.Location = new System.Drawing.Point(237, 41);
            this.warningVoiceBt.Name = "warningVoiceBt";
            this.warningVoiceBt.Size = new System.Drawing.Size(111, 23);
            this.warningVoiceBt.TabIndex = 23;
            this.warningVoiceBt.Text = "选择声音";
            this.warningVoiceBt.UseVisualStyleBackColor = true;
            this.warningVoiceBt.Click += new System.EventHandler(this.infoVoiceBt_Click);
            // 
            // errorVoiceBt
            // 
            this.errorVoiceBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.errorVoiceBt.Location = new System.Drawing.Point(237, 76);
            this.errorVoiceBt.Name = "errorVoiceBt";
            this.errorVoiceBt.Size = new System.Drawing.Size(111, 23);
            this.errorVoiceBt.TabIndex = 24;
            this.errorVoiceBt.Text = "选择声音";
            this.errorVoiceBt.UseVisualStyleBackColor = true;
            this.errorVoiceBt.Click += new System.EventHandler(this.infoVoiceBt_Click);
            // 
            // fatalErrorVoiceBt
            // 
            this.fatalErrorVoiceBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fatalErrorVoiceBt.Location = new System.Drawing.Point(237, 112);
            this.fatalErrorVoiceBt.Name = "fatalErrorVoiceBt";
            this.fatalErrorVoiceBt.Size = new System.Drawing.Size(111, 23);
            this.fatalErrorVoiceBt.TabIndex = 25;
            this.fatalErrorVoiceBt.Text = "选择声音";
            this.fatalErrorVoiceBt.UseVisualStyleBackColor = true;
            this.fatalErrorVoiceBt.Click += new System.EventHandler(this.infoVoiceBt_Click);
            // 
            // infoColorPickEdit
            // 
            this.infoColorPickEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.infoColorPickEdit.Location = new System.Drawing.Point(120, 6);
            this.infoColorPickEdit.Name = "infoColorPickEdit";
            this.infoColorPickEdit.Size = new System.Drawing.Size(111, 23);
            this.infoColorPickEdit.TabIndex = 26;
            this.infoColorPickEdit.Text = "选择颜色";
            this.infoColorPickEdit.UseVisualStyleBackColor = true;
            this.infoColorPickEdit.Click += new System.EventHandler(this.fatalErrorColorPickEdit_TextChanged);
            // 
            // warnColorPickEdit
            // 
            this.warnColorPickEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.warnColorPickEdit.Location = new System.Drawing.Point(120, 41);
            this.warnColorPickEdit.Name = "warnColorPickEdit";
            this.warnColorPickEdit.Size = new System.Drawing.Size(111, 23);
            this.warnColorPickEdit.TabIndex = 27;
            this.warnColorPickEdit.Text = "选择颜色";
            this.warnColorPickEdit.UseVisualStyleBackColor = true;
            this.warnColorPickEdit.Click += new System.EventHandler(this.fatalErrorColorPickEdit_TextChanged);
            // 
            // errorColorPickEdit
            // 
            this.errorColorPickEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.errorColorPickEdit.Location = new System.Drawing.Point(120, 76);
            this.errorColorPickEdit.Name = "errorColorPickEdit";
            this.errorColorPickEdit.Size = new System.Drawing.Size(111, 23);
            this.errorColorPickEdit.TabIndex = 28;
            this.errorColorPickEdit.Text = "选择颜色";
            this.errorColorPickEdit.UseVisualStyleBackColor = true;
            this.errorColorPickEdit.Click += new System.EventHandler(this.fatalErrorColorPickEdit_TextChanged);
            // 
            // fatalErrorColorPickEdit
            // 
            this.fatalErrorColorPickEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fatalErrorColorPickEdit.Location = new System.Drawing.Point(120, 112);
            this.fatalErrorColorPickEdit.Name = "fatalErrorColorPickEdit";
            this.fatalErrorColorPickEdit.Size = new System.Drawing.Size(111, 23);
            this.fatalErrorColorPickEdit.TabIndex = 29;
            this.fatalErrorColorPickEdit.Text = "选择颜色";
            this.fatalErrorColorPickEdit.UseVisualStyleBackColor = true;
            this.fatalErrorColorPickEdit.Click += new System.EventHandler(this.fatalErrorColorPickEdit_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Okbt
            // 
            this.Okbt.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Okbt.Location = new System.Drawing.Point(193, 238);
            this.Okbt.Name = "Okbt";
            this.Okbt.Size = new System.Drawing.Size(84, 30);
            this.Okbt.TabIndex = 1;
            this.Okbt.Text = "确定";
            this.Okbt.UseVisualStyleBackColor = true;
            this.Okbt.Click += new System.EventHandler(this.button1_Click);
            // 
            // Cancel
            // 
            this.Cancel.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Cancel.Location = new System.Drawing.Point(406, 238);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 30);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "取消";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // setDef
            // 
            this.setDef.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.setDef.Location = new System.Drawing.Point(283, 238);
            this.setDef.Name = "setDef";
            this.setDef.Size = new System.Drawing.Size(117, 30);
            this.setDef.TabIndex = 3;
            this.setDef.Text = "设为默认";
            this.setDef.UseVisualStyleBackColor = true;
            this.setDef.Click += new System.EventHandler(this.setDef_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 280);
            this.Controls.Add(this.setDef);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Okbt);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox levelComboBox;
        private System.Windows.Forms.ComboBox infoVoiceT;
        private System.Windows.Forms.ComboBox warnVioceT;
        private System.Windows.Forms.ComboBox errorVoiceT;
        private System.Windows.Forms.ComboBox fatalErrorVT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button infoVoiceBt;
        private System.Windows.Forms.Button warningVoiceBt;
        private System.Windows.Forms.Button errorVoiceBt;
        private System.Windows.Forms.Button fatalErrorVoiceBt;
        private System.Windows.Forms.Button infoColorPickEdit;
        private System.Windows.Forms.Button warnColorPickEdit;
        private System.Windows.Forms.Button errorColorPickEdit;
        private System.Windows.Forms.Button fatalErrorColorPickEdit;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button Okbt;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button setDef;
    }
}