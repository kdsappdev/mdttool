using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Aliyun.Monitor.Plugin.Model
{
    public class LoadInfo
    {
        public string bucketName { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string MonitorFolder { get; set; }
        public string MonitorName { get; set; }
    }
}
