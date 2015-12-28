using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.SolutionDesign.Plugin.Utils
{
    public class StaticText
    {
        public static readonly string RootNodeName = "解决方案";

        public static class ToolStripName
        {
            public static readonly string Add = "新建";
            public static readonly string Delete = "删除";
            public static readonly string Export = "导入";
            public static readonly string Save = "保存";
        }

        public static class FilePath
        {
            public static readonly string Directory = "\\data";
            public static readonly string FileName = "\\solution.xml";
        }
    }
}
