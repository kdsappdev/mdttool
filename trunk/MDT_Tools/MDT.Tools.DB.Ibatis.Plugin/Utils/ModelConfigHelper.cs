using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MDT.Tools.DB.Ibatis.Plugin.Utils
{
    public class ModelConfigHelper
    {
        static readonly XmlDocument _doc=new XmlDocument();
        static readonly Dictionary<String, ModelConfig> dic = new Dictionary<string, ModelConfig>(); 
        public ModelConfigHelper(string path)
        {
            LoadXMLFromFile(path);
        }

        static ModelConfigHelper()
        {
            LoadXMLFromFile(ConstHelper.ModelConfigPath);
        }

        public static void LoadXMLFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }
            _doc.Load(path);
           XmlNodeList xmlNodeList= _doc.SelectNodes("/abatorConfiguration/abatorContext/table");
            if (xmlNodeList != null && xmlNodeList.Count > 0)
            {
                foreach (XmlNode node in xmlNodeList)
                {
                    if (node != null && node.Attributes != null)
                    {
                        ModelConfig temp = new ModelConfig();
                        if (node.Attributes["schema"] != null)
                            temp.Schema = node.Attributes["schema"].InnerText;
                        if (node.Attributes["tableName"] != null)
                            temp.TableName = node.Attributes["tableName"].InnerText;
                        if (node.Attributes["domainObjectName"] != null)
                            temp.DomainObjectName = node.Attributes["domainObjectName"].InnerText;
                        if (node.HasChildNodes)
                        {
                            if (temp.TableName == "OFR_GRPG_TBL")
                                Console.WriteLine("text");
                            foreach (XmlNode key in node.ChildNodes)
                            {
                                if (key != null && key.Attributes != null)
                                {
                                    if (key.Attributes["column"] != null)
                                        temp.GeneratedKey.Column = key.Attributes["column"].InnerText;
                                    if (key.Attributes["sqlStatement"] != null)
                                        temp.GeneratedKey.SqlStatement = key.Attributes["sqlStatement"].InnerText;
                                    if (key.Attributes["identity"] != null)
                                        temp.GeneratedKey.Identity =
                                            "true".Equals(key.Attributes["identity"].InnerText.ToLower());
                                }
                            }
                        }
                        if (dic.ContainsKey(temp.TableName))
                            dic.Remove(temp.TableName);
                        dic.Add(temp.TableName,temp);
                    }
                }
            }
        }

        public static bool IsExist(string key)
        {
            return dic.ContainsKey(key);
        }

        public static ModelConfig GetModelConfig(string key)
        {
            if (IsExist(key))
                return dic[key];
            return null;
        }
    }

    public class ModelConfig
    {
        public ModelConfig()
        {
            GeneratedKey=new GeneratedKey();
        }
        public string Schema { get; set; }
        public string TableName { get; set; }
        public string DomainObjectName { get; set; }
        public GeneratedKey GeneratedKey { get; set; }

        public bool HasGeneratedKey
        {
            get
            {
                return !string.IsNullOrEmpty(GeneratedKey.Column);
            }
        }
    }

    public class GeneratedKey
    {
        public string Column { get; set; }
        public string SqlStatement { get; set; }
        public bool Identity { get; set; }
    }
}