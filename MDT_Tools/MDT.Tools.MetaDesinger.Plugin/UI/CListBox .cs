using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MDT.Tools.MetaDesinger.Plugin.UI
{
    public class CListBox : CheckedListBox
    {

        ToolTip tip;

        public CListBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer
                 , true);
            tip = new ToolTip();

        }

        private string tipStr = "";
        private void SetTipMessage(string strTip)
        {
            if (tipStr != strTip)
            {
                tip.SetToolTip(this, strTip);
                tipStr = strTip;
            }
        }
        private List<CListBoxItem> cItem = new List<CListBoxItem>();
        public List<CListBoxItem> CItem
        {
            get { return cItem; }
            set { cItem = value; }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            
            //base.OnMouseClick(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int idx = IndexFromPoint(e.Location); // 获取鼠标所在的项索引  
            if (idx <= 0) //鼠标所在位置没有 项  
            {
                SetTipMessage(""); //设置提示信息为空  
                return;
            }
            string txt = CItem[idx].CnItem; //获取项文本  
            SetTipMessage(txt); //设置提示信息  
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.ItemHeight = 22;
            e.DrawBackground();
            e.DrawFocusRectangle();
            Brush myBrush = Brushes.Black;
            if (e.Index > -1 && e.Index < this.cItem.Count)
            {
                CListBoxItem item = CItem[e.Index];
                item.Location = e.Bounds;
                item.Paint(e.Graphics);
            }
        }
    }
}
