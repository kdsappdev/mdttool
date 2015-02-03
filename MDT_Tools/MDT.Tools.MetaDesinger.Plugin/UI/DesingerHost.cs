using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MDT.Tools.MetaDesinger.Plugin.UI
{
    public partial class DesingerHost :UserControl
    {
        public DesingerHost()
        {
            InitializeComponent();
        }

        public bool TopLevel { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            
                //ControlPaint.DrawGrid(e.Graphics, this.ClientRectangle, new Size(10, 10), Color.White); //绘制底层网格
            base.OnPaint(e);
        }
    }
}
