using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.Fix.Common.Model
{
    public class FieldDic
    {
        public int Number { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }

        private List<Value> _values=new List<Value>();
        public List<Value> Values
        {
            get { return _values; }
            set { _values = value; }
        }
    }
}
