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
    public partial class DesingerLayer : UserControl
    {
        public DBSubPlugin DBSubPlugin { get; set; }
        private Recter recter=new Recter();
        private DesingerHost _dDesingerHost;
        public DesingerHost DesingerHost
        {
            get { return _dDesingerHost; }
            set
            {
                _dDesingerHost = value;
                Rectangle r = DesingerHost.Bounds;
                r = DesingerHost.Parent.RectangleToScreen(r);
                r = this.RectangleToClient(r);
                recter.IsForm = true;
                recter.Rect = r;
            }
        }

        public DesingerLayer()
        {
            InitializeComponent();


          
        }
        protected override void OnPaintBackground(PaintEventArgs e) //不画背景
        {
            //base.OnPaintBackground(e);
        }
        private void DesingerLayer_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            
        }

        private void DesingerLayer_DragDrop(object sender, DragEventArgs e)
        {
            DataRow dr = (DataRow)e.Data.GetData(typeof(DataRow));
            
                TableDesingerHandler tableDesingerHandler=new TableDesingerHandler();
                DBSubPlugin.process(new DataRow[]{dr}, tableDesingerHandler);
                TableDesingerUI t=new TableDesingerUI();
               t.TableInfo = tableDesingerHandler.tableInfos[0];
                 t.init();
                t.Location = DesingerHost.PointToClient(new Point(e.X, e.Y));
                DesingerHost.Controls.Add(t);
                t.BringToFront();
                _currentCtrl = t;
                Rectangle r = DesingerHost.RectangleToScreen(t.Bounds);
                recter.Rect = this.RectangleToClient(r);
                 
                //recter.Rect = t.Bounds;
                Invalidate2(false);
              
             
        }
        Control _currentCtrl = null; //当前被操作控件
        private void Invalidate2(bool mouseUp)
        {
            Invalidate();
            if (Parent != null) //更新父控件
            {
                Rectangle rc = new Rectangle(this.Location, this.Size);
                Parent.Invalidate(rc, true);
            }
            if (mouseUp) //鼠标弹起 更新底层控件
            {
                if (_currentCtrl != null) //更新底层控件的位置、大小
                {
                    Rectangle r = recter.Rect;
                    r = RectangleToScreen(r);
                    r = DesingerHost.RectangleToClient(r);
                    recter.IsForm = false;
                    _currentCtrl.SetBounds(r.Left, r.Top, r.Width, r.Height);
                }else
                {
                  
                    Rectangle r = recter.Rect;
                    r = this.RectangleToScreen(r);
                    r = Parent.RectangleToClient(r);
                    recter.IsForm = true;
                    DesingerHost.SetBounds(r.Left, r.Top, r.Width, r.Height);
                
                }

            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (recter != null&&!recter.IsForm) //绘制被操作控件周围的方框
            {
                recter.Draw(e.Graphics);
            }
            base.OnPaint(e);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams para = base.CreateParams;
                para.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT 透明支持
                return para;
            }
        }


        #region 代理所有用户操作
        Point _firstPoint = new Point();
        bool _mouseDown = false;
        DragType _dragType = DragType.None;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!_mouseDown) //鼠标形状
            {
                DragType dt = recter.GetMouseDragType(e.Location);
                switch (dt)
                {
                    case DragType.Top:
                        {
                            Cursor = Cursors.SizeNS;
                            break;
                        }
                    case DragType.RightTop:
                        {
                            Cursor = Cursors.SizeNESW;
                            break;
                        }
                    case DragType.RightBottom:
                        {
                            Cursor = Cursors.SizeNWSE;
                            break;
                        }
                    case DragType.Right:
                        {
                            Cursor = Cursors.SizeWE;
                            break;
                        }
                    case DragType.LeftTop:
                        {
                            Cursor = Cursors.SizeNWSE;
                            break;
                        }
                    case DragType.LeftBottom:
                        {
                            Cursor = Cursors.SizeNESW;
                            break;
                        }
                    case DragType.Left:
                        {
                            Cursor = Cursors.SizeWE;
                            break;
                        }
                    case DragType.Center:
                        {
                            Cursor = Cursors.SizeAll;
                            break;
                        }
                    case DragType.Bottom:
                        {
                            Cursor = Cursors.SizeNS;
                            break;
                        }
                    default:
                        {
                            Cursor = Cursors.Default;
                            break;
                        }
                }
            }
            else
            {
                switch (_dragType) //改变方框位置大小
                {
                    case DragType.Top:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            recter.Rect = new Rectangle(recter.Rect.X, recter.Rect.Y + delta.Y, recter.Rect.Width, recter.Rect.Height + delta.Y * (-1));
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.RightTop:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            recter.Rect = new Rectangle(recter.Rect.X, recter.Rect.Y + delta.Y, recter.Rect.Width + delta.X, recter.Rect.Height + delta.Y * (-1));
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.RightBottom:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            recter.Rect = new Rectangle(recter.Rect.X, recter.Rect.Y, recter.Rect.Width + delta.X, recter.Rect.Height + delta.Y);
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.Right:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            recter.Rect = new Rectangle(recter.Rect.X, recter.Rect.Y, recter.Rect.Width + delta.X, recter.Rect.Height);
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.LeftTop:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            recter.Rect = new Rectangle(recter.Rect.X + delta.X, recter.Rect.Y + delta.Y, recter.Rect.Width + delta.X * (-1), recter.Rect.Height + delta.Y * (-1));
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.LeftBottom:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            recter.Rect = new Rectangle(recter.Rect.X + delta.X, recter.Rect.Y, recter.Rect.Width + delta.X * (-1), recter.Rect.Height + delta.Y);
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.Left:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            recter.Rect = new Rectangle(recter.Rect.X + delta.X, recter.Rect.Y, recter.Rect.Width + delta.X * (-1), recter.Rect.Height);
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.Center:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            recter.Rect = new Rectangle(recter.Rect.X + delta.X, recter.Rect.Y + delta.Y, recter.Rect.Width, recter.Rect.Height);
                            _firstPoint = e.Location;
                            break;
                        }
                    case DragType.Bottom:
                        {
                            Point delta = new Point(e.Location.X - _firstPoint.X, e.Location.Y - _firstPoint.Y);
                            recter.Rect = new Rectangle(recter.Rect.X, recter.Rect.Y, recter.Rect.Width, recter.Rect.Height + delta.Y);
                            _firstPoint = e.Location;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            if (_mouseDown)
            {
                Invalidate2(false);
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //左键
            {
                bool flag = false;
                foreach (Control c in DesingerHost.Controls) //遍历控件容器 看是否选中其中某一控件
                {
                    Rectangle r = c.Bounds;
                    r = DesingerHost.RectangleToScreen(r);
                    r = this.RectangleToClient(r);
                    Rectangle rr = r;
                    rr.Inflate(10,10);
                    if (rr.Contains(e.Location))
                    {
                        recter.Rect = r;
                        _currentCtrl = c;
                        
                        recter.IsForm = false;
                        flag = true;
                        Invalidate2(false);
                        break;
                    }
                }
                if (!flag) //没有控件被选中，判断是否选中控件容器
                {
                    Rectangle r = DesingerHost.Bounds;
                    r = Parent.RectangleToScreen(r);
                    r = this.RectangleToClient(r);
                    if (r.Contains(e.Location))
                    {
                        recter.Rect = r;
                        recter.IsForm = true;
                        _currentCtrl = null;
                       
                        Invalidate2(false);
                    }
                }
                DragType dt = recter.GetMouseDragType(e.Location);  //判断是否可以进行鼠标操作
                if (dt != DragType.None)
                {
                    _mouseDown = true;
                    _firstPoint = e.Location;
                    _dragType = dt;
                }
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //左键弹起
            {
                _firstPoint = new Point();
                _mouseDown = false;
                _dragType = DragType.None;
                Invalidate2(true);
            }
            base.OnMouseUp(e);
        }
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            drgevent.Effect = DragDropEffects.Copy;
            base.OnDragEnter(drgevent);
        }
       
        #endregion

    }
}
