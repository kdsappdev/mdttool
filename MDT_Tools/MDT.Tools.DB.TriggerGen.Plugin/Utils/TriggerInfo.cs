using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.DB.TriggerGen.Plugin.Utils
{
   public class TriggerInfo
    {
        public const  string YukonTriggerName = "TG_$TABLENAME";

        public const string MQProdcedureName = "SP_STP_$TABLENAME";
        public const string MQTriggerName = "TG_STP_IN_$TABLENAME";      
        public const string MQTopicName = "'ATS.' || $LOCATION || '.$TABLENAME.CHANGED'";

        
      
    }
}
