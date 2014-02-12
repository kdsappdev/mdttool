namespace MDT.Tools.DB.SetComment.Plugin.UI
{
    partial class tableInfoForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tableInfoForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvTableInfo = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataPrecision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataScale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataNullAble = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataDefault = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsPrimaryKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsForeginKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnExecute = new System.Windows.Forms.Button();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbExecuteDB = new System.Windows.Forms.CheckBox();
            this.tbScript = new ICSharpCode.TextEditor.TextEditorControl();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableInfo)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbScript);
            this.splitContainer1.Size = new System.Drawing.Size(532, 432);
            this.splitContainer1.SplitterDistance = 177;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgvTableInfo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(532, 177);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // dgvTableInfo
            // 
            this.dgvTableInfo.AllowUserToAddRows = false;
            this.dgvTableInfo.AllowUserToDeleteRows = false;
            this.dgvTableInfo.AllowUserToOrderColumns = true;
            this.dgvTableInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTableInfo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTableInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTableInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colDataType,
            this.colDataLength,
            this.colDataPrecision,
            this.colDataScale,
            this.colDataNullAble,
            this.colDataDefault,
            this.colIsPrimaryKey,
            this.colIsForeginKey,
            this.colComment});
            this.dgvTableInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTableInfo.Location = new System.Drawing.Point(3, 38);
            this.dgvTableInfo.Name = "dgvTableInfo";
            this.dgvTableInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvTableInfo.RowTemplate.Height = 23;
            this.dgvTableInfo.Size = new System.Drawing.Size(526, 136);
            this.dgvTableInfo.TabIndex = 0;
            this.dgvTableInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTableInfo_CellValueChanged);
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colName.HeaderText = "列名";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 54;
            // 
            // colDataType
            // 
            this.colDataType.DataPropertyName = "DataType";
            this.colDataType.HeaderText = "数据类型";
            this.colDataType.Name = "colDataType";
            this.colDataType.ReadOnly = true;
            this.colDataType.Width = 78;
            // 
            // colDataLength
            // 
            this.colDataLength.DataPropertyName = "DataLength";
            this.colDataLength.HeaderText = "长度";
            this.colDataLength.Name = "colDataLength";
            this.colDataLength.ReadOnly = true;
            this.colDataLength.Width = 54;
            // 
            // colDataPrecision
            // 
            this.colDataPrecision.DataPropertyName = "DataPrecision";
            this.colDataPrecision.HeaderText = "精度";
            this.colDataPrecision.Name = "colDataPrecision";
            this.colDataPrecision.ReadOnly = true;
            this.colDataPrecision.Width = 54;
            // 
            // colDataScale
            // 
            this.colDataScale.DataPropertyName = "DataScale";
            this.colDataScale.HeaderText = "小数位数";
            this.colDataScale.Name = "colDataScale";
            this.colDataScale.ReadOnly = true;
            this.colDataScale.Width = 78;
            // 
            // colDataNullAble
            // 
            this.colDataNullAble.DataPropertyName = "DataNullAble";
            this.colDataNullAble.HeaderText = "是否Null";
            this.colDataNullAble.Name = "colDataNullAble";
            this.colDataNullAble.ReadOnly = true;
            this.colDataNullAble.Width = 78;
            // 
            // colDataDefault
            // 
            this.colDataDefault.DataPropertyName = "DataDefault";
            this.colDataDefault.HeaderText = "默认值";
            this.colDataDefault.Name = "colDataDefault";
            this.colDataDefault.ReadOnly = true;
            this.colDataDefault.Width = 66;
            // 
            // colIsPrimaryKey
            // 
            this.colIsPrimaryKey.DataPropertyName = "IsPrimaryKeys";
            this.colIsPrimaryKey.HeaderText = "主键";
            this.colIsPrimaryKey.Name = "colIsPrimaryKey";
            this.colIsPrimaryKey.ReadOnly = true;
            this.colIsPrimaryKey.Width = 54;
            // 
            // colIsForeginKey
            // 
            this.colIsForeginKey.DataPropertyName = "isForeignkey";
            this.colIsForeginKey.HeaderText = "外键";
            this.colIsForeginKey.Name = "colIsForeginKey";
            this.colIsForeginKey.ReadOnly = true;
            this.colIsForeginKey.Width = 54;
            // 
            // colComment
            // 
            this.colComment.DataPropertyName = "Comments";
            this.colComment.HeaderText = "备注";
            this.colComment.Name = "colComment";
            this.colComment.Width = 54;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnExecute, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbComment, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.cbExecuteDB, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(526, 29);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(3, 3);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 4;
            this.btnExecute.Text = "执行";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // tbComment
            // 
            this.tbComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbComment.Location = new System.Drawing.Point(464, 3);
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(59, 21);
            this.tbComment.TabIndex = 5;
            this.tbComment.TextChanged += new System.EventHandler(this.tbComment_TextChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(388, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 29);
            this.label1.TabIndex = 6;
            this.label1.Text = "表名备注:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbExecuteDB
            // 
            this.cbExecuteDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbExecuteDB.Location = new System.Drawing.Point(101, 6);
            this.cbExecuteDB.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.cbExecuteDB.Name = "cbExecuteDB";
            this.cbExecuteDB.Size = new System.Drawing.Size(125, 20);
            this.cbExecuteDB.TabIndex = 7;
            this.cbExecuteDB.Text = "是否执行到数据库";
            this.cbExecuteDB.UseVisualStyleBackColor = true;
            // 
            // tbScript
            // 
            this.tbScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbScript.Encoding = ((System.Text.Encoding)(resources.GetObject("tbScript.Encoding")));
            this.tbScript.LineViewerStyle = ICSharpCode.TextEditor.Document.LineViewerStyle.FullRow;
            this.tbScript.Location = new System.Drawing.Point(0, 0);
            this.tbScript.Name = "tbScript";
            this.tbScript.ShowEOLMarkers = true;
            this.tbScript.ShowInvalidLines = false;
            this.tbScript.ShowSpaces = true;
            this.tbScript.ShowTabs = true;
            this.tbScript.ShowVRuler = true;
            this.tbScript.Size = new System.Drawing.Size(532, 251);
            this.tbScript.TabIndex = 3;
            // 
            // tableInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 432);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "tableInfoForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableInfo)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvTableInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label label1;
        private ICSharpCode.TextEditor.TextEditorControl tbScript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataPrecision;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataScale;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataNullAble;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataDefault;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsPrimaryKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsForeginKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
        private System.Windows.Forms.CheckBox cbExecuteDB;
    }
}
