using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace MDT.Tools.DB.TriggerGen.Plugin.Gen
{
   public class GenNone:IGenTrigger
    {
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            MessageBox.Show("不能生成此类型的触发器！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
