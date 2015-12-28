using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Eventlog.Monitor.Plugin.Model
{
    [Serializable]
    public class EventMessage
    {
        private string _type;

        public string type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _content;

        public string content
        {
            get { return _content; }
            set { _content = value; }
        }
        private string _level;

        public string level
        {
            get { return _level; }
            set { _level = value; }
        }
        private string _date;

        public string date
        {
            get { return _date; }
            set { _date = value; }
        }
        private string _time;

        public string time
        {
            get { return _time; }
            set { _time = value; }
        }
        private string _memo;

        public string memo
        {
            get { return _memo; }
            set { _memo = value; }
        }

    }
}
