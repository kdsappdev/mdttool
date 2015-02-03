using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace MDT.Tools.MetaDesinger.Plugin.UI
{
    /// <summary>
    /// 选中控件时，周围出现的方框
    /// </summary>
    class Recter
    {
        Rectangle _rect = new Rectangle();
        bool _isform = false;  //是否为窗体周围的方框（如果是，则位置不可改变，且只有从下、右、右下三个方向改变大小）

        public Rectangle Rect
        {
            get
            {
                return _rect;
            }
            set
            {
                _rect = value;
            }
        }
        public bool IsForm
        {
            get { return _isform; }
            set
            {
                _isform = value;
            }
        }
        public Recter()
        {

        }
        /// <summary>
        /// 绘制方框
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            Rectangle rect = _rect;
            
            using (Pen p = new Pen(Brushes.Black,1))
            {
                p.DashStyle = DashStyle.Dot;
                rect.Inflate(new Size(+1, +1));
                g.DrawRectangle(p, rect); //方框

                p.DashStyle = DashStyle.Solid;
                
                //8个方块
                g.FillRectangle(Brushes.White, new Rectangle(rect.Left - 6, rect.Top - 6, 6, 6));
                g.FillRectangle(Brushes.White, new Rectangle(rect.Left + rect.Width / 2 - 3, rect.Top - 6, 6, 6));
                g.FillRectangle(Brushes.White, new Rectangle(rect.Left + rect.Width, rect.Top - 6, 6, 6));
                g.FillRectangle(Brushes.White, new Rectangle(rect.Left - 6, rect.Top + rect.Height / 2 - 3, 6, 6));
                g.FillRectangle(Brushes.White, new Rectangle(rect.Left - 6, rect.Top + rect.Height, 6, 6));
                g.FillRectangle(Brushes.White, new Rectangle(rect.Left + rect.Width, rect.Top + rect.Height / 2 - 3, 6, 6));
                g.FillRectangle(Brushes.White, new Rectangle(rect.Left + rect.Width / 2 - 3, rect.Top + rect.Height, 6, 6));
                g.FillRectangle(Brushes.White, new Rectangle(rect.Left + rect.Width, rect.Top + rect.Height, 6, 6));

                g.DrawRectangle(p, new Rectangle(rect.Left - 6, rect.Top - 6, 6, 6));
                g.DrawRectangle(p, new Rectangle(rect.Left + rect.Width / 2 - 3, rect.Top - 6, 6, 6));
                g.DrawRectangle(p, new Rectangle(rect.Left + rect.Width, rect.Top - 6, 6, 6));
                g.DrawRectangle(p, new Rectangle(rect.Left - 6, rect.Top + rect.Height / 2 - 3, 6, 6));
                g.DrawRectangle(p, new Rectangle(rect.Left - 6, rect.Top + rect.Height, 6, 6));
                g.DrawRectangle(p, new Rectangle(rect.Left + rect.Width, rect.Top + rect.Height / 2 - 3, 6, 6));
                g.DrawRectangle(p, new Rectangle(rect.Left + rect.Width / 2 - 3, rect.Top + rect.Height, 6, 6));
                g.DrawRectangle(p, new Rectangle(rect.Left + rect.Width, rect.Top + rect.Height, 6, 6));
            }
            
        }
        /// <summary>
        /// 判断鼠标操作类型
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DragType GetMouseDragType(Point p)
        {
            Rectangle _rect = this._rect;
            _rect.Inflate(new Size(3, 3));
            if (new Rectangle(_rect.Left - 2, _rect.Top - 2, 4, 4).Contains(p) && !_isform)
            {
                return DragType.LeftTop;
            }
            if (new Rectangle(_rect.Left + 2, _rect.Top - 2, _rect.Width - 4, 4).Contains(p) && !_isform)
            {
                return DragType.Top;
            }
            if (new Rectangle(_rect.Left - 2, _rect.Top + 2, 4, _rect.Height - 4).Contains(p) && !_isform)
            {
                return DragType.Left;
            }
            if (new Rectangle(_rect.Left - 2, _rect.Top + _rect.Height - 2, 4, 4).Contains(p) && !_isform)
            {
                return DragType.LeftBottom;
            }
            if (new Rectangle(_rect.Left + 2, _rect.Top + _rect.Height - 2, _rect.Width - 4, 4).Contains(p) && !_isform)
            {
                return DragType.Bottom;
            }
            if (new Rectangle(_rect.Left + _rect.Width - 2, _rect.Top + _rect.Height - 2, 4, 4).Contains(p) && !_isform)
            {
                return DragType.RightBottom;
            }
            if (new Rectangle(_rect.Left + _rect.Width - 2, _rect.Top + 2, 4, _rect.Height - 4).Contains(p) && !_isform)
            {
                return DragType.Right;
            }
            if (new Rectangle(_rect.Left + _rect.Width - 2, _rect.Top - 2, 4, 4).Contains(p) && !_isform)
            {
                return DragType.RightTop;
            }
            if (new Rectangle(_rect.Left + 2, _rect.Top + 2, _rect.Width - 4, _rect.Height - 4).Contains(p) && !_isform)
            {
                return DragType.Center;
            }
            return DragType.None;
        }
        
    }
    /// <summary>
    /// 鼠标操作类型
    /// </summary>
    enum DragType
    {
        None,
        Left,
        Top,
        Right,
        Bottom,
        LeftTop,
        RightTop,
        LeftBottom,
        RightBottom,
        Center
    }
}
