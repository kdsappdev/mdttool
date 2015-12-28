namespace MDT.Tools.Event.Monitor.Plugin
{
    partial class Form1
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtMsg = new System.Windows.Forms.Label();
            this.settingButton = new System.Windows.Forms.Button();
            this.soundOff = new System.Windows.Forms.Button();
            this.OkBt = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.TimeBox = new System.Windows.Forms.TextBox();
            this.TmMsg = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.62315F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.37685F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel2.Controls.Add(this.txtMsg, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.settingButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.soundOff, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.OkBt, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(871, 58);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // txtMsg
            // 
            this.txtMsg.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMsg.AutoSize = true;
            this.txtMsg.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMsg.Location = new System.Drawing.Point(3, 8);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(0, 12);
            this.txtMsg.TabIndex = 5;
            // 
            // settingButton
            // 
            this.settingButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.settingButton.Location = new System.Drawing.Point(660, 3);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(75, 23);
            this.settingButton.TabIndex = 6;
            this.settingButton.Text = "配置";
            this.settingButton.UseVisualStyleBackColor = true;
            this.settingButton.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // soundOff
            // 
            this.soundOff.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.soundOff.Location = new System.Drawing.Point(746, 3);
            this.soundOff.Name = "soundOff";
            this.soundOff.Size = new System.Drawing.Size(75, 23);
            this.soundOff.TabIndex = 7;
            this.soundOff.Text = "静音";
            this.soundOff.UseVisualStyleBackColor = true;
            this.soundOff.Click += new System.EventHandler(this.soundOff_Click);
            // 
            // OkBt
            // 
            this.OkBt.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.OkBt.Location = new System.Drawing.Point(746, 32);
            this.OkBt.Name = "OkBt";
            this.OkBt.Size = new System.Drawing.Size(75, 23);
            this.OkBt.TabIndex = 10;
            this.OkBt.Text = "确定";
            this.OkBt.UseVisualStyleBackColor = true;
            this.OkBt.Click += new System.EventHandler(this.OkBt_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.55272F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.44728F));
            this.tableLayoutPanel3.Controls.Add(this.TimeBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.TmMsg, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(443, 32);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(292, 23);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // TimeBox
            // 
            this.TimeBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TimeBox.Location = new System.Drawing.Point(235, 3);
            this.TimeBox.Name = "TimeBox";
            this.TimeBox.Size = new System.Drawing.Size(32, 21);
            this.TimeBox.TabIndex = 10;
            this.TimeBox.TextChanged += new System.EventHandler(this.TimeBox_TextChanged);
            this.TimeBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TimeBox_KeyDown);
            // 
            // TmMsg
            // 
            this.TmMsg.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TmMsg.AutoSize = true;
            this.TmMsg.Location = new System.Drawing.Point(56, 5);
            this.TmMsg.Name = "TmMsg";
            this.TmMsg.Size = new System.Drawing.Size(173, 12);
            this.TmMsg.TabIndex = 9;
            this.TmMsg.Text = "你希望几秒钟接受一次消息呢？";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoScrollMinSize = new System.Drawing.Size(797, 459);
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgv, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(877, 512);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // dgv
            // 
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Enabled = false;
            this.dgv.Location = new System.Drawing.Point(3, 67);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 23;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(871, 442);
            this.dgv.TabIndex = 2;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 512);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

       
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label txtMsg;
        private System.Windows.Forms.Button settingButton;
        private System.Windows.Forms.Button soundOff;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button OkBt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label TmMsg;
        private System.Windows.Forms.TextBox TimeBox;
        private System.Windows.Forms.ErrorProvider errorProvider1;

    }
}