using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Aliyun.Monitor.Plugin.Model
{
    public class MFileInfo
    {
        public Int64 SeqNo { get; set; }
        public string FileName { get; set; }
        public string Size { get; set; }
        public DateTime LastModified { get; set; }
        public string MonitorName { get; set; }
        public string BucketName { get; set; }
        public string Status { get; set; }
    }
}
