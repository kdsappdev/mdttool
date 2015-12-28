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
        public delegate void DrawLineDel(
            TableDesingerUI sourUI, int sourIndex, int sourX, int sourY, TableDesingerUI destUI, int destIndex,
            int destX, int destY);

        public event DrawLineDel DrawLineEvent;
        private string seqStr = ",自增长";
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
                        currTable.clbField.ClearSelected();
                    }
                    currTable = value;
                    currTable.panel1.BackColor = Color.SkyBlue;
                    //currTable.clbField.SetSelected(0,true);
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
                lcTitle.Text = string.Format("{0}", TableInfo.TableName);
                
                toolTip1.SetToolTip(lcTitle,string.Format("{0}",string.IsNullOrEmpty(TableInfo.TableComments)?"":TableInfo.TableComments));

                clbField.CItem.Add(new CListBoxItem() { CnItem = "", DisItem = "*(所有列)", Site = clbField });
                foreach (var col in TableInfo.Columns)
                {
                    clbField.CItem.Add(new CListBoxItem(){CnItem = col.Comments,DisItem = col.Name,Site = clbField});
                    clbField.Items.Add(new CListBoxItem() { CnItem = col.Comments, DisItem = col.Name });
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

        
        #region 设置seq
        private void clbField_MouseDown(object sender, MouseEventArgs e)
        {
            CurrTable = this;
            if (e.Button == MouseButtons.Right&&clbField.SelectedIndex>=1)
            {
               
                if (clbField.Items[clbField.SelectedIndex].ToString().Contains(seqStr))
                {
                    tsmiSeq.Checked = true;
                }
                else
                {
                    tsmiSeq.Checked = false;
                }
                cmsField.Show(clbField, e.Location);
            }
            else
            {

                int idx =  clbField.IndexFromPoint(e.Location);
                if (idx > -1 && idx < clbField.Items.Count)
                {
                    CListBoxItem item = clbField.CItem[idx];
                    bool isCheck = item.isCheck(e.Location);
                    if (isCheck)
                    {
                        item.IsSelected = !item.IsSelected;
                        if (idx == 0)
                        {
                            for (int i = 1; i < clbField.Items.Count; i++)
                            {
                                CListBoxItem it = clbField.CItem[i];

                                it.IsSelected = item.IsSelected;
                            }
                        }
                    }
                    clbField.Invalidate();

                }

                int index = ((ListBox)sender).IndexFromPoint(e.X, e.Y);

                if (index >= 1)
                {
                    var p = this.Parent.PointToClient(PointToScreen(new Point(e.X, e.Y + 28)));
                    ((ListBox)sender).DoDragDrop(new object[] { this, index, p.X, p.Y }, DragDropEffects.Link);
                }
            }
        }

        private void tsmiSeq_CheckedChanged(object sender, EventArgs e)
        {
            int index = clbField.SelectedIndex;
            var col = TableInfo.Columns[index - 1];
            if(tsmiSeq.Checked)
            {
              
                clbField.Items[index]=string.Format("{0}{1}", col.Name,
                                                     seqStr);
            }
            else
            {
                clbField.Items[index]=string.Format("{0}{1}", col.Name,
                                                     "");
            }

        }
        #endregion

        private void clbField_DragDrop(object sender, DragEventArgs e)
        {
            
                int index = clbField.IndexFromPoint(clbField.PointToClient(new Point(e.X, e.Y)));
                if (index >= 1 && e.Data.GetDataPresent(typeof (object[])))
                {
                    index -= 1;
                    object[] os = (object[]) e.Data.GetData(typeof (object[]));
                    TableDesingerUI sourUI = (TableDesingerUI) os[0];
                    int sourIndex = (int) os[1] - 1;
                    int sourX = (int) os[2];
                    int sourY = (int) os[3];
                    if (DrawLineEvent != null&&sourUI!=this)
                    {
                        var p =this.Parent.PointToClient(new Point(e.X, e.Y));
                        DrawLineEvent(sourUI, sourIndex, sourX, sourY, this, index,
                                      p.X, p.Y);
                    }



                }
             

        }

        private void clbField_DragOver(object sender, DragEventArgs e)
        {
            int index = clbField.IndexFromPoint(clbField.PointToClient(new Point(e.X, e.Y)));
            if (index >= 1)
            {
                e.Effect = DragDropEffects.Link;
                 
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        #region 获取界面数据
        #endregion

        



    }
}
