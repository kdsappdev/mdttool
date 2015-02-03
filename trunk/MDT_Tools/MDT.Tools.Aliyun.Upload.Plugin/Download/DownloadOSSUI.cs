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
        #endregion

        public DownloadOSSUI()
        {
            InitializeComponent();
        }

        private void DownloadOSSUI_Load(object sender, EventArgs e)
        {
            ossConfig = new OssConfig();
            tempDirectoryWatcher = new FileSystemWatcher();
            tempDirectoryWatcher.Path = Path.GetTempPath();
            tempDirectoryWatcher.Filter = string.Format("{0}*.tmp", DRAG_SOURCE_PREFIX);
            tempDirectoryWatcher.NotifyFilter = NotifyFilters.FileName;
            tempDirectoryWatcher.IncludeSubdirectories = false;
            tempDirectoryWatcher.EnableRaisingEvents = true;
            tempDirectoryWatcher.Created += new FileSystemEventHandler(TempDirectoryWatcherCreated);
            btnBackoff.Enabled = false;
        }

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
            string id = teaccessId.Text.Trim();
            string key = teaccesskey.Text.Trim();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
            {
                MessageBox.Show("Access key Id 和 Access key 不能为空。");
                return;
            }
            setEnabled(false);
            ThreadPool.QueueUserWorkItem(o => { ConnectOssClient(); });

        }
        #endregion 


         
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
                     ObjectListing olist = ossClient.ListObjects(bu.Name);
                     IEnumerable<OssObjectSummary> ossSumm = olist.ObjectSummaries;
                     foreach (OssObjectSummary obj in ossSumm)
                     {
                         setStrSplit(bu.Name, obj);
                     }
                 }
                 MessageBox.Show("连接成功。", "info");
                 dicFolder.Add("Bucket", bList);
                 setDetails(bucketName);
                 setEnabled(true);
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message, "error");
                 setEnabled(true);
             }
         }
        #endregion

        #region 分割路径
         private void setStrSplit(string buckeName, OssObjectSummary oss)
         {
             string str = oss.Key;
             string[] sList = str.Split('/');
             int len = sList.Length;
             for (int i = 0; i < len; i++)
             {
                 if (i < len -1)
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
                 else if(i == len-1)
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
                 buckeName +="/"+ sList[i];
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
                ColumnHeader co = new ColumnHeader();
                co.Text = "名称";
                this.listView1.Columns.Add("名称", 200, HorizontalAlignment.Left);
                this.listView1.Columns.Add("类型", 120, HorizontalAlignment.Left);
                this.listView1.Columns.Add("大小", 120, HorizontalAlignment.Left);
                this.listView1.Columns.Add("修改时间", 120, HorizontalAlignment.Left);
                if (dicFolder.ContainsKey(name))
                {
                    List<string> folders = dicFolder[name];
                    int count = folders.Count;
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
                    for (int j = 0; j < count; j++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.ImageIndex = 0;
                        lvi.Text = files[j].fileName;
                        string[] str = files[j].fileName.Split('.');
                        lvi.SubItems.Add(str[str.Length - 1]);
                        string tempFileExtension = "." + str[str.Length - 1];
                        if (!string.IsNullOrEmpty(tempFileExtension))
                        {
                            if (!imageList1.Images.Keys.Contains(tempFileExtension))
                            {
                                Icon icon = Icons.IconFromExtension(tempFileExtension, Icons.SystemIconSize.Small);
                                if (icon != null)
                                    imageList1.Images.Add(tempFileExtension, icon);
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
                        size = decimal.Ceiling(size);
                        lvi.SubItems.Add(size + "KB");
                        lvi.SubItems.Add(files[j].LastModified + "");
                        this.listView1.Items.Add(lvi);
                    }
                }
                this.listView1.EndUpdate();
            }
        }
        #endregion 

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
                    lvi.ImageIndex = 0;
                    lvi.Text = files[j].fileName;
                    string[] str = files[j].fileName.Split('.');
                    string tempFileExtension = "." + str[str.Length - 1];
                    if (!string.IsNullOrEmpty(tempFileExtension))
                    {
                        if (!imageList1.Images.Keys.Contains(tempFileExtension))
                        {
                            Icon icon = Icons.IconFromExtension(tempFileExtension, Icons.SystemIconSize.Small);
                            if (icon != null)
                                imageList1.Images.Add(tempFileExtension, icon);
                        }
                        int i= imageList1.Images.Keys.IndexOf(tempFileExtension);
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
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (string.IsNullOrEmpty(bucket))
                        {
                            bucket = name;
                        }
                        else
                        {
                            bucket += "/" + name;
                        }
                        if (dicFolder.ContainsKey(bucket) || dicfile.ContainsKey(bucket))
                        {
                            textpath = bucket;
                            bucketName = bucket;
                            AddLinklabel(textpath);
                            setDetails(textpath);
                            if (bucket == "Bcket")
                            {
                                btnBackoff.Enabled = false;
                            }
                            else
                            {
                                btnBackoff.Enabled = true;
                            }
                           

                        }
                    }

                }
            }
        }
        #endregion 

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
                        s = paths[0] +"/"+ paths[1] + "...." + paths[i - 2] + "/"+paths[i - 1];
                        if (s.Length > 50)
                        {
                            s = paths[0] + "/"+ "...." + "/" + paths[i - 1];
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

        private void getDownFile(string name)
        {
            string bucket = textpath;
             
        }

        #region 下载
        private  void downloadFile(OssClient client, String bucketName, String key, String filename)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DragNDrop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region 组装下载路径
        private void fileSplit(string name, string path,string itmeName)
        {
            string key ="";
            string bucketNmae = "";
            string bucket = textpath;
            bucket = name;
            if (dicFolder.ContainsKey(bucket))
            {
                List<string> list = dicFolder[bucket];
                foreach (string str in list)
                {
                    string keyStr = bucket+"/"+str;
                    fileSplit(keyStr, path, itmeName);
                }
            }

            if (dicfile.ContainsKey(bucket))
            {
                List<OssSummary> list = dicfile[bucket];
                foreach (OssSummary os in list)
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
                    }
                    if (!string.IsNullOrEmpty(fileName))
                        filePath = filePath + "/" + fileName + "/" + os.fileName;
                    string keyName = "";
                    if (string.IsNullOrEmpty(key))
                    {
                        keyName = os.fileName;
                    }
                    else
                    {
                        keyName = key + "/" + os.fileName;
                    }

                    downloadFile(ossClient, bucketNmae, keyName, filePath);
                    AliyunUp.SetProgress((int)os.Size);

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

        #region  计算总大小
        private long sunSize = 0;
        private long getSunSize(string name, string path, string itmeName)
        {
            string bucket = textpath;
            bucket = name;
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
                    sunSize += os.Size;
                }
            }
            if (!dicFolder.ContainsKey(bucket) && !dicfile.ContainsKey(bucket))
            {
                string path1 = path + "/" + itmeName;
                
            }
            return sunSize;
        }
        #endregion 

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

        private void getFiles(string bucket, string fileName, string path)
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
                        AliyunUp.SetEnable(false);
                        AliyunUp.SetProgressMax((int)os.Size);
                        downloadFile(ossClient, bucketNmae, keyName, filePath);
                        AliyunUp.SetProgress((int)os.Size);
                        AliyunUp.SetEnable(true);
                    }
                }
            }

        }



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

        #region FileSystemWatcher Methods
        private void TempDirectoryWatcherCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                CreateFileWatchers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DragNDrop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FileWatcherCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                OnFileDropPathFound(e.FullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DragNDrop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnFileDropPathFound(string dropedFilePath)
        {
            try
            {
                if (dropedFilePath.Trim() != string.Empty && objDragItem != null)
                {
                    string dropPath = dropedFilePath.Substring(0, dropedFilePath.LastIndexOf('\\'));

                    if (File.Exists(dropedFilePath))
                        File.Delete(dropedFilePath);

                    string itmeName = objDragItem.ToString();
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        if (getIsFile(itmeName))
                        {
                            getFiles(textpath, itmeName, dropPath);

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
                            sunSize = 0;
                            long size  =  getSunSize(bucket, dropPath, itmeName);
                            if (size != 0)
                            {
                                AliyunUp.SetEnable(false);
                                AliyunUp.SetProgressMax((int)size);
                            }
                            
                            fileSplit(bucket, dropPath, itmeName);
                        }

                        AliyunUp.SetEnable(true);
                    });
                   
                }
                objDragItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DragNDrop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        public void CreateFileWatchers()
        {
            try
            {
                if (watchers == null)
                {
                    int i = 0;
                    Hashtable tempWatchers = new Hashtable();
                    FileSystemWatcher watcher;
                    foreach (string driveName in Directory.GetLogicalDrives())
                    {
                        if (Directory.Exists(driveName))
                        {
                            watcher = new FileSystemWatcher();
                            watcher.Filter = string.Format("{0}*.tmp", DRAG_SOURCE_PREFIX);
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
                MessageBox.Show(ex.Message, "DragNDrop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(ex.Message, "DragNDrop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(ex.Message, "DragNDrop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region 鼠标放上事件
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
                    MessageBox.Show(ex.Message, "DragNDrop Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion 

        #region 鼠标单击事件
        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                return;
            }
            

        }
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


        private void LargeView(string bucketName)
        {
            if (InvokeRequired)
            {
                Invoke(new setDetailsInfo(setDetails), new object[] { bucketName });
            }
            else
            {
                this.listView1.Clear();
                this.listView1.Items.Clear();
                this.listView1.View = View.LargeIcon;
                this.listView1.LargeImageList = this.imageList2;

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
                        lvi.ImageIndex = 0;
                        lvi.Text = files[j].fileName;
                        string[] str = files[j].fileName.Split('.');
                        string tempFileExtension = "." + str[str.Length - 1];
                        if (!string.IsNullOrEmpty(tempFileExtension))
                        {
                            if (!imageList2.Images.Keys.Contains(tempFileExtension))
                            {
                                Icon icon = Icons.IconFromExtension(tempFileExtension, Icons.SystemIconSize.Small);
                                if (icon != null)
                                    imageList2.Images.Add(tempFileExtension, icon);
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
                this.listView1.EndUpdate();
            }
        }

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


        private void SmaillView(string bucketName)
        {
            if (InvokeRequired)
            {
                Invoke(new setDetailsInfo(setDetails), new object[] { bucketName });
            }
            else
            {
                this.listView1.Clear();
                this.listView1.Items.Clear();
                this.listView1.View = View.SmallIcon;
                this.listView1.SmallImageList = this.imageList1;
                this.listView1.BeginUpdate();
                setList(bucketName);
                this.listView1.EndUpdate();
            }
        }

        private void ListView(string bucketName)
        {
            if (InvokeRequired)
            {
                Invoke(new setDetailsInfo(setDetails), new object[] { bucketName });
            }
            else
            {
                this.listView1.Clear();
                this.listView1.Items.Clear();
                this.listView1.View = View.List;
                this.listView1.SmallImageList = this.imageList1;
                this.listView1.BeginUpdate();
                setList(bucketName);
                this.listView1.EndUpdate();
            }
        }

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


        

    }
      
}
