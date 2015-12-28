using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MDT.Tools.SolutionDesign.Plugin.Utils
{
    public class DataHelper
    {
        public static void saveXml(object obj)
        {
            string path = System.Windows.Forms.Application.StartupPath + StaticText.FilePath.Directory;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = path + StaticText.FilePath.FileName;

            serializerXml(obj, path);
        }

        public static void serializerXml(object obj, string path)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(path);
                XmlSerializer s = new XmlSerializer(obj.GetType());
                s.Serialize(writer, obj);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static object deserializeXml(Type type, string filePath)
        {
            StreamReader reader = null;
            object obj = null;
            try
            {
                filePath = System.Windows.Forms.Application.StartupPath + filePath;
                reader = new StreamReader(filePath);
                XmlSerializer s = new XmlSerializer(type);
               
                obj = s.Deserialize(reader);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return obj;
        }
    }
}
