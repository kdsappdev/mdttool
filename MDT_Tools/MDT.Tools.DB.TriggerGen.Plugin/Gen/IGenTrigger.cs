using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.DB.Common;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.TriggerGen.Plugin.UI;
using System.Windows.Forms;
using System.Threading;

namespace MDT.Tools.DB.TriggerGen.Plugin.Gen
{
    public abstract class IGenTrigger : AbstractHandler
    {
        protected string TriggerName
        {
            get;
            set;
        }

        protected void display(string context ,string text)
        {
            FileHelper.Write(".\\data\\trigger\\" + TriggerName + ".sql", new string[] { context });
            ShowForm(context,text);
        }

        private void ShowForm(string context,string text)
        {
            ExecuteSql execute = new ExecuteSql(context,text);
            execute.gt = this;
            execute.Show(Panel);
        }
    }
}
