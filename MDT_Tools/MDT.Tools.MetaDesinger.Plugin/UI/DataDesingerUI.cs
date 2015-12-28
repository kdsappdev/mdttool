using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools.MetaDesinger.Plugin.UI
{
    public partial class DataDesingerUI : DockContent
    {
       
        public DataDesingerUI()
        {
            InitializeComponent();
            
            
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //saveBit();
        }

        #region 保存界面截图
        private void saveBit()
        {
            var bit1 = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bit1, new Rectangle(0, 0, this.Width, this.Height));

            bit1.Save("DataDesingerUI.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);//包括标题栏和边框

            bit1.Dispose();

        }

        #endregion
    }


}
