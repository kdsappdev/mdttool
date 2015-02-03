using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.DB.Common;

namespace MDT.Tools.MetaDesinger.Plugin.UI
{
    public partial class TableDesingerUI : UserControl
    {
        private static Recter recter = new Recter();
        private static TableDesingerUI currTable = null;
        public static TableDesingerUI CurrTable
        {
            get { return currTable; }
            set {
                if (currTable != value)
                {
                    if(currTable!=null)
                    {
                        currTable.panel1.BackColor = Color.CornflowerBlue;
                    }
                    currTable = value;
                    currTable.panel1.BackColor = Color.SkyBlue;
                    currTable.BringToFront();

                    Rectangle r = currTable.Bounds;

                    recter.Rect = currTable.RectangleToClient(r);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (recter != null && !recter.IsForm)
            {
                recter.Draw(e.Graphics);
            }
            base.OnPaint(e);
        }

        private int clbFieldHeigh;
        public TableDesingerUI()
        {
            InitializeComponent();
            clbFieldHeigh=clbField.Height;
        }

        public TableInfo TableInfo { get; set; }

        public void init()
        {
            if(TableInfo!=null)
            {
                lcTitle.Text = string.Format("{0}{1}",TableInfo.TableName,string.IsNullOrEmpty(TableInfo.TableComments)?"":"("+TableInfo.TableComments+")");
                
                toolTip1.SetToolTip(lcTitle,string.Format("{0}",lcTitle.Text));
                foreach (var col in TableInfo.Columns)
                {
                    clbField.Items.Add(string.Format("{0}{1}", col.Name,
                                                     string.IsNullOrEmpty(col.Comments) ? "" : "(" + col.Comments + ")"));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clbField.Visible = !clbField.Visible;
            if(!clbField.Visible)
            {
                this.Height -= clbFieldHeigh;
            }
            else
            {
                this.Height += clbFieldHeigh;
            }
        }

        #region 窗口移动
        private bool dlagMouseMD;
        private bool dlagMouseUP;
        private Point pointMD;

        private void lcTitle_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
            if (dlagMouseMD && !dlagMouseUP)
            {
                var pt = new Point();
                pt.X = Location.X;
                pt.Y = Location.Y;
                pt.X += e.X - pointMD.X;
                pt.Y += e.Y - pointMD.Y;
                Location = pt;
            }
        }

        private void lcTitle_MouseDown(object sender, MouseEventArgs e)
        {
            CurrTable = this;
            pointMD = e.Location;
            dlagMouseMD = true;
            dlagMouseUP = false;
        }

        private void lcTitle_MouseUp(object sender, MouseEventArgs e)
        {
            dlagMouseMD = false;
            dlagMouseUP = true;
        }

        private void lcTitle_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
        #endregion
        
        private void clbField_SizeChanged(object sender, EventArgs e)
        {
            clbField.Height = clbField.Height;
        }

        private void TableDesingerUI_MouseMove(object sender, MouseEventArgs e)
        {

        }

    }
}
