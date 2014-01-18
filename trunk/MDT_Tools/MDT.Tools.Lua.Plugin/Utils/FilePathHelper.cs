using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Lua.Plugin.Utils
{
    internal class FilePathHelper
    {
        private FilePathHelper()
        { }
       
        public static readonly string LuaScriptPath = Application.StartupPath + "\\script\\";
    }
}
