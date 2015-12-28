using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.Aliyun.Monitor.Plugin.Model;
using System.Data;

namespace MDT.Tools.Aliyun.Monitor.Plugin.Monitor
{
    public interface IFilesHelper
    {
        DataTable load(string mNmae, string bName);
        DataTable select();
        int insert(MFileInfo fi);
        string queryMaxId(string Column,string MonitorName, string BucketName);
        void init();
        void Start();
        void Stop();
        void Add(string VKInterval, MonitorUI mu, string name);
        void deleteJob(string jobName);
        int update(string seqNo);
        void AddDayTiming(string DayTiming, MonitorUI mu, string jobName);
        DataTable selectParam(string bName, string mNmae, string FileName);
    }
}
