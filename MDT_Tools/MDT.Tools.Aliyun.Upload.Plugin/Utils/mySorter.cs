using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace MDT.Tools.Aliyun.Upload.Plugin.Utils
{
    public class mySorter:IComparer
    {

        private Comparer comparer;
        private int sortColumn;
        private SortOrder sortOrder;
        private ListView listView1;
        public mySorter()
        {
            sortColumn = 0;
            sortOrder = SortOrder.None;
            comparer = Comparer.Default;
        }
        //指定进行排序的列
        public int SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }
        //指定按升序或降序进行排序
        public SortOrder SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }
        public ListView ListView1
        {
            get { return listView1; }
            set { listView1 = value; }
        }
        public int Compare(object x, object y)
        {
            int CompareResult=-1;
            ListViewItem itemX = (ListViewItem)x;
            ListViewItem itemY = (ListViewItem)y;
            string iX = itemX.SubItems[this.sortColumn].Text;
            string iY = itemY.SubItems[this.sortColumn].Text;
            DateTime dtx = new DateTime();
            DateTime dty = new DateTime();
            //if(this.sortColumn == 2)
            if (DateTime.TryParse(iX, out dtx) && DateTime.TryParse(iY, out dty))
            {
                CompareResult = DateTime.Compare(dtx, dty);
            }
            else
            {
                CompareResult = comparer.Compare(iX, iY);
            }
           
            if (this.SortOrder == SortOrder.Ascending)
                return CompareResult;
            else
                if (this.SortOrder == SortOrder.Descending)
                    return (-CompareResult);
                else
                    return 0;
        }
    }
}
