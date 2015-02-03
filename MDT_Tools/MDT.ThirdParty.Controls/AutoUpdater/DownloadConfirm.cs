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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MDT.ThirdParty.Controls
{
    public partial class DownloadConfirm : Form
    {
        #region The private fields
        
        List<DownloadFileInfo> downloadFileList = null;
        public bool IsCanCancel
        {
            set { btnCancel.Visible = value; }
        }

        #endregion

        #region The constructor of DownloadConfirm
        public DownloadConfirm(List<DownloadFileInfo> downloadfileList)
        {
            InitializeComponent();

            downloadFileList = downloadfileList;
        }
        #endregion

        #region The private method
        private void OnLoad(object sender, EventArgs e)
        {
            setUI();
            foreach (DownloadFileInfo file in this.downloadFileList)
            {
                ListViewItem item = new ListViewItem(new string[] { file.FileName, file.LastVer, file.Size.ToString() });
            }

            this.Activate();
            this.Focus();
        }
        #endregion

        private void setUI()
        {
            label2.Text = string.Format("系统检查到有一个新版可用，你想现在就下载吗？");
            
            label3.Text = string.Format("名称： {0}", ConstFile.AppName);
        }

        

    }
}