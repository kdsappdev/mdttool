namespace MDT.Tools.DB.Csharp_ModelGen.Plugin.UI
{
    partial class Csharp_ModelGenConfigUI
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
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbNameSpace = new System.Windows.Forms.TextBox();
            this.tbOutPut = new System.Windows.Forms.TextBox();
            this.tbTableFilter = new System.Windows.Forms.TextBox();
            this.btnBrower = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.cbShowForm = new System.Windows.Forms.CheckBox();
            this.gbInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.tableLayoutPanel1);
            this.gbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbInfo.Location = new System.Drawing.Point(0, 0);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(696, 285);
            this.gbInfo.TabIndex = 0;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "基本信息";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbNameSpace, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbOutPut, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbTableFilter, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnBrower, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbShowForm, 2, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 141F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(690, 265);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "主命名空间:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(10, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "输出目录:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(10, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 141);
            this.label3.TabIndex = 2;
            this.label3.Text = "过滤表名前缀:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbNameSpace
            // 
            this.tbNameSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbNameSpace.Location = new System.Drawing.Point(145, 10);
            this.tbNameSpace.Name = "tbNameSpace";
            this.tbNameSpace.Size = new System.Drawing.Size(408, 21);
            this.tbNameSpace.TabIndex = 3;
            // 
            // tbOutPut
            // 
            this.tbOutPut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbOutPut.Location = new System.Drawing.Point(145, 36);
            this.tbOutPut.Name = "tbOutPut";
            this.tbOutPut.Size = new System.Drawing.Size(408, 21);
            this.tbOutPut.TabIndex = 4;
            this.tbOutPut.TextChanged += new System.EventHandler(this.tbOutPut_TextChanged);
            // 
            // tbTableFilter
            // 
            this.tbTableFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTableFilter.Location = new System.Drawing.Point(145, 63);
            this.tbTableFilter.Multiline = true;
            this.tbTableFilter.Name = "tbTableFilter";
            this.tbTableFilter.Size = new System.Drawing.Size(408, 135);
            this.tbTableFilter.TabIndex = 5;
            // 
            // btnBrower
            // 
            this.btnBrower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrower.Location = new System.Drawing.Point(559, 36);
            this.btnBrower.Name = "btnBrower";
            this.btnBrower.Size = new System.Drawing.Size(62, 21);
            this.btnBrower.TabIndex = 6;
            this.btnBrower.Text = "浏览";
            this.btnBrower.UseVisualStyleBackColor = true;
            this.btnBrower.Click += new System.EventHandler(this.btnBrower_Click);
            // 
            // cbShowForm
            // 
            this.cbShowForm.AutoSize = true;
            this.cbShowForm.Checked = true;
            this.cbShowForm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowForm.Location = new System.Drawing.Point(145, 204);
            this.cbShowForm.Name = "cbShowForm";
            this.cbShowForm.Size = new System.Drawing.Size(132, 16);
            this.cbShowForm.TabIndex = 7;
            this.cbShowForm.Text = "是否显示生成的代码";
            this.cbShowForm.UseVisualStyleBackColor = true;
            // 
            // Csharp_ModelGenConfigUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.gbInfo);
            this.Name = "Csharp_ModelGenConfigUI";
            this.Size = new System.Drawing.Size(696, 285);
            this.Load += new System.EventHandler(this.Csharp_ModelGenConfigUI_Load);
            this.gbInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbNameSpace;
        private System.Windows.Forms.TextBox tbOutPut;
        private System.Windows.Forms.TextBox tbTableFilter;
        private System.Windows.Forms.Button btnBrower;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox cbShowForm;
    }
}
