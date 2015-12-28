using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.SolutionDesign.Plugin.Model.NullModel
{
    public class NullNodeObject : NodeObject
    {
        public bool isNull()
        {
            return true;
        }
    }
}
