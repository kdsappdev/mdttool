using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.SolutionDesign.Plugin.Model
{
    [Serializable]
    public class TreeObject : Object
    {
        public TreeObject()
        {
        }

        //private NodeObject rootNode;

        //public NodeObject RootNode
        //{
        //    get { return rootNode; }
        //    set { rootNode = value; }
        //}

        //public TreeObject(NodeObject rootNode)
        //{
        //    this.rootNode = rootNode;
        //}

        private List<NodeObject> childNodes = new List<NodeObject>();

        public List<NodeObject> ChildNodes
        {
            get { return childNodes; }
            set { childNodes = value; }
        }
    }
}
