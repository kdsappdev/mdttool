using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDT.Tools.Core.Utils
{
   public class DataTableHelper
    {
        public static void CompareDt(DataTable dt1, DataTable dt2, string[] keyFields,
             out DataTable dtRetAdd, out DataTable dtRetDif1, out DataTable dtRetDif2,
             out DataTable dtRetDel)
        {
            //为三个表拷贝表结构
            dtRetDel = dt1.Clone();
            dtRetAdd = dtRetDel.Clone();
            dtRetDif1 = dtRetDel.Clone();
            dtRetDif2 = dtRetDel.Clone();

            

            DataView dv1 = dt1.DefaultView;
            DataView dv2 = dt2.DefaultView;

            //先以第一个表为参照，看第二个表是修改了还是删除了
            foreach (DataRowView dr1 in dv1)
            {
                StringBuilder sb=new StringBuilder();
                foreach (string key in keyFields)
                {
                    sb.AppendFormat("{0}='{1}'", key, dr1[key]).Append(" And ");
                }
                dv2.RowFilter = sb.ToString().Substring(0, sb.ToString().Length - 5);
                if (dv2.Count > 0)
                {
                    if (!CompareUpdate(dr1, dv2[0]))//比较是否有不同的
                    {
                        dtRetDif1.Rows.Add(dr1.Row.ItemArray);//修改前
                        dtRetDif2.Rows.Add(dv2[0].Row.ItemArray);//修改后
                        
                        continue;
                    }
                }
                else
                {
                    //已经被删除的
                    dtRetDel.Rows.Add(dr1.Row.ItemArray);
                }
            }

            //以第一个表为参照，看记录是否是新增的
            dv2.RowFilter = "";//清空条件
            foreach (DataRowView dr2 in dv2)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in keyFields)
                {
                    sb.AppendFormat("{0}='{1}'", key, dr2[key]).Append(" And ");
                }
                dv1.RowFilter = sb.ToString().Substring(0, sb.ToString().Length - 5);
                if (dv1.Count == 0)
                {
                    //新增的
                    dtRetAdd.Rows.Add(dr2.Row.ItemArray);
                }
            }
        }

        //比较是否有不同的
        private static bool CompareUpdate(DataRowView dr1, DataRowView dr2)
        {
            //行里只要有一项不一样，整个行就不一样,无需比较其它
            object val1;
            object val2;
            for (int i = 1; i < dr1.Row.ItemArray.Length; i++)
            {
                val1 = dr1[i];
                val2 = dr2[i];
                if (!val1.Equals(val2))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
