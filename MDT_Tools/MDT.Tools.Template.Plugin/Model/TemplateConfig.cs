using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Template.Plugin.Model
{
    public class TemplateConfig
    {
        public int TemplateNum { get; set; }
        private  List<TemplateParas> _templateParas=new List<TemplateParas>();
        public List<TemplateParas> TemplateParas
        {
            get { return _templateParas; }
            set { _templateParas = value; }
        }
    }

    public class TemplateParas
    {
        public string MenuName { get; set; }
        public string DataTye { get; set; }
        public string TemplateName { get; set; }
        public string SaveFileEncoding { get; set; }
        public string SaveFilePath { get; set; }
        public string SaveFileName { get; set; }
        public bool IsAutoGenSaveFileName { get; set; }
        public bool IsShowGenCode { get; set; }
        public string CodeLanguage { get; set; }
    }
}
