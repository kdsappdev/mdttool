using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.Fix.Common.Model
{
    public class Fix
    {
        private int _major = 4;
        public int Major
        {
            get { return _major; }
            set { _major = value; }
        }
        private int _minor = 4;
        public int Minor
        {
            get { return _minor; }
            set { _minor = value; }
        }
        private Header _header = new Header();
        public Header Header
        {
            get { return _header; }
            set { _header = value; }
        }

        private Trailer _trailer = new Trailer();
        public Trailer Trailer
        {
            get { return _trailer; }
            set { _trailer = value; }
        }

        private List<Message> _messages = new List<Message>();
        public List<Message> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }

        private List<Component> _components = new List<Component>();
        public List<Component> Components
        {
            get { return _components; }
            set { _components = value; }
        }

        private List<FieldDic> _fields = new List<FieldDic>();
        public List<FieldDic> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }
    }
}
