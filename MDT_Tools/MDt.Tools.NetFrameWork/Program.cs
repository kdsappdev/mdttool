using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Win32;

namespace MDT.Tools.NetFrameWork
{
    class Program
    {
        static void Main(string[] args)
        {
            GetVersionFromRegistry();
            Get45or451FromRegistry();
            Console.ReadLine();

        }
        private static void GetVersionFromRegistry()
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
                                Console.WriteLine(versionKeyName + "  " + name + "  SP" + sp);
                            }

                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();
                            install = subKey.GetValue("Install", "").ToString();
                            if (install == "") //no install info, must be later.
                                Console.WriteLine(versionKeyName + "  " + name);
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    Console.WriteLine("  " + subKeyName + "  " + name + "  SP" + sp);
                                }
                                else if (install == "1")
                                {
                                    Console.WriteLine("  " + subKeyName + "  " + name);
                                }

                            }

                        }

                    }
                }
            }

        }
        private static void Get45or451FromRegistry()
        {
            try
            {


                using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\")
                    )
                {
                    if (ndpKey != null)
                    {
                        int releaseKey = (int) ndpKey.GetValue("Release");
                        {
                            if (releaseKey == 378389)

                                Console.WriteLine("The .NET Framework version 4.5 is installed");

                            if (releaseKey == 378758)

                                Console.WriteLine("The .NET Framework version 4.5.1  is installed");

                        }
                    }
                }
            }

            catch (Exception)
            {

                
            }
        }
    }
}
