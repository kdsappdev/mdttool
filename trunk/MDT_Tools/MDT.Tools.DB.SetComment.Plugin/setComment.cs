using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using MDT.Tools.Core.UI;
using MDT.Tools.Core.Utils;


using WeifenLuo.WinFormsUI.Docking;

namespace MDT.Tools.DB.SetComment.Plugin
{
    /// <summary>
    /// 设置备注
    /// </summary>
    internal class setComment
    {
        public string dbName;
        public string dbType;
        public string dbConnectionString;
        public ToolStripStatusLabel tsslMessage;
        public ToolStripProgressBar tspbLoadDBProgress;
        public string DBtable;
        public string DBtablesColumns;
        public string DBviews;
        public string DBtablesPrimaryKeys;
        public DataSet dsTable;
        public DataSet dsTableColumn;
        public DataSet dsTablePrimaryKey;
        public ContextMenuStrip MainContextMenu;
        public ToolStripItem tsiGen;
        public DockPanel Panel;

        public Encoding OriginalEncoding;
        public Encoding TargetEncoding;
        public string PluginName;
        public DataRow[] DrTables;
        public void set(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                setStatusBar("");
                setEnable(false);
                DrTables = drTables;
                if (drTables != null && dsTableColumns != null)
                {
                    for (int i = 0; i < drTables.Length; i++)
                    {
                        DataRow drTable = drTables[i];

                        DataRow[] drTableColumns = dsTableColumns.Tables[dbName + DBtablesColumns].Select("TABLE_NAME = '" + drTable["name"].ToString() + "'", "COLUMN_ID ASC");
                        set(drTable, drTableColumns);
                    }

                }


            }
            catch (Exception ex)
            {
                setStatusBar(string.Format("异常[{0}]", ex.Message));
            }
            finally
            {
                setEnable(true);
            }

        }


        public void set(DataRow drTable, DataRow[] drTableColumns)
        {
            
            string tableName = drTable["name"] + "";
            string tableNameComment = drTable["comments"] + "";
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add("列名");
            dt.Columns.Add("数据类型");

            dt.Columns.Add("是否NULL");
            dt.Columns.Add("默认值");
            dt.Columns.Add("主键");
            dt.Columns.Add("外键");
            dt.Columns.Add("备注");

           
            foreach (var dr in drTableColumns)
            {
                var temp = dt.NewRow();
                dt.Rows.Add(temp);
                string columnName = dr["COLUMN_NAME"] as string;
                string dataType = dr["DATA_TYPE"] + "";
                string dataScale = dr["DATA_SCALE"] + "";
                string dataLength = dr["DATA_LENGTH"] + "";
                string dataPrecision = dr["Data_Precision"] + "";
                if (string.IsNullOrEmpty(dataPrecision)) //字符串、日期等
                {
                    if ("DATE".Equals(dataType))
                    {
                        dataType = dataType + "()";
                    }
                    else
                    {
                        dataType = dataType + "(" + dataLength + ")";
                    }
                }
                else //数字类型等
                {
                    if ("0".Equals(dataScale))
                    {
                        dataType = dataType + "(" + dataPrecision + ")";
                    }
                    else
                    {
                        dataType = dataType + "(" + dataPrecision + "," + dataScale + ")";
                    }
                }
                temp["列名"] = columnName;
                temp["数据类型"] = dataType;
                temp["是否NULL"] = dr["NULLABLE"];
                temp["默认值"] = dr["DATA_DEFAULT"];
                DataRow[] drs = dsTablePrimaryKey.Tables[dbName + DBtablesPrimaryKeys].Select("TABLE_NAME = '" + tableName + "' AND COLUMN_NAME ='" + columnName + "' and constraint_type='P'");

                if (drs != null && drs.Length > 0)
                {
                    temp["主键"] = "Y";
                }
                else
                {
                    temp["主键"] = "N";
                }

                DataRow[] drs2 = dsTablePrimaryKey.Tables[dbName + DBtablesPrimaryKeys].Select("TABLE_NAME = '" + tableName + "' AND COLUMN_NAME ='" + columnName + "' and constraint_type='R'");

                if (drs2 != null && drs2.Length > 0)
                {
                    temp["外键"] = "Y";
                }
                else
                {
                    temp["外键"] = "N";
                }
                string comments = dr["COMMENTS"] + "";
                if (OriginalEncoding != null && TargetEncoding != null && !string.IsNullOrEmpty(comments))
                {
                    comments = MDT.Tools.Core.Utils.EncodingHelper.ConvertEncoder(OriginalEncoding, TargetEncoding, comments);
               
                }
                temp["备注"] = comments;
            }
            if (OriginalEncoding != null && TargetEncoding != null && !string.IsNullOrEmpty(tableNameComment))
            {
                tableNameComment = MDT.Tools.Core.Utils.EncodingHelper.ConvertEncoder(OriginalEncoding, TargetEncoding, tableNameComment);

            }
            UI.tableInfoForm t = new UI.tableInfoForm();
            t.sc = this;
            t.drTable = drTable;
            t.drTableColumns = drTableColumns;
            t.TableNameComment = tableNameComment;
            t.TableName = tableName;
            t.DataTable = dt;
            t.Show(Panel);
        }

        #region
        protected void setProgreesEditValue(int i)
        {
            if (MainContextMenu.InvokeRequired)
            {
                SimpleInt s = new SimpleInt(setProgreesEditValue);
                MainContextMenu.Invoke(s, new object[] { i });
            }
            else
            {
                tspbLoadDBProgress.Value = i;
                ;
            }

        }
        delegate void SimpleInt(int i);
        delegate void SimpleStr(string str);
        protected void setProgressMax(int i)
        {
            if (MainContextMenu.InvokeRequired)
            {
                SimpleInt s = new SimpleInt(setProgressMax);
                MainContextMenu.Invoke(s, new object[] { i });

            }
            else
            {
                tspbLoadDBProgress.Maximum = i;
            }

        }
        protected void setProgress(int i)
        {
            if (MainContextMenu.InvokeRequired)
            {
                SimpleInt s = new SimpleInt(setProgress);
                MainContextMenu.Invoke(s, new object[] { i });

            }
            else
            {
                if (i + (int)tspbLoadDBProgress.Value > tspbLoadDBProgress.Maximum)
                {
                    tspbLoadDBProgress.Value = tspbLoadDBProgress.Maximum;
                }
                else
                {
                    tspbLoadDBProgress.Value += i;
                };
            }

        }
        protected void setStatusBar(string str)
        {
            if (MainContextMenu.InvokeRequired)
            {
                SimpleStr s = new SimpleStr(setStatusBar);
                MainContextMenu.Invoke(s, new object[] { str });

            }
            else
            {
                tsslMessage.Text = str;

            }
        }

        private delegate void SimpleBool(bool flag);
        private void setEnable(bool flag)
        {
            if (MainContextMenu.InvokeRequired)
            {
                SimpleBool s = new SimpleBool(setEnable);
                MainContextMenu.Invoke(s, new object[] { flag });

            }
            else
            {
                tsiGen.Enabled = flag;
                tspbLoadDBProgress.Visible = !flag;
            }
        }
        #endregion


    }
}
