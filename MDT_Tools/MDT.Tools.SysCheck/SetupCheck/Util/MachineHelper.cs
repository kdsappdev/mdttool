using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Drawing.Text;
using System.Drawing;
using Microsoft.Win32;
using System.Collections;
using Atf.Installer.SetupCheck;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

namespace Atf.Installer.SetupCheck.Util
{
    public class MachineHelper
    {

        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
        private static Mutex mutex = null;
        private const double Giga = 1000.0 * 1000.0 * 1000.0;
        private const double Million = 1000.0 * 1000.0;
        private const double C2E20 = 1024 * 1024.0;
        public static bool IsConnectedInternet()
        {
            int i = 0;
            if (InternetGetConnectedState(out i, 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //检测下载地址
        public static bool canUpdate(string url)
        {
            try
            {
                WebClient web = new WebClient();
                byte[] buffer = web.DownloadData(url);
                string res = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
                if (res.Length <= 0)
                {
                    return false;
                }
                if (System.Text.ASCIIEncoding.ASCII.GetString(web.DownloadData(res)).Length <= 0)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static void canPing(string ip, out string msg)
        {
            msg = "";
            try
            {

           
            Process iProcess = new Process();
            iProcess.StartInfo.FileName = "cmd.exe";
            iProcess.StartInfo.UseShellExecute = false;
            iProcess.StartInfo.RedirectStandardInput = true;
            iProcess.StartInfo.RedirectStandardOutput = true;
            iProcess.StartInfo.RedirectStandardError = true;
            iProcess.StartInfo.CreateNoWindow = true;
            iProcess.Start();
            iProcess.StandardInput.WriteLine("ping " + ip + " -n 20");
            iProcess.StandardInput.WriteLine("exit");
            msg = iProcess.StandardOutput.ReadToEnd();
            }
            catch (Exception e)
            {

                LogHelper.Error(e);
            }
        }

        //域名解析
        private static string getUrl(string url)
        {
            IPHostEntry hostinfo = Dns.GetHostByName(url);
            IPAddress[] aryIP = hostinfo.AddressList;
            string result = aryIP[0].ToString();
            return result;
        }

        public static string canTelnet(string ipadd, int port)
        {
            string strTelnet = "";
            try
            {
                IPAddress ip = IPAddress.Parse(ipadd.Contains("www") ? MachineHelper.getUrl(ipadd) : ipadd);
                IPEndPoint point = new IPEndPoint(ip, port);
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(point);
                strTelnet = "0000";
            }
            catch (SocketException e)
            {
                if (e.ErrorCode != 10061)
                {
                    strTelnet = "0001";
                }
            }
            return strTelnet;
        }



        public static bool CheckProcessIsMultiple(string name)
        {
            bool newMutexCreated = false;
            try
            {
                string mutexName = "Global\\" + name;
                mutex = new Mutex(false, mutexName, out newMutexCreated);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return !newMutexCreated;
        }

        public static bool Is64BitProcess()
        {

            return IntPtr.Size == 8;

        }

        public static string getKey()
        {
            DateTime dt = DateTime.Now;
            return string.Format("{0:0000}_{1:00}_{2:00}_{3:00}_{4:00}_{5:00}.log", dt.Year, dt.Month, dt.Day,
                dt.Hour,
                dt.Minute, dt.Second);
        }

        public static int GetOSBit()
        {
            try
            {
                string addressWidth = String.Empty;
                ConnectionOptions mConnOption = new ConnectionOptions();
                ManagementScope mMs = new ManagementScope(@"\\localhost", mConnOption);
                ObjectQuery mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(mMs, mQuery);
                ManagementObjectCollection mObjectCollection = mSearcher.Get();
                foreach (ManagementObject mObject in mObjectCollection)
                {
                    addressWidth = mObject["AddressWidth"].ToString();
                }
                return Int32.Parse(addressWidth);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 32;
            }
        }

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }


        public static bool getPhysicalMemory()
        {
            bool is64 = false;
            try
            {
                string addressWidth = String.Empty;
                ConnectionOptions mConnOption = new ConnectionOptions();
                ManagementScope mMs = new ManagementScope("//localhost", mConnOption);
                ObjectQuery mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(mMs, mQuery);
                ManagementObjectCollection mObjectCollection = mSearcher.Get();
                foreach (ManagementObject mObject in mObjectCollection)
                {
                    addressWidth = mObject["AddressWidth"].ToString();
                }
                if (addressWidth.Contains("64"))
                {
                    is64 = true;
                }
            }
            catch (Exception ex)
            {
            }
            return is64;
        }

        /// <summary>
        /// 获取系统内存大小
        /// </summary>
        /// <returns>double</returns>
        /// 
        public static double GetPhysicalMemory()
        {
            double sized = 0;

            ObjectQuery winQuery = new ObjectQuery("SELECT TotalVisibleMemorySize FROM CIM_OperatingSystem");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);
            ManagementObjectCollection col = searcher.Get();
            UInt64 capacity = 0;
            foreach (ManagementObject item in col)
            {
                capacity = (System.UInt64)item.GetPropertyValue("TotalVisibleMemorySize");
            }
            sized = capacity / Million;


            return sized;
        }

        /// <summary>
        /// 获取安装目录空间大小
        /// </summary>
        /// <returns>double</returns>
        public static double GetFreeSapce(String local)
        {
            ulong space = 0;
            try
            {
                ManagementClass diskClass = new ManagementClass("Win32_LogicalDisk");
                ManagementObjectCollection disks = diskClass.GetInstances();

                foreach (ManagementObject disk in disks)
                {
                    if (disk["Name"] != null)
                    {
                        if (disk["Name"].ToString() == local)
                        {
                            if (disk["FreeSpace"] != null)
                                space = (ulong)disk["FreeSpace"];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                Console.WriteLine(e.Message);
            }
            return (space / C2E20);
        }

        /// <summary>
        /// 获取分辨率
        /// </summary>
        /// <returns>string</returns>
        public static Rectangle GetResolution()
        {
            Rectangle rect = new Rectangle();
            try
            {
                rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                //rect.Width;//屏幕宽
                //rect.Height;//屏幕高
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                Console.WriteLine(e.Message);
            }
            return rect;
        }


        /// <summary>
        /// 获取系统色彩深度
        /// </summary>
        /// <returns>string</returns>
        public static int GetColorDepth()
        {
            return System.Windows.Forms.Screen.PrimaryScreen.BitsPerPixel;
        }

        /// <summary>
        /// 获取操作系统版本
        /// </summary>
        /// <returns>string</returns>
        public static string[] GetOperatingSystem()
        {

            string[] osInfo = new string[2];
            try
            {
                RegistryKey rk;
                rk = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");

                osInfo[0] = rk.GetValue("CurrentVersion").ToString();
                osInfo[1] = rk.GetValue("ProductName").ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                Console.WriteLine(ex.Message);
            }
            return osInfo;

        }

        /// <summary>
        /// 获取本机最新.netframework版本
        /// </summary>
        /// <returns>string</returns>
        public static List<string> GetNetFramework(out int i)
        {
            i = 0;
            List<string> strlist = new List<string>();
            StringBuilder sb = new StringBuilder();
            try
            {
                using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                {
                    foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                    {
                        if (versionKeyName.StartsWith("v"))
                        {

                            RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                            string name = (string)versionKey.GetValue("Version", "");
                            string sp = versionKey.GetValue("SP", "").ToString();
                            string install = versionKey.GetValue("Install", "").ToString();
                            if (install == "") //no install info, must be later.
                                Console.WriteLine(versionKeyName + "  " + name);
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    sb.AppendLine(versionKeyName + "  " + name + "  SP" + sp + " ; ");
                                    if ((versionKeyName.Contains("v2.0") && sp != "2") || (versionKeyName.Contains("v3.0") && sp != "2") || (versionKeyName.Contains("v3.5") && sp != "1"))
                                    {
                                        i = -1;
                                    }
                                    else if ((versionKeyName.Contains("v2.0") && sp == "2") || (versionKeyName.Contains("v3.0") && sp == "2") || (versionKeyName.Contains("v3.5") && sp == "1"))
                                    {
                                        if (i != -1)
                                        {
                                            i++;
                                        }
                                    }
                                }

                            }
                            if (name != "")
                            {
                                continue;
                            }
                        }
                    }
                }

                try
                {
                    using (
                        RegistryKey ndpKey =
                            Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
                    {
                        int releaseKey = (int) ndpKey.GetValue("Release");
                        {
                            if (releaseKey == 378389)

                                sb.AppendLine("v4.5 " + " ; ");

                            if (releaseKey == 378758)

                                sb.AppendLine("v4.5.1" + " ; ");

                        }
                    }
                }catch(Exception e)
                {
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
            strlist.Add(sb.ToString());
            return strlist;
        }

        /// <summary>
        /// 获取浏览器（IE6以上）
        /// </summary>
        /// <returns>string</returns>
        public static object[] GetBrowser()
        {
            string bv = "";
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Internet Explorer");

                string[] names = rk.GetValueNames();
                List<string> keySet = new List<string>(names);
                if (keySet.Contains("svcVersion"))
                {
                    bv = (string)rk.GetValue("svcVersion");
                }
                else if (keySet.Contains("Version"))
                {
                    bv = (string)rk.GetValue("Version");
                }
                else
                {
                    bv = "0.0.0";
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                Console.WriteLine(ex.Message);
                bv = "0.0.0";
            }
            double v = GetVersionNum(bv);
            object[] obj = new object[2];
            obj[0] = v;
            obj[1] = bv;
            return obj;
        }

        private static double GetVersionNum(string version)
        {
            double vsn = 0;
            try
            {
                version = version.Replace(',', '.');
                StringBuilder sb = new StringBuilder(version);

                int start = version.IndexOf('.', version.IndexOf('.') + 1);
                sb = sb.Replace(".", "", start, sb.Length - start);
                double.TryParse(sb.ToString(), out vsn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return vsn;
        }


        /// <summary>
        /// 获取字体大小
        /// </summary>
        /// <returns>string</returns>
        public static string GetFontSize()
        {
            return "满足";
        }

        /// <summary>
        /// 获取http1.1
        /// </summary>
        /// <returns>string</returns>
        public static bool IsEnableHttp1_1()
        {
            int http1_1 = 0;
            try
            {
                RegistryKey rk =
                    Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
                http1_1 = (int)rk.GetValue("EnableHttp1_1");
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                Console.WriteLine(e.Message);
            }
            return http1_1 == 1;
        }

        /// <summary>
        /// 获取系统安装的语言
        /// </summary>
        /// <returns>bool</returns>
        public static List<string> GetLanguage()
        {
            List<string> language = new List<string>();
            try
            {
                foreach (InputLanguage c in InputLanguage.InstalledInputLanguages)
                {
                    language.Add(c.Culture.Name);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                Console.WriteLine(e.Message);
            }
            return language;
        }

        public static bool ContainsChineseLanguage()
        {
            return GetLanguage().Contains("zh-CN");
        }

        /// <summary>
        /// 获取系统安装的字体
        /// </summary>
        /// <returns>bool</returns>
        public static List<string> GetFont()
        {

            List<string> fontSet = new List<string>();
            try
            {
                InstalledFontCollection MyFont = new InstalledFontCollection();
                System.Drawing.FontFamily[] MyFontFamilies = MyFont.Families;
                int Count = MyFontFamilies.Length;
                for (int i = 0; i < Count; i++)
                {
                    string FontName = MyFontFamilies[i].Name;
                    fontSet.Add(FontName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return fontSet;
        }

        public static List<string> GetAllFont()
        {
            Fonts font = new Fonts();
            font.GetFonts();
            List<FontFamily> list = font.FontFamilies;
            List<string> listSet = new List<string>();
            LogHelper.Debug("All font:");
            foreach (FontFamily f in list)
            {
                listSet.Add(f.FontName);
                LogHelper.Debug(f.FontName);
            }
            font.FontFamilies = null;

            return listSet;
        }

        public static bool ContainsYaHeiFont()
        {
            List<string> font = GetAllFont();
            return font.Contains("微软雅黑") || font.Contains("Microsoft YaHei") || font.Contains("微软雅黑(TrueType)") || font.Contains("微软雅黑 Bold");
        }

        public static bool text()
        {
            return false;
        }

        public static Point GetDpi()
        {
            Point p = new Point();
            using (ManagementClass mc = new ManagementClass("Win32_DesktopMonitor"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {

                    int PixelsPerXLogicalInch = 0; // dpi for x
                    int PixelsPerYLogicalInch = 0; // dpi for y

                    foreach (ManagementObject each in moc)
                    {
                        PixelsPerXLogicalInch = int.Parse((each.Properties["PixelsPerXLogicalInch"].Value.ToString()));
                        PixelsPerYLogicalInch = int.Parse((each.Properties["PixelsPerYLogicalInch"].Value.ToString()));
                    }
                    p.X = PixelsPerXLogicalInch;
                    p.Y = PixelsPerYLogicalInch;
                }
            }
            return p;
        }

        public static double UsingPerfmon(string pname)
        {
            double d;


            using (var p1 = new PerformanceCounter("Process", "% Processor Time", pname))
            {
                d = p1.NextValue() / Environment.ProcessorCount;
            }
            return d;

        }
    }
}
