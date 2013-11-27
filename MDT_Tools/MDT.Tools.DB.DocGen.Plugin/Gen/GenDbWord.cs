using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using MDT.Tools.Core.UI;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.DocGen.Plugin.Utils;
using Word;

namespace MDT.Tools.DB.DocGen.Plugin.Gen
{
    public class GenDbWord
    {
        public string dbName;
        public string dbType;
        public ToolStripStatusLabel tsslMessage;
        public ToolStripProgressBar tspbLoadDBProgress;
        public string DBtable;
        public string DBtablesColumns;
        public string DBviews;
        public string DBtablesPrimaryKeys;
        public DataSet dsTableColumn;
        public DataSet dsTablePrimaryKey;
        public ContextMenuStrip MainContextMenu;
        public ToolStripItem tsiDocGen;
        public Encoding OriginalEncoding;
        public Encoding TargetEncoding;
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
                tsiDocGen.Enabled = flag;
                tspbLoadDBProgress.Visible = !flag;
            }
        }

        public void GenCode(System.Data.DataRow[] drTables, System.Data.DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            setEnable(false);
            string msg = string.Empty;
            Stopwatch sw = Stopwatch.StartNew();
            FileHelper.DeleteDirectory(FilePathHelper.ExportDBDocPath);
            if (drTables != null && dsTableColumns != null && dsTablePrimaryKeys != null)
            {

                setStatusBar(string.Format("正在生成{0}数据库文档", dbName));
                setProgreesEditValue(0);
                setProgress(0);
                setProgressMax(drTables.Length);

                string path = FilePathHelper.ExportDBDocPath + dbName + "_DB_{0}_{1}" + ".doc";

                FileHelper.CreateDirectory(path);
                Object Nothing = System.Reflection.Missing.Value;
                Word.Application dbWordApp = new Word.Application();
                dbWordApp.NormalTemplate.Saved = true;

                Word.Document dbDoc = dbWordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                dbWordApp.ActiveWindow.View.Type = WdViewType.wdOutlineView;
                dbWordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryHeader;
                dbWordApp.ActiveWindow.ActivePane.Selection.InsertAfter(string.Format("{0} {1}数据库文档", dbName, dbType));
                dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                dbWordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument;
                string tableName = "";
                int begin = 0;
                int end = 0;
                #region
                try
                {

                    for (int j = 0; j < drTables.Length; j++)
                    {
                        try
                        {
                            if ((j + 1) % 10 == 0 || (j + 1).Equals(drTables.Length))
                            {
                                begin = end;
                                end = j;
                            }
                            #region 样式
                            DataRow drTable = drTables[j];

                            DataRow[] drTableColumns = dsTableColumns.Tables[dbName + DBtablesColumns].Select("TABLE_NAME = '" + drTable["name"].ToString() + "'", "COLUMN_ID ASC");

                            tableName = drTable["name"] as string;//表名
                            string tableComments = drTable["comments"] as string;//表说明
                            setStatusBar(string.Format("正在生成{0}中{1}表信息,共{2}张表，已生成了{3}张表", dbName, tableName, drTables.Length, j));
                            dbWordApp.Selection.ParagraphFormat.LineSpacing = 15f;
                            object count = 14;
                            object WdLine = Word.WdUnits.wdLine;
                            dbWordApp.Selection.MoveDown(ref WdLine, ref count, ref Nothing);
                            dbWordApp.Selection.TypeParagraph();
                            dbWordApp.Selection.TypeText("表" + Convert.ToString(j + 1) + ":" + tableName + "(" + tableComments + ")");


                            Word.Table newTable = dbDoc.Tables.Add(dbWordApp.Selection.Range, drTableColumns.Length + 2, 7, ref Nothing, ref Nothing);

                            newTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            newTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                            newTable.Columns[1].Width = 110f;  //列明
                            newTable.Columns[2].Width = 90f; //数据类型
                            newTable.Columns[3].Width = 50f; //是否NULL
                            newTable.Columns[4].Width = 40f; //默认值
                            newTable.Columns[5].Width = 30f;  //主键
                            newTable.Columns[6].Width = 30f;  //外键
                            newTable.Columns[7].Width = 130f;  //列说明
                            #endregion

                            #region 填充表名称(表格内)
                            newTable.Cell(1, 1).Range.Text = "表名：" + tableName + "(" + tableComments + ")";
                            newTable.Cell(1, 1).Range.Bold = 2;//设置单元格中字体为粗体
                            //合并单元格
                            newTable.Cell(1, 1).Merge(newTable.Cell(1, 7));
                            newTable.Cell(1, 1).Range.Font.Color = Word.WdColor.wdColorRed;//设置单元格内字体颜色
                            newTable.Cell(1, 1).Select();   //选中
                            dbWordApp.Selection.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;//垂直居中
                            dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中
                            #endregion

                            #region 填充列标题
                            newTable.Cell(2, 1).Range.Text = "列名";
                            newTable.Cell(2, 1).Range.Font.Color = Word.WdColor.wdColorBlack;//设置单元格内字体颜色
                            newTable.Cell(2, 2).Range.Font.Bold = 2;
                            newTable.Cell(2, 2).Range.Font.Size = 9;
                            newTable.Cell(2, 2).Select();   //选中
                            newTable.Cell(2, 2).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中

                            newTable.Cell(2, 2).Range.Text = "数据类型";
                            newTable.Cell(2, 2).Range.Font.Color = Word.WdColor.wdColorBlack;//设置单元格内字体颜色
                            newTable.Cell(2, 2).Range.Font.Bold = 2;
                            newTable.Cell(2, 2).Range.Font.Size = 9;
                            newTable.Cell(2, 2).Select();   //选中
                            newTable.Cell(2, 2).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中

                            newTable.Cell(2, 3).Range.Text = "是否NULL";
                            newTable.Cell(2, 3).Range.Font.Color = Word.WdColor.wdColorBlack;//设置单元格内字体颜色
                            newTable.Cell(2, 3).Range.Font.Bold = 2;
                            newTable.Cell(2, 3).Range.Font.Size = 9;
                            newTable.Cell(2, 3).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            newTable.Cell(2, 3).Select();   //选中
                            dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中

                            newTable.Cell(2, 4).Range.Text = "默认值";
                            newTable.Cell(2, 4).Range.Font.Color = Word.WdColor.wdColorBlack;//设置单元格内字体颜色
                            newTable.Cell(2, 4).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            newTable.Cell(2, 4).Range.Font.Bold = 2;
                            newTable.Cell(2, 4).Range.Font.Size = 9;
                            newTable.Cell(2, 4).Select();   //选中
                            dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中

                            newTable.Cell(2, 5).Range.Text = "主键";
                            newTable.Cell(2, 5).Range.Font.Color = Word.WdColor.wdColorBlack;//设置单元格内字体颜色
                            newTable.Cell(2, 5).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            newTable.Cell(2, 5).Range.Font.Bold = 2;
                            newTable.Cell(2, 5).Range.Font.Size = 9;
                            newTable.Cell(2, 5).Select();   //选中
                            dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中

                            newTable.Cell(2, 6).Range.Text = "外键";
                            newTable.Cell(2, 6).Range.Font.Color = Word.WdColor.wdColorBlack;//设置单元格内字体颜色
                            newTable.Cell(2, 6).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            newTable.Cell(2, 6).Range.Font.Bold = 2;
                            newTable.Cell(2, 6).Range.Font.Size = 9;
                            newTable.Cell(2, 6).Select();   //选中
                            dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中

                            newTable.Cell(2, 7).Range.Text = "说明";
                            newTable.Cell(2, 7).Range.Font.Color = Word.WdColor.wdColorBlack;//设置单元格内字体颜色
                            newTable.Cell(2, 7).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            newTable.Cell(2, 7).Range.Font.Bold = 2;
                            newTable.Cell(2, 7).Range.Font.Size = 9;
                            newTable.Cell(2, 7).Select();   //选中
                            dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中
                            #endregion

                            #region 填充表格内容
                            for (int i = 0; i < drTableColumns.Length; i++)
                            {

                                string tableColumnName = drTableColumns[i]["COLUMN_NAME"] as string;//列名

                                string tableColumnComments = drTableColumns[i]["COMMENTS"] as string;//列说明

                                string tableColumnDataType = drTableColumns[i]["DATA_TYPE"] as string;//列类型

                                string tableColumnDataScale = drTableColumns[i]["DATA_SCALE"] + "";//列精度

                                string tableColumnDataLength = drTableColumns[i]["DATA_LENGTH"] + "";//列长度

                                string tableColumnDataNullAble = drTableColumns[i]["NULLABLE"] as string;//可空

                                string tableColumnDataDefault = drTableColumns[i]["DATA_DEFAULT"] as string;//默认值

                                string tableColumnDataPrecision = drTableColumns[i]["Data_Precision"] + "";

                                if (string.IsNullOrEmpty(tableColumnDataPrecision)) //字符串、日期等
                                {
                                    if ("DATE".Equals(tableColumnDataType))
                                    {
                                        tableColumnDataType = tableColumnDataType + "()";
                                    }
                                    else
                                    {
                                        tableColumnDataType = tableColumnDataType + "(" + tableColumnDataLength + ")";
                                    }
                                }
                                else //数字类型等
                                {
                                    if ("0".Equals(tableColumnDataScale))
                                    {
                                        tableColumnDataType = tableColumnDataType + "(" + tableColumnDataPrecision + ")";
                                    }
                                    else
                                    {
                                        tableColumnDataType = tableColumnDataType + "(" + tableColumnDataPrecision + "," + tableColumnDataScale + ")";
                                    }
                                }

                                //列名
                                newTable.Cell(i + 3, 1).Range.Text = tableColumnName;
                                newTable.Cell(i + 3, 1).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                                //数据类型
                                newTable.Cell(i + 3, 2).Range.Text = tableColumnDataType;
                                newTable.Cell(i + 3, 2).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;


                                //是否Null
                                newTable.Cell(i + 3, 3).Range.Text = tableColumnDataNullAble;
                                newTable.Cell(i + 3, 3).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                newTable.Cell(i + 3, 3).Select();
                                dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中

                                //默认值
                                newTable.Cell(i + 3, 4).Range.Text = tableColumnDataDefault;
                                newTable.Cell(i + 3, 4).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                newTable.Cell(i + 3, 4).Select();
                                dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中


                                //主键

                                newTable.Cell(i + 3, 5).Range.Text = "";

                                DataRow[] dr = dsTablePrimaryKeys.Tables[dbName + DBtablesPrimaryKeys].Select("TABLE_NAME = '" + tableName + "' AND COLUMN_NAME ='" + tableColumnName + "' and constraint_type='P'");

                                if (dr != null && dr.Length > 0)
                                {
                                    newTable.Cell(i + 3, 5).Range.Text = "Y";
                                }
                                newTable.Cell(i + 3, 5).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                newTable.Cell(i + 3, 5).Select();
                                dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中

                                //外键

                                newTable.Cell(i + 3, 6).Range.Text = "";
                                DataRow[] dr2 = dsTablePrimaryKeys.Tables[dbName + DBtablesPrimaryKeys].Select("TABLE_NAME = '" + tableName + "' AND COLUMN_NAME ='" + tableColumnName + "' and constraint_type='R'");
                                if (dr2 != null && dr2.Length > 0)
                                {
                                    newTable.Cell(i + 3, 6).Range.Text = "Y";
                                }
                                newTable.Cell(i + 3, 6).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                newTable.Cell(i + 3, 6).Select();
                                dbWordApp.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;//水平居中


                                //说明
                                newTable.Cell(i + 3, 7).Range.Text = EncodingHelper.ConvertEncoder(OriginalEncoding, TargetEncoding, tableColumnComments);
                                newTable.Cell(i + 3, 7).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                            }

                            #endregion


                            setProgress(1);
                        }
                        catch (Exception ex)
                        {
                            msg = ex.Message;
                            string str = (string.Format("{0}生成数据库文档丢失{1}表", dbName, tableName));
                            setStatusBar(str);
                            LogHelper.Error(ex);
                            LogHelper.Debug(str);
                        }
                        finally
                        {
                            #region 释放资源
                            if ((j + 1) % 10 == 0 || (j + 1).Equals(drTables.Length))
                            {
                                try
                                {
                                    object fileName = string.Format(path, begin, end);
                                    dbDoc.SaveAs(ref fileName, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                                }
                                catch (Exception ex)
                                {
                                    msg = ex.Message;
                                    LogHelper.Error(ex);
                                }
                                try
                                {
                                    dbDoc.Close(ref Nothing, ref Nothing, ref Nothing);
                                }
                                catch
                                { }
                                if (dbDoc != null)
                                {
                                    try
                                    {
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(dbDoc);
                                    }
                                    catch
                                    { }
                                    dbDoc = null;
                                }
                                GC.Collect();
                                if (!(j + 1).Equals(drTables.Length))
                                {
                                    dbDoc = dbWordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                                }
                            }
                            #endregion
                        }
                    }

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    LogHelper.Error(ex);
                }
                finally
                {
                    #region 释放资源

                    try
                    {
                        dbWordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
                    }
                    catch
                    { }
                    if (dbWordApp != null)
                    {
                        try
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(dbWordApp);
                        }
                        catch
                        { }
                        dbWordApp = null;
                    }
                    GC.Collect();


                    #endregion
                }
                #endregion
            }
            sw.Stop();

            if (string.IsNullOrEmpty(msg))
            {
                setStatusBar(string.Format("{0}数据库文档生成成功", dbName));
                openPath(FilePathHelper.ExportDBDocPath);
            }
            else
            {
                setStatusBar(string.Format("{0}数据库文档生成失败[{1}]", dbName, msg));
            }
            setEnable(true);
        }

        private void openPath(string path)
        {
            if (MainContextMenu.InvokeRequired)
            {
                SimpleStr s = new SimpleStr(openPath);
                MainContextMenu.Invoke(s, new object[] { path });
            }
            else
            {
                DialogResult result = MessageBox.Show(MainContextMenu,string.Format("{0}数据库文档保存成功,是否要打开文档保存目录.", dbName), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result.Equals(DialogResult.Yes))
                {
                    Process.Start("Explorer.exe", path);
                }
            }
        }
    }
}
