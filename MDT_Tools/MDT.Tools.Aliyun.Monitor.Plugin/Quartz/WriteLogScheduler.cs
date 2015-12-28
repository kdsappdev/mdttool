using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.Core.Log;
using MDT.Tools.Aliyun.Monitor.Plugin.Monitor;
using System.Collections.ObjectModel;
using Quartz;
using Quartz.Impl;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Aliyun.Monitor.Plugin.Quartz
{
    public  class WriteLogScheduler
    {
        static IScheduler sched = null;
        static int number = 0;
        public static void init()
        {
            try
            {
                if (sched == null)
                {
                    ISchedulerFactory sf = new StdSchedulerFactory();
                    sched = sf.GetScheduler();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }



        public static void Add(string VKInterval, MonitorUI mu, string jobName)
        {
            try
            {
                JobDataMap jd = new JobDataMap();
                number++;
                jd.Put("Monitor", mu);
                JobDetail job1 = new JobDetail(jobName, jobName, typeof(MonitorJob));
                job1.JobDataMap = jd;
                if (!string.IsNullOrEmpty(VKInterval))
                {
                    CronTrigger trigger1 = new CronTrigger(jobName, jobName, jobName, jobName,
                                                           VKInterval);
                    sched.ScheduleJob(job1, trigger1);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }



        public static void AddDayTiming(string DayTiming, MonitorUI mu, string jobName)
        {
            try
            {
                number++;
                JobDataMap jd = new JobDataMap();
                jd.Put("Monitor", mu);
                JobDetail job1 = new JobDetail(jobName, jobName, typeof(QuartzJob));
                job1.JobDataMap = jd;
                if (!string.IsNullOrEmpty(DayTiming))
                {
                    CronTrigger trigger1 = new CronTrigger(jobName, jobName, jobName, jobName,
                                                           DayTiming);
                    sched.ScheduleJob(job1, trigger1);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }

      
        public static  bool deleteJob(string jobName, string groupName)
        {
            bool status = false;
            if (!sched.IsShutdown)
            {
                status = sched.DeleteJob(jobName, groupName);
                if (number <= 1)
                {
                    sched.Shutdown();
                    sched = null;
                }
            }
            number--;
            return status;
        }

        public static void Start()
        {
            try
            {
                sched.Start();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }





        public static bool IsStart()
        {
            return sched.IsStarted;
        }
        public static void Stop()
        {
            try
            {
                if (sched != null)
                {
                    sched.Shutdown();
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }


    }
}
