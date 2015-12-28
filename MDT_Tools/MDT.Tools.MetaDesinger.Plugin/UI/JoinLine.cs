using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MDT.Tools.MetaDesinger.Plugin.UI
{
   public class JoinLine : UserControl
    {
        
        public JoinLine()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // JoinLine
            // 
            this.Name = "JoinLine";
            this.Size = new System.Drawing.Size(286, 10);
            this.ResumeLayout(false);

        }
        
    }
}
