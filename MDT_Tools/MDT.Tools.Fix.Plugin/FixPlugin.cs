using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.Utils;
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
                _explorer.Show(Application.Panel, DockState.DockLeft);
            }
            _mainTool = Application.MainMenu;
            _explorer.Text = "Fix协议信息";
            registerObject(PluginShareHelper.CmsSubPlugin, cmsSubPlugin);
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
                _tvFix.AllowDrop = true;
                _tvFix.DragEnter += new DragEventHandler(_tvFix_DragEnter);
                _tvFix.DragDrop += new DragEventHandler(_tvFix_DragDrop);
                _tvFix.AfterCheck += TvDbAfterCheck;
                _tvFix.MouseClick += TvDbMouseClick;
                _explorer.Controls.Add(_tvFix);
            }
        }

        void _tvFix_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effect = DragDropEffects.Move;
        }

        void _tvFix_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
           
            
            try
            {
                parseFix(path);
                File.Copy(path, FilePathHelper.FixXml, true);
                loadFix();
            }
            catch (Exception ex)
            {

                MessageBox.Show(_tvFix,string.Format("文件错误[{0}]",ex.Message), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        #region 获取选择项

        private delegate object[] GetCheckTableDel();
        private object[] GetCheckTable()
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new GetCheckTableDel(GetCheckTable);
                return _mainTool.Invoke(s, null) as DataRow[];
            }

            var tnCheckList = new List<TreeNode>();
            GetTnCheck(_tvFix.Nodes, tnCheckList);
            var drTable = new object[0];
            if (tnCheckList.Count > 0)
            {
                drTable = new object[tnCheckList.Count];
                for (int i = 0; i < drTable.Length; i++)
                {
                    drTable[i] = ((NodeTag)tnCheckList[i].Tag).O;
                }
            }
            return drTable;
        }

        private delegate void GetTnCheckDel(TreeNodeCollection collection, IList<TreeNode> tnCheckList);

        private void GetTnCheck(TreeNodeCollection collection, IList<TreeNode> tnCheckList)
        {
            if (_mainTool.InvokeRequired)
            {
                var s = new GetTnCheckDel(GetTnCheck);
                _mainTool.Invoke(s, null);
            }
            else
            {
                foreach (TreeNode tn in collection)
                {
                    if (tn.Checked && tn.Tag is NodeTag)
                    {
                        tnCheckList.Add(tn);
                    }
                    if (tn.Nodes.Count > 0)
                    {
                        GetTnCheck(tn.Nodes, tnCheckList);
                    }
                }
            }
        }

        #endregion

        private bool checkFlag = true;
        void TvDbAfterCheck(object sender, TreeViewEventArgs e)
        {
            if (checkFlag)
            {
                checkFlag = false;
                bool check = e.Node.Checked;
                foreach (TreeNode node in e.Node.Nodes)
                {
                    node.Checked = check;
                }

                object[] o = GetCheckTable();
                if (o != null)
                {
                    Type type = null;
                    bool isContinue = true;
                    foreach (var o1 in o)
                    {
                        if (type == null)
                        {
                            type = o1.GetType();
                        }
                        else
                        {
                            if (!type.Equals(o1.GetType()))
                            {
                                isContinue = false;
                                break;

                            }
                        }
                    }

                    if (!isContinue)
                    {
                        MessageBox.Show(_tvFix, "不能选择不同节点,请选则同一类节点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        e.Node.Checked = !e.Node.Checked;
                        foreach (TreeNode node in e.Node.Nodes)
                        {
                            node.Checked = e.Node.Checked;
                        }
                        checkFlag = true;
                        return;
                    }
                }
                registerObject(PluginShareHelper.FixCurrentCheck, o);
                bool flag = false;
                if (o != null && o.Length > 0)
                {
                    flag = true;
                }
                broadcast(PluginShareHelper.BroadCastCheckFixNumberIsGreaterThan0, flag);
                checkFlag = true;
            }
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
                var fixNode = new TreeNode { Text = TagType.Fix.ToString() + fixM.Major + "." + fixM.Minor, Tag = TagType.Header };

                AddTreeNode(collection, fixNode);

                var headerNode = new TreeNode { Text = TagType.Header.ToString(), Tag = TagType.Header };
                headerNode.Tag = new NodeTag(TagType.Header, fixM.Header);
                AddTreeNode(fixNode.Nodes, headerNode);



                var messagesNode = new TreeNode { Text = TagType.Messages.ToString(), Tag = TagType.Messages };
                AddTreeNode(fixNode.Nodes, messagesNode);

                foreach (Fix.Common.Model.Message message in fixM.Messages)
                {
                    //新建一个结点 =                 
                    TreeNode node = new TreeNode { Text = message.Name };
                    node.Tag = new NodeTag(TagType.Message, message);
                    TreeNodeimageIndex(node, _isSelected);
                    AddTreeNode(messagesNode.Nodes, node); //加入到结点集合中              

                }

                var componentNode = new TreeNode { Text = TagType.Components.ToString(), Tag = TagType.Components };
                AddTreeNode(fixNode.Nodes, componentNode);
                foreach (Fix.Common.Model.Component component in fixM.Components)
                {
                    //新建一个结点 =                 
                    TreeNode node = new TreeNode { Text = component.Name };
                    node.Tag = new NodeTag(TagType.Component, component);
                    TreeNodeimageIndex(node, _isSelected);
                    AddTreeNode(componentNode.Nodes, node); //加入到结点集合中
                }
                var fieldsNode = new TreeNode { Text = TagType.Fields.ToString(), Tag = TagType.Fields };
                AddTreeNode(fixNode.Nodes, fieldsNode);

                foreach (Fix.Common.Model.FieldDic fieldDic in fixM.Fields)
                {
                    //新建一个结点 =                 
                    TreeNode node = new TreeNode { Text = fieldDic.Name };
                    node.Tag = new NodeTag(TagType.Field, fieldDic);
                    TreeNodeimageIndex(node, _isSelected);
                    AddTreeNode(fieldsNode.Nodes, node); //加入到结点集合中              

                }
                registerObject(PluginShareHelper.FixFieldDic, fixM.Fields);
                var trailerNode = new TreeNode { Text = TagType.Trailer.ToString(), Tag = TagType.Trailer };
                AddTreeNode(fixNode.Nodes, trailerNode);
                trailerNode.Tag = new NodeTag(TagType.Trailer, fixM.Trailer);
                fixNode.Expand();
            }

        }



        private bool _isSelected = false;


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
        private Fix.Common.Model.Fix fixM = new Fix.Common.Model.Fix();

        private void loadFix()
        {
            SetTbDbEnable(false);
            ClearTree();
            fixM= parseFix(FilePathHelper.FixXml);
            CreateRootNode(_tvFix.Nodes);
            //ExandAllTreeNode();
            SetTbDbEnable(true);
        }

        private Fix.Common.Model.Fix parseFix(string path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNodeList fixNodes = document.GetElementsByTagName("fix");
            XmlNode fixNode = fixNodes[0];
            if(fixNode==null)
            {
                throw new Exception("错误的Fix文件");
            }
            Fix.Common.Model.Fix fix = new Fix.Common.Model.Fix();
            fix.Major = int.Parse(fixNode.Attributes["major"].Value);
            fix.Minor = int.Parse(fixNode.Attributes["minor"].Value);
            foreach (XmlNode node in fixNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "header":
                        parseHeader(node, fix);
                        break;
                    case "components":
                        parseComponents(node,fix);
                        break;
                    case "messages":
                        parseMessages(node,fix);
                        break;
                    case "fields":
                        parseFields(node,fix);
                        break;
                    case "trailer":
                        parseTrailer(node,fix);
                        break;

                }
            }
            fix.Fields.Add(new FieldDic() { Name = "StandardHeader", Type = "STRING",Number = -1});
            fix.Fields.Add(new FieldDic() { Name = "StandardTrailer", Type = "STRING", Number = -1 });
            foreach (var component in fix.Components)
            {
                fix.Fields.Add(new FieldDic() { Name = component.Name, Type = "STRING", Number = -1 });
            }
            return fix;
        }

        private void parseHeader(XmlNode node,Fix.Common.Model.Fix fix)
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
                    parseGroup(g, xn,fix);
                    fix.Header.Groups.Add(g);

                } if (xn.Name == "component")
                {
                    Component c = new Component();
                    parseComponent(c, xn,fix);
                    fix.Header.Components.Add(c);

                }
            }
        }

        private void parseTrailer(XmlNode node, Fix.Common.Model.Fix fix)
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
                    parseGroup(g, xn,fix);
                    fix.Trailer.Groups.Add(g);
                }

                else
                    if (xn.Name == "component")
                    {
                        Component c = new Component();
                        parseComponent(c, xn,fix);
                        fix.Trailer.Components.Add(c);

                    }
            }

        }

        private void parseGroup(Group g, XmlNode node, Fix.Common.Model.Fix fix)
        {
            g.Name = node.Attributes["name"].Value;
            g.Required = node.Attributes["required"].Value == "Y";
            LogHelper.Debug(string.Format("\t\tgroup:{0},{1}", g.Name, g.Required));
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "field")
                {
                    Field f = new Field();
                    f.Name = xn.Attributes["name"].Value;
                    f.Required = xn.Attributes["required"].Value == "Y";
                    LogHelper.Debug(string.Format("\t\t\t\tfield:{0},{1}", f.Name, f.Required));
                    g.Fields.Add(f);
                }
                else if (xn.Name == "group")
                {
                    Group gt = new Group();
                    parseGroup(gt, xn,fix);
                    g.Groups.Add(gt);
                }
                else if (xn.Name == "component")
                {
                    Component c = new Component();
                    parseComponent(c, xn,fix);
                    g.Components.Add(c);
                }

            }
        }
        private void parseComponents(XmlNode node, Fix.Common.Model.Fix fix)
        {
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "component")
                {
                    Component c = new Component();
                    parseComponent(c, xn,fix);
                    fix.Components.Add(c);
                }
            }
        }
        private void parseMessages(XmlNode node, Fix.Common.Model.Fix fix)
        {
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "message")
                {
                    Fix.Common.Model.Message m = new Fix.Common.Model.Message();
                    parseMessage(m, xn,fix);
                    fix.Messages.Add(m);
                }
            }
        }
        private void parseMessage(Fix.Common.Model.Message m, XmlNode node, Fix.Common.Model.Fix fix)
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
                    f.Required = xn.Attributes["required"].Value == "Y";
                    m.Fields.Add(f);
                }
                else if (xn.Name == "group")
                {
                    Group gt = new Group();
                    parseGroup(gt, xn,fix);
                    m.Groups.Add(gt);
                }
                else if (xn.Name == "component")
                {
                    Component ct = new Component();
                    parseComponent(ct, xn,fix);
                    m.Components.Add(ct);

                }


            }
        }

        private void parseComponent(Component c, XmlNode node, Fix.Common.Model.Fix fix)
        {

            c.Name = node.Attributes["name"].Value;

            if (node.Attributes.Count == 2)
            {
                if (node.Attributes[1].Name == "required")
                {
                    c.Required = node.Attributes["required"].Value == "Y";
                }
            }
            LogHelper.Debug(string.Format("component:{0},{1}", c.Name, c.Required));
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "field")
                {
                    Field f = new Field();
                    f.Name = xn.Attributes["name"].Value;
                    f.Required = xn.Attributes["required"].Value == "Y";
                    LogHelper.Debug(string.Format("\t\tfield:{0},{1}", f.Name, f.Required));
                    c.Fields.Add(f);
                }
                else if (xn.Name == "group")
                {
                    Group gt = new Group();
                    parseGroup(gt, xn,fix);
                    c.Groups.Add(gt);
                }
                else if (xn.Name == "component")
                {
                    Component ct = new Component();
                    parseComponent(ct, xn,fix);
                    c.Components.Add(ct);

                }

            }
        }
        private void parseFields(XmlNode node, Fix.Common.Model.Fix fix)
        {


            foreach (XmlNode xn in node.ChildNodes)
            {
                if ((xn.Name+"").ToLower() == "field")
                {
                    FieldDic fd = new FieldDic();
                    parseFieldDic(fd, xn,fix);
                    fix.Fields.Add(fd);
                }
                else
                {
                    LogHelper.Warn(string.Format("{0} not field",xn.Name));
                }
            }
        }
        private void parseFieldDic(FieldDic fd, XmlNode node, Fix.Common.Model.Fix fix)
        {

            fd.Name = node.Attributes["name"].Value;
            fd.Number = int.Parse(node.Attributes["number"].Value);
            fd.Type = node.Attributes["type"].Value;
            LogHelper.Debug(string.Format("field:{0},{1},{2}", fd.Name, fd.Number, fd.Type));
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name == "value")
                {
                    Value v = new Value();
                    v.Enum = xn.Attributes["enum"].Value;
                    v.Description = xn.Attributes["description"].Value;
                    LogHelper.Debug(string.Format("\t\tvalue:{0},{1}", v.Enum, v.Description));
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
        Message,
        Components,
        Component,
        Fields,
        Field,
        None

    }
}
