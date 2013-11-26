using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.DB.Csharp_Model.Plugin.Utils
{
    public static class DataTypeMappingHelper
    {
        public static string GetCSharpDataTypeByDbType(string dbType, string dbDataType, string dbDataScale, string dbDataLenght,bool isNullAble)
        {
            string str = "";
            int scale = 0;
            int lenght = 0;
            int.TryParse(dbDataScale, out scale);
            int.TryParse(dbDataLenght, out lenght);
            switch (dbType)
            {
                case "Oracle":
                    str = OracleDBForDataTypeMappingHelper.GetCSharpDataTypeByDbType(dbDataType, scale, lenght,isNullAble);
                    break;
                case "Sql Server":
                    str = SqlServerDBForDataTypeMappingHelper.GetCSharpDataTypeByDbType(dbDataType, scale, lenght,isNullAble);
                    break;
            }
            return str;
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
                        if (dbDataScale > 0)
                        {
                            str = "decimal";
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

                if (temp.Equals("BINARY")||temp.Equals("IMAGE"))
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
