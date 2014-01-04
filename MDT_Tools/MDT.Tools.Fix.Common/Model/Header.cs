using System;
using System.Collections.Generic;
using System.Text;
 

namespace MDT.Tools.Fix.Common.Model
{
   public class Header
    {
       private List<Field> _fields=new List<Field>();
       public List<Field> Fields
       {
           get { return _fields; }
           set { _fields = value; }
       }
       private List<Group> _groups = new List<Group>();
       public List<Group> Groups
       {
           get { return _groups; }
           set { _groups = value; }
       }
    }
}
