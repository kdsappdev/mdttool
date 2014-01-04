using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Fix.Plugin.UI;
using MDT.Tools.Fix.Plugin.Utils;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.Fix.Common.Model;
namespace MDT.Tools.Fix.Plugin
{
    public class FixPlugin : AbstractPlugin
    {
        #region 插件信息
        private int _tag = 21;
        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 21; }
        }

        public override string PluginName
        {
            get { return "Fix信息插件"; }
        }

        public override string Description
        {
            get { return "加载Fix Xml信息"; }
        }

        public override string Author
        {
            get { return "孔德帅"; }
        }
        #endregion

        #region 字段
        private FixExplorer _explorer = null;
        private MenuStrip _mainTool = null;
        private TreeView _tvFix = new TreeView();
        private ContextMenuStrip cmsSubPlugin = new ContextMenuStrip();
        delegate void Simple();
        #endregion

        protected override void load()
        {
            foreach (IDockContent content in Application.Panel.Contents)
            {
                _explorer = content as FixExplorer;
            }
            if (_explorer == null)
            {
                _explorer = new FixExplorer();
                _explorer.Show(Application.Panel, DockState.DockLeftAutoHide);
            }
            _mainTool = Application.MainMenu;
            _explorer.Text = "Fix协议信息";

            AddTreeControl();

            loadFix();

        }


        #region 增加树形控件
        private void AddTreeControl()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(AddTreeControl);
                _mainTool.Invoke(s, null);
            }
            else
            {
                _tvFix.Dock = DockStyle.Fill;
                _tvFix.CheckBoxes = true;
                _tvFix.AfterCheck += TvDbAfterCheck;
                _tvFix.MouseClick += TvDbMouseClick;
                _explorer.Controls.Add(_tvFix);
            }
        }


        void TvDbAfterCheck(object sender, TreeViewEventArgs e)
        {
            bool check = e.Node.Checked;
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = check;
            }

            //DataRow[] dr = GetCheckTable();

            //bool flag = false;
            //if (dr != null && dr.Length > 0)
            //{
            //    flag = true;
            //}
            //broadcast(PluginShareHelper.BroadCastCheckFixNumberIsGreaterThan0, flag);
        }

        void TvDbMouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                cmsSubPlugin.Show(_tvFix, e.Location);
            }
        }
        private void RemoveTreeControl()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(RemoveTreeControl);
                _mainTool.Invoke(s, null);
            }
            else
            {
                _explorer.Controls.Remove(_tvFix);
            }

        }
        #endregion

        #region 清除树形结构

        private void ClearTree()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(ClearTree);
                _mainTool.Invoke(s, null);
            }
            else
            {
                _tvFix.Nodes.Clear();
            }
        }
        #endregion

        #region TreeView

        private void ExandAllTreeNode()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new Simple(ExandAllTreeNode);
                _mainTool.Invoke(s, null);

            }
            else
            {
                _tvFix.ExpandAll();
            }

        }

        private delegate void CreateRootNodeDel(TreeNodeCollection collection);
        private void CreateRootNode(TreeNodeCollection collection)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new CreateRootNodeDel(CreateRootNode);
                _mainTool.Invoke(s, new object[] { collection });

            }
            else
            {
                var fixNode = new TreeNode { Text = TagType.Fix.ToString(), Tag = TagType.Header };
                AddTreeNode(collection, fixNode);

                var headerNode = new TreeNode { Text = TagType.Header.ToString(), Tag = TagType.Header };
                AddTreeNode(fixNode.Nodes, headerNode);

                var messagesNode = new TreeNode { Text = TagType.Messages.ToString(), Tag = TagType.Messages };
                AddTreeNode(fixNode.Nodes, messagesNode);

                var componentNode = new TreeNode { Text = TagType.Components.ToString(), Tag = TagType.Components };
                AddTreeNode(fixNode.Nodes, componentNode);

                var fieldsNode = new TreeNode { Text = TagType.Fields.ToString(), Tag = TagType.Fields };
                AddTreeNode(fixNode.Nodes, fieldsNode);

                var trailerNode = new TreeNode { Text = TagType.Trailer.ToString(), Tag = TagType.Trailer };
                AddTreeNode(fixNode.Nodes, trailerNode);
            }

        }

        private delegate void AddNodesDel(TreeNodeCollection collection, DataTable treeDataTable);
        private void AddNodes(TreeNodeCollection collection, DataTable treeDataTable)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new AddNodesDel(AddNodes);
                _mainTool.Invoke(s, new object[] { collection, treeDataTable });

            }
            else
            {
                DataRowCollection rows = treeDataTable.Rows;

                foreach (DataRow row in rows)
                {
                    //新建一个结点 =                 
                    TreeNode node = CreateTreeNode(row);

                    TreeNodeimageIndex(node, _isSelected);

                    AddTreeNode(collection, node); //加入到结点集合中              

                }
                return;
            }
        }

        private bool _isSelected = false;
        private delegate TreeNode CreateTreeNodeDel(DataRow row);
        private TreeNode CreateTreeNode(DataRow row)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new CreateTreeNodeDel(CreateTreeNode);
                return _mainTool.Invoke(s, new object[] { row }) as TreeNode;

            }
            else
            {
                if (row != null)
                {
                    var node = new TreeNode { Text = row["name"] as string };


                    string strTag = row["type"].ToString();
                    if (!string.IsNullOrEmpty(strTag))
                    {
                        var tag = (TagType)Enum.Parse(typeof(TagType), strTag, true);
                        node.Tag = new NodeTag(tag, row);
                    }
                    return node;
                }
            }
            return null;
        }

        private delegate void AddTreeNodeDel(TreeNodeCollection collection, TreeNode node);
        private void AddTreeNode(TreeNodeCollection collection, TreeNode node)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new AddTreeNodeDel(AddTreeNode);
                _mainTool.Invoke(s, new object[] { collection, node });
            }
            else
            {
                collection.Add(node);
            }

        }
        private delegate void TreeNodeimageIndexDel(TreeNode node, bool selected);
        private void TreeNodeimageIndex(TreeNode node, bool selected)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new TreeNodeimageIndexDel(TreeNodeimageIndex);
                _mainTool.Invoke(s, new object[] { node, selected });
            }
            else
            {
                var nodeTag = node.Tag as NodeTag;
                selected = false;
                if (nodeTag != null)
                {
                    switch (nodeTag.Tag)
                    {
                        case TagType.Header:
                            node.ImageIndex = 7;
                            node.SelectedImageIndex = node.ImageIndex;
                            break;
                        case TagType.Messages:
                            if (selected)
                            {
                                node.ImageIndex = 0;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            else
                            {
                                node.ImageIndex = 1;
                                node.SelectedImageIndex = node.ImageIndex;
                            }

                            break;
                        case TagType.Components:
                            if (selected)
                            {
                                node.ImageIndex = 3;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            else
                            {
                                node.ImageIndex = 4;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            break;
                        case TagType.Fields:
                            if (selected)
                            {
                                node.ImageIndex = 5;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            else
                            {
                                node.ImageIndex = 6;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            break;
                        case TagType.Trailer:
                            if (selected)
                            {
                                node.ImageIndex = 5;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            else
                            {
                                node.ImageIndex = 6;
                                node.SelectedImageIndex = node.ImageIndex;
                            }
                            break;
                    }
                }
            }
        }
        #endregion

        #region 解析Fix
        private Fix.Common.Model.Fix fix = new Fix.Common.Model.Fix();

        private void loadFix()
        {
            SetTbDbEnable(false);
            ClearTree();
            CreateRootNode(_tvFix.Nodes);
            parseFix();
            ExandAllTreeNode();
            SetTbDbEnable(true);
        }

        private void parseFix()
        {
            XmlDocument document = new XmlDocument();
            document.Load(FilePathHelper.FixXml);
            XmlNodeList fixNodes = document.GetElementsByTagName("fix");
            XmlNode fixNode = fixNodes[0];
            fix.Major = int.Parse(fixNode.Attributes["major"].Value);
            fix.Minor = int.Parse(fixNode.Attributes["minor"].Value);
            foreach (XmlNode node in fixNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "header":
                        parseHeader(node);
                        break;
                    case "components":
                        parseComponents(node);
                        break;
                    case "messages":
                        parseMessages(node);
                        break;
                    case "fields":
                        parseFields(node);
                        break;
                    case "trailer":
                        parseTrailer(node);
                        break;

                }
            }



        }

        private void parseHeader(XmlNode node)
        {
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "field")
                {
                    Field f = new Field();
                    f.Name = xn.Attributes["name"].Value;
                    f.Required = xn.Attributes["required"].Value == "Y";
                    fix.Header.Fields.Add(f);
                }
                else if (xn.Name == "group")
                {
                    Group g = new Group();
                    parseGroup(g, xn);
                    fix.Header.Groups.Add(g);

                }
            }
        }

        private void parseTrailer(XmlNode node)
        {
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "field")
                {
                    Field f = new Field();
                    f.Name = xn.Attributes["name"].Value;
                    f.Required = xn.Attributes["required"].Value == "Y";
                    fix.Trailer.Fields.Add(f);
                }
                else if (xn.Name == "group")
                {
                    Group g = new Group();
                    parseGroup(g, xn);
                    fix.Trailer.Groups.Add(g);

                }
            }
        }

        private void parseGroup(Group g, XmlNode node)
        {
            g.Name = node.Attributes["name"].Value;
            g.Required = node.Attributes["required"].Value == "Y";
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "field")
                {
                    Field f = new Field();
                    f.Name = xn.Attributes["name"].Value;
                    f.Required = xn.Attributes["required"].Value == "Y";
                    g.Fields.Add(f);
                }
                else if (xn.Name == "group")
                {
                    Group gt = new Group();
                    parseGroup(gt, xn);
                    g.Groups.Add(gt);
                }
                else if (xn.Name == "component")
                {
                    Component c = new Component();
                    parseComponent(c, xn);
                    g.Components.Add(c);
                }

            }
        }
        private void parseComponents(XmlNode node)
        {
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "component")
                {
                    Component c = new Component();
                    parseComponent(c, xn);
                    fix.Components.Add(c);
                }
            }
        }
        private void parseMessages(XmlNode node)
        {
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "message")
                {
                    Fix.Common.Model.Message m = new Fix.Common.Model.Message();
                    parseMessage(m, xn);
                    fix.Messages.Add(m);
                }
            }
        }
        private void parseMessage(Fix.Common.Model.Message m, XmlNode node)
        {
            m.Name = node.Attributes["name"].Value;
            m.MsgType = node.Attributes["msgtype"].Value;
            m.MsgCat = node.Attributes["msgcat"].Value;
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "field")
                {
                    Field f = new Field();
                    f.Name = xn.Attributes["name"].Value;
                    f.Required = xn.Attributes["required"].Value == "Y" ? true : false;
                    m.Fields.Add(f);
                }
                else if (xn.Name == "group")
                {
                    Group gt = new Group();
                    parseGroup(gt, xn);
                    m.Groups.Add(gt);
                }
                else if (xn.Name == "component")
                {
                    Component ct = new Component();
                    parseComponent(ct, xn);
                    m.Components.Add(ct);

                }
                

            }
        }

        private void parseComponent(Component c, XmlNode node)
        {
            
            c.Name = node.Attributes["name"].Value;

            if (node.Attributes.Count == 2)
            {
                c.Required = node.Attributes["required"].Value == "Y" ? true : false;
            }
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "field")
                {
                    Field f = new Field();
                    f.Name = xn.Attributes["name"].Value;
                    f.Required = xn.Attributes["required"].Value == "Y" ? true : false;
                    c.Fields.Add(f);
                }
                else if (xn.Name == "group")
                {
                    Group gt = new Group();
                    parseGroup(gt, xn);
                    c.Groups.Add(gt);
                }
                else if (xn.Name == "component")
                {
                    Component ct = new Component();
                    parseComponent(ct, xn);
                    c.Components.Add(ct);

                }

            }
        }
        private void parseFields(XmlNode node)
        {
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "field")
                {
                    FieldDic fd = new FieldDic();
                    parseFieldDic(fd, xn);
                    fix.Fields.Add(fd);
                }
            }
        }
        private void parseFieldDic(FieldDic fd, XmlNode node)
        {
            fd.Name = node.Attributes["name"].Value;
            fd.Number = int.Parse(node.Attributes["number"].Value);
            fd.Type = node.Attributes["type"].Value;
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "value")
                {
                    Value v = new Value();
                    v.Enum = xn.Attributes["enum"].Value;
                    v.Description = xn.Attributes["description"].Value;
                    fd.Values.Add(v);
                }
            }
        }

        #endregion

        #region Enable
        private delegate void SimpleBool(bool flag);
        private void SetTbDbEnable(bool flag)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new SimpleBool(SetTbDbEnable);
                _mainTool.Invoke(s, new object[] { flag });
            }
            else
            {
                _tvFix.Enabled = flag;
            }
        }
        #endregion

    }
    internal class NodeTag
    {
        public NodeTag()
        { }
        public NodeTag(TagType tag, object o)
        {

            _tag = tag;
            _o = o;
        }


        readonly TagType _tag = TagType.None;

        public TagType Tag
        {
            get { return _tag; }
        }

        readonly object _o;

        public object O
        {
            get { return _o; }
        }
    }
    internal enum TagType
    {
        Fix,
        Header,
        Trailer,
        Messages,
        Components,
        Fields,
        None

    }
}
