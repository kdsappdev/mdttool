using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MDT.Tools.Core.Utils;





namespace MDT.Tools
{
    internal class IniConfigHelper
    {       

        private const string Group = "runtimeconfig";
        private const string FramePluginKey = "framepluginkey";
        private const string FunctionPluginKey = "functionpluginkey";

        public static RunTimeConfig ReadRunTimeConfig(string runTimePath,string publicKey)
        {
            RunTimeConfig rtc = new RunTimeConfig();
            try
            {
                var fi = new FileInfo(runTimePath);
                StreamReader sr = fi.OpenText();
                string content = sr.ReadToEnd();
                if (!string.IsNullOrEmpty(publicKey))
                {
                    content = EncrypterHelper.DecryptRASString(content, publicKey);
                }
                string[] temps = content.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                sr.Close();
                string group = "";
                foreach (string str in temps)
                {
                    if (str.Contains("[") && str.Contains("]"))
                    {
                        group = str.Replace("[", "").Replace("]", "");
                        continue;
                    }
                    int index = str.IndexOf('=');
                    if (index >= 1)
                    {
                        try
                        {
                            string key = str.Substring(0, index);
                            string value = str.Substring(index + 1, str.Length - index - 1).Trim();
                            if (group.ToLower().Equals(group.ToLower()))
                            {
                                switch (key.ToLower())
                                {
                                    case FramePluginKey:
                                        rtc.FramePluginKey = value;
                                        break;

                                    case FunctionPluginKey:
                                        rtc.FunctionPluginKey = value;
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {                           
                            LogHelper.Error(ex);
                            Environment.Exit(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return rtc;
        }
    }
}
