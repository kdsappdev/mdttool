using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MDT.Tools.Uninstall
{
    class Program
    {
        static void Main(string[] args)
        {
            string productCode = "{E4134388-32C9-4510-B700-53444CC7455E}";

            Process.Start("bin\\msiexec.exe", string.Format("/x {0}", productCode));
                
        }
    }
}
