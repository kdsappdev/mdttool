/**
* 类功能说明：
* 公共功能类
*/

using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Aliyun.Upload.Plugin
{  
    #region XMLHelper    
    /// <summary>
    /// xml解析类
    /// </summary>
    public class XMLHelper
    {
        private XmlNode m_Root = null;
        private XmlDocument m_document = new XmlDocument();

        public XmlNode GetRoot()
        {
            if (m_document != null)
            {
                return m_document.DocumentElement;
            }
            return null;
        }

        /// <summary>
        /// Xml解析类构造函数
        /// </summary>
        /// <param name="strXml">Xml字符串，必须符合Xml语法(可以没声明)</param>
        public XMLHelper(string strXml)
        {
            LoadXmlStr(strXml);
        }

        /// <summary>
        /// 加载Xml串
        /// </summary>
        /// <param name="strXml">Xml串</param>
        private void LoadXmlStr(String strXml)
        {
            if (!String.IsNullOrEmpty(strXml))
            {
                try
                {
                    m_document.LoadXml(strXml);
                    m_Root = m_document.DocumentElement;
                }
                catch (Exception e)
                {
                    LogHelper.Error( e);
                }
            }
        }

        /// <summary>
        /// 加载Xml文件
        /// </summary>
        /// <param name="strXml">Xml文件路径</param>
        private void LoadXmlFile(String strPath)
        {
            if (!String.IsNullOrEmpty(strPath))
            {
                try
                {
                    m_document.Load(strPath);
                    m_Root = m_document.DocumentElement;
                }
                catch (Exception e)
                {
                    LogHelper.Error(e);
                }
            }
        }

        /// <summary>
        /// 获取某个特定节点
        /// </summary>
        /// <param name="xPath">节点路径</param>
        /// <returns>返回节点，同C#</returns>
        public XmlNode GetNode(string xPath)
        {
            try
            {
                return m_Root.SelectSingleNode(xPath);
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                return null;
            }
        }

        /// <summary>
        /// 获取相同的节点列表
        /// </summary>
        /// <param name="xPath">节点路径</param>
        /// <returns>返回节点，同C#</returns>
        public XmlNodeList GetNodeList(string xPath)
        {
            try
            {
                return m_Root.SelectNodes(xPath);
            }
            catch (Exception e)
            {
               LogHelper.Error(e);
                return null;
            }
        }

        /// <summary>
        /// 获取相对根节点的某个节点值
        /// </summary>
        /// <param name="xPath">路径</param>
        /// <returns>String，不存在返回""</returns>
        public String GetNodeInnerText(string xPath)
        {
            XmlNode node = GetNode(xPath);
            if (node == null)
            {
                return "";
            }
            return node.InnerText;
        }

        /// <summary>
        /// 获取相对根节点的某个节点的xml值
        /// </summary>
        /// <param name="xPath">路径</param>
        /// <returns>String，不存在返回""</returns>
        public String GetNodeOuterXml(string xPath)
        {
            XmlNode node = GetNode(xPath);
            if (node == null)
            {
                return "";
            }
            return node.OuterXml;
        }


        /// <summary>
        /// 获取node节点下某项的值
        /// </summary>
        /// <returns>Decimal，不存在返回0.00</returns>
        public static decimal GetNodeValueToDecimal(XmlNode root, string nodeName)
        {
            XmlNode nodeTemp = null;
            try
            {
                nodeTemp = root.SelectSingleNode(nodeName.Trim());
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                return new decimal(0);
            }

            if (nodeTemp == null)
            {
                return new decimal(0);
            }

            return Decimal.Parse(String.IsNullOrEmpty(nodeTemp.InnerText) ? "0" : nodeTemp.InnerText);
        }

        /// <summary>
        /// 获取node节点下某项的值
        /// </summary>
        /// <returns>[eg: 100,000.00]</returns>
        public static String GetNodeValueToDecimalA(XmlNode root, string nodeName)
        {
            return String.Format("{0:n}", GetNodeValueToDecimal(root, nodeName));
        }

        /// <summary>
        /// 获取node节点下某项的值
        /// </summary>
        /// <returns>[eg: 100 000.00]</returns>
        public static String GetNodeValueToDecimalB(XmlNode root, string nodeName)
        {
            return String.Format("{0:n}", GetNodeValueToDecimalA(root, nodeName)).Replace(',', ' ');
        }

        /// <summary>
        ///  获取node节点下某项的值
        /// </summary>
        /// <returns>String，不存在返回""</returns>
        public static string GetNodeValueToString(XmlNode root, string nodeName)
        {
            XmlNode nodeTemp = null;
            try
            {
                nodeTemp = root.SelectSingleNode(nodeName.Trim());
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                return null;
            }

            if (nodeTemp == null)
            {
                return "";
            }
            return nodeTemp.InnerText;
        }
    }

    #endregion

    /// <summary>
    /// xml序列化，反序列化类
    /// </summary> 
    #region XmlSerialize
    public class XmlSerialize
    {
        /// <summary>
        /// 对象序列化为xml字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {

            StringWriter writer = new StringWriter();
            XmlSerializer s = new XmlSerializer(obj.GetType());
            s.Serialize(writer, obj);
            string str = writer.ToString();
            writer.Close();
            return str;
        }

        /// <summary>
        /// 对象序列化到文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="file"></param>
        public static void SerializeFile(object obj, string file)
        {
            StreamWriter writer = new StreamWriter(file);
            XmlSerializer s = new XmlSerializer(obj.GetType());
            s.Serialize(writer, obj);
            writer.Close();
        }

        /// <summary>
        /// xml串序列化到对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            StringReader reader = new StringReader(xml);
            XmlSerializer s = new XmlSerializer(type);
            object obj = null;
            try
            {
                obj = s.Deserialize(reader);
            }
            catch (Exception e)
            {
                //LogHelper.Error(e);
            }
            reader.Close();
            return obj;
        }

        /// <summary>
        /// xml文件列化到对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static object DeserializeFile(Type type, string file)
        {
            StreamReader reader = new StreamReader(file);
            XmlSerializer s = new XmlSerializer(type);
            object obj = null;
            try
            {
                obj = s.Deserialize(reader);
            }
            catch (Exception e)
            {
               // LogHelper.Error(e);
            }
            reader.Close();
            return obj;
        }
    }

    #endregion

    /// <summary>
    /// Xml转化为TreeNode，XML显示类
    /// </summary>
    #region TreeNodeFormat
    public static class TreeNodeFormat
    {
        private static XmlNode m_Root = null;
        private static TreeNode m_TreeNode = new TreeNode();

        /// <summary>
        /// xml转换到TreeNode
        /// </summary>
        /// <param name="strXml">Xml串</param>
        /// <returns>TreeNode</returns>
        public static TreeNode FormatTreeNode(string strXml)
        {
            if (!String.IsNullOrEmpty(strXml))
            {
                try
                {
                    XmlDocument m_document = new XmlDocument();
                    m_document.LoadXml(strXml);
                    m_Root = m_document.DocumentElement;
                }
                catch (Exception e)
                {
                    LogHelper.Error( e);
                }
                m_TreeNode = new TreeNode();

                treeNode(m_Root, m_TreeNode);
            }

            return m_TreeNode;
        }

        /// <summary>
        /// xml递归遍历
        /// </summary>
        /// <param name="node">xml节点</param>
        /// <param name="rootnode">tree节点</param>
        private static void treeNode(XmlNode node, TreeNode rootnode)
        {
            if (node.ChildNodes == null || node.ChildNodes.Count == 0)
            {
                return;
            }
            String str = node.FirstChild.GetType().Name;
            if (node.ChildNodes.Count == 1 && node.FirstChild.GetType().Name.Equals("XmlText"))
            {
                rootnode.Nodes.Add(node.OuterXml);
            }
            else
            {
                TreeNode temp1 = new TreeNode("<" + node.Name + ">");
                TreeNode temp2 = new TreeNode("</" + node.Name + ">");

                rootnode.Nodes.Add(temp1);
                rootnode.Nodes.Add(temp2);

                foreach (XmlNode n in node.ChildNodes)
                {
                    treeNode(n, temp1);
                }
            }
        }

        /// <summary>
        ///  TreeNode转换到xml
        /// </summary>
        /// <param name="tree">TreeNode</param>
        /// <returns>xml串</returns>
        public static String FormatXml(TreeNode tree)
        {
            return "";
        }
    }
    #endregion
}
 