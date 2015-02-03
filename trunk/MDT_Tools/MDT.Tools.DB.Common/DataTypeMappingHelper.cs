using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.Common
{
    public class DataTypeMappingHelper
    {
        public static string GetCSharpDataTypeByDbType(string dbType, string dbDataType, string dbDataScale, string dbDataLenght, bool isNullAble)
        {
            string str = "";
            int scale = 0;
            int lenght = 0;
            int.TryParse(dbDataScale, out scale);
            int.TryParse(dbDataLenght, out lenght);
            switch (dbType)
            {
                case "Oracle":
                    str = OracleDBForDataTypeMappingHelper.GetCSharpDataTypeByDbType(dbDataType, scale, lenght, isNullAble);
                    break;
                case "Sql Server":
                    str = SqlServerDBForDataTypeMappingHelper.GetCSharpDataTypeByDbType(dbDataType, scale, lenght, isNullAble);
                    break;
            }
            return str;
        }
        public static string GetJavaDataTypeByDbType(string dbType, string dbDataType, string dbDataScale, string dbDataLenght, bool isNullAble)
        {
            string str = "";
            int scale = 0;
            int lenght = 0;
            int.TryParse(dbDataScale, out scale);
            int.TryParse(dbDataLenght, out lenght);
            switch (dbType)
            {
                case "Oracle":
                    str = OracleDBForDataTypeMappingHelper.GetJavaDataTypeByDbType(dbDataType, scale, lenght, isNullAble);
                    break;
                case "Sql Server":
                    //str = SqlServerDBForDataTypeMappingHelper.GetCSharpDataTypeByDbType(dbDataType, scale, lenght, isNullAble);
                    break;
            }
            return str;
        }
        public static Dictionary<string, DataType> GetDataType(string dbType)
        {
            var dic = new Dictionary<string, DataType>();
            switch (dbType)
            {
                case "Oracle":
                    dic = OracleDBForDataTypeMappingHelper.GetOracleDataType();
                    break;
                case "Sql Server":
                    
                    break;
            }
            return dic;
        }

    }
    internal class OracleDBForDataTypeMappingHelper
    {
        private OracleDBForDataTypeMappingHelper()
        { }
        public static string GetCSharpDataTypeByDbType(string dbDataType, int dbDataScale, int dbDataLenght, bool isNullAble)
        {
            string str = "string";
            if (!string.IsNullOrEmpty(dbDataType))
            {
                string temp = dbDataType.ToUpper();
                if (temp.Equals("CHAR") || temp.Equals("NVARCHAR") || temp.Equals("NVARCHAR2"))
                {
                    str = "string";
                }
                else
                    if (temp.Equals("NUMBER"))
                    {
                        //if (dbDataScale > 0)
                        {
                            str = "decimal";
                        }
                        //else
                        //    if (dbDataLenght < 10)
                        //    {
                        //        str = "int";
                        //    }
                        //    else
                        //    {
                        //        str = "long";
                        //    }
                    }
                    else
                        if (temp.Equals("DATE"))
                        {
                            str = "DateTime";
                        }

            }
            if (isNullAble)
            {
                if (!"string".Equals(str))
                    str = str + "?";
            }
            return str;
        }
        public static string GetJavaDataTypeByDbType(string dbDataType, int dbDataScale, int dbDataLenght, bool isNullAble)
        {
            string str = "String";
            if (!string.IsNullOrEmpty(dbDataType))
            {
                string temp = dbDataType.ToUpper();
                if (temp.Equals("CHAR") || temp.Equals("NVARCHAR") || temp.Equals("NVARCHAR2"))
                {
                    str = "String";
                }
                else
                    if (temp.Equals("NUMBER"))
                    {
                        if (dbDataScale > 0)
                        {
                            str = "BigDecimal";
                        }
                        else
                            if (dbDataLenght < 10)
                            {
                                str = "int";
                            }
                            else
                            {
                                str = "long";
                            }
                    }
                    else
                        if (temp.Equals("DATE"))
                        {
                            str = "Date";
                        }
                        else if(temp.Contains("TIMESTAMP"))
                        {
                            str = "Timestamp";
                        }else if(temp.Equals("CLOB"))
                        {
                            str = "Object";
                        }

            }
       
            return str;
        }
        #region Oracle 数据类型
        public static Dictionary<string, DataType> GetOracleDataType()
        {
            Dictionary<string, DataType> dic = new Dictionary<string, DataType>();
            dic.Add("VARCHAR2", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("CHAR", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("NUMBER", new DataType() { Size = false, Precision = true, Scale = true });
            dic.Add("INTEGER", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("DATE", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("LONG", new DataType() { Size = false, Precision = false, Scale = false });


            dic.Add("LONG RAW", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("RAW", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("NVARCHAR2", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("ROWID", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("NCHAR", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("MLSLABEL", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("CLOB", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("NCLOB", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("BLOB", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("BFILE", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("FLOAT", new DataType() { Size = false, Precision = true, Scale = false });
            dic.Add("UROWID", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("BINARY_DOUBLE", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("BINARY_FLOAT", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("URITYPE", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("CHAR VARYING", new DataType() { Size = true, Precision = false, Scale = false });

            dic.Add("CHARACTER", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("CHARACTER VARYING", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("DECIMAL", new DataType() { Size = false, Precision = true, Scale = true });
            dic.Add("DOUBLE PRECISION", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("INT", new DataType() { Size = false, Precision = false, Scale = false });

            dic.Add("NATIONAL CHAR", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("NATIONAL CHAR VARYING", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("NATIONAL CHARACTER", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("NATIONAL CHARACTER VARYING", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("NCHAR VARYING", new DataType() { Size = true, Precision = false, Scale = false });

            dic.Add("NUMERIC", new DataType() { Size = false, Precision = true, Scale = true });

            dic.Add("REAL", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("SMALLINT", new DataType() { Size = false, Precision = false, Scale = false });
            dic.Add("VARCHAR", new DataType() { Size = true, Precision = false, Scale = false });
            dic.Add("INTERVAL", new DataType() { Size = false, Precision = false, Scale = false });

            dic.Add("TIMESTAMP", new DataType() { Size = false, Precision = false, Scale = false });
            return dic;
        }

        #endregion
    }

    internal class SqlServerDBForDataTypeMappingHelper
    {
        private SqlServerDBForDataTypeMappingHelper()
        { }
        public static string GetCSharpDataTypeByDbType(string dbDataType, int dbDataScale, int dbDataLenght, bool isNullAble)
        {
            string str = "string";
            if (!string.IsNullOrEmpty(dbDataType))
            {
                string temp = dbDataType.ToUpper();
                if (temp.Equals("CHAR") || temp.Equals("NVARCHAR") || temp.Equals("NCHAR") || temp.Equals("VARCHAR") || temp.Equals("TEXT") || temp.Equals("NTEXT") || temp.Equals("XML") || temp.Equals("TIMESTAMP"))
                {
                    str = "string";
                }

                if (temp.Equals("INT") || temp.Equals("TINYINT") || temp.Equals("SMALLINT"))
                {
                    str = "int";

                }
                if (temp.Equals("BIGINT"))
                {
                    str = "long";
                }
                if (temp.Equals("DATETIME"))
                {
                    str = "DateTime";
                }
                if (temp.Equals("BIT"))
                {
                    str = "BYTE";
                }
                if (temp.Equals("DATETIME") || temp.Equals("SAMLLDATETIME"))
                {
                    str = "DateTime";
                }
                if (temp.Equals("FLOAT"))
                {
                    str = "float";
                }

                if (temp.Equals("BINARY") || temp.Equals("IMAGE"))
                {
                    str = "BYTE[]";
                }

                if (temp.Equals("DECIMAL") || temp.Equals("MONEY") || temp.Equals("SMALLMONEY"))
                {
                    str = "DECIMAL";
                }
                if (temp.Equals("UNIQUEIDENTIFIER"))
                {
                    str = "Guid";
                }
                
            }
            if (isNullAble)
            {
                if (!"string".Equals(str))
                    str = str + "?";
            }
            return str;
        }
    }
}
