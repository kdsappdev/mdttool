using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Template.Plugin.Utils
{
    internal class FilePathHelper
    {
        private FilePathHelper()
        { }
        public static readonly string SystemConfig = Application.StartupPath + "\\control\\templateconfig.ini";
        public static readonly string ExportTemplatePath = Application.StartupPath + "\\data\\Template\\";
        public static readonly string TemplatesPath = Application.StartupPath + "\\templates\\";
    
    }
}
