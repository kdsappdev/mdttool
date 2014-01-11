using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDT.Tools.Core.Utils;
using MDT.Tools.Fix.Common.Model;
using System.IO;
using System.Windows.Forms;
namespace MDT.Tools.Fix.Common.Utils
{
    public class FixHelper
    {
        private static List<FieldDic> _fieldDics = new List<FieldDic>();
        private static Dictionary<string, FieldDic> dic = new Dictionary<string, FieldDic>();
        private static bool isSetFieldDic = false;
        public static List<FieldDic> FieldDics
        {
            get { return _fieldDics; }
            set
            {
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


        public static string GetFieldOrGroupType(string codeLanage, string name)
        {
            string type = "";
            string key = (name + "").ToLower();
            if (dic.ContainsKey(key))
            {
                type = dic[key].Type;
            }

            type = GetCodeLanageType(codeLanage, type);

            return type;
        }


        #region Fix Map
        static Dictionary<string, string> fieldsTypeDic = new Dictionary<string, string>();
        private static readonly string fixmapConfigPath = Application.StartupPath + "\\control\\fixmap.ini";
        private static string getkey(string codeLangage, string fixType)
        {
            return string.Format("FixTo{0}{1}", codeLangage, fixType);
        }
        static FixHelper()
        {
            fieldsTypeDic = initfieldsTypeDic();
        }
        public static string GetCodeLanageType(string codeLangage, string fixType)
        {
            string type = fixType;
            string key = getkey(codeLangage, fixType);
            if (fieldsTypeDic.ContainsKey(key))
            {
                type = fieldsTypeDic[key];
            }
            return type;
        }

        public static string StrFirstToLower(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str[0].ToString().ToLower() + str.Substring(1, str.Length - 1);
            }
            return str;
        }
        private static Dictionary<string, string> initfieldsTypeDic()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            FileStream fs = new FileStream(fixmapConfigPath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string line;
            string group = "";
            try
            {
                while ((line = sr.ReadLine()) != null)
                {

                    line = line.Trim();
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("[") && line.Contains("]"))
                        {
                            group = line.Replace("[", "").Replace("]", "").Trim();
                        }
                        else
                        {
                            string[] strs = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (strs != null && strs.Length.Equals(2))
                            {
                                string key = group + strs[0];
                                if (!dic.ContainsKey(key))
                                {
                                    dic.Add(key, strs[1]);
                                }
                                else
                                {
                                    LogHelper.Warn("ric exits ");
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                try
                {
                    sr.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }
            return dic;

        }
        #endregion

    }
}
