using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MDT.Tools.SolutionDesign.Plugin.Interface;
using MDT.Tools.SolutionDesign.Plugin.Model;
using MDT.Tools.SolutionDesign.Plugin.Utils;

namespace MDT.Tools.SolutionDesign.Plugin
{
    public class SolutionPluginDomain
    {
        #region 代理

        public delegate void delegateMenuStatus(Control control, bool status);

        #endregion

        public ISolutionPlugin ISolutionPlugin { get; set; }

        public SolutionPluginDomain(ISolutionPlugin iSolutionPlugin)
        {
            this.ISolutionPlugin = iSolutionPlugin;
        }

        public void setMenuItemEnable(ToolStripItem control, bool status)
        {
            control.Visible = control.Enabled = status;
        }

        public void createNode(TreeNode treeNode, string name)
        {
            NodeObject nodeObject = treeNode.Tag as NodeObject;
            if (nodeObject != null)
            {
                switch (nodeObject.NodeTag)
                {
                    case NodeTag.Root:
                        NodeObject no = createNodeObject(name, NodeTag.Group);
                        nodeObject.ChildNodes.Add(no);
                        treeNode.Nodes.Add(createTreeNode(name, no, (int) ImageIndex.Group));

                        break;
                    case NodeTag.Group:
                        NodeObject noo = createNodeObject(name, NodeTag.Node);
                        nodeObject.ChildNodes.Add(noo);
                        treeNode.Nodes.Add(createTreeNode(name, noo, (int) ImageIndex.Node));

                        break;
                    default:
                        break;
                }
            }

            ISolutionPlugin.expandTree(treeNode);
        }

        private TreeNode createTreeNode(string name, object nodeObject, int imageIndex)
        {
            TreeNode tn = new TreeNode()
            {
                Text = name,
                Tag = nodeObject,
                ImageIndex = imageIndex
            };

            return tn;
        }

        private NodeObject createNodeObject(string name, NodeTag nodeTag)
        {
            NodeObject no = new NodeObject()
            {
                Name = name,
                NodeTag = nodeTag
            };

            return no;
        }


        public void deleteNode(TreeNode treeNode)
        {
            NodeObject nodeObject = treeNode.Tag as NodeObject;
            if (nodeObject != null)
            {
                if (nodeObject.NodeTag != NodeTag.Root)
                {
                    TreeNode parentNode = treeNode.Parent;
                    if (parentNode != null)
                    {
                        NodeObject parentNodeObject = parentNode.Tag as NodeObject;
                        parentNode.Nodes.Remove(treeNode);
                        if (parentNodeObject != null)
                            parentNodeObject.ChildNodes.Remove(nodeObject);

                        ISolutionPlugin.expandTree(parentNode);
                    }
                }
                else
                {
                    treeNode.Nodes.Clear();
                    nodeObject.ChildNodes.Clear();

                    ISolutionPlugin.expandTree(treeNode);
                }
            }
        }

        public void save(object obj)
        {
            DataHelper.saveXml(obj);
        }

        public object loadXml(Type type, string filePath)
        {
            return DataHelper.deserializeXml(type, filePath);
        }

        public void createTree(TreeNode root, TreeObject treeObject)
        {
            if (treeObject == null)
                return;
            foreach (NodeObject nodeObject in treeObject.ChildNodes)
            {
                if (nodeObject.NodeTag == NodeTag.Root)
                {
                    root.Tag = nodeObject;
                    foreach (NodeObject no in nodeObject.ChildNodes)
                    {
                        createNode(no, root);
                    }
                }
            }

            ISolutionPlugin.expandTree(root);
        }

        public void createNode(NodeObject nodeObject, TreeNode node)
        {
            if (nodeObject.NodeTag == NodeTag.Group)
            {
                TreeNode group = new TreeNode()
                {
                    Text = nodeObject.Name,
                    Tag = nodeObject,
                    ImageIndex = (int) ImageIndex.Group
                };
                node.Nodes.Add(group);

                foreach (NodeObject no in nodeObject.ChildNodes)
                    createNode(no, group);
            }
            else if (nodeObject.NodeTag == NodeTag.Node)
            {
                TreeNode tn = new TreeNode()
                {
                    Text = nodeObject.Name,
                    Tag = nodeObject,
                    ImageIndex = (int) ImageIndex.Node
                };

                node.Nodes.Add(tn);
            }
        }
    }
}
