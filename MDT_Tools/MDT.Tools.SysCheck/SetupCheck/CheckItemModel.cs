using System;
using System.Collections.Generic;
 
using System.Text;

namespace MDT.Tools.SysCheck.SetupCheck
{
    public class CheckItemModel
    {
        public string Name { get; set; }
        public string IsInstall { get; set; }
        public string CheckResult { get; set; }
        public string IsOk { get; set; }
        public string AResult { get; set; }
        public string Ip { get; set; }
        public string Delay { get; set; }
        public override string ToString()
        {
            return Ip + Name + "\t" + IsInstall + "\t" + CheckResult + Delay + "\t" + IsOk + "\t" + AResult;
        }
    }
}
