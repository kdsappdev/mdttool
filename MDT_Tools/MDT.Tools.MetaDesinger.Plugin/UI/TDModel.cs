using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.DB.Common;

namespace MDT.Tools.MetaDesinger.Plugin.UI
{
    [Serializable]
    public class TDDesinger
    {
        private List<TDTable> tDTables = new List<TDTable>();
        public List<TDTable> TDTables
        {
            get { return tDTables; }
            set { tDTables = value; }
        }

        private List<TDRelation> tDRelation = new List<TDRelation>();
        public List<TDRelation> TDRelation
        {
            get { return tDRelation; }
            set { tDRelation = value; }
        }
    }
    [Serializable]
    public class TDTable : TableInfo
    {


        private List<TDColumn> columns = new List<TDColumn>();

        public new List<TDColumn> Columns
        {
            get { return columns; }
            set { columns = value; }
        }
    }
    [Serializable]
    public class TDColumn : ColumnInfo
    {
        public bool IsSeq { get; set; }
        private List<TDRelation> relations=new List<TDRelation>();
        public List<TDRelation> Relations
        {
            get { return relations; }
            set { relations = value; }
        }
    }
    [Serializable]
    public class TDRelation
    {
    }
}
