using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MDT.Tools.DB.Csharp_CodeGen.Plugin.Utils
{
    public class IbatisConfigHelper
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        public string GetClassName(string table)
        {
            string str = "";
            if (dic.ContainsKey(table))
            {
                str = dic[table];
            }
            return str;
        }

        public void ReadConfig(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList xn = doc.SelectNodes("abatorConfiguration/abatorContext/table");

            foreach (XmlNode xn1 in xn)
            {
                XmlElement xe = (XmlElement)xn1;
                string table = xe.GetAttribute("tableName") + "";
                string className = xe.GetAttribute("domainObjectName") + "";
                if (!dic.ContainsKey(table))
                {
                    dic.Add(table, className);
                }

            }
        }
    }
}
