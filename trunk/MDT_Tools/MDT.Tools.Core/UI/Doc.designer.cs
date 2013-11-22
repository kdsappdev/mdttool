namespace MDT.Tools.Core.UI
{
    partial class Doc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Doc));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.contextMenuTabPage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Location = new System.Drawing.Point(0, 4);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(448, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Visible = false;
            // 
            // contextMenuTabPage
            // 
            this.contextMenuTabPage.Name = "contextMenuTabPage";
            this.contextMenuTabPage.Size = new System.Drawing.Size(61, 4);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 28);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(448, 365);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // Doc
            // 
            this.ClientSize = new System.Drawing.Size(448, 393);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.mainMenu);
            this.Font = new System.Drawing.Font("ËÎÌו", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "Doc";
            this.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.TabPageContextMenuStrip = this.contextMenuTabPage;
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ContextMenuStrip contextMenuTabPage;
		private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}