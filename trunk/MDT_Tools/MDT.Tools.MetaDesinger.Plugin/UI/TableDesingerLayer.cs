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
    public partial class TableDesingerLayer : UserControl
    {
        public DBSubPlugin DBSubPlugin { get; set; }
        public TableDesingerLayer()
        {
            InitializeComponent();
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
            t.TableInfo = tableDesingerHandler.tableInfos[0];
            t.init();
            t.Location =PointToClient(new Point(e.X, e.Y));
            Controls.Add(t);
            TableDesingerUI.CurrTable = t;
        }
    }
}
