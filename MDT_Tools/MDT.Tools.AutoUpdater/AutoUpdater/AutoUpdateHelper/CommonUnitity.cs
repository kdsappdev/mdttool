/*****************************************************************
 * Copyright (C) Knights Warrior Corporation. All rights reserved.
 * 
 * Author:   圣殿骑士（Knights Warrior） 
 * Email:    KnightsWarrior@msn.com
 * Website:  http://www.cnblogs.com/KnightsWarrior/       http://knightswarrior.blog.51cto.com/
 * Create Date:  5/8/2010 
 * Usage:
 *
 * RevisionHistory
 * Date         Author               Description
 * 
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace KnightsWarriorAutoupdater
{
    class CommonUnitity
    {
        public static string SystemBinUrl = AppDomain.CurrentDomain.BaseDirectory;

        private static void KillProcess(string processName)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            foreach (Process thisproc in Process.GetProcessesByName(processName))
            {
                if (!thisproc.CloseMainWindow())
                {
                    thisproc.Kill();
                    GC.Collect();
                }
                Process[] prcs = Process.GetProcesses();
                foreach (Process p in prcs)
                {
                    if (p.ProcessName.ToLower().Equals(processName.ToLower()))
                    {
                        p.Kill();
                    }
                }
            }
        }

        public static void RestartApplication()
        {
            string exe = System.Configuration.ConfigurationSettings.AppSettings["exe"];
            KillProcess(exe.Replace(".exe",""));
            Process.Start(exe);
            Environment.Exit(0);
        }

        public static string GetFolderUrl(DownloadFileInfo file)
        {
            string folderPathUrl = string.Empty;
            int folderPathPoint = file.DownloadUrl.IndexOf("/", 15) + 1;
            string filepathstring = file.DownloadUrl.Substring(folderPathPoint);
            int folderPathPoint1 = filepathstring.IndexOf("/");
            string filepathstring1 = filepathstring.Substring(folderPathPoint1 + 1);
            if (filepathstring1.IndexOf("/") != -1)
            {
                string[] ExeGroup = filepathstring1.Split('/');
                for (int i = 0; i < ExeGroup.Length - 1; i++)
                {
                    folderPathUrl += "\\" + ExeGroup[i];
                }
                if (!Directory.Exists(SystemBinUrl + ConstFile.TEMPFOLDERNAME + folderPathUrl))
                {
                    Directory.CreateDirectory(SystemBinUrl + ConstFile.TEMPFOLDERNAME + folderPathUrl);
                }
            }
            return folderPathUrl;
        }
    }
}
