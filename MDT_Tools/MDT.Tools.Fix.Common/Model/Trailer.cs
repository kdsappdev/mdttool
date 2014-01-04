using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.Fix.Common.Model
{
    public class Trailer
    {
        private List<Field> _fields = new List<Field>();
        public List<Field> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }
    }
}
