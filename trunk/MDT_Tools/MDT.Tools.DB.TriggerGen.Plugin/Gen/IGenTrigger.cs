using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.DB.Common;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.DB.TriggerGen.Plugin.Gen
{
   public abstract class IGenTrigger : AbstractHandler
    {
       protected string TriggerName
       {
           get;
           set;
       }

       protected void display(string context)
       {
           CodeShow(TriggerName, context);
           FileHelper.Write(".\\data\\trigger\\" + TriggerName + ".sql", new string[] { context });
       }
    }
}
