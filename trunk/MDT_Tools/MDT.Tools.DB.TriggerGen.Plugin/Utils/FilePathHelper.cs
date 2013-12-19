using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.DB.TriggerGen.Plugin.Utils
{
    internal class FilePathHelper
    {
        private FilePathHelper()
        { }
        public static readonly string TemplatesPath = Application.StartupPath + "\\templates\\";
        //public static readonly string TemplatesPath = @"D:\xyb\svn\mdttool\MDT_Tools\MDT.Tools.DB.TriggerGen.Plugin\templates";
    }
}
