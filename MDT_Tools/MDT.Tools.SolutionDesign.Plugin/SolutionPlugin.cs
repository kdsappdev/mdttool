using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using MDT.Tools.Core.Plugin;
using MDT.Tools.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.SolutionDesign.Plugin.UI;
using MDT.Tools.SolutionDesign.Plugin.Properties;
using MDT.Tools.SolutionDesign.Plugin.Utils;
using MDT.Tools.SolutionDesign.Plugin.Model;
using MDT.Tools.SolutionDesign.Plugin.Interface;
using MDT.Tools.SolutionDesign.Plugin.Model.NullModel;

namespace MDT.Tools.SolutionDesign.Plugin
{
    public class SolutionPlugin : AbstractPlugin, ISolutionPlugin
    {
        #region 插件描述

        private int tag = 34;
        public override int Tag
        {
            get { return this.tag; }
            set { tag = value; }
        }

        public override int PluginKey
        {
            get { return 34; }
        }

        public override string PluginName
        {
            get { return  "解决方案插件"; }
        }

        public override string Description
        {
            get { return  "基础代码生成"; }
        }

        public override string Author
        {
            get { return  "饶佳琪"; }
        }

        #endregion

        public SolutionPluginDomain SolutionPluginDomain = null;

        #region 控件

        private TreeView tvSolution = null;
        private ContextMenuStrip cmsMenu = null;
        private SolutionExplorer solutionExplorer = null;
        private TreeNode root = null;
        private ImageList ilTreeImage = null;

        #endregion

        #region 数据源

        private TreeObject treeObject = null;
        #endregion

        #region 菜单选项

        private ToolStripItem tsAdd = null;
        private ToolStripItem tsDelete = null;
        private ToolStripItem tsExport = null;
        private ToolStripItem tsSave = null;
        #endregion

        #region 当前聚焦的树节点

        private TreeNode curryFocusedNode = null;

        #endregion


        public SolutionPlugin()
        {
            initComponent();
        }

        #region 初始化

        private void initComponent()
        {
            NodeObject rootNode = new NodeObject()
            {
                Name = StaticText.RootNodeName,
                NodeTag = NodeTag.Root
            };
            treeObject = new TreeObject();//(rootNode);
            treeObject.ChildNodes.Add(rootNode);

            ilTreeImage = new ImageList();
            ilTreeImage.Images.Add(Resources.solution);
            ilTreeImage.Images.Add(Resources.project);
            ilTreeImage.Images.Add(Resources.direct);

            cmsMenu = new ContextMenuStrip();
            cmsMenu.Opening += new System.ComponentModel.CancelEventHandler(cmsMenu_Opening);

            tvSolution = new TreeView();
            tvSolution.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvSolution_NodeMouseClick);
            tvSolution.ImageList = ilTreeImage;
            tvSolution.ContextMenuStrip = cmsMenu;
            tvSolution.Dock = DockStyle.Fill;
            tvSolution.AfterSelect += new TreeViewEventHandler(tvSolution_AfterSelect);

            tsAdd = new ToolStripMenuItem();
            tsAdd.Text = StaticText.ToolStripName.Add;
            tsAdd.Click += new EventHandler(tsAdd_Click);

            tsDelete = new ToolStripMenuItem();
            tsDelete.Text = StaticText.ToolStripName.Delete;
            tsDelete.Click += new EventHandler(tsDelete_Click);

            tsExport = new ToolStripMenuItem();
            tsExport.Text = StaticText.ToolStripName.Export;
            tsExport.Click += new EventHandler(tsExport_Click);

            tsSave = new ToolStripMenuItem();
            tsSave.Text = StaticText.ToolStripName.Save;
            tsSave.Click += new EventHandler(tsSave_Click);

            cmsMenu.Items.AddRange(new ToolStripItem[] { tsAdd, tsDelete,  tsSave });//tsExport,

            root = new TreeNode(StaticText.RootNodeName);
            root.ImageIndex = (int)ImageIndex.Root;
            root.Tag = rootNode;
            tvSolution.Nodes.Add(root);

            curryFocusedNode = root;

            SolutionPluginDomain = new SolutionPluginDomain(this);
        }

        void tvSolution_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            NodeObject no = node.Tag as NodeObject;
            if (no != null)
            {
                if (no.NodeTag == NodeTag.Root)
                    node.SelectedImageIndex = (int) ImageIndex.Root;
                else if (no.NodeTag == NodeTag.Group)
                    node.SelectedImageIndex = (int) ImageIndex.Group;
                else
                    node.SelectedImageIndex = (int) ImageIndex.Node;
            }
        }

        #endregion

        protected override void load()
        {
            foreach (IDockContent content in Application.Panel.Contents)
            {
                solutionExplorer = content as SolutionExplorer;
            }
            if (solutionExplorer == null)
            {
                solutionExplorer = new SolutionExplorer();
                solutionExplorer.Show(Application.Panel, DockState.DockLeft);
            }

            solutionExplorer.Text = StaticText.RootNodeName;
            solutionExplorer.Icon = Resources.flag;

            loadData();

            solutionExplorer.Controls.Add(tvSolution);

            root.ExpandAll();
        }

        private void loadData()
        {
            object obj = SolutionPluginDomain.loadXml(typeof (TreeObject),
                StaticText.FilePath.Directory + StaticText.FilePath.FileName);

            if (obj as TreeObject != null)
            {
                this.treeObject = obj as TreeObject;
                SolutionPluginDomain.createTree(root, obj as TreeObject);
            }
        }

        void tsSave_Click(object sender, EventArgs e)
        {
            SolutionPluginDomain.save(treeObject);
        }

        void tsExport_Click(object sender, EventArgs e)
        {
            //object obj = SolutionPluginDomain.loadXml(typeof (TreeObject),
            //    StaticText.FilePath.Directory + StaticText.FilePath.FileName);

            //SolutionPluginDomain.createTree(root, obj as TreeObject);
        }

        void tsDelete_Click(object sender, EventArgs e)
        {
            if (curryFocusedNode != null)
            {
                SolutionPluginDomain.deleteNode(curryFocusedNode);
            }
        }

        void tsAdd_Click(object sender, EventArgs e)
        {
            if (curryFocusedNode != null)
            {
                NodeObject nodeObject = curryFocusedNode.Tag as NodeObject;
                if (nodeObject != null)
                {
                    switch (nodeObject.NodeTag)
                    {
                        case NodeTag.Root:

                            Form1 form1 = new Form1();
                            form1.sendValue += new Action<string>(form1_sendValue);
                            //form1.Left = Screen.PrimaryScreen.Bounds.Width/2 - form1.Width/2;
                            //form1.Top = Screen.PrimaryScreen.Bounds.Height/2 - form1.Height/2;
                            //form1.Location = new Point(form1.Left, form1.Top);
                            form1.Show();

                            break;
                        case NodeTag.Group:

                            Form2 form2 = new Form2();
                            form2.sendValue += new Action<string>(form1_sendValue);

                            form2.Show();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void form1_sendValue(string obj)
        {
            SolutionPluginDomain.createNode(curryFocusedNode, obj);
        }

        private void tvSolution_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                curryFocusedNode = e.Node;
        }

        void cmsMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (curryFocusedNode != null)
            {
                NodeObject nodeObject = curryFocusedNode.Tag as NodeObject;
                if (nodeObject != null)
                {
                    switch (nodeObject.NodeTag)
                    {
                        case NodeTag.Root:
                            SolutionPluginDomain.setMenuItemEnable(tsAdd, true);
                            SolutionPluginDomain.setMenuItemEnable(tsDelete, true);
                            SolutionPluginDomain.setMenuItemEnable(tsExport, true);
                            SolutionPluginDomain.setMenuItemEnable(tsSave, true);

                            break;
                        case NodeTag.Group:
                            SolutionPluginDomain.setMenuItemEnable(tsAdd, true);
                            SolutionPluginDomain.setMenuItemEnable(tsDelete, true);
                            SolutionPluginDomain.setMenuItemEnable(tsExport, false);
                            SolutionPluginDomain.setMenuItemEnable(tsSave, false);

                            break;
                        case NodeTag.Node:
                            SolutionPluginDomain.setMenuItemEnable(tsAdd, false);
                            SolutionPluginDomain.setMenuItemEnable(tsDelete, true);
                            SolutionPluginDomain.setMenuItemEnable(tsExport, false);
                            SolutionPluginDomain.setMenuItemEnable(tsSave, false);

                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #region ISolutionPlugin接口实现

        public void expandTree(TreeNode treeNode)
        {
            if (tvSolution.InvokeRequired)
                tvSolution.Invoke(new Action<TreeNode>(expandTree), new object[] { treeNode });
            else
                treeNode.Expand();
        }

        #endregion
    }
}
