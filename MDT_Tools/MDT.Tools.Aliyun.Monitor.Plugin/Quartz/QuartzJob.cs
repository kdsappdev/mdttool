using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Threading;
using MDT.Tools.Aliyun.Monitor.Plugin.Monitor;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Aliyun.Monitor.Plugin.Quartz
{
    public class QuartzJob:IJob
    {
        public void Execute(JobExecutionContext context)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    lock (context)
                    {
                        object mo = context.JobDetail.JobDataMap.Get("Monitor");
                        MonitorUI mu = (MonitorUI)mo;
                        mu.init();
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
        }
    }
}
