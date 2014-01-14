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
        


        //#region 新加类型
        //private List<string> names = new List<string>();

        //public List<string> Names
        //{
        //    get { return names; }
        //    set { names = value; }
        //}
        //private List<int> num = new List<int>();

        //public List<int> Num
        //{
        //    get { return num; }
        //    set { num = value; }
        //}
        
        //private List<string> types = new List<string>();

        //public List<string> Types
        //{
        //    get { return types; }
        //    set { types = value; }
        //}
        //#endregion

    }
}
