using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.SolutionDesign.Plugin.Model
{
    [Serializable]
    public class NodeObject
    {
        private string name;

        public string Name 
        {
            get { return name; }
            set { name = value; }
        }

        private NodeTag nodeTag;

        public NodeTag NodeTag 
        {
            get { return nodeTag; }
            set { nodeTag = value; }
        }

        private List<NodeObject> childNodes = new List<NodeObject>();

        public List<NodeObject> ChildNodes 
        {
            get { return childNodes; }
            set { childNodes = value; }
        }

        private object data;

        public object Data 
        {
            get { return data; }
            set { data = value; }
        }

        public bool isNull()
        {
            return false;
        }
    }
}
