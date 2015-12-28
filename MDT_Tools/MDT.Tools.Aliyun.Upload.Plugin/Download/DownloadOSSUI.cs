using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Aliyun.Common.Oss;
using Aliyun.OpenServices.OpenStorageService;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Collections;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.Core.Plugin;
using MDT.Tools.Aliyun.Upload.Plugin.DataMemory;
using MDT.Tools.Core.Log;
using MDT.Tools.Aliyun.Upload.Plugin.Utils;
using System.Diagnostics;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Aliyun.Upload.Plugin.Download
{
    public partial class DownloadOSSUI : DockContent
    {
        #region 属性
        private OssConfig ossConfig = null;
        private OssClient ossClient = null;
        private string bucketName = "Bucket";
        private string textpath = "";
        private string dragItemTempFileName = string.Empty;
        private bool itemDragStart = false;
        private const string DRAG_SOURCE_PREFIX = "__DragNDrop__Temp__";
        private static object objDragItem;
        private static FileSystemWatcher tempDirectoryWatcher;
        private static Hashtable watchers = null;
        public AliyunUp AliyunUp { get; set; }
        private XMLDataMemory _dataMemory = new XMLDataMemory();
        private static string ModuleName = "Download";
        private ILog logHelper;
        private mySorter sorter = null;
        #endregion

        public DownloadOSSUI()
        {
            InitializeComponent();
        }

        #region Load
        private void DownloadOSSUI_Load(object sender, EventArgs e)
        {
            listView1.AllowDrop = true;
            sorter = new mySorter();
            this.listView1.ListViewItemSorter = sorter;
            sorter.SortOrder = SortOrder.Ascending;
            ossConfig = new OssConfig();
            logHelper = new TextLogHelper(txtMsg, this);
            tempDirectoryWatcher = new FileSystemWatcher();
            tempDirectoryWatcher.Path = Path.GetTempPath();
            tempDirectoryWatcher.Filter = string.Format("{0}*.tmp", DRAG_SOURCE_PREFIX);
            tempDirectoryWatcher.NotifyFilter = NotifyFilters.FileName;
            tempDirectoryWatcher.IncludeSubdirectories = false;
            tempDirectoryWatcher.EnableRaisingEvents = true;
            tempDirectoryWatcher.Created += new FileSystemEventHandler(TempDirectoryWatcherCreated);
            btnBackoff.Enabled = false;
            listView1.MouseDown += new MouseEventHandler(listView1_MouseDown);
            GetMemory();

        }
        #endregion

        #region  鼠标点击
        void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListView list = sender as ListView;
                if (list.HitTest(e.X, e.Y).Item == null)
                {
                    if (!string.IsNullOrEmpty(textpath))
                    {
                        if (listView1.Items.Count > 0)
                        {
                            tsmiSelectAll.Enabled = true;

                        }
                        else
                        {
                            tsmiSelectAll.Enabled = false;

                        }
                        tsmRefresh.Enabled = true;
                        tsmDelete.Enabled = false;
                        tsmiURL.Enabled = false;

                    }
                    else
                    {
                        tsmRefresh.Enabled = false;
                        tsmiSelectAll.Enabled = false;
                        tsmDelete.Enabled = false;
                        tsmiURL.Enabled = false;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(textpath))
                    {
                        tsmRefresh.Enabled = true;
                        tsmiSelectAll.Enabled = true;
                        tsmDelete.Enabled = true;
                        tsmiURL.Enabled = true;
                    }
                    else
                    {
                        tsmRefresh.Enabled = false;
                        tsmiSelectAll.Enabled = false;
                        tsmDelete.Enabled = false;
                        tsmiURL.Enabled = false;
                    }
                }
                if (listView1.Items.Count > 0)
                {
                    tsmiSee.Enabled = true;
                }
                else
                {
                    tsmiSee.Enabled = false;
                }
                contextMenuStrip1.Show(list, new Point(e.X, e.Y));
            }
        }
        #endregion

        #region 连接 按钮 事件
        private Dictionary<string, List<OssSummary>> dicfile = new Dictionary<string, List<OssSummary>>();
        private Dictionary<string, List<string>> dicFolder = new Dictionary<string, List<string>>();
        private void tsbConnect_Click(object sender, EventArgs e)
        {

            if (dicfile.Count > 0)
            {
                dicfile.Clear();
            }
            if (dicFolder.Count > 0)
            {
                dicFolder.Clear();
            }
            LargeView1();
            string id = teaccessId.Text.Trim();
            string key = teaccesskey.Text.Trim();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
            {
                SetMsg("Access key Id 和 Access key 不能为空。");
                return;
            }
            setEnabled(false);
            textpath = "";
            bucketName = "Bucket";
            AddLinklabel(textpath);
            btnBackoff.Enabled = false;
            setNumberText("");
            ThreadPool.QueueUserWorkItem(o =>
            {
                ConnectOssClient();
            });

        }
        #endregion

        #region Memory
        private void GetMemory()
        {
            teaccessId.Text = GetMemory(MemoryName.AccessKeyId);
            teaccesskey.Text = GetMemory(MemoryName.AccessKey);
        }

        private string GetMemory(MemoryName mn)
        {
            return _dataMemory.GetData(string.Format("{0}_{1}", ModuleName, mn));
        }

        private void SetMemory()
        {
            string id = teaccessId.Text.Trim();
            string key = teaccesskey.Text.Trim();
            SetMemory(MemoryName.AccessKeyId, id);
            SetMemory(MemoryName.AccessKey, key);
        }

        private void SetMemory(MemoryName mn, string value)
        {
            _dataMemory.SetData(string.Format("{0}_{1}", ModuleName, mn), value);
        }
        #endregion

        #region  按钮禁用
        private delegate void setEnabledChanage(bool b);
        private void setEnabled(bool b)
        {
            if (InvokeRequired)
            {
                Invoke(new setEnabledChanage(setEnabled), new object[] { b });
            }
            else
            {
                tsbConnect.Enabled = b;
            }
        }
        #endregion

        #region 代理刷新RichTextBox
        private void SetMsg(string msg)
        {
            if (this.InvokeRequired)
            {
                Action<string> method = new Action<string>(SetMsg);
                if (!this.IsDisposed && this.IsHandleCreated)
                    this.Invoke(method, new object[] { msg });
            }
            else
            {
                if (string.IsNullOrEmpty(msg))
                {
                    txtMsg.Clear();
                }
                else
                {
                    string DateTimeFormat = "yyyy/MM/dd HH:mm:ss.fff";
                    msg = string.Format("{0} DEBUG: {1}", DateTime.Now.ToString(DateTimeFormat), msg);
                    txtMsg.AppendText(msg + "\n");
                    txtMsg.ScrollToCaret();
                }
            }
        }
        #endregion

        #region  listvie清除
        private void LargeView1()
        {
            if (InvokeRequired)
            {
                Invoke(new setDetailsInfo(setDetails), new object[] { null });
            }
            else
            {
                this.listView1.Clear();
                this.listView1.Items.Clear();
            }
        }
        #endregion

        #region 连接
        private void ConnectOssClient()
        {
            string id = teaccessId.Text.Trim();
            string key = teaccesskey.Text.Trim();
            ossConfig.AccessId = id;
            ossConfig.AccessKey = key;

            IEnumerable<Bucket> buck = null;
            try
            {
                ossClient = new OssClient(id, key);
                buck = ossClient.ListBuckets();
                List<string> bList = new List<string>();
                foreach (Bucket bu in buck)
                {
                    bList.Add(bu.Name);
                    //ObjectListing olist = null;
                    //ListObjectsRequest request = new ListObjectsRequest(bu.Name);
                    //request.MaxKeys = 1000;
                    //do
                    //{
                    //    olist = ossClient.ListObjects(request);
                    //    IEnumerable<OssObjectSummary> ossSumm = olist.ObjectSummaries;
                    //    if (ossSumm.Count() > 0)
                    //    {
                    //        foreach (OssObjectSummary obj in ossSumm)
                    //        {
                    //            setStrSplit(bu.Name, obj);
                    //        }
                    //        if (olist.IsTrunked)
                    //        {
                    //            request.Marker = olist.NextMarker;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        List<OssSummary> lis = new List<OssSummary>();
                    //        OssSummary os = new OssSummary();
                    //        lis.Add(os);
                    //        dicfile.Add(bu.Name, lis);
                    //    }
                    //} while (olist.IsTrunked);
                }
                SetMsg("连接成功。");
                dicFolder.Add("Bucket", bList);
                setDetails(bucketName);
                setEnabled(true);
                SetMemory();
            }
            catch (Exception ex)
            {
                logHelper.Error(ex.Message);
                setEnabled(true);
            }
        }
        #endregion

        #region 分割路径
        private void setStrSplit(string buckeName, OssObjectSummary oss)
        {
            try
            {
                string str = oss.Key;
                string[] sList = str.Split('/');
                int len = sList.Length;
                for (int i = 0; i < len; i++)
                {
                    if (i < len - 1)
                    {

                        if (!dicFolder.ContainsKey(buckeName))
                        {
                            List<string> lis = new List<string>();
                            lis.Add(sList[i]);
                            dicFolder.Add(buckeName, lis);
                        }
                        else
                        {
                            List<string> s = dicFolder[buckeName];
                            if (!s.Contains(sList[i]))
                            {
                                s.Add(sList[i]);
                            }
                        }
                    }
                    else if (i == len - 1)
                    {

                        if (!dicfile.ContainsKey(buckeName))
                        {
                            List<OssSummary> lis = new List<OssSummary>();
                            OssSummary os = toPo(oss);
                            os.fileName = sList[i];
                            lis.Add(os);
                            dicfile.Add(buckeName, lis);
                        }
                        else
                        {
                            List<OssSummary> s = dicfile[buckeName];
                            OssSummary os = toPo(oss);
                            os.fileName = sList[i];
                            if (!s.Contains(os))
                            {
                                s.Add(os);
                            }
                        }
                    }
                    buckeName += "/" + sList[i];
                }
            }
            catch (Exception e)
            {
                logHelper.Error(e.Message);
            }
        }
        #endregion

        #region  toPo
        private OssSummary toPo(OssObjectSummary oos)
        {
            OssSummary os = new OssSummary();
            os.Size = oos.Size;
            os.LastModified = oos.LastModified;
            return os;
        }
        #endregion


        #region  请求阿里云文件
        private void requestFiles(string path)
        {
            ObjectListing olist = null;
            string bucketName = "";
            string filePath = "";
            if (string.IsNullOrEmpty(textpath))
            {
                bucketName = path;
            }
            else
            {
                int i = path.IndexOf('/');
                if (i > 0)
                {
                    bucketName = path.Substring(0, i);
                    filePath = path.Substring(i + 1);
                }
                else
                {
                    bucketName = path;
                }
            }
            if (dicFolder.ContainsKey(path))
            {
                dicFolder.Remove(path);
            }
            if (dicfile.ContainsKey(path))
            {
                dicfile.Remove(path);
            }
            ListObjectsRequest request = new ListObjectsRequest(bucketName);
            request.MaxKeys = 1000;
            request.Delimiter = "/";
            if (filePath.Length > 0)
            {
                request.Prefix = filePath + "/";
            }

            do
            {
                olist = ossClient.ListObjects(request);
                IEnumerable<OssObjectSummary> ossSumm = olist.ObjectSummaries;
                IEnumerable<string> ieStrs = olist.CommonPrefixes;
                if (ossSumm.Count() > 0)
                {
                    foreach (OssObjectSummary obj in ossSumm)
                    {
                        setStrSplit(bucketName, obj);
                    }
                    if (olist.IsTrunked)
                    {
                        request.Marker = olist.NextMarker;
                    }
                }
                else
                {
                    List<OssSummary> lis = new List<OssSummary>();
                    OssSummary os = new OssSummary();
                    lis.Add(os);
                    if (filePath.Length == 0)
                    {
                        dicfile.Add(bucketName, lis);
                    }
                    else
                    {
                        dicfile.Add(path, lis);
                    }
                }

                
                string key = bucketName;
                if (!string.IsNullOrEmpty(filePath))
                {
                    key += "/" + filePath;
                }
                foreach (string str in ieStrs)
                {
                    splitFolder(key, str);
                }

            } while (olist.IsTrunked);
        }
        #endregion

        private void splitFolder(string key,string str)
        {

            string s = "";
            int i = str.IndexOf('/');
            if (i > 0)
            {
                string[] strs = str.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length == 1)
                {
                    s = strs[0];
                }
                else
                {
                    s = strs[strs.Length - 1];
                }
            }
            else
            {
                s = str;
            }
            if (!dicFolder.ContainsKey(key))
            {
                List<string> list = new List<string>();
                list.Add(s);
                dicFolder.Add(key, list);
            }
            else
            {
                List<string> list = dicFolder[key];
                list.Add(s);
            }
        }

        #region 详细 显示
        private delegate void setDetailsInfo(string name);
        private void setDetails(string name)
        {
            if (InvokeRequired)
            {
                Invoke(new setDetailsInfo(setDetails), new object[] { name });
            }
            else
            {
                this.listView1.Clear();
                this.listView1.Items.Clear();
                this.listView1.BeginUpdate();
                this.listView1.SmallImageList = this.imageList1;
                listView1.View = View.Details;
                this.listView1.FullRowSelect = true;
                ColumnHeader co = new ColumnHeader();
                co.Text = "名称";
                this.listView1.Columns.Add("名称", 200, HorizontalAlignment.Left);
                this.listView1.Columns.Add("类型", 120, HorizontalAlignment.Left);
                this.listView1.Columns.Add("大小", 120, HorizontalAlignment.Left);
                this.listView1.Columns.Add("修改时间", 120, HorizontalAlignment.Left);
                string fileName = "";
                string countText = "";
                if (!string.IsNullOrEmpty(name))
                {
                    if (name.IndexOf('/') > 0)
                    {
                        string[] file = name.Split('/');
                        fileName = file[file.Length - 1];
                    }
                    else
                    {
                        fileName = name;
                        if (fileName == "Bucket")
                        {
                            fileName = "";
                        }

                    }

                    countText = fileName;
                }
                if (dicFolder.ContainsKey(name))
                {
                    List<string> folders = dicFolder[name];
                    int count = folders.Count;
                    countText += "  文件夹：" + count;
                    for (int j = 0; j < count; j++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.ImageIndex = 0;
                        lvi.Text = folders[j];
                        lvi.SubItems.Add("文件夹");
                        lvi.SubItems.Add("");
                        lvi.SubItems.Add("");
                        this.listView1.Items.Add(lvi);
                    }
                }
                if (dicfile.ContainsKey(name))
                {
                    List<OssSummary> files = dicfile[name];
                    int count = files.Count;
                    if (count == 1 && !string.IsNullOrEmpty(files[0].fileName))
                    {
                        countText += "  文件：" + count;
                    }
                    else if (count > 1)
                    {
                        countText += "  文件：" + count;
                    }
                    for (int j = 0; j < count; j++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        if (!string.IsNullOrEmpty(files[j].fileName))
                        {
                            lvi.ImageIndex = 0;
                            lvi.Text = files[j].fileName;
                            string[] str = files[j].fileName.Split('.');
                            lvi.SubItems.Add(str[str.Length - 1]);
                            string tempFileExtension = "." + str[str.Length - 1];
                            if (!string.IsNullOrEmpty(tempFileExtension))
                            {
                                if (!imageList1.Images.Keys.Contains(tempFileExtension))
                                {
                                    try
                                    {
                                        Icon icon = Icons.IconFromExtension(tempFileExtension, Icons.SystemIconSize.Small);
                                        if (icon != null)
                                            imageList1.Images.Add(tempFileExtension, icon);
                                    }
                                    catch (Exception ex)
                                    {
                                        //logHelper.Error(ex.Message);
                                    }
                                }
                                int i = imageList1.Images.Keys.IndexOf(tempFileExtension);
                                if (i == -1)
                                {
                                    lvi.ImageIndex = 5;
                                }
                                else
                                {
                                    lvi.ImageIndex = i;
                                }
                            }
                            decimal size = files[j].Size / 1024M;
                            if (size > 1024M)
                            {
                                size = size / 1024M;
                                if (size > 1024M)
                                {
                                    size = size / 1024M;
                                    size = size * 100;
                                    size = decimal.Ceiling(size);
                                    size = size / 100;
                                    lvi.SubItems.Add(size + "G");
                                }
                                size = size * 100;
                                size = decimal.Ceiling(size);
                                size = size / 100;
                                lvi.SubItems.Add(size + "M");
                            }
                            else
                            {
                                size = decimal.Ceiling(size);
                                lvi.SubItems.Add(size + "KB");
                            }
                            lvi.SubItems.Add(files[j].LastModified.AddHours(8) + "");
                            this.listView1.Items.Add(lvi);
                        }

                    }
                }
                this.listView1.EndUpdate();

                this.sorter.SortColumn = 3;
                this.sorter.SortOrder = SortOrder.Descending;
                this.listView1.Sort();
                setNumberText(countText);
            }
        }
        #endregion

        #region  setList
        private void setList(string name)
        {

            if (dicFolder.ContainsKey(name))
            {
                List<string> folders = dicFolder[name];
                int count = folders.Count;
                for (int j = 0; j < count; j++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = 0;
                    lvi.Text = folders[j];
                    this.listView1.Items.Add(lvi);
                }
            }
            if (dicfile.ContainsKey(name))
            {
                List<OssSummary> files = dicfile[name];
                int count = files.Count;
                for (int j = 0; j < count; j++)
                {
                    ListViewItem lvi = new ListViewItem();
                    if (!string.IsNullOrEmpty(files[j].fileName))
                    {
                        lvi.ImageIndex = 0;
                        lvi.Text = files[j].fileName;
                        string[] str = files[j].fileName.Split('.');
                        string tempFileExtension = "." + str[str.Length - 1];
                        if (!string.IsNullOrEmpty(tempFileExtension))
                        {
                            if (!imageList1.Images.Keys.Contains(tempFileExtension))
                            {
                                try
                                {
                                    Icon icon = Icons.IconFromExtension(tempFileExtension, Icons.SystemIconSize.Small);
                                    if (icon != null)
                                        imageList1.Images.Add(tempFileExtension, icon);
                                }
                                catch (Exception ex)
                                {
                                    //logHelper.Error(ex.Message);
                                }
                            }
                            int i = imageList1.Images.Keys.IndexOf(tempFileExtension);
                            if (i == -1)
                            {
                                lvi.ImageIndex = 5;
                            }
                            else
                            {
                                lvi.ImageIndex = i;
                            }

                        }
                        this.listView1.Items.Add(lvi);
                    }


                }
            }

        }
        #endregion

        #region 双击按钮
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
                if (info.Item != null)
                {
                    var videoitem = info.Item as ListViewItem;
                    string name = videoitem.Text;
                    string bucket = textpath;
                    string key = "";
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (string.IsNullOrEmpty(bucket))
                        {
                            bucket = name;
                            key = "Bucket";
                        }
                        else
                        {
                            bucket += "/" + name;
                            key = textpath;
                        }
                        ThreadPool.QueueUserWorkItem(o =>
                        {
                            if (dicFolder.ContainsKey(key))
                            {
                                if (dicFolder[key].Contains(name))
                                {
                                    requestFiles(bucket);
                                }
                            }
                            if (dicFolder.ContainsKey(bucket) || dicfile.ContainsKey(bucket))
                            {
                                textpath = bucket;
                                bucketName = bucket;
                                AddLinklabel(textpath);
                                setDetails(textpath);
                                if (bucket == "Bucket")
                                {
                                    BtnEnabled(false);
                                }
                                else
                                {
                                    BtnEnabled(true);
                                }
                            }
                        });
                    }

                }
            }
        }
        #endregion

        private void BtnEnabled(bool b)
        {
            if (InvokeRequired)
            {
                Invoke(new setEnabledChanage(BtnEnabled), new object[] { b });
            }
            else
            {
                btnBackoff.Enabled = b;
            }
        }


        #region 显示路径
        private delegate void AddStr(string s);
        private void AddLinklabel(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new AddStr(AddLinklabel), new object[] { s });
            }
            else
            {
                if (!string.IsNullOrEmpty(s))
                {
                    string[] paths = s.Split('/');
                    int length = s.Length;
                    int i = paths.Length;
                    if (length > 50)
                    {
                        s = paths[0] + "/" + paths[1] + "...." + paths[i - 2] + "/" + paths[i - 1];
                        if (s.Length > 50)
                        {
                            s = paths[0] + "/" + "...." + "/" + paths[i - 1];
                        }
                    }
                }

                label3.Text = s;
            }
        }
        #endregion

        #region 上一次 按钮
        private void btnBackoff_Click(object sender, EventArgs e)
        {
            string[] paths = textpath.Split('/');
            int len = paths.Length;
            if (paths.Count() > 0)
            {
                if (len >= 2)
                {
                    string path = "";
                    int lastLen = textpath.LastIndexOf('/');
                    path = textpath.Substring(0, lastLen);
                    if (dicFolder.ContainsKey(path) || dicfile.ContainsKey(path))
                    {
                        setDetails(path);
                        bucketName = path;
                        textpath = path;
                        AddLinklabel(textpath);
                    }
                }
                else
                {
                    setDetails("Bucket");
                    bucketName = "Bucket";
                    textpath = "";
                    AddLinklabel(textpath);
                }

                if (textpath == "" || textpath == "Bucket")
                {
                    btnBackoff.Enabled = false;
                }
            }
        }
        #endregion

        #region 下载
        private void downloadFile(OssClient client, String bucketName, String key, String filename, string file_Name)
        {
            GetObjectRequest downloadRequest = new GetObjectRequest(bucketName, key);

            int last = filename.LastIndexOf('/');
            string path = "";
            if (last > 0)
            {
                path = filename.Substring(0, last);
            }
            else
            {
                path = filename;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.OpenOrCreate);
                client.GetObject(downloadRequest, fs);
                SetMsg(file_Name + "文件下载完成");
            }
            catch (Exception ex)
            {
                SetMsg(file_Name + "文件下载失败");
                logHelper.Error(ex.Message);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                    fs.Dispose();
                }
            }

        }
        #endregion

        #region 组装路径下载
        private void fileSplit(string name, string path, string itmeName)
        {
            string key = "";
            string bucketNmae = "";
            string bucket = name;
            if (dicFolder.ContainsKey(bucket))
            {
                List<string> list = dicFolder[bucket];
                foreach (string str in list)
                {
                    string keyStr = bucket + "/" + str;
                    fileSplit(keyStr, path, itmeName);
                }
            }

            if (dicfile.ContainsKey(bucket))
            {
                List<OssSummary> list = dicfile[bucket];
                foreach (OssSummary os in list)
                {
                    if (string.IsNullOrEmpty(os.fileName))
                    {
                        string yunPath = getSelectFile(bucket, itmeName);
                        if (yunPath == bucket)
                        {
                            path = path + "/" + yunPath;
                        }
                        else
                        {
                            path = path + yunPath;
                        }
                        if (!Directory.Exists(path))
                        {
                            string[] strs = bucket.Split('/');
                            Directory.CreateDirectory(path);
                            SetMsg(strs[strs.Length - 1] + "文件创建成功。");
                        }

                    }
                    else
                    {
                        string filePath = "";
                        filePath = path;
                        int len = bucket.IndexOf('/');
                        string fileName = bucket;
                        if (len > 0)
                        {
                            bucketNmae = bucket.Substring(0, len);
                            int end = bucket.Length - len - 1;
                            len = len + 1;
                            key = bucket.Substring(len, end);
                            fileName = getSelectFile(bucket, itmeName);
                        }
                        else
                        {
                            bucketNmae = bucket;
                            fileName = "/" + fileName;
                        }
                        if (!string.IsNullOrEmpty(fileName))

                            filePath = filePath + fileName + "/" + os.fileName;
                        string keyName = "";
                        if (string.IsNullOrEmpty(key))
                        {
                            keyName = os.fileName;
                        }
                        else
                        {
                            keyName = key + "/" + os.fileName;
                        }
                        SetMsg(os.fileName + "文件 开始下载...");
                        downloadFile(ossClient, bucketNmae, keyName, filePath, os.fileName);
                        AliyunUp.SetProgress((int)os.Size);
                    }

                }
            }
            if (!dicFolder.ContainsKey(bucket) && !dicfile.ContainsKey(bucket))
            {
                string path1 = path + "/" + itmeName;
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
            }

        }
        #endregion

        #region 组装路径
        private string getSelectFile(string bucket, string itmeName)
        {
            string[] files = bucket.Split('/');
            bool status = false;
            string name = "";
            int leng = files.Length;

            for (int i = 0; i < leng; i++)
            {
                if (i == 0 && files[i].ToString() == itmeName)
                {
                    return bucket;
                }
                else if (files[i].ToString() == itmeName)
                {
                    status = true;
                }
                if (status)
                {
                    name += "/" + files[i];
                }
            }
            return name;
        }
        #endregion

        #region 以文件下载
        private void downloadFiles(string bucket, string fileName, string path)
        {
            string bucketNmae = "";
            string key = "";
            if (dicfile.ContainsKey(bucket))
            {
                List<OssSummary> list = dicfile[bucket];
                foreach (OssSummary os in list)
                {

                    string filePath = "";
                    filePath = path;
                    int len = bucket.IndexOf('/');
                    if (len > 0)
                    {
                        bucketNmae = bucket.Substring(0, len);
                        int end = bucket.Length - len - 1;
                        len = len + 1;
                        key = bucket.Substring(len, end);
                    }
                    else
                    {
                        bucketNmae = bucket;
                    }

                    filePath = filePath + "/" + os.fileName;


                    string keyName = "";
                    if (string.IsNullOrEmpty(key))
                    {
                        keyName = os.fileName;
                    }
                    else
                    {
                        keyName = key + "/" + os.fileName;
                    }
                    if (os.fileName == fileName)
                    {
                        SetMsg(os.fileName + "文件 开始下载...");
                        downloadFile(ossClient, bucketNmae, keyName, filePath, os.fileName);
                    }
                }
            }

        }
        #endregion

        #region  判断是文件还是文件夹
        private bool getIsFile(string name)
        {
            string bucket = textpath;
            bool status = false;
            if (!string.IsNullOrEmpty(name))
            {
                if (dicfile.ContainsKey(bucket))
                {
                    List<OssSummary> oss = dicfile[bucket];

                    var lt = oss.Where(o =>
                    {
                        if (o.fileName == name)
                        {
                            return true;
                        }
                        return false;
                    }).ToList();
                    if (lt.Count > 0)
                    {
                        status = true;
                    }
                }
            }

            return status;
        }
        #endregion

        #region FileSystemWatcher Methods
        private void TempDirectoryWatcherCreated(object sender, FileSystemEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(o =>
              {
                  CreateFileWatchers();
              });
        }

        private void FileWatcherCreated(object sender, FileSystemEventArgs e)
        {
            try
            {

                OnFileDropPathFound(e.FullPath);

            }
            catch (Exception ex)
            {
                LogHelper.Error("FileWatcherCreated:" + ex.Message);
            }
        }

        #region  根据路径下载
        private void OnFileDropPathFound(string dropedFilePath)
        {
            if (dropedFilePath.Trim() != string.Empty && objDragItem != null)
            {
                string dropPath = dropedFilePath.Substring(0, dropedFilePath.LastIndexOf('\\'));

                if (File.Exists(dropedFilePath))
                    File.Delete(dropedFilePath);
                int lenth = listView1.SelectedItems.Count;
                List<string> list = new List<string>();
                string[] itmeNames = null;

                if (lenth > 1)
                {
                    itmeNames = new string[lenth];
                    for (int i = 0; i < lenth; i++)
                    {
                        itmeNames[i] = listView1.SelectedItems[i].Text;
                    }
                }
                else
                {
                    itmeNames = new string[1];
                    string itmeName = objDragItem.ToString();
                    itmeNames[0] = itmeName;
                }
                ThreadPool.QueueUserWorkItem(o =>
                {
                    logHelper.Info("下载路径：" + dropPath);
                    if (itmeNames != null && itmeNames.Count() > 0)
                    {
                        foreach (string itmeName in itmeNames)
                        {
                            if (getIsFile(itmeName))
                            {
                                logHelper.Info("下载文件：" + itmeName);
                                downloadFiles(textpath, itmeName, dropPath);
                            }
                            else
                            {
                                string bucket = textpath;
                                if (string.IsNullOrEmpty(bucket))
                                {
                                    bucket = itmeName;
                                }
                                else
                                {
                                    bucket += "/" + itmeName;
                                }
                                logHelper.Info("下载文件夹：" + itmeName);
                                SetMsg(itmeName + "文件夹开始下载 ");
                                try
                                {
                                    fileSplit(bucket, dropPath, itmeName);
                                    SetMsg(itmeName + "文件夹完成下载 ");
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error("fileSplit:" + ex.Message);
                                }

                            }
                        }
                    }
                    else
                    {
                        SetMsg("请选择文件。");
                    }
                });
            }
            objDragItem = null;

        }
        #endregion



        public void CreateFileWatchers()
        {
            try
            {
                if (watchers == null)
                {
                    int i = 0;
                    Hashtable tempWatchers = new Hashtable();
                    FileSystemWatcher watcher;
                    string[] drives = Directory.GetLogicalDrives();
                    foreach (string driveName in drives)
                    {
                        if (Directory.Exists(driveName))
                        {
                            watcher = new FileSystemWatcher();
                            watcher.Filter = string.Format("{0}{1}.tmp", DRAG_SOURCE_PREFIX, listView1.SelectedItems[0].Text);
                            watcher.NotifyFilter = NotifyFilters.FileName;
                            watcher.Created += new FileSystemEventHandler(FileWatcherCreated);
                            watcher.IncludeSubdirectories = true;
                            watcher.Path = driveName;
                            watcher.EnableRaisingEvents = true;
                            tempWatchers.Add("file_watcher" + i.ToString(), watcher);
                            i = i + 1;
                        }
                    }
                    watchers = tempWatchers;
                    tempWatchers = null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("CreateFileWatchers:" + ex.Message);
            }
        }

        public void ClearFileWatchers()
        {
            try
            {
                if (watchers != null && watchers.Count > 0)
                {
                    for (int i = 0; i < watchers.Count; i++)
                    {
                        ((FileSystemWatcher)watchers["file_watcher" + i.ToString()]).Dispose();
                    }
                    watchers.Clear();
                    watchers = null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClearFileWatchers:" + ex.Message);
            }
        }

        #endregion

        #region DragMethods
        private void ClearDragData()
        {
            try
            {
                if (File.Exists(dragItemTempFileName))
                    File.Delete(dragItemTempFileName);
                objDragItem = null;
                dragItemTempFileName = string.Empty;
                itemDragStart = false;
                ClearFileWatchers();
            }
            catch (Exception ex)
            {
                logHelper.Error(ex.Message);
            }
        }

        private void CreateDragItemTempFile(string dragItemTempFileName)
        {
            FileStream fsDropFile = null;

            try
            {
                fsDropFile = new FileStream(dragItemTempFileName, FileMode.Create);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "DragNDrop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (fsDropFile != null)
                {
                    fsDropFile.Flush();
                    fsDropFile.Close();
                    fsDropFile.Dispose();
                }
            }
        }

        #endregion

        #region 鼠标指针移动事件
        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
                return;
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                return;
            }
            if (itemDragStart && objDragItem != null)
            {
                if (listView1.SelectedItems.Count > 0 && !string.IsNullOrEmpty(Path.GetTempPath()))
                    dragItemTempFileName = string.Format("{0}{1}{2}.tmp", Path.GetTempPath(), DRAG_SOURCE_PREFIX, listView1.SelectedItems[0].Text);
                else
                    return;
                try
                {

                    CreateDragItemTempFile(dragItemTempFileName);
                    string[] fileList = new string[] { dragItemTempFileName };
                    DataObject fileDragData = new DataObject(DataFormats.FileDrop, fileList);
                    DoDragDrop(fileDragData, DragDropEffects.Move);

                    ClearDragData();
                }
                catch (Exception ex)
                {
                    LogHelper.Error("MouseMove:" + ex.Message);
                }
            }
        }
        #endregion

        #region 鼠标移动事件
        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ClearDragData();
            if (e.Button == MouseButtons.Left && listView1.SelectedItems.Count > 0)
            {
                objDragItem = listView1.SelectedItems[0].Text;
                itemDragStart = true;
            }
        }

        #endregion

        #region  大图标显示
        private void LargeView(string bucketName)
        {
            if (InvokeRequired)
            {
                Invoke(new setDetailsInfo(LargeView), new object[] { bucketName });
            }
            else
            {
                this.listView1.Clear();
                this.listView1.Items.Clear();
                this.listView1.View = View.LargeIcon;
                this.listView1.LargeImageList = this.imageList2;
                this.sorter.SortColumn = 0;
                this.listView1.BeginUpdate();

                if (dicFolder.ContainsKey(bucketName))
                {
                    List<string> folders = dicFolder[bucketName];
                    int count = folders.Count;
                    for (int j = 0; j < count; j++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = folders[j];

                        lvi.ImageIndex = 0;
                        this.listView1.Items.Add(lvi);
                    }
                }
                if (dicfile.ContainsKey(bucketName))
                {
                    List<OssSummary> files = dicfile[bucketName];
                    int count = files.Count;
                    for (int j = 0; j < count; j++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        if (!string.IsNullOrEmpty(files[j].fileName))
                        {
                            lvi.ImageIndex = 0;
                            lvi.Text = files[j].fileName;
                            string[] str = files[j].fileName.Split('.');
                            string tempFileExtension = "." + str[str.Length - 1];
                            if (!string.IsNullOrEmpty(tempFileExtension))
                            {
                                if (!imageList2.Images.Keys.Contains(tempFileExtension))
                                {
                                    try
                                    {
                                        Icon icon = Icons.IconFromExtension(tempFileExtension, Icons.SystemIconSize.Small);
                                        if (icon != null)
                                            imageList2.Images.Add(tempFileExtension, icon);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                int i = imageList2.Images.Keys.IndexOf(tempFileExtension);
                                if (i == -1)
                                {
                                    lvi.ImageIndex = 2;
                                }
                                else
                                {
                                    lvi.ImageIndex = i;
                                }
                            }
                            this.listView1.Items.Add(lvi);
                        }

                    }
                }
                this.listView1.EndUpdate();
                this.sorter.SortColumn = 0;
                this.sorter.SortOrder = SortOrder.Descending;
                this.listView1.Sort();

            }
        }
        #endregion

        private void tsmDetails_Click(object sender, EventArgs e)
        {
            setDetails(bucketName);
        }

        private void tsmLarge_Click(object sender, EventArgs e)
        {
            LargeView(bucketName);
        }

        private void tsmSmaill_Click(object sender, EventArgs e)
        {
            SmaillView(bucketName);
        }

        private void tsmList_Click(object sender, EventArgs e)
        {
            ListView(bucketName);
        }

        #region 小图标显示
        private void SmaillView(string bucketName)
        {
            if (InvokeRequired)
            {
                Invoke(new setDetailsInfo(SmaillView), new object[] { bucketName });
            }
            else
            {
                this.listView1.Clear();
                this.listView1.Items.Clear();
                this.listView1.View = View.SmallIcon;
                this.listView1.SmallImageList = this.imageList1;
                this.sorter.SortColumn = 0;
                this.listView1.BeginUpdate();
                setList(bucketName);
                this.listView1.EndUpdate();
                this.listView1.EndUpdate();
                this.sorter.SortColumn = 0;
                this.sorter.SortOrder = SortOrder.Descending;
                this.listView1.Sort();

            }
        }
        #endregion

        #region  list显示
        private void ListView(string bucketName)
        {
            if (InvokeRequired)
            {
                Invoke(new setDetailsInfo(ListView), new object[] { bucketName });
            }
            else
            {
                this.listView1.Clear();
                this.listView1.Items.Clear();
                this.listView1.View = View.List;
                this.listView1.SmallImageList = this.imageList1;
                this.sorter.SortColumn = 0;
                this.listView1.BeginUpdate();
                setList(bucketName);
                this.listView1.EndUpdate();
                this.sorter.SortColumn = 0;
                this.sorter.SortOrder = SortOrder.Descending;
                this.listView1.Sort();
            }
        }
        #endregion

        private void btnBackoff_MouseEnter(object sender, EventArgs e)
        {
            btnBackoff.FlatAppearance.BorderSize = 1;
            btnBackoff.FlatAppearance.BorderColor = Color.FromArgb(0, 192, 192);
        }

        private void btnBackoff_MouseLeave(object sender, EventArgs e)
        {
            btnBackoff.FlatAppearance.BorderSize = 0;
            btnBackoff.FlatAppearance.BorderColor = Color.Empty;
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            SetMsg(null);
        }

        private void setNumberText(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new setDetailsInfo(setNumberText), new object[] { s });
            }
            else
            {
                label4.Text = s;
            }
        }

        #region  listview 拖动事件
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textpath))
                {
                    MessageBox.Show("请选择一个 bucket目录。");
                    return;
                }
                string yunPath = "";
                string[] str = textpath.Split('/');
                if (str.Length > 0)
                {
                    ossConfig.BucketName = str[0];
                    if (str.Length > 1)
                    {
                        int i = textpath.IndexOf('/');
                        if (i > 0)
                        {
                            yunPath = textpath.Substring(i + 1);
                        }
                    }
                }

                String[] files = e.Data.GetData(DataFormats.FileDrop, false) as String[];
                ThreadPool.QueueUserWorkItem(o =>
                {
                    foreach (string srcfile in files)
                    {
                        if (System.IO.Directory.Exists(srcfile))
                        {
                            if (dicGetFilesAll != null && dicGetFilesAll.Count > 0)
                            {
                                dicGetFilesAll.Clear();
                            }
                            DirectoryInfo d = new DirectoryInfo(srcfile);
                            getFiles(d, srcfile);
                            uploadFolder(srcfile, yunPath);
                        }
                        else if (System.IO.File.Exists(srcfile))
                        {
                            uploadFile(srcfile, yunPath);
                        }
                    }
                    getBucketList();
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error("DragDrop:" + ex.Message);
            }
        }
        #endregion

        #region 更新选择 bucketName 的文件夹
        private void getBucketList()
        {
            try
            {
                if (string.IsNullOrEmpty(textpath))
                {
                    return;
                }
                string[] str = textpath.Split('/');
                string bName = str[0];
                RemoveDic(bName);
                ObjectListing olist = null;
                ListObjectsRequest request = new ListObjectsRequest(bName);
                request.MaxKeys = 1000;
                do
                {
                    olist = ossClient.ListObjects(request);
                    IEnumerable<OssObjectSummary> ossSumm = olist.ObjectSummaries;
                    if (ossSumm.Count() > 0)
                    {
                        foreach (OssObjectSummary obj in ossSumm)
                        {
                            setStrSplit(bName, obj);
                        }
                        if (olist.IsTrunked)
                        {
                            request.Marker = olist.NextMarker;
                        }
                    }
                    else
                    {
                        List<OssSummary> lis = new List<OssSummary>();
                        OssSummary os = new OssSummary();
                        lis.Add(os);
                        dicfile.Add(bName, lis);
                    }
                } while (olist.IsTrunked);
                setDetails(bucketName);
            }
            catch (Exception e)
            {

                logHelper.Error(e.Message);
            }
        }

        private void RemoveDic(string bName)
        {
            if (dicfile.ContainsKey(bName))
            {
                dicfile.Remove(bName);
            }
            if (dicFolder.ContainsKey(bName))
            {
                List<string> list = dicFolder[bName];
                dicFolder.Remove(bName);
                foreach (string str in list)
                {
                    string key = bName + "/" + str;
                    RemoveDic(key);
                }
            }
        }
        #endregion

        #region listview 拖动 属性设置
        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion

        #region 获取一个文件夹所有文件
        private Dictionary<string, List<string>> dicGetFilesAll = new Dictionary<string, List<string>>();
        private void getFiles(DirectoryInfo d, string selectedPath)
        {
            try
            {
                FileInfo[] fis = d.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    string file = selectedPath + "\\" + fi.Name;
                    List<string> list = new List<string>();
                    list.Add(file);
                    if (!dicGetFilesAll.ContainsKey(selectedPath))
                    {
                        dicGetFilesAll.Add(selectedPath, list);
                    }
                    else
                    {
                        List<string> list1 = dicGetFilesAll[selectedPath];
                        list1.Add(file);
                    }
                }

                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    string path = selectedPath + "\\" + di.Name;
                    DirectoryInfo dir = new DirectoryInfo(path);
                    getFiles(dir, path);
                }
            }
            catch (Exception e)
            {
                logHelper.Error(e.Message);
            }
        }
        #endregion

        #region 以文件上传
        private void uploadFile(string selectedPath, string yunPath)
        {
            try
            {
                string file = selectedPath;
                string filName = getFileName(selectedPath);
                MutiPartUpload(file, filName, yunPath);
            }
            catch (Exception e)
            {
                logHelper.Error(e.Message);
            }
        }
        #endregion

        #region 得到文件名字
        private string getFileName(string s)
        {
            string[] str = s.Split('\\');
            string path = str[str.Length - 1];
            return path;
        }
        #endregion

        #region 以文件夹上传
        private void uploadFolder(string selectedPath, string yunPath)
        {
            try
            {
                string st = "";
                if (selectedPath != null)
                {
                    st = getFileName(selectedPath);
                }

                foreach (string s in dicGetFilesAll.Keys)
                {
                    List<string> list = dicGetFilesAll[s];
                    foreach (string str in list)
                    {
                        string path = str;
                        string key = str.Replace(selectedPath, "").Replace("\\", "/").TrimStart('/');
                        if (!string.IsNullOrEmpty(key))
                        {
                            if (!string.IsNullOrEmpty(st))
                            {
                                key = st + "/" + key;
                            }
                        }
                        else
                        {
                            key = st;
                        }

                        MutiPartUpload(path, key, yunPath);
                    }

                }

            }
            catch (Exception e)
            {
                logHelper.Error(e.Message);
            }

        }
        #endregion

        #region  文件上传
        public void MutiPartUpload(string fileName, string key, string yunPath)
        {
            try
            {

                SetMsg("开始上传:" + key);
                if (!string.IsNullOrEmpty(yunPath))
                {
                    key = yunPath + "/" + key;
                }
                InitiateMultipartUploadRequest initRequest =
                                new InitiateMultipartUploadRequest(ossConfig.BucketName, key);
                InitiateMultipartUploadResult initResult = ossClient.InitiateMultipartUpload(initRequest);


                // 设置每块为 1M 
                int partSize = 1024 * 1024 * 1;

                FileInfo partFile = new FileInfo(fileName);

                // 计算分块数目 
                int partCount = (int)(partFile.Length / partSize);
                if (partFile.Length % partSize != 0)
                {
                    partCount++;

                }
                SetMsg("数据分块上传，一共:" + partCount + "块");
                // 新建一个List保存每个分块上传后的ETag和PartNumber 
                List<PartETag> partETags = new List<PartETag>();

                for (int i = 0; i < partCount; i++)
                {
                    int number = i + 1;
                    SetMsg("第" + number + "块,上传开始");
                    // 获取文件流 
                    FileStream fis = new FileStream(partFile.FullName, FileMode.Open);

                    // 跳到每个分块的开头 
                    long skipBytes = partSize * i;
                    fis.Position = skipBytes;
                    //fis.skip(skipBytes); 

                    // 计算每个分块的大小 
                    long size = partSize < partFile.Length - skipBytes ?
                            partSize : partFile.Length - skipBytes;

                    // 创建UploadPartRequest，上传分块 
                    UploadPartRequest uploadPartRequest = new UploadPartRequest(ossConfig.BucketName, key, initResult.UploadId);
                    uploadPartRequest.InputStream = fis;
                    uploadPartRequest.PartSize = size;
                    uploadPartRequest.PartNumber = (i + 1);
                    UploadPartResult uploadPartResult = ossClient.UploadPart(uploadPartRequest);

                    // 将返回的PartETag保存到List中。 
                    partETags.Add(uploadPartResult.PartETag);

                    // 关闭文件 
                    fis.Close();
                    SetMsg("第" + number + "块,上传完毕");
                }

                CompleteMultipartUploadRequest completeReq = new CompleteMultipartUploadRequest(ossConfig.BucketName, key, initResult.UploadId);
                foreach (PartETag partETag in partETags)
                {
                    completeReq.PartETags.Add(partETag);
                }
                //  红色标注的是与JAVA的SDK有区别的地方 

                //完成分块上传 
                SetMsg("合并数据块开始");
                CompleteMultipartUploadResult completeResult = ossClient.CompleteMultipartUpload(completeReq);
                SetMsg("合并数据块结束");
                // 返回最终文件的MD5，用于用户进行校验 

                SetMsg(key + " 上传成功.");
            }
            catch (Exception ex)
            {
                logHelper.Error(ex.Message);
            }

        }
        #endregion

        #region 排序
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 3 || e.Column == 1)
            {
                if (e.Column == this.sorter.SortColumn)
                {
                    if (this.sorter.SortOrder == SortOrder.Ascending)
                        this.sorter.SortOrder = SortOrder.Descending;
                    else
                        if (this.sorter.SortOrder == SortOrder.Descending)
                            this.sorter.SortOrder = SortOrder.Ascending;
                        else
                            return;
                }
                else
                {
                    this.sorter.SortColumn = e.Column;
                }
                this.listView1.Sort();
            }
        }

        #endregion

        #region 删除按钮
        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textpath))
            {
                if (listView1.SelectedItems != null)
                {
                    int lenth = listView1.SelectedItems.Count;
                    string[] itmeNames = null;
                    if (lenth > 1)
                    {
                        itmeNames = new string[lenth];
                        for (int i = 0; i < lenth; i++)
                        {
                            itmeNames[i] = listView1.SelectedItems[i].Text;
                        }
                    }
                    else if (lenth == 1)
                    {
                        itmeNames = new string[1];
                        string itmeName = listView1.SelectedItems[0].Text;
                        itmeNames[0] = itmeName;
                    }
                    if (itmeNames.Count() == 0)
                    {
                        logHelper.Debug("没有选择要删除项。");
                        return;
                    }

                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        foreach (string s in itmeNames)
                        {
                            if (getIsFile(s))
                            {
                                delFile(s);

                            }
                            else
                            {
                                requestFiles(textpath+"/"+s);
                                SetMsg(s + "文件夹开始删除。");
                                delFolder(textpath+"/"+s, s);
                                SetMsg(s + "文件夹删除完成。");

                            }
                        }
                        //getBucketList();
                        requestFiles(textpath);
                        setDetails(bucketName);
                    });

                }
            }
        }
        #endregion

        #region 文件删除
        private void delFile(string itemName)
        {
            string[] str = textpath.Split('/');
            string bName = "";
            if (str.Length > 0)
            {
                bName = str[0];
            }
            int i = textpath.IndexOf('/');
            string path = "";
            if (i > 0)
            {
                path = textpath.Substring(i + 1);
            }
            if (!string.IsNullOrEmpty(path))
            {
                itemName = path + "/" + itemName;
            }
            SetMsg(itemName + "文件开始删除。");
            try
            {
                ossClient.DeleteObject(bName, itemName);
                SetMsg(itemName + "文件删除成功。");
            }
            catch (Exception e)
            {
                logHelper.Error(itemName + "文件删除失败。" + e.Message);
            }

        }
        #endregion

        #region 文件夹删除
        private void delFolder(string path, string itemName)
        {
            try
            {
                string bucket = path;
                if (dicfile.ContainsKey(bucket))
                {
                    List<OssSummary> list = dicfile[bucket];
                    foreach (OssSummary os in list)
                    {
                        string[] str = bucket.Split('/');
                        string bName = "";
                        if (str.Length > 0)
                        {
                            bName = str[0];
                        }
                        int i = bucket.IndexOf('/');
                        string path1 = "";
                        if (i > 0)
                        {
                            path1 = bucket.Substring(i + 1);
                        }
                        //if (!string.IsNullOrEmpty(os.fileName))
                        {
                            if (!string.IsNullOrEmpty(path1))
                            {
                                path1 = path1 + "/" + os.fileName;
                            }
                            else
                            {
                                path1 = os.fileName;
                            }
                            SetMsg(os.fileName + "文件开始删除。");
                            try
                            {
                                ossClient.DeleteObject(bName, path1);
                                SetMsg(os.fileName + "文件删除成功。");
                            }
                            catch (Exception ex)
                            {
                                logHelper.Error(os.fileName + "文件夹删除失败。" + ex.Message);
                            }
                        }

                    }
                }
                if (dicFolder.ContainsKey(bucket))
                {
                    try
                    {
                        List<string> list = dicFolder[bucket];
                        foreach (string str in list)
                        {
                            string keyStr = bucket + "/" + str;
                            requestFiles(keyStr);
                            delFolder(keyStr, itemName);
                        }
                    }
                    catch (Exception ex)
                    { }
                }
            }
            catch (Exception e)
            {
                logHelper.Error(e.Message);
            }
        }
        #endregion

        private void tsmiSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Selected = true;
            }
        }

        #region 刷新按钮
        private void tsmRefresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textpath))
            {
                try
                {
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        BtnEnabled(false);
                        //getBucketList();
                        if (!string.IsNullOrEmpty(textpath))
                        {
                            requestFiles(textpath);
                            setDetails(bucketName);
                            SetMsg("刷新成功。");
                        }
                        BtnEnabled(true);
                    });
                }
                catch (Exception ex)
                {
                    logHelper.Error(ex.Message);
                    BtnEnabled(true); 
                }
            }
        }
        #endregion

        #region URl
        private void tsmiURL_Click(object sender, EventArgs e)
        {
            try
            {
                bool status = false;
                if (listView1.SelectedItems.Count > 0)
                {
                    string itmeName = listView1.SelectedItems[0].Text;
                    if (!string.IsNullOrEmpty(textpath))
                    {
                        if (dicfile.ContainsKey(textpath))
                        {
                            string name = "";
                            string[] str = textpath.Split('/');
                            if (str.Length > 0)
                            {
                                name = str[0];
                            }
                            List<OssSummary> oss = dicfile[textpath];
                            bool result = false;
                            foreach (OssSummary o in oss)
                            {
                                if (itmeName == o.fileName)
                                {
                                    result = true;
                                    string path = "";
                                    int i = textpath.IndexOf('/');
                                    if (i > 0)
                                    {
                                        path = textpath.Substring(i + 1);
                                        path = path + "/" + itmeName;
                                    }
                                    else
                                    {
                                        path = itmeName;
                                    }
                                    ThreadPool.QueueUserWorkItem(o1 =>
                                    {
                                        DateTime expiration = DateTime.Now;
                                        expiration.AddHours(2);
                                        GeneratePresignedUriRequest uriRequest = new GeneratePresignedUriRequest(name, path, SignHttpMethod.Get);
                                        uriRequest.Expiration = expiration;
                                        Uri uri = ossClient.GeneratePresignedUri(uriRequest);
                                        SetMsg("文件：" + itmeName + " URl：" + uri.AbsoluteUri);
                                        showUI(uri.AbsoluteUri);
                                    });
                                }


                            }

                            if (!result)
                            {
                                status = true;
                            }
                        }
                        else
                        {
                            status = true;
                        }
                    }
                    else
                    {
                        status = true;
                    }
                }
                else
                {
                    status = true;
                }

                if (status)
                {
                    SetMsg("请选择一个文件。");
                }
            }
            catch (Exception ex)
            {
                logHelper.Error(ex.Message);
            }
        }
        #endregion

        #region URlShow
        private delegate void ResultUIText(string s);
        private void showUI(string s)
        {
            if (InvokeRequired)
                Invoke(new ResultUIText(showUI), new object[] { s });
            else
            {
                ResultUI rs = new ResultUI();
                rs.setURL(s);
                rs.Show();
            }
        }
        #endregion

        private void txtMsg_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }


    }

}
