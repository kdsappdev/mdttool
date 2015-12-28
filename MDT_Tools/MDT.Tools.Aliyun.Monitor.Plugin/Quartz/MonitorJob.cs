using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.Core.Utils;
using MDT.Tools.Aliyun.Monitor.Plugin.Monitor;
using System.Threading;
using Quartz;

namespace MDT.Tools.Aliyun.Monitor.Plugin.Quartz
{
    public class MonitorJob : IJob
    {
        #region IJob 成员

        public void Execute(JobExecutionContext context)
        {

            try
            {
                lock (context)
                {
                    object mo = context.JobDetail.JobDataMap.Get("Monitor");
                    MonitorUI mu = (MonitorUI)mo;
                    mu.ConnectOssClient();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }

        }

        #endregion
    }
}
