using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Common;
using MDT.Tools.DB.Ibatis.Plugin.Utils;

namespace MDT.Tools.DB.Ibatis.Plugin.Gen
{
    public class GenIbatis:AbstractHandler
    {
        public GenIbatis()
        {
            AddContextMenu();
        }
         
        public static readonly string TemplatesPath = Application.StartupPath + "\\templates\\";
        private GenHelper genHelper=new GenHelper();
        private readonly NVelocityHelper nVelocityHelper = new NVelocityHelper(TemplatesPath);
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            base.process(drTables, dsTableColumns, dsTablePrimaryKeys);
            ThreadPool.QueueUserWorkItem(o =>
            {
                foreach (TableInfo table in tableInfos)
                {
                    string tableName = genHelper.GetProperty(table.TableName);

                    string tempPo = GenPo(table);
                    DisplayJavaCode(tempPo, tableName + ConstHelper.PoTail, ConstHelper.PoPath);

                    string tempPoExample = GenPoExample(table);
                    DisplayJavaCode(tempPoExample, tableName + ConstHelper.PoExampleTail, ConstHelper.PoPath);

                    string tempDao = GenDao(table);
                    DisplayJavaCode(tempDao, tableName + ConstHelper.DaoTail, ConstHelper.DaoPath);

                    string tempDaoImpl = GenDaoImpl(table);
                    DisplayJavaCode(tempDaoImpl, tableName + ConstHelper.DaoImplTail, ConstHelper.DaoPath);

                    string tempSqlMap = GenSqlMap(table);
                    DisplayXMLCode(tempSqlMap, table.TableName + ConstHelper.SqlMapTail, ConstHelper.DaoPath);

                }
            });

        }

        private string GenPo(TableInfo table)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("table", table);
            dic.Add("package", ConstHelper.PoPackage);
            dic.Add("GenHelper", genHelper);
            return nVelocityHelper.GenByTemplate("mdt.po.java.vm", dic);
        }
        private string GenPoExample(TableInfo table)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("table", table);
            dic.Add("package", ConstHelper.PoPackage);
            dic.Add("GenHelper", genHelper);
            return nVelocityHelper.GenByTemplate("mdt.poExample.java.vm", dic);
        }

        private string GenDao(TableInfo table)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("table", table);
            dic.Add("daopackage", ConstHelper.DaoPackage);
            dic.Add("popackage", ConstHelper.PoPackage);
            dic.Add("GenHelper", genHelper);
            string temp = genHelper.GetGeneratedKeyName(table.TableName);
            if (!string.IsNullOrEmpty(temp))
            {
                foreach (var column in table.Columns)
                {
                    if (temp.Equals(column.Name))
                    {
                        dic.Add("KeyJavaType", column.JavaType);
                        dic.Add("KeyField", genHelper.GetField(temp));
                    }
                }
            }

            return nVelocityHelper.GenByTemplate("mdt.dao.java.vm", dic);
        }

        private string GenDaoImpl(TableInfo table)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("table", table);
            dic.Add("daopackage", ConstHelper.DaoPackage);
            dic.Add("popackage", ConstHelper.PoPackage);
            dic.Add("GenHelper", genHelper);
            string temp = genHelper.GetGeneratedKeyName(table.TableName);
            if (!string.IsNullOrEmpty(temp))
            {
                foreach (var column in table.Columns)
                {
                    if (temp.Equals(column.Name))
                    {
                        dic.Add("KeyJavaType", column.JavaType);
                        dic.Add("KeyField", genHelper.GetField(temp));
                        dic.Add("KeyProperty", genHelper.GetProperty(temp));
                    }
                }
            }

            return nVelocityHelper.GenByTemplate("mdt.daoImpl.java.vm", dic);
        }

        private string GenSqlMap(TableInfo table)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("table", table);
            dic.Add("daopackage", ConstHelper.DaoPackage);
            dic.Add("popackage", ConstHelper.PoPackage);
            dic.Add("GenHelper", genHelper);
            string temp = genHelper.GetGeneratedKeyName(table.TableName);
            if (!string.IsNullOrEmpty(temp))
            {
                foreach (var column in table.Columns)
                {
                    if (temp.Equals(column.Name))
                    {
                        dic.Add("KeyJavaType", column.JavaType);
                        dic.Add("ColumnKey", column);
                        dic.Add("KeyField", genHelper.GetField(temp));
                    }
                }
            }

            return nVelocityHelper.GenByTemplate("mdt.SqlMap.xml.vm", dic);
        }

        private void DisplayJavaCode(string context, string fileName, string filePath)
        {
            DisplayCode(context, fileName, "", filePath);
        }

        private void DisplayXMLCode(string context, string fileName, string filePath)
        {
            //DisplayCode(context, fileName, "XML", filePath);
            DisplayCode(context, fileName, "", filePath);
        }

        private void DisplayCode(string context, string fileName, string codeLanguage, string filePath)
        {
            FileHelper.Write(filePath + fileName, new[] { context });
            CodeShow( fileName,context);
        }
    }
}