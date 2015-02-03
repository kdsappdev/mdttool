using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Aliyun.Upload.Plugin.Download
{
    public class OssSummary
    {
        public string fileName { get; set; }
        public long Size { get;  set; }
        public DateTime LastModified { get; set; }
    }
}
