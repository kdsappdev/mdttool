namespace MDT.Tools.DB.Plugin.UI
{
    partial class DBConfigForm
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
            this.lbcDBServerName = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.teServerName = new DevExpress.XtraEditors.TextEdit();
            this.teUserName = new DevExpress.XtraEditors.TextEdit();
            this.teUserPwd = new DevExpress.XtraEditors.TextEdit();
            this.btnTestConnection = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.cbeConfigType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lbcConfigName = new DevExpress.XtraEditors.LabelControl();
            this.teConfigName = new DevExpress.XtraEditors.TextEdit();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teServerName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teUserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teUserPwd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeConfigType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teConfigName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lbcDBServerName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelControl2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelControl3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelControl4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.teServerName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.teUserName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.teUserPwd, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnTestConnection, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnOk, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.cbeConfigType, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbcConfigName, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.teConfigName, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(423, 174);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbcDBServerName
            // 
            this.lbcDBServerName.Appearance.Options.UseTextOptions = true;
            this.lbcDBServerName.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lbcDBServerName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbcDBServerName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbcDBServerName.Location = new System.Drawing.Point(3, 3);
            this.lbcDBServerName.Name = "lbcDBServerName";
            this.lbcDBServerName.Size = new System.Drawing.Size(82, 22);
            this.lbcDBServerName.TabIndex = 0;
            this.lbcDBServerName.Text = "服务器名:";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Options.UseTextOptions = true;
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl2.Location = new System.Drawing.Point(3, 31);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(82, 22);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "用户名称:";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Options.UseTextOptions = true;
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl3.Location = new System.Drawing.Point(3, 59);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(82, 22);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "用户密码:";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Options.UseTextOptions = true;
            this.labelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl4.Location = new System.Drawing.Point(3, 115);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(82, 22);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "数据库类型:";
            // 
            // teServerName
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.teServerName, 3);
            this.teServerName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.teServerName.Location = new System.Drawing.Point(91, 3);
            this.teServerName.Name = "teServerName";
            this.teServerName.Size = new System.Drawing.Size(309, 21);
            this.teServerName.TabIndex = 4;
            // 
            // teUserName
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.teUserName, 3);
            this.teUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.teUserName.Location = new System.Drawing.Point(91, 31);
            this.teUserName.Name = "teUserName";
            this.teUserName.Size = new System.Drawing.Size(309, 21);
            this.teUserName.TabIndex = 5;
            // 
            // teUserPwd
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.teUserPwd, 3);
            this.teUserPwd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.teUserPwd.Location = new System.Drawing.Point(91, 59);
            this.teUserPwd.Name = "teUserPwd";
            this.teUserPwd.Size = new System.Drawing.Size(309, 21);
            this.teUserPwd.TabIndex = 6;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestConnection.Location = new System.Drawing.Point(23, 143);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(62, 23);
            this.btnTestConnection.TabIndex = 9;
            this.btnTestConnection.Text = "测试连接";
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(256, 143);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(63, 23);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(325, 143);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbeConfigType
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cbeConfigType, 3);
            this.cbeConfigType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbeConfigType.EditValue = "Oracle";
            this.cbeConfigType.Location = new System.Drawing.Point(91, 115);
            this.cbeConfigType.Name = "cbeConfigType";
            this.cbeConfigType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeConfigType.Properties.Items.AddRange(new object[] {
            "Oracle",
            "Sql Server"});
            this.cbeConfigType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbeConfigType.Size = new System.Drawing.Size(309, 21);
            this.cbeConfigType.TabIndex = 8;
            // 
            // lbcConfigName
            // 
            this.lbcConfigName.Appearance.Options.UseTextOptions = true;
            this.lbcConfigName.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lbcConfigName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbcConfigName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbcConfigName.Location = new System.Drawing.Point(3, 87);
            this.lbcConfigName.Name = "lbcConfigName";
            this.lbcConfigName.Size = new System.Drawing.Size(82, 22);
            this.lbcConfigName.TabIndex = 12;
            this.lbcConfigName.Text = "配置项名称:";
            // 
            // teConfigName
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.teConfigName, 3);
            this.teConfigName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.teConfigName.Location = new System.Drawing.Point(91, 87);
            this.teConfigName.Name = "teConfigName";
            this.teConfigName.Size = new System.Drawing.Size(309, 21);
            this.teConfigName.TabIndex = 7;
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // DBConfigForm
            // 
            this.AcceptButton = this.btnTestConnection;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(423, 174);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DBConfigForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库配置信息";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.teServerName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teUserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teUserPwd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbeConfigType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teConfigName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.LabelControl lbcDBServerName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit teServerName;
        private DevExpress.XtraEditors.TextEdit teUserName;
        private DevExpress.XtraEditors.TextEdit teUserPwd;
        private DevExpress.XtraEditors.SimpleButton btnTestConnection;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.ComboBoxEdit cbeConfigType;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
        private DevExpress.XtraEditors.LabelControl lbcConfigName;
        private DevExpress.XtraEditors.TextEdit teConfigName;
    }
}