using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.Common
{
    public class TableInfo
    {
        private string tableName;

        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        private string tableComments;

        public string TableComments
        {
            get { return tableComments; }
            set { tableComments = value; }
        }
        private List<ColumnInfo> columns = new List<ColumnInfo>();

        public List<ColumnInfo> Columns
        {
            get { return columns; }
            set { columns = value; }
        }
    }

    public class ColumnInfo
    {
        private bool isChanged = false;
        public bool IsChanged
        {
            get { return isChanged; }
            set { isChanged = value; }

        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string comments;
        private string oldComments;
        private bool isfirst = true;
        public string Comments
        {
            get { return comments; }
            set { 
                comments = value;
                if (isfirst)
                {
                    oldComments = comments;
                    isfirst = false;
                }
                if (comments != oldComments)
                {
                    isChanged = true;
                }
                else
                {
                    isChanged = false;
                }

            }
        }
        private string dataType;

        public string DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }
        private string dataScale;

        public string DataScale
        {
            get { return dataScale; }
            set { dataScale = value; }
        }
        private string dataLength;

        public string DataLength
        {
            get { return dataLength; }
            set { dataLength = value; }
        }
        private bool dataNullAble=true;

        public bool DataNullAble
        {
            get { return dataNullAble; }
            set { dataNullAble = value; }
        }
        private string dataDefault;

        public string DataDefault
        {
            get { return dataDefault; }
            set { dataDefault = value; }
        }
        private string dataPrecision;

        public string DataPrecision
        {
            get { return dataPrecision; }
            set { dataPrecision = value; }
        }
        private bool isPrimaryKeys;

        public bool IsPrimaryKeys
        {
            get { return isPrimaryKeys; }
            set { isPrimaryKeys = value; }
        }
        private bool isForeignkey;

        public bool IsForeignkey
        {
            get { return isForeignkey; }
            set { isForeignkey = value; }
        }

        #region 扩展

        public bool CommentsIsNull
        {
            get { return string.IsNullOrEmpty(comments); }
        }

        public string JdbcType
        {
            get
            {
                string retval = "VARCHAR";
                if (!string.IsNullOrEmpty(dataType))
                {
                    string temp = dataType.ToUpper();
                    if ("NVARCHAR".Equals(temp) || "NVARCHAR2".Equals(temp) || "VARCHAR2".Equals(temp))
                    {
                        retval = "VARCHAR";
                    }
                    else if ("NUMBER".Equals(temp))
                    {
                        retval = "DECIMAL";
                    }
                    else if ("CLOB".Equals(temp))
                    {
                        retval = "OTHER";
                    }
                    else if ("DATE".Equals(temp))
                    {
                        retval = "TIMESTAMP";
                    }
                    else
                    {
                        retval = temp;
                    }
                }
                return retval;
            }
        }

        public string JavaType
        {
            get
            {
                string retval = "String";
                if (!string.IsNullOrEmpty(dataType))
                {
                    string temp = dataType.ToUpper();
                    if ("CHAR".Equals(temp) || "VARCHAR".Equals(temp) || "VARCHAR2".Equals(temp) ||
                        "NVARCHAR".Equals(temp) || "NVARCHAR2".Equals(temp))
                    {
                        retval = "String";
                    }
                    else if ("NUMBER".Equals(temp))
                    {
                        retval = "BigDecimal";
                    }
                    else if ("DATE".Equals(temp))
                    {
                        retval = "Date";
                    }
                    else if ("TIMESTAMP".Contains(temp))
                    {
                        retval = "Timestamp";
                    }
                    else if ("CLOB".Equals(temp))
                    {
                        retval = "Object";
                    }
                }
                return retval;
            }
        }
        #endregion
    }
}
