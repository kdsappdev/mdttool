using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;
namespace MDT.Tools.Core.Utils
{
    /// <summary>
    /// ReflectionHelper
    /// ������
    /// 
    /// �޸ļ�¼
    ///   
    ///         2010.8.9 �汾��1.0 �׵�˧ ����
    /// 
    /// �汾��1.0
    /// 
    /// <author>
    ///        <name>�׵�˧</name>
    ///        <date>2010.8.9</date>
    /// </author> 
    /// </summary>
    public class ReflectionHelper
    {
        #region ����ָ��Ŀ¼����������

        /// <summary>
        ///  ����ָ��Ŀ¼�����г����е�����������baseType������
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="directorySearched">������Ŀ¼</param>
        /// <param name="searchChildFolder">�Ƿ�������Ŀ¼�еĳ���</param>
        /// <param name="config">�߼����ã����Դ���null����Ĭ������</param>        
        /// <returns>���д�BaseType�����������б�</returns>
        public static IList<Type> LoadDerivedType(Type baseType, string directorySearched, bool searchChildFolder, TypeLoadConfig config)
        {
            if (config == null)
            {
                config = new TypeLoadConfig();
            }

            IList<Type> derivedTypeList = new List<Type>();
            if (searchChildFolder)
            {
                LoadDerivedTypeInAllFolder(baseType, derivedTypeList, directorySearched, config);
            }
            else
            {
                LoadDerivedTypeInOneFolder(baseType, derivedTypeList, directorySearched, config);
            }

            return derivedTypeList;
        }
        #endregion

        #region LoadDerivedTypeInAllFolder
        private static void LoadDerivedTypeInAllFolder(Type baseType, IList<Type> derivedTypeList, string folderPath, TypeLoadConfig config)
        {
            LoadDerivedTypeInOneFolder(baseType, derivedTypeList, folderPath, config);
            string[] folders = Directory.GetDirectories(folderPath);
            foreach (string nextFolder in folders)
            {
                LoadDerivedTypeInAllFolder(baseType, derivedTypeList, nextFolder, config);
            }
        }
        #endregion

        #region LoadDerivedTypeInOneFolder
        private static void LoadDerivedTypeInOneFolder(Type baseType, IList<Type> derivedTypeList, string folderPath, TypeLoadConfig config)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                if (config.TargetFilePostfix != null)
                {
                    if (!file.EndsWith(config.TargetFilePostfix))
                    {
                        continue;
                    }
                }

                Assembly asm = null;

                #region Asm ���س��򼯵��ڴ���
                try
                {
                    if (config.CopyToMemory)
                    {
                        byte[] addinStream = FileHelper.ReadFileReturnBytes(file);
                        asm = Assembly.Load(addinStream);
                    }
                    else
                    {
                        asm = Assembly.LoadFrom(file);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }

                if (asm == null)
                {
                    continue;
                }
                #endregion

                Type[] types = asm.GetTypes();

                foreach (Type t in types)
                {
                    if (t.IsPublic && (t.IsSubclassOf(baseType) || baseType.IsAssignableFrom(t)))
                    {
                        bool canLoad = config.LoadAbstractType || (!t.IsAbstract);
                        if (canLoad)
                        {
                            derivedTypeList.Add(t);
                        }
                    }
                }
            }

        }
        #endregion

        #region ��ȡdll����ʱ��
        public static DateTime GetPe32Time(string fileName)
        {
            int seconds;
            using (var br = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read)))
            {
                var bs = br.ReadBytes(2);
                const string msg = "�Ƿ���PE32�ļ�";
                if (bs.Length != 2) throw new Exception(msg);
                if (bs[0] != 'M' || bs[1] != 'Z') throw new Exception(msg);
                br.BaseStream.Seek(0x3c, SeekOrigin.Begin);
                var offset = br.ReadByte();
                br.BaseStream.Seek(offset, SeekOrigin.Begin);
                bs = br.ReadBytes(4);
                if (bs.Length != 4) throw new Exception(msg);
                if (bs[0] != 'P' || bs[1] != 'E' || bs[2] != 0 || bs[3] != 0) throw new Exception(msg);
                bs = br.ReadBytes(4);
                if (bs.Length != 4) throw new Exception(msg);
                seconds = br.ReadInt32();
            }
            return DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc).
               AddSeconds(seconds).ToLocalTime();
        }
        #endregion

        #region ��ȡdll�汾
        public static string GetVersion(Assembly assembly)
        {
            return assembly.GetName().Version.ToString();
        }

        #endregion

        #region �жϳ����Ƿ�Debug,Release

        public static bool IsDebugVersion(string assemblyName)
        {
            Assembly assembly = Assembly.LoadFile(assemblyName);
            Debug.Assert(assembly != null);
            foreach (DebuggableAttribute attribute in assembly.GetCustomAttributes(typeof(DebuggableAttribute), false))
            {
                if (attribute.IsJITTrackingEnabled) return true;
            }
            return false;
        }
        #endregion


        #region
        #region ˽�з���
        #region ��ö���������
        //private static Dictionary<string, Dictionary<string, string>> dic2 = new Dictionary<string, Dictionary<string, string>>();
        private static Dictionary<string, Dictionary<string, PropertyInfo>> dic3 = new Dictionary<string, Dictionary<string, PropertyInfo>>();
        /// <summary>
        /// ��ö���������
        /// </summary>
        /// <param name="o">����</param>
        /// <returns>��������������</returns>
        public static Dictionary<string, PropertyInfo> GetObjectPropertyName(Type type)
        {
            Dictionary<string, PropertyInfo> dicp = null;
            if (object.Equals(type, null))
            {
                throw new System.Exception("������Ϊ��");
            }
            Type objectType = type;
            string objectName = objectType.ToString();
            if (!dic3.ContainsKey(objectName))
            {
                PropertyInfo[] objectPropertyInfo = objectType.GetProperties();

                dicp = new Dictionary<string, PropertyInfo>();
                foreach (PropertyInfo pt in objectPropertyInfo)
                {
                    dicp.Add(pt.Name.ToUpper(), pt);
                }
                dic3.Add(objectName, dicp);
            }
            dicp = dic3[objectName];
            return dicp;
        }
        #endregion

        private static Dictionary<string, Dictionary<string, MethodInfo>> dic5 = new Dictionary<string, Dictionary<string, MethodInfo>>();

        public static Dictionary<string, MethodInfo> GetObjectMethodName(Type type)
        {
            Dictionary<string, MethodInfo> dicp = null;
            if (object.Equals(type, null))
            {
                throw new System.Exception("������Ϊ��");
            }
            Type objectType = type;
            string objectName = objectType.ToString();
            if (!dic5.ContainsKey(objectName))
            {
                MethodInfo[] objectMethodInfo = objectType.GetMethods();

                dicp = new Dictionary<string, MethodInfo>();
                foreach (MethodInfo pt in objectMethodInfo)
                {
                    if (!dicp.ContainsKey(pt.Name))
                        dicp.Add(pt.Name, pt);
                }
                dic5.Add(objectName, dicp);
            }
            dicp = dic5[objectName];
            return dicp;
        }
        public static MethodInfo GetMethodInfo(Type type,string methodName)
        {
            MethodInfo methodInfo = null;
            Dictionary<string, MethodInfo> dic = GetObjectMethodName(type);
            if(dic.ContainsKey(methodName))
            {
                methodInfo = dic[methodName];
            }
            return methodInfo;
        }

        #region ��ö�������ֵ
        private static Dictionary<string, Dictionary<string, string>> dicp = new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<string, string> GetObjectPropertyValue(object o)
        {
            Dictionary<string, string> dic = null;
            if (object.Equals(o, null))
            {
                throw new System.Exception("����o����ΪNULL");
            }
            Type objectType = o.GetType();
            if (!dicp.ContainsKey(objectType.ToString()))
            {
                PropertyInfo[] objectPropertyInfo = objectType.GetProperties();
                dic = new Dictionary<string, string>();
                foreach (PropertyInfo pt in objectPropertyInfo)
                {
                    object value = pt.GetValue(o, null);
                    if (object.Equals(value, null))//�ж�ֵ�Ƿ��ǿ�ֵ
                    {
                        value = "";
                    }
                    dic.Add(pt.Name, value.ToString());
                }
                dicp.Add(objectType.ToString(), dic);
            }
            dic = dicp[objectType.ToString()] as Dictionary<string, string>;
            return dic;
        }
        #endregion
        #region ��ö�����
        /// <summary>
        /// ��ö�����
        /// </summary>
        /// <param name="o">����</param>
        /// <returns>��������</returns>
        public static string GetObjectName(object o)
        {
            if (object.Equals(o, null))
            {
                throw new System.Exception("������Ϊ��");
            }
            Type objectType = o.GetType();
            return objectType.Name;
        }
        #endregion
        #region ��ö���һ����ֵ
        public static string GetObjectPropertyValue(object o, string propertyName)
        {
            if (object.Equals(o, null))
            {
                throw new System.Exception("������Ϊ��");
            }
            Type objectType = o.GetType();
            PropertyInfo pt = objectType.GetProperty(propertyName);//���������Ϣ
            object value = pt.GetValue(o, null);//�õ�ֵ
            if (object.Equals(value, null))//�ж�ֵ�Ƿ��ǿ�ֵ
            {
                value = "";
            }
            return value.ToString();
        }
        #endregion


        #endregion
        #region ����ת��
        public static Object StringToObject(Type t, String str, ref bool isSuccess)
        {
            Object o = str;
            if (t == typeof(string))
            {
                o = str;
                isSuccess = true;
            }
            if (t == typeof(DateTime) || t == typeof(DateTime?))
            {
                DateTime dt = new DateTime();
                if (DateTime.TryParse(str, out dt))
                {
                    o = dt;
                    isSuccess = true;
                }

            }
            if (t == typeof(int) || t == typeof(int?))
            {
                int i = 0;
                if (int.TryParse(str, out i))
                {
                    o = i;
                    isSuccess = true;
                }
            }
            if (t == typeof(decimal) || t == typeof(decimal?))
            {
                decimal d = 0;
                if (decimal.TryParse(str, out d))
                {
                    o = d;
                    isSuccess = true;
                }
            }
            if (t == typeof(float) || t == typeof(float?))
            {
                float d = 0;
                if (float.TryParse(str, out d))
                {
                    o = d;
                    isSuccess = true;
                }
            }
            if (t == typeof(double) || t == typeof(double?))
            {
                double d = 0;
                if (double.TryParse(str, out d))
                {
                    o = d;
                    isSuccess = true;
                }
            }

            return o;
        }

        #endregion

        #endregion

    }
    #region TypeLoadConfig
    public class TypeLoadConfig
    {
        #region Ctor
        public TypeLoadConfig()
        {
            LoadAbstractType = false;
        }

        public TypeLoadConfig(bool copyToMem, bool loadAbstract, string postfix)
        {
            CopyToMemory = copyToMem;
            LoadAbstractType = loadAbstract;
            _targetFilePostfix = postfix;
        }
        #endregion

        #region �������ڴ�

        /// <summary>
        /// CopyToMem �Ƿ񽫳��򼯿������ڴ�����
        /// </summary>
        public bool CopyToMemory { get; set; }

        #endregion

        #region ���س�������

        /// <summary>
        /// LoadAbstractType �Ƿ���س�������
        /// </summary>
        public bool LoadAbstractType { get; set; }

        #endregion

        #region Ŀ����򼯵ĺ�׺��
        private string _targetFilePostfix = ".dll";
        /// <summary>
        /// TargetFilePostfix ������Ŀ����򼯵ĺ�׺��
        /// </summary>
        public string TargetFilePostfix
        {
            get { return _targetFilePostfix; }
            set { _targetFilePostfix = value; }
        }
        #endregion
    }
    #endregion
}

