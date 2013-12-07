using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.DB.SetComment.Plugin
{
    internal class FilePathHelper
    {
        private FilePathHelper()
        { }
        public static readonly string TemplatesPath = Application.StartupPath + "\\plugin\\MDT.Tools.DB.SetComment.Plugin\\templates\\";
    
    }
}
