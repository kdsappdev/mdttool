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
using System.Text;
using System.Xml;

namespace KnightsWarriorAutoupdater
{
    public class RemoteFile
    {
        #region The private fields
        private string path = "";
        private string url = "";
        private string lastver = "";
        private int size = 0;
        private bool needRestart = false;
        private string md5 = "";
        #endregion

        #region The public property
        public string Path { get { return path; } }
        public string Url { get { return url; } }
        public string LastVer { get { return lastver; } }
        public int Size { get { return size; } }
        public bool NeedRestart { get { return needRestart; } }
        public string MD5
        {
            get { return md5; }
        }

        #endregion

        #region The constructor of AutoUpdater
        public RemoteFile(XmlNode node)
        {
            this.path = node.Attributes["path"].Value;
            this.url = node.Attributes["url"].Value;
            try
            {
                this.lastver = node.Attributes["lastver"].Value;
                this.size = Convert.ToInt32(node.Attributes["size"].Value);
            }
            catch
            {
            }
            this.needRestart = Convert.ToBoolean(node.Attributes["needRestart"].Value);
            try
            {
                this.md5 = node.Attributes["md5"].Value;
            }
            catch
            {
            }
            
        }
        #endregion
    }
}
