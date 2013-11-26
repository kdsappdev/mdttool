using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.DB.Csharp_Model.Plugin.Utils
{
    internal class FilePathHelper
    {
        private FilePathHelper()
        { }

        public static readonly string ExportCsharpModelPath = Application.StartupPath + "\\MDT.Tools.DB.Csharp_ModelGen.Plugin\\data\\";   
    }
}
