using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.Core.Utils;
using MDT.Tools.Fix.Common.Model;

namespace MDT.Tools.Fix.Common.Utils
{
   public class FixHelper
    {
       private static List<FieldDic> _fieldDics=new List<FieldDic>();
       private static Dictionary<string,FieldDic> dic=new Dictionary<string, FieldDic>();
       private static bool isSetFieldDic = false;
       public static List<FieldDic> FieldDics
       {
           get { return _fieldDics; }
           set {
               if (!isSetFieldDic)
               {
                   _fieldDics = value;
                   if (_fieldDics != null)
                   {
                       foreach (var fieldDic in _fieldDics)
                       {
                           if (!dic.ContainsKey(fieldDic.Name))
                           {
                               dic.Add(fieldDic.Name.ToLower(), fieldDic);
                           }
                           else
                           {
                               LogHelper.Warn(fieldDic.Name + " is exist.");
                           }
                       }
                       isSetFieldDic = true;
                   }
               }
           }
       }

       public string GetFieldOrGroupType(string name)
       {
           string type = "";
           string key = (name + "").ToLower();
           if(dic.ContainsKey(key))
           {
               type = dic[key].Type;
           }
           return type;
       }

    }
}
