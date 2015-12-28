using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Aliyun.OpenServices.OpenStorageService;
using MDT.Tools.Aliyun.Monitor.Plugin.Model;
using MDT.Tools.Aliyun.Monitor.Plugin.Quartz;
using SuperKeys;
using System.Threading;
using MDT.Tools.Aliyun.Monitor.Plugin.Utils;
using MDT.Tools.Core.Utils;
using System.IO;
using System.Diagnostics;

namespace MDT.Tools.Aliyun.Monitor.Plugin.Monitor
{
    public partial class MonitorUI : DockContent
    {
        public string id;
        public string key;
        public string bucketName;
        public string MonitorFolder;
        private OssClient ossClient = null;
        private IFilesHelper filesHelper = new FilesHelper();
        private DataTable loadData = new DataTable();
        public string VKInterval;
        private string MName;
        private DateTime UpdateDate;
        public string DownloadPath;
        private MonitorSelect monitorSelect;
        private List<string> keys = new List<string>();
        private Dictionary<string, DateTime> dicKeys = new Dictionary<string, DateTime>();
        public MonitorUI()
        {
            InitializeComponent();
        }


        public void clearPage(int index)
        {
            if (this.tabControl1.TabPages.Count > index)
            {
                this.tabControl1.TabPages.RemoveAt(index);
            }
        }


        #region  load page
        private void MonitorUI_Load(object sender, EventArgs e)
        {
            label3.Text = "0";   
            if (monitorSelect == null)
            {
                monitorSelect = new MonitorSelect();
            }
            foreach (var load in MonitorPlugin.dicLoad)
            {
                if(this.DockHandler.TabText.Equals(load.Value.MonitorName))
                {
                    id = load.Value.id;
                    key = load.Value.key;
                    bucketName = load.Value.bucketName;
                    MonitorFolder = load.Value.MonitorFolder;
                    monitorSelect.bucketName = load.Value.bucketName;
                    monitorSelect.id = id;
                    monitorSelect.key = key;
                    monitorSelect.DownloadPath = MonitorPlugin.DownloadPath;
                    VKInterval = MonitorPlugin.VKInterval;
                    DownloadPath = MonitorPlugin.DownloadPath;
                    filesHelper.init();
                    ossClient = new OssClient(id, key);
                    tabPage1.Text = bucketName + "/" + MonitorFolder;
                    MName = Calm.getName();
                    filesHelper.Add(VKInterval, this, MName);
                    filesHelper.Start();
                    this.Disposed += new EventHandler(MonitorUI_Disposed);
                    initDataTable();
                    ConnectOssClient();
                }
            }
            tabPage2.Text = bucketName + "历史查询";
            monitorSelect.Dock = DockStyle.Fill;
            this.tabPage2.Controls.Add(monitorSelect);

        }
        #endregion

        private void initDataTable()
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

        public delegate void InitData();
        public void init()
        {
            if (InvokeRequired&&!this.IsDisposed)
            {
                Invoke(new InitData(init), new object[] { });
            }
            else
            {
                MonitorFolder = DateTime.Today.ToString("yyyyMMdd");
                tabPage1.Text = bucketName + "/" + MonitorFolder;
                
            }

        }
        void MonitorUI_Disposed(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                filesHelper.deleteJob(MName);
            });
        }

        #region  连接
        public void ConnectOssClient()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    requestOssClient();
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.Message);
                }
            });

        }


        private void requestOssClient()
        {
            bool status = false;
            init();
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
                                if (!string.IsNullOrEmpty(fi.FileName))
                                {
                                    lock (fi)
                                    {
                                        if (keys.Contains(fi.FileName))
                                        {
                                        }
                                        else
                                        {
                                            status = true;
                                            keys.Add(fi.FileName);
                                            setDateTable(fi);
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                    if (olist.IsTrunked)
                    {
                        request.Marker = olist.NextMarker;
                    }
                }
            } while (olist.IsTrunked);
            
            if (status)
            {
                UpdateDate = DateTime.Now;
            }
            textChanged();
        }
        #endregion

        private delegate void setGridView(MFileInfo fi);
        private void setDateTable(MFileInfo fi)
        {
            if (InvokeRequired && this.IsDisposed && !this.IsHandleCreated)
            {
                Invoke(new setGridView(setDateTable), new object[] { fi });

            }
            else
            {
                loadData.Rows.Add(new object[] { fi.SeqNo, fi.FileName, fi.Size, fi.LastModified, fi.MonitorName, fi.BucketName, "Y" });
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

        #region 获取最大日期
        private DateTime getMaxDate()
        {
            DateTime date = new DateTime();
            string date1 = filesHelper.queryMaxId("LastModified", MonitorFolder, bucketName);
            if (!string.IsNullOrEmpty(date1))
            {
                DateTime.TryParse(date1, out date);
            }
            else
            {
                date = DateTime.Today;
            }
            return date;

        }
        #endregion

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

        #region   dataGridView 刷新 Cell
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DateTime time = DateTime.Now;
                    string FileName = gvData.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string FileTime = gvData.Rows[e.RowIndex].Cells[3].Value.ToString();
                    DateTime fileTime =new DateTime();
                    DateTime.TryParse(FileTime, out fileTime);
                    if (!dicKeys.ContainsKey(FileName)) 
                    {
                        dicKeys.Add(FileName,fileTime);
                    }
                    if (dicKeys[FileName].AddMinutes(1) >= time)
                    {
                        e.CellStyle.BackColor = Color.Red;

                    }

                    //DataRow dr = this.loadData.Rows[e.RowIndex];
                    //if (changeDic.ContainsKey(dr["account"]))
                    //    e.CellStyle.BackColor = Color.Red;

                    //if (UpdateDate.AddMinutes(1) >= time)
                    {
                        //string name = gvData.Rows[e.RowIndex].Cells[3].Value.ToString();

                        //DateTime dt1 = new DateTime();
                        //if (DateTime.TryParse(name, out dt1))
                        {

                            //if (dt1 >= UpdateDate)
                            //{
                            //    e.CellStyle.BackColor = Color.Red;
                            //}
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message);
                }

            }
        }
        #endregion

        #region textBox 值改变
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textChanged();
        }

        private void textChanged()
        {
            try
            {
                string fileNames = "";
                string text = textBox1.Text.Trim();

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
                    for (int i = 0; i < drArr.Length; i++)
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
                label3.Text = i.ToString();
            }
        }

        #endregion

        #region 查看 按钮
        private void tsmiSelect_Click(object sender, EventArgs e)
        {
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
        #endregion

        #region 下载
        private void downloadFile(OssClient client, String bucketName, string key, string filePath)
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
                MessageBox.Show(this,"文件下载失败。");
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

        //private void tsmiDelete_Click(object sender, EventArgs e)
        //{
        //    ThreadPool.QueueUserWorkItem(o =>
        //    {
        //        try
        //        {
        //            int count = dataGridView1.SelectedRows.Count;
        //            bool b = false;
        //            for (int i = 0; i < count; i++)
        //            {
        //                DataRow dr = (dataGridView1.SelectedRows[i].DataBoundItem as DataRowView).Row;
        //                string seqNo = dr[0].ToString();
        //                if (filesHelper.update(seqNo) > 0)
        //                {
        //                    b = true;
        //                }
        //            }
        //            if (b)
        //            {
        //                loadData = filesHelper.select();
        //                textChanged();
        //            }
        //        }
        //        catch (Exception e1)
        //        {
        //            LogHelper.Error(e1.Message);
        //        }
        //    });
        //}

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int count = gvData.RowCount;
                if (count == 0)
                {
                    tsmiSelect.Enabled = false;

                }
                else
                {
                    tsmiSelect.Enabled = true;
                }
                contextMenuStrip1.Show(gvData, new Point(e.X, e.Y));
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
