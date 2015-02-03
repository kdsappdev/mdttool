namespace MDT.Tools.MetaDesinger.Plugin.UI
{
    partial class TableDesingerUI
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.clbField = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMinMax = new System.Windows.Forms.Button();
            this.lcTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel1.Controls.Add(this.clbField, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(207, 176);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // clbField
            // 
            this.clbField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbField.FormattingEnabled = true;
            this.clbField.Location = new System.Drawing.Point(7, 39);
            this.clbField.Margin = new System.Windows.Forms.Padding(0);
            this.clbField.Name = "clbField";
            this.clbField.Size = new System.Drawing.Size(193, 128);
            this.clbField.TabIndex = 0;
            this.clbField.SizeChanged += new System.EventHandler(this.clbField_SizeChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(7, 7);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(193, 32);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.Controls.Add(this.btnMinMax, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lcTitle, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(193, 32);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnMinMax
            // 
            this.btnMinMax.BackColor = System.Drawing.SystemColors.Control;
            this.btnMinMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMinMax.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMinMax.Location = new System.Drawing.Point(163, 3);
            this.btnMinMax.Name = "btnMinMax";
            this.btnMinMax.Size = new System.Drawing.Size(27, 26);
            this.btnMinMax.TabIndex = 0;
            this.btnMinMax.Text = "▁";
            this.btnMinMax.UseVisualStyleBackColor = false;
            this.btnMinMax.Click += new System.EventHandler(this.button1_Click);
            // 
            // lcTitle
            // 
            this.lcTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcTitle.Location = new System.Drawing.Point(3, 0);
            this.lcTitle.Name = "lcTitle";
            this.lcTitle.Size = new System.Drawing.Size(154, 32);
            this.lcTitle.TabIndex = 1;
            this.lcTitle.Text = "label1";
            this.lcTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lcTitle.MouseLeave += new System.EventHandler(this.lcTitle_MouseLeave);
            this.lcTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lcTitle_MouseMove);
            this.lcTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lcTitle_MouseDown);
            this.lcTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lcTitle_MouseUp);
            // 
            // TableDesingerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TableDesingerUI";
            this.Size = new System.Drawing.Size(207, 176);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TableDesingerUI_MouseMove);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckedListBox clbField;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnMinMax;
        private System.Windows.Forms.Label lcTitle;
    }
}
