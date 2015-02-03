using System;
using System.IO;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace MDT.Tools.Core.Utils
{
    public class JsonHelper
    {
        public static string JsonFormat(string source)
        {
            string dest = source;
            if (!string.IsNullOrEmpty(source))
            {
               
                try
                {
                    string json = source.Replace("{,", "{").Replace("[,", "[");
                    JsonSerializer s = new JsonSerializer();
                    JsonReader reader = new JsonTextReader(new StringReader(json));

                    object jsonObject = s.Deserialize(reader);
                    if (jsonObject != null)
                    {
                        StringWriter sWriter = new StringWriter();
                        s.Serialize(new JsonTextWriter(sWriter)
                                        {
                                            Formatting = Formatting.Indented,
                                            Indentation = 4,
                                            IndentChar = ' '
                                        }, jsonObject);
                        dest = sWriter.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dest;
        }
    }
}