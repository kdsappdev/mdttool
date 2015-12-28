using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;
using System.IO;
using Aliyun.OpenServices.OpenStorageService;
using System.Threading;
using System.Diagnostics;
using MDT.Tools.Aliyun.Monitor.Plugin.Model;

namespace MDT.Tools.Aliyun.Monitor.Plugin.Monitor
{
    public partial class MonitorSelect : UserControl
    {
        public string bucketName;
        public string DownloadPath;
        private IFilesHelper filesHelper = new FilesHelper();
        private DataTable loadData = new DataTable();
        private OssClient ossClient = null;
        public string id;
        public string key;
        private string MonitorFolder;
        public MonitorSelect()
        {
            InitializeComponent();
        }

        private void MonitorSelect_Load(object sender, EventArgs e)
        {
            dtDate.Value = DateTime.Today;
            label4.Text = "0";
            init();
        }

        private delegate void EnabeldChanged(bool b);
        private void setEnabeld(bool b)
        {
            if (InvokeRequired)
            {
                Invoke(new EnabeldChanged(setEnabeld), new object[] { b });
            }
            else
            {
                btSelect.Enabled = b;
            }
        }


        private void init() 
        {

            if (loadData.Rows.Count == 0)
            {
                DataColumn dc1 = new DataColumn("SeqNo", Type.GetType("System.Int64"));
                DataColumn dc2 = new DataColumn("FileName", Type.GetType("System.String"));
                DataColumn dc3 = new DataColumn("Size", Type.GetType("System.String"));
                DataColumn dc4 = new DataColumn("LastModified", Type.GetType("System.DateTime"));
                DataColumn dc5 = new DataColumn("MonitorName", Type.GetType("System.String"));
                DataColumn dc6 = new DataColumn("BucketName", Type.GetType("System.String"));
                DataColumn dc7 = new DataColumn("Status", Type.GetType("System.String"));
                loadData.Columns.Add(dc1);
                loadData.Columns.Add(dc2);
                loadData.Columns.Add(dc3);
                loadData.Columns.Add(dc4);
                loadData.Columns.Add(dc5);
                loadData.Columns.Add(dc6);
                loadData.Columns.Add(dc7);
            }

        }

        private void clear() //清除dataTable里的值
        {
            loadData.Rows.Clear();
            
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            clear();

            ThreadPool.QueueUserWorkItem(o =>
            {
                setEnabeld(false);
                MonitorFolder = dtDate.Text.Trim();
                if (!string.IsNullOrEmpty(MonitorFolder))
                {
                    MonitorFolder = dtDate.Value.ToString("yyyyMMdd");
                }
                //string fName = tbFileName.Text.Trim();
                //loadData = filesHelper.selectParam(bucketName, mName, fName);
                //setLabelText(loadData.Rows.Count);
                //setDataSource(loadData);
               // dataGridView1.Rows.Clear();
                try
                {
                    requestOssClient();
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message);
                }
                    setEnabeld(true);            
            });

        }


        #region  连接
        private void requestOssClient()
        {
            ObjectListing olist = null;
            if (ossClient == null)
            {
                ossClient = new OssClient(id, key);
            }
            ListObjectsRequest request = new ListObjectsRequest(bucketName);
            request.MaxKeys = 1000;
            request.Delimiter = "/";
            request.Prefix = MonitorFolder + "/";
            do
            {
                olist = ossClient.ListObjects(request);
                IEnumerable<OssObjectSummary> ossSumm = olist.ObjectSummaries;
                if (ossSumm.Count() > 0)
                {

                    foreach (OssObjectSummary obj in ossSumm)
                    {
                        //if (obj.Key.Contains(MonitorFolder))
                        {
                            MFileInfo fi = new MFileInfo();
                            string keyName = obj.Key;
                            if (keyName.IndexOf('/') > 0)
                            {
                                string[] str = keyName.Split('/');
                                fi.FileName = str[str.Length - 1];
                                fi.Size = toSize(obj.Size);
                                DateTime time = obj.LastModified;
                                time = time.AddHours(8);
                                fi.LastModified = time;
                                fi.MonitorName = MonitorFolder;
                                fi.BucketName = bucketName;
                                setDateTable(fi);
                            }
                        }
                    }
                    if (olist.IsTrunked)
                    {
                        request.Marker = olist.NextMarker;
                    }
                }
            } while (olist.IsTrunked);
          // loadData = filesHelper.select();
            textChanged();
        }
        #endregion

        private delegate void setGridView(MFileInfo fi);
        private void setDateTable(MFileInfo fi)
        {
            if (InvokeRequired)
            {
                Invoke(new setGridView(setDateTable), new object[] { fi });

            }
            else
            {
                loadData.Rows.Add(new object[] { fi.SeqNo, fi.FileName, fi.Size, fi.LastModified, fi.MonitorName, fi.BucketName, "Y" });
            }
        }

        #region toSize
        private string toSize(decimal d)
        {
            string str = "";
            decimal size = d / 1024M;
            if (size > 1024M)
            {
                size = size / 1024M;
                if (size > 1024M)
                {
                    size = size / 1024M;
                    size = size * 100;
                    size = decimal.Ceiling(size);
                    size = size / 100;
                    str = size + "G";
                }
                size = size * 100;
                size = decimal.Ceiling(size);
                size = size / 100;
                str = size + "M";
            }
            else
            {
                size = decimal.Ceiling(size);
                str = size + "KB";
            }
            return str;
        }
        #endregion

        private void tbFileName_TextChanged(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                textChanged();
            });
        }

        private void textChanged()
        {
            try
            {
                string text = tbFileName.Text.Trim();
                string fileNames = "";
                if (!string.IsNullOrEmpty(text))
                {
                    string[] strs = text.Split(' ');
                    foreach (string s in strs)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            if (string.IsNullOrEmpty(fileNames))
                            {
                                fileNames = " FileName LIKE '%" + s + "%'";
                            }
                            else
                            {
                                fileNames += " and FileName LIKE '%" + s + "%'";
                            }
                        }
                    }
                    DataTable dtNew1 = loadData.Clone();
                    DataRow[] drArr = loadData.Select(fileNames, "LastModified desc");
                    for (int i = 0; i < drArr.Length; i++)
                    {
                        dtNew1.ImportRow(drArr[i]);
                    }
                    setDataSource(dtNew1);
                    setLabelText(dtNew1.Rows.Count);
                }
                else
                {
                    DataTable dtNew2 = loadData.Clone();
                    DataRow[] drArr = loadData.Select(fileNames, "LastModified desc");
                    for (int i=0; i < drArr.Length; i++)
                    {
                        dtNew2.ImportRow(drArr[i]);
                    }
                    setDataSource(dtNew2);
                    setLabelText(dtNew2.Rows.Count);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }

        private delegate void setLabel(int i);
        private void setLabelText(int i)
        {
            if (InvokeRequired)
            {
                Invoke(new setLabel(setLabelText), new object[] { i });
            }
            else
            {
                label4.Text = i.ToString();
            }
        }

        #region  刷新数据
        private delegate void setData(DataTable dt);
        private void setDataSource(DataTable dt)
        {
            if (InvokeRequired)
            {
                Invoke(new setData(setDataSource), new object[] { dt });
            }
            else
            {
                gvData.DataSource = dt;
            }
        }
        #endregion

        private void tsmSee_Click(object sender, EventArgs e)
        {
            if (ossClient == null)
            {
                ossClient = new OssClient(id, key);
            }
            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    SeeInfo();
                }
                catch (Exception e1)
                {
                    LogHelper.Error(e1.Message);
                }
            });
        }



        private void SeeInfo()
        {
            if (string.IsNullOrEmpty(DownloadPath))
            {
                DownloadPath = System.Windows.Forms.Application.StartupPath + "\\resources\\DownloadFile";
            }
            if (!Directory.Exists(DownloadPath))
            {
                Directory.CreateDirectory(DownloadPath);
            }
            else
            {
                Directory.Delete(DownloadPath, true);
                Directory.CreateDirectory(DownloadPath);
            }
            string fileName = "";
            int i = gvData.CurrentRow.Index;
            if (i >= 0 && i < gvData.Rows.Count)
            {
                try
                {
                    DataRow dr = (gvData.SelectedRows[0].DataBoundItem as DataRowView).Row;
                    fileName = dr[1].ToString();
                    string filePath = DownloadPath + "\\" + fileName;
                    string keyName = dr[4] + "/" + fileName;
                    downloadFile(ossClient, bucketName, keyName, filePath);
                }
                catch (Exception e1)
                {
                    LogHelper.Error(e1.Message);
                }
            }
            else
            {
                MessageBox.Show(this, "请重新选择。");
            }
        }


        #region 下载
        private void downloadFile(OssClient client, string bucketName, string key, string filePath)
        {
            bool b = false;
            FileStream fs = null;
            try
            {
                GetObjectRequest downloadRequest = new GetObjectRequest(bucketName, key);
                fs = new FileStream(filePath, FileMode.OpenOrCreate);
                client.GetObject(downloadRequest, fs);
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                b = true;
                MessageBox.Show(this, "文件下载失败。");
                LogHelper.Error(ex.Message);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                    fs.Dispose();
                }
                if (b)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        if (!Directory.Exists(DownloadPath))
                        {
                            Directory.CreateDirectory(DownloadPath);
                        }
                    }
                }
            }

        }
        #endregion
    }
}
