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
    public partial class TableDesingerLayer : ScrollableControl
    {
        public DBSubPlugin DBSubPlugin { get; set; }
        public DataDesingerUI DataDesingerUI { get; set; }

        public TableDesingerLayer()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles(); 
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            drgevent.Effect = DragDropEffects.Copy;
            base.OnDragEnter(drgevent);
        }

        private void TableDesingerLayer_DragDrop(object sender, DragEventArgs e)
        {
            DataRow dr = (DataRow)e.Data.GetData(typeof(DataRow));
            TableDesingerHandler tableDesingerHandler = new TableDesingerHandler();
            DBSubPlugin.process(new DataRow[] { dr }, tableDesingerHandler);
            TableDesingerUI t = new TableDesingerUI();
             
            t.DrawLineEvent += new TableDesingerUI.DrawLineDel(t_DrawLineEvent);
            t.TableInfo = tableDesingerHandler.tableInfos[0];
            t.init();
            t.Location =PointToClient(new Point(e.X, e.Y));
            Controls.Add(t);
            
            TableDesingerUI.CurrTable = t;
        }
        
        protected ConnectionCollection connections=new ConnectionCollection();
        public Connection AddConnection(Connection con)
        {
            connections.Add(con);
            con.Site = this;
             
            this.Invalidate();
            return con;
        }
        
        public Connection AddConnection(Connector from, Connector to)
        {
            Connection con = AddConnection(from.Point, to.Point);
            from.AttachConnector(con.From);
            to.AttachConnector(con.To);
            AddConnection(con);

            return con;

        }

        public Connection AddConnection(Point from, Point to)
        {
            Connection con = new Connection(from, to);
            this.AddConnection(con);
            return con;
        }
        void t_DrawLineEvent(TableDesingerUI sourUI, int sourIndex, int sourX, int sourY, TableDesingerUI destUI, int destIndex, int destX, int destY)
        {
           var con= AddConnection(new Point(sourX, sourY), new Point(destX, destY));
            con.From.Site = sourUI;
            con.To.Site = destUI;
            //MessageBox.Show(string.Format("sourIndex:{0},x:{1},y:{2};destIndex:{3},x:{4},y:{5}", sourUI.TableInfo.Columns[sourIndex].Name, sourX, sourY,
            //                                destUI.TableInfo.Columns[destIndex].Name, destX, destY));
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //use the best quality, with a performance penalty
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //similarly for the connections
            for (int k = 0; k < connections.Count; k++)
            {
                connections[k].Paint(g);
                connections[k].From.Paint(g);
                connections[k].To.Paint(g);
            }

        }
        protected bool tracking = false;
        
        protected Point refp;
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Point p = new Point(e.X, e.Y);

            #region LMB & RMB

            //test for connectors
            for (int k = 0; k < connections.Count; k++)
            {
                if (connections[k].From.Hit(p))
                {
                    UpdateSelected(connections[k].From);
                    tracking = true;
                    refp = p;
                    return;
                }

                if (connections[k].To.Hit(p))
                {
                    UpdateSelected(connections[k].To);
                    tracking = true;
                    refp = p;
                    return;
                }
            }

            
             
            if (selectedEntity != null) selectedEntity.IsSelected = false;
            selectedEntity = null;
            Invalidate();
            refp = p; //useful for all kind of things
           
            #endregion
        }
        protected Entity selectedEntity;
        private void UpdateSelected(Entity oEnt)
        {
            if (selectedEntity != null)
            {
                selectedEntity.IsSelected = false;
                selectedEntity.Invalidate();
            }
            selectedEntity = oEnt;
            oEnt.IsSelected = true;
            oEnt.Invalidate();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point p = new Point(e.X, e.Y);


            if (tracking)
            {
                selectedEntity.Move(new Point(p.X - refp.X, p.Y - refp.Y));
                refp = p;
                Invalidate();
                 
            }
        }

  
    }
}
