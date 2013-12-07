﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.DB.Csharp_CodeGen.Plugin.Utils
{
    internal class FilePathHelper
    {
        private FilePathHelper()
        { }
        public static readonly string SystemConfig = Application.StartupPath + "\\plugin\\MDT.Tools.DB.Csharp_CodeGen.Plugin\\config.ini";
        public static readonly string ExportCsharpModelPath = Application.StartupPath + "\\plugin\\MDT.Tools.DB.Csharp_CodeGen.Plugin\\data\\";
        public static readonly string TemplatesPath = Application.StartupPath + "\\plugin\\MDT.Tools.DB.Csharp_CodeGen.Plugin\\templates\\";
    }
}