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

namespace MDT.ThirdParty.Controls
{
    public class ConstFile
    {
        private static string appExe = "";
        public static string AppExe
        {
            get { return appName; }
            set
            {
                appExe = value;
                ROOLBACKFILE = value + ".exe";
                 
            }
        }
        private static string appName = "";
        public static string AppName
        {
            get { return appName; }
            set { appName = value;
                CANCELORNOT = string.Format("{0} 正在更新中。你真的要取消吗？",value);
                NOTNETWORK = string.Format("{0}更新不成功, {0}现在将重新启动,尝试再次更新，请单击确定重新启动程序！",value);
            }
        }
        public static string TEMPFOLDERNAME = "temp";
        public static string CONFIGFILEKEY = "config_";
        public static string FILENAME = "control\\AutoUpdater.config";
        public static string ROOLBACKFILE = "MDT Smart Kit.exe";
        public static string MESSAGETITLE = "自动升级";
        public static string CANCELORNOT = "MDT Smart Kit 正在更新中。你真的要取消吗？";
        public static string APPLYTHEUPDATE = "程序需要重新启动来应用更新，请单击确定重新启动程序！";
        public static string NOTNETWORK = "MDT Smart Kit更新不成功, MDT Smart Kit现在将重新启动,尝试再次更新，请单击确定重新启动程序！";
    }
}
