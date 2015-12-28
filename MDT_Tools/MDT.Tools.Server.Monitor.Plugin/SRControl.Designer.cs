namespace MDT.Tools.Server.Monitor.Plugin
{
    partial class SRControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.goView1 = new Northwoods.Go.GoView();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.goView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(711, 496);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // goView1
            // 
            this.goView1.AllowInsert = false;
            this.goView1.AllowLink = false;
            this.goView1.ArrowMoveLarge = 10F;
            this.goView1.ArrowMoveSmall = 1F;
            this.goView1.BackColor = System.Drawing.Color.White;
            this.goView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goView1.DragsRealtime = true;
            this.goView1.Location = new System.Drawing.Point(3, 3);
            this.goView1.Name = "goView1";
            this.goView1.Size = new System.Drawing.Size(705, 465);
            this.goView1.TabIndex = 2;
            this.goView1.Text = "goView1";
            this.goView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.goView1_MouseClick);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 471);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(705, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SRControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SRControl";
            this.Size = new System.Drawing.Size(711, 496);
            this.Load += new System.EventHandler(this.SRControl_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Northwoods.Go.GoView goView1;
        private System.Windows.Forms.Label label1;

    }
}
