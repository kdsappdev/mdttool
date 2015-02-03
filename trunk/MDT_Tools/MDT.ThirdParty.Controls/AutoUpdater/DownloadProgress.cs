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
using System.Web;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Xml;

namespace MDT.ThirdParty.Controls
{
    public partial class DownloadProgress : Form
    {
        #region The private fields
        private bool isFinished = false;
        private List<DownloadFileInfo> downloadFileList = null;
        private List<DownloadFileInfo> allFileList = null;
        private ManualResetEvent evtDownload = null;
        private ManualResetEvent evtPerDonwload = null;
        private WebClient clientDownload = null;

        #endregion

        #region The constructor of DownloadProgress
        public DownloadProgress(List<DownloadFileInfo> downloadFileListTemp)
        {
            InitializeComponent();

            this.downloadFileList = downloadFileListTemp;
            allFileList = new List<DownloadFileInfo>();
            foreach (DownloadFileInfo file in downloadFileListTemp)
            {
                allFileList.Add(file);
            }
        }
        #endregion

        #region The method and event
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isFinished && DialogResult.No == MessageBox.Show(ConstFile.CANCELORNOT, ConstFile.MESSAGETITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                e.Cancel = true;
                return;
            }
            else
            {
                if (clientDownload != null)
                    clientDownload.CancelAsync();

                evtDownload.Set();
                evtPerDonwload.Set();
            }
        }
        private void setUI()
        {
            label3.Text = string.Format("正在更新 {0}", ConstFile.AppName);
            Text = string.Format("{0} 更新中", ConstFile.AppName);
            label7.Text = string.Format("名称： {0}", ConstFile.AppName);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            setUI();
            evtDownload = new ManualResetEvent(true);
            evtDownload.Reset();
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcDownload));
        }

        long total = 0;
        long nDownloadedTotal = 0;
        private bool bCancel = false;
        private void ProcDownload(object o)
        {
            string tempFolderPath = Path.Combine(CommonUnitity.SystemBinUrl, ConstFile.TEMPFOLDERNAME);
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }


            evtPerDonwload = new ManualResetEvent(false);

            foreach (DownloadFileInfo file in this.downloadFileList)
            {
                total += file.Size;
            }
            try
            {
                while (!evtDownload.WaitOne(0, false) && !bCancel)
                {
                    if (this.downloadFileList.Count == 0)
                        break;
                    this.SetProcessBar(100, (int)(nDownloadedTotal * 100 / total));
                    DownloadFileInfo file = this.downloadFileList[0];

                    LogHelper.Debug(string.Format("Start Download:{0}", file.FileName));
                    //Debug.WriteLine(String.Format("Start Download:{0}", file.FileName));

                    this.ShowCurrentDownloadFileName(file.FileName);

                    //Download
                    if (clientDownload == null)
                    {
                        clientDownload = new WebClient();

                        //Added the function to support proxy
                        clientDownload.Proxy = new System.Net.WebProxy();
                        clientDownload.Proxy.Credentials = CredentialCache.DefaultCredentials;
                        clientDownload.Credentials = System.Net.CredentialCache.DefaultCredentials;
                        //End added

                        clientDownload.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
                                                                      {
                                                                          try
                                                                          {
                                                                              this.SetProcessBar(e.ProgressPercentage,
                                                                                                 (int)
                                                                                                 ((nDownloadedTotal +
                                                                                                   e.BytesReceived) * 100 /
                                                                                                  total));
                                                                          }
                                                                          catch (Exception ex)
                                                                          {
                                                                              LogHelper.Error(ex);
                                                                              //EventLog.WriteEntry("DownloadProgress", ex.Message,
                                                                              //                   EventLogEntryType.Error);

                                                                          }

                                                                      };

                        clientDownload.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) =>
                                                                    {
                                                                        try
                                                                        {
                                                                            //DealWithDownloadErrors();
                                                                            DownloadFileInfo dfile =
                                                                                e.UserState as DownloadFileInfo;
                                                                            nDownloadedTotal += dfile.Size;
                                                                            this.SetProcessBar(100,
                                                                                              (int)
                                                                                              (nDownloadedTotal * 100 /
                                                                                               total));



                                                                            evtPerDonwload.Set();
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            LogHelper.Error(ex);
                                                                            //EventLog.WriteEntry("DownloadProgress", ex.Message,
                                                                            //EventLogEntryType.Error);
                                                                        }

                                                                    };
                    }
                    evtPerDonwload.Reset();

                    //Download the folder file
                    string tempFolderPath1 = CommonUnitity.GetFolderUrl(file);
                    if (!string.IsNullOrEmpty(tempFolderPath1))
                    {
                        tempFolderPath = Path.Combine(CommonUnitity.SystemBinUrl, ConstFile.TEMPFOLDERNAME);
                        tempFolderPath += tempFolderPath1;
                    }
                    else
                    {
                        tempFolderPath = Path.Combine(CommonUnitity.SystemBinUrl, ConstFile.TEMPFOLDERNAME);
                    }

                    clientDownload.DownloadFileAsync(new Uri(file.DownloadUrl), Path.Combine(tempFolderPath, file.FileFullName), file);

                    //Wait for the download complete
                    evtPerDonwload.WaitOne();



                    //Remove the downloaded files
                    this.downloadFileList.Remove(file);
                }
                if (clientDownload != null)
                {
                    clientDownload.Dispose();
                    clientDownload = null;
                }
                if (bCancel)
                {
                    ShowErrorAndRestartApplication();
                }

            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("DownloadProgress", ex.Message,
                LogHelper.Error(ex);                                                                                  //EventLogEntryType.Error);
                ShowErrorAndRestartApplication();
                //throw;
            }

            //When the files have not downloaded,return.
            if (downloadFileList.Count > 0)
            {
                return;
            }


            foreach (DownloadFileInfo file in this.allFileList)
            {
                string tempUrlPath = CommonUnitity.GetFolderUrl(file);
                string oldPath = string.Empty;
                string newPath = string.Empty;
                if (!string.IsNullOrEmpty(tempUrlPath))
                {
                    oldPath = Path.Combine(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1), file.FileName);
                    newPath = Path.Combine(CommonUnitity.SystemBinUrl + ConstFile.TEMPFOLDERNAME + tempUrlPath, file.FileName);
                }
                else
                {
                    oldPath = Path.Combine(CommonUnitity.SystemBinUrl, file.FileName);
                    newPath = Path.Combine(CommonUnitity.SystemBinUrl + ConstFile.TEMPFOLDERNAME, file.FileName);
                }

                System.IO.FileInfo f = new FileInfo(newPath);
                if (!file.Size.ToString().Equals(f.Length.ToString()))
                {
                    LogHelper.Debug(string.Format("{0}.Size({1})!={2}.Size({3})", file.FileName, file.Size, f.Name, f.Length));
                    //EventLog.WriteEntry("DownloadProgress", string.Format("{0}.Size({1})!={2}.Size({3})",file.FileName,file.Size,f.Name,f.Length),
                    //EventLogEntryType.Error);
                    ShowErrorAndRestartApplication();
                }
            }

            LogHelper.Debug("All Downloaded");
             
            try
            {
                foreach (DownloadFileInfo file in this.allFileList)
                {
                    string tempUrlPath = CommonUnitity.GetFolderUrl(file);
                    string oldPath = string.Empty;
                    string newPath = string.Empty;

                    if (!string.IsNullOrEmpty(tempUrlPath))
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1), file.FileName);
                        newPath = Path.Combine(CommonUnitity.SystemBinUrl + ConstFile.TEMPFOLDERNAME + tempUrlPath, file.FileName);
                    }
                    else
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl, file.FileName);
                        newPath = Path.Combine(CommonUnitity.SystemBinUrl + ConstFile.TEMPFOLDERNAME, file.FileName);
                    }
                    //Added for dealing with the config file download errors
                    string newfilepath = string.Empty;
                    if (newPath.Substring(newPath.LastIndexOf(".") + 1).Equals(ConstFile.CONFIGFILEKEY))
                    {
                        if (System.IO.File.Exists(newPath))
                        {
                            if (newPath.EndsWith("_"))
                            {
                                newfilepath = newPath;
                                newPath = newPath.Substring(0, newPath.Length - 1);
                                oldPath = oldPath.Substring(0, oldPath.Length - 1);
                            }
                            File.Move(newfilepath, newPath);
                        }
                    }
                    //End added

                    if (File.Exists(oldPath))
                    {
                        MoveFolderToOld(oldPath, newPath);
                    }
                    else
                    {
                        //Edit for config_ file
                        if (!string.IsNullOrEmpty(tempUrlPath))
                        {
                            if (!Directory.Exists(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1)))
                            {
                                Directory.CreateDirectory(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1));
                                MoveFolderToOld(oldPath, newPath);
                            }
                            else
                            {
                                MoveFolderToOld(oldPath, newPath);
                            }
                        }
                        else
                        {
                            MoveFolderToOld(oldPath, newPath);
                        }

                    }
                }


            }
            catch (Exception ex)
            {

                LogHelper.Error(ex);
                 
                //EventLog.WriteEntry("DownloadProgress", ex.Message, EventLogEntryType.Error);
                ShowErrorAndRestartApplication();
            }
            finally
            {
                this.allFileList.Clear();

                if (this.downloadFileList.Count == 0)
                    Exit(true);
                else
                    Exit(false);

                evtDownload.Set();
            }
        }

        //To delete or move to old files
        void MoveFolderToOld(string oldPath, string newPath)
        {
            LogHelper.Debug(string.Format("MoveFolderToOld oldPath:{0},newPath:{1}", oldPath, newPath));
            if (File.Exists(oldPath + ".old"))
            {
                File.Delete(oldPath + ".old");
            }

            if (File.Exists(oldPath))
            {
                File.Move(oldPath, oldPath + ".old");

            }

            File.Move(newPath, oldPath);
            File.Delete(oldPath + ".old");
        }

        delegate void ShowCurrentDownloadFileNameCallBack(string name);
        private void ShowCurrentDownloadFileName(string name)
        {
            if (this.labelCurrentItem.InvokeRequired)
            {
                ShowCurrentDownloadFileNameCallBack cb = new ShowCurrentDownloadFileNameCallBack(ShowCurrentDownloadFileName);
                this.Invoke(cb, new object[] { name });
            }
            else
            {
                this.labelCurrentItem.Text = name;
            }
        }

        delegate void SetProcessBarCallBack(int current, int total);
        private void SetProcessBar(int current, int total)
        {
            if (this.progressBarCurrent.InvokeRequired)
            {
                SetProcessBarCallBack cb = new SetProcessBarCallBack(SetProcessBar);
                this.Invoke(cb, new object[] { current, total });
            }
            else
            {
                this.progressBarCurrent.Value = current;
                this.progressBarTotal.Value = total;
            }
        }

        delegate void ExitCallBack(bool success);
        private void Exit(bool success)
        {
            if (this.InvokeRequired)
            {
                ExitCallBack cb = new ExitCallBack(Exit);
                this.Invoke(cb, new object[] { success });
            }
            else
            {
                this.isFinished = success;
                this.DialogResult = success ? DialogResult.OK : DialogResult.Cancel;
                this.Close();
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {

            if (clientDownload != null)
            {
                clientDownload.CancelAsync();
            }
            bCancel = true;
            evtDownload.Set();
            evtPerDonwload.Set();

        }

        private void DealWithDownloadErrors()
        {
            try
            {
                //Test Network is OK or not.
                Config config = Config.LoadConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstFile.FILENAME));
                WebClient client = new WebClient();
                client.DownloadString(config.ServerUrl);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                // EventLog.WriteEntry("DownloadProgress", ex.Message, EventLogEntryType.Error);

                ShowErrorAndRestartApplication();
            }
        }

        private void ShowErrorAndRestartApplication()
        {
 
            CommonUnitity.ShowErrorAndRestartApplication();
        }

        #endregion
    }
}