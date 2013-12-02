namespace MDT.Tools.DB.Csharp_ModelGen.Plugin.UI
{
    partial class Csharp_CodeGenConfigUI
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
            this.tbModelNameSpace = new System.Windows.Forms.TextBox();
            this.tbOutPut = new System.Windows.Forms.TextBox();
            this.tbTableFilter = new System.Windows.Forms.TextBox();
            this.btnBrower = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDALNameSpace = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.rbtnDefault = new System.Windows.Forms.RadioButton();
            this.rbtnIbatis = new System.Windows.Forms.RadioButton();
            this.tbIbatis = new System.Windows.Forms.TextBox();
            this.btnIbatisBrower = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbShowForm = new System.Windows.Forms.CheckBox();
            this.cbShowComment = new System.Windows.Forms.CheckBox();
            this.tbBLLNameSpace = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbPluginName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbIDALNameSpace = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.gbInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.tableLayoutPanel1);
            this.gbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbInfo.Location = new System.Drawing.Point(0, 0);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(648, 331);
            this.gbInfo.TabIndex = 0;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "基本信息";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbModelNameSpace, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbOutPut, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.tbTableFilter, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnBrower, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbDALNameSpace, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbBLLNameSpace, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label7, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbPluginName, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.label8, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbIDALNameSpace, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(642, 311);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Model命名空间:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(10, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "输出目录:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(10, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 85);
            this.label3.TabIndex = 2;
            this.label3.Text = "过滤表名前缀:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbModelNameSpace
            // 
            this.tbModelNameSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbModelNameSpace.Location = new System.Drawing.Point(145, 10);
            this.tbModelNameSpace.Name = "tbModelNameSpace";
            this.tbModelNameSpace.Size = new System.Drawing.Size(426, 21);
            this.tbModelNameSpace.TabIndex = 3;
            // 
            // tbOutPut
            // 
            this.tbOutPut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbOutPut.Location = new System.Drawing.Point(145, 140);
            this.tbOutPut.Name = "tbOutPut";
            this.tbOutPut.Size = new System.Drawing.Size(426, 21);
            this.tbOutPut.TabIndex = 4;
            this.tbOutPut.TextChanged += new System.EventHandler(this.tbOutPut_TextChanged);
            // 
            // tbTableFilter
            // 
            this.tbTableFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTableFilter.Location = new System.Drawing.Point(145, 167);
            this.tbTableFilter.Multiline = true;
            this.tbTableFilter.Name = "tbTableFilter";
            this.tbTableFilter.Size = new System.Drawing.Size(426, 79);
            this.tbTableFilter.TabIndex = 5;
            // 
            // btnBrower
            // 
            this.btnBrower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrower.Location = new System.Drawing.Point(577, 140);
            this.btnBrower.Name = "btnBrower";
            this.btnBrower.Size = new System.Drawing.Size(62, 21);
            this.btnBrower.TabIndex = 6;
            this.btnBrower.Text = "浏览";
            this.btnBrower.UseVisualStyleBackColor = true;
            this.btnBrower.Click += new System.EventHandler(this.btnBrower_Click);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(10, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 26);
            this.label4.TabIndex = 8;
            this.label4.Text = "DAL命名空间:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDALNameSpace
            // 
            this.tbDALNameSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDALNameSpace.Location = new System.Drawing.Point(145, 62);
            this.tbDALNameSpace.Name = "tbDALNameSpace";
            this.tbDALNameSpace.Size = new System.Drawing.Size(426, 21);
            this.tbDALNameSpace.TabIndex = 9;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtnDefault, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtnIbatis, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.tbIbatis, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnIbatisBrower, 4, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(145, 252);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(426, 30);
            this.tableLayoutPanel3.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 31);
            this.label6.TabIndex = 0;
            this.label6.Text = "类名:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbtnDefault
            // 
            this.rbtnDefault.AutoSize = true;
            this.rbtnDefault.Checked = true;
            this.rbtnDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbtnDefault.Location = new System.Drawing.Point(44, 3);
            this.rbtnDefault.Name = "rbtnDefault";
            this.rbtnDefault.Size = new System.Drawing.Size(47, 25);
            this.rbtnDefault.TabIndex = 1;
            this.rbtnDefault.TabStop = true;
            this.rbtnDefault.Text = "默认";
            this.rbtnDefault.UseVisualStyleBackColor = true;
            this.rbtnDefault.CheckedChanged += new System.EventHandler(this.rbtnDefault_CheckedChanged);
            // 
            // rbtnIbatis
            // 
            this.rbtnIbatis.AutoSize = true;
            this.rbtnIbatis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbtnIbatis.Location = new System.Drawing.Point(97, 3);
            this.rbtnIbatis.Name = "rbtnIbatis";
            this.rbtnIbatis.Size = new System.Drawing.Size(59, 25);
            this.rbtnIbatis.TabIndex = 2;
            this.rbtnIbatis.Text = "ibatis";
            this.rbtnIbatis.UseVisualStyleBackColor = true;
            this.rbtnIbatis.CheckedChanged += new System.EventHandler(this.rbtnIbatis_CheckedChanged);
            // 
            // tbIbatis
            // 
            this.tbIbatis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbIbatis.Location = new System.Drawing.Point(162, 3);
            this.tbIbatis.Name = "tbIbatis";
            this.tbIbatis.Size = new System.Drawing.Size(192, 21);
            this.tbIbatis.TabIndex = 3;
            // 
            // btnIbatisBrower
            // 
            this.btnIbatisBrower.Location = new System.Drawing.Point(360, 3);
            this.btnIbatisBrower.Name = "btnIbatisBrower";
            this.btnIbatisBrower.Size = new System.Drawing.Size(63, 23);
            this.btnIbatisBrower.TabIndex = 4;
            this.btnIbatisBrower.Text = "浏览";
            this.btnIbatisBrower.UseVisualStyleBackColor = true;
            this.btnIbatisBrower.Click += new System.EventHandler(this.btnIbatisBrower_Click);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(10, 249);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 36);
            this.label5.TabIndex = 11;
            this.label5.Text = "代码生成规则:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.cbShowForm, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbShowComment, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(145, 288);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(426, 39);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // cbShowForm
            // 
            this.cbShowForm.AutoSize = true;
            this.cbShowForm.Checked = true;
            this.cbShowForm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowForm.Location = new System.Drawing.Point(3, 3);
            this.cbShowForm.Name = "cbShowForm";
            this.cbShowForm.Size = new System.Drawing.Size(132, 16);
            this.cbShowForm.TabIndex = 7;
            this.cbShowForm.Text = "是否显示生成的代码";
            this.cbShowForm.UseVisualStyleBackColor = true;
            // 
            // cbShowComment
            // 
            this.cbShowComment.AutoSize = true;
            this.cbShowComment.Location = new System.Drawing.Point(141, 3);
            this.cbShowComment.Name = "cbShowComment";
            this.cbShowComment.Size = new System.Drawing.Size(120, 16);
            this.cbShowComment.TabIndex = 8;
            this.cbShowComment.Text = "是否显示注释信息";
            this.cbShowComment.UseVisualStyleBackColor = true;
            // 
            // tbBLLNameSpace
            // 
            this.tbBLLNameSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbBLLNameSpace.Location = new System.Drawing.Point(145, 88);
            this.tbBLLNameSpace.Name = "tbBLLNameSpace";
            this.tbBLLNameSpace.Size = new System.Drawing.Size(426, 21);
            this.tbBLLNameSpace.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(10, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(129, 26);
            this.label7.TabIndex = 14;
            this.label7.Text = "BLL命名空间:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbPluginName
            // 
            this.tbPluginName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPluginName.Location = new System.Drawing.Point(145, 114);
            this.tbPluginName.Name = "tbPluginName";
            this.tbPluginName.Size = new System.Drawing.Size(426, 21);
            this.tbPluginName.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(10, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(129, 26);
            this.label8.TabIndex = 16;
            this.label8.Text = "插件名字:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbIDALNameSpace
            // 
            this.tbIDALNameSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbIDALNameSpace.Location = new System.Drawing.Point(145, 36);
            this.tbIDALNameSpace.Name = "tbIDALNameSpace";
            this.tbIDALNameSpace.Size = new System.Drawing.Size(426, 21);
            this.tbIDALNameSpace.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(10, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(129, 26);
            this.label9.TabIndex = 18;
            this.label9.Text = "IDAL命名空间:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "xml|*.xml";
            // 
            // Csharp_CodeGenConfigUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.gbInfo);
            this.Name = "Csharp_CodeGenConfigUI";
            this.Size = new System.Drawing.Size(648, 331);
            this.Load += new System.EventHandler(this.Csharp_ModelGenConfigUI_Load);
            this.gbInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbModelNameSpace;
        private System.Windows.Forms.TextBox tbOutPut;
        private System.Windows.Forms.TextBox tbTableFilter;
        private System.Windows.Forms.Button btnBrower;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox cbShowForm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDALNameSpace;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rbtnDefault;
        private System.Windows.Forms.RadioButton rbtnIbatis;
        private System.Windows.Forms.TextBox tbIbatis;
        private System.Windows.Forms.Button btnIbatisBrower;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox tbBLLNameSpace;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbPluginName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbShowComment;
        private System.Windows.Forms.TextBox tbIDALNameSpace;
        private System.Windows.Forms.Label label9;
    }
}
