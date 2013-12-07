using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using MDT.Tools.Core.UI;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.SetComment.Plugin.UI;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.DB.Common;
namespace MDT.Tools.DB.SetComment.Plugin
{
    /// <summary>
    /// 设置备注
    /// </summary>
    internal class setComment:AbstractHandler
    {
        
        public DataRow[] DrTables;
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                base.process(drTables, dsTableColumns, dsTablePrimaryKeys);
                setStatusBar("");
                setEnable(false);
                foreach (var tableInfo in tableInfos)
                {
                    set(tableInfo);
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

        public void set(TableInfo tableInfo)
        {
            tableInfoForm t = new tableInfoForm();
            t.sc = this;
            t.TableInfo = tableInfo;
            
            t.Show(Panel);
        }       
    }
}
