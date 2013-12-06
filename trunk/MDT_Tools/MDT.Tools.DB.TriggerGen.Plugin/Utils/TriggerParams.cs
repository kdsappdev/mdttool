using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MDT.Tools.DB.TriggerGen.Plugin.Utils
{
   public class TriggerParams
    {
        private TriggerType trigger = TriggerType.None;
        private DataRow[] drTable;

        public TriggerType Trigger
        {
            get { return trigger; }
            set { trigger = value; }
        }       

        public DataRow[] DrTable
        {
            get { return drTable; }
            set { drTable = value; }
        }
    }
}
