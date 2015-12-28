using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aliyun.OpenServices.OpenStorageService;
using System.Collections;
using System.IO;
using System.Threading;
using System.Diagnostics;
using MDT.Tools.Aliyun.Common.Oss;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.Core.Log;
using MDT.Tools.Aliyun.Upload.Plugin.DataMemory;

namespace MDT.Tools.Aliyun.Upload.Plugin.UploadUI
{
    public partial class UploadOSSUI : DockContent
    {
        private OssConfig ossConfig=null;
        private OssClient ossClient = null;
        private string selectedPath = "";
        private ILog logHelper;
        private static string ModuleName = "UploadOSSUI";
        private XMLDataMemory _dataMemory = new XMLDataMemory();
        public UploadOSSUI()
        {
            InitializeComponent();
        }
        private void UploadOSSUI_Load(object sender, EventArgs e)
        {
            logHelper = new Log4netLog();
            ossConfig = new OssConfig();
            setEnaled(false);
            GetMemory();
           
        }

        Dictionary<string, string> dicfilter = new Dictionary<string, string>();
        private void button2_Click(object sender, EventArgs e)
        {
            if (rbUpFile.Checked == true)
            {
                openFile();
            }
            else if (rbUpfolder.Checked == true)
            {
                openFolder();
            }
        }

        #region 打开文件选择窗口
        private Dictionary<string, string> dicfiles = new Dictionary<string, string>();
        private void openFile()
        {
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.Multiselect = true;
            fileDialog1.InitialDirectory = "d://";
            fileDialog1.Filter = "All files (*.*)|*.*";
            fileDialog1.FilterIndex = 1;
            fileDialog1.RestoreDirectory = true;
            if (fileDialog1.ShowDialog() == DialogResult.OK)
            {
              string[]  files = fileDialog1.FileNames;
                foreach (string s in files)
                {
                    this.tefile.AppendText(s);
                    string[] str = s.Split('\\');
                    string name = str[str.Length - 1];
                    dicfiles.Add(name, s);
                    if (files.Count() > 1)
                    {
                        name = name + ",";
                    }
                    this.tefileName.AppendText(name);
                }

            }
            else
            {
                tefile.Text = "";
            }
        }
        #endregion 

        #region 打开文件夹选择窗口
        private void openFolder()
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                selectedPath = fd.SelectedPath;
                string st = getFileName(selectedPath);
                tefileName.Text = st;
                tefile.Text = selectedPath;
                DirectoryInfo d = new DirectoryInfo(selectedPath);
                getFiles(d, selectedPath);
            }
        }
        #endregion

        #region 以文件上传
        private void uploadFile()
        {
            string file = tefile.Text.Trim();
            string key = teaccesskey.Text.Trim();
            ossConfig.BucketName = cbBucket.Text.Trim();
            string filName = tefileName.Text.Trim();
            if (dicfiles.Count <= 0)
            {
                MessageBox.Show("请选择一个文件。","info");
                return;
            }
            ThreadPool.QueueUserWorkItem(o =>
            {
                if (dicfiles.Count > 1)
                {
                    foreach (string str in dicfiles.Keys)
                    {
                        string path = dicfiles[str];
                        MutiPartUpload(path, str);
                    }
                }
                else if(dicfiles.Count == 1)
                {
                    MutiPartUpload(file, filName);
                }
 
            });
        }
        #endregion 

        #region 以文件夹上传
        private void uploadFolder()
        {
            ossConfig.BucketName = cbBucket.Text.Trim();
            string st = tefileName.Text.Trim();
            if (st == null)
            {
                st = getFileName(selectedPath);
            }
            if (dicfolder.Count <= 0)
            {
                MessageBox.Show("请选择一个文件夹。", "info");
                return;
            }
            foreach (string s in dicfolder.Keys)
            {
                List<string> list = dicfolder[s];
                foreach (string str in list)
                {
                    string path = str;
                    string key = str.Replace(selectedPath, "").Replace("\\", "/").TrimStart('/');
                    if (key != null)
                    {
                        key = st + "/" + key;
                    }
                    else
                    {
                        key = st;
                    }

                    ThreadPool.QueueUserWorkItem(o =>
                        {
                            MutiPartUpload(path, key);
                        });
            
                }
            }

        }
        #endregion 


        #region 代理添加Bucket目录
        private delegate  void setComboBox(IEnumerable<Bucket> buck);
       private void setItems(IEnumerable<Bucket> buck)
       {
           if (InvokeRequired)
           {
               Invoke(new setComboBox(setItems), new object[] { buck });
           }
           else
           {
               cbBucket.Items.Clear();
               foreach (Bucket obj in buck)
               {

                   cbBucket.Items.Add(obj.Name);
               }
           }
       }
        #endregion 

        #region 代理显示上传信息
       private delegate void setTexBox(string s);
       public void setTexItems(string s)
       {
           if (InvokeRequired)
           {
               Invoke(new setTexBox(setTexItems), new object[] { s });
           }
           else
           {
               s = s+"\r\n";
               teDetail.AppendText(s);
               teDetail.ScrollToCaret();
           }
       }
       #endregion 

       #region  文件上传
       public void MutiPartUpload(string fileName, string key)
       {
           try
           {

               setTexItems("开始上传:" + key);
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
               setTexItems("数据分块上传，一共:" + partCount + "块");
               // 新建一个List保存每个分块上传后的ETag和PartNumber 
               List<PartETag> partETags = new List<PartETag>();

               for (int i = 0; i < partCount; i++)
               {
                   int number = i + 1;
                   setTexItems("第" + number + "块,上传开始");
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
                   setTexItems("第" + number + "块,上传完毕");
               }

               CompleteMultipartUploadRequest completeReq = new CompleteMultipartUploadRequest(ossConfig.BucketName, key, initResult.UploadId);
               foreach (PartETag partETag in partETags)
               {
                   completeReq.PartETags.Add(partETag);
               }
               //  红色标注的是与JAVA的SDK有区别的地方 

               //完成分块上传 
               setTexItems("合并数据块开始");
               CompleteMultipartUploadResult completeResult = ossClient.CompleteMultipartUpload(completeReq);
               setTexItems("合并数据块结束");
               // 返回最终文件的MD5，用于用户进行校验 

               setTexItems(key + " 上传成功.");
           }
           catch (Exception ex)
           {
               logHelper.Error(ex.Message);
           }

       }
       #endregion 

       #region 获取一个文件夹所有文件
       private  Dictionary<string, List<string>> dicfolder = new Dictionary<string, List<string>>();
        private void getFiles(DirectoryInfo d, string selectedPath)
       {
           FileInfo[] fis = d.GetFiles();
           foreach (FileInfo fi in fis)
           {
               string file = selectedPath+"\\"+fi.Name;
               List<string> list = new List<string>();
               list.Add(file);
               if (!dicfolder.ContainsKey(fi.Name))
               {
                   dicfolder.Add(fi.Name, list);
               }
               else
               {
                   List<string> list1 = dicfolder[fi.Name];
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
       #endregion 


        private delegate void simpleB(bool b);
        private void setEnaled(bool b)
        {
            if (InvokeRequired)
            {
                Invoke(new simpleB(setEnaled), new object[] { b });
            }
            else
            {
                this.tsbupload.Enabled = b;
            }
        }

        private void setConnEnaled(bool b)
        {
            if (InvokeRequired)
            {
                Invoke(new simpleB(setEnaled), new object[] { b });
            }
            else
            {
                this.tsbConnect.Enabled = b;
            }
        }

        private string getFileName(string s)
        {
            string[] str = s.Split('\\');
            string path = str[str.Length - 1];
            return path;
        }

        private delegate void setLbabelText(string s, string s2);
        private void setLabelChenged(string s,string s2)
        {
            if (InvokeRequired)
            {
                Invoke(new setLbabelText(setLabelChenged), new object[] { s, s2 });
            }
            else
            {
                this.label3.Text = s;
                this.label5.Text = s2;
            }
        }
        private void rbUpFile_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUpFile.Checked == true)
            {
                tefile.Text = "";
                tefileName.Text = "";
                setLabelChenged("文件路径：", "上传文件名称：");
            }
        }

        private void rbUpfolder_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUpfolder.Checked == true)
            {
                tefile.Text = "";
                tefileName.Text = "";
                setLabelChenged("文件夹路径：", "上传文件夹名称：");
            }
        }


        private void tsmiClear_Click(object sender, EventArgs e)
        {
            teDetail.Text = "";
        }


       
        private void tsbConnect_Click(object sender, EventArgs e)
        {
            string id = teaccessId.Text.Trim();
            string key = teaccesskey.Text.Trim();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
            {
                MessageBox.Show("Access id 或 acess  key 不能为空。", "");
                return;
            }
            ossConfig.AccessId = id;
            ossConfig.AccessKey = key;
            setConnEnaled(false);
            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    ossClient = new OssClient(id, key);
                    IEnumerable<Bucket> buck = ossClient.ListBuckets();
                    setTexItems("连接成功。");
                    setEnaled(true);
                    setConnEnaled(true);
                    setItems(buck);
                    SetMemory();
                }
                catch (Exception ex)
                {
                    setTexItems("连接失败。\r\n 失败原因：" + ex.Message);
                    setEnaled(false);
                    setConnEnaled(true);
                }
            });
           
        }
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
        private void tsbupload_Click(object sender, EventArgs e)
        {
            string Bucket =cbBucket.Text.Trim();
            if (string.IsNullOrEmpty(Bucket))
            {
                MessageBox.Show("Bucket 名称不能为空。","info");
                return;
            }
            if (rbUpFile.Checked == true)
            {
                ThreadPool.QueueUserWorkItem(o => { uploadFile(); });
                
            }
            else if (rbUpfolder.Checked == true)
            {
                ThreadPool.QueueUserWorkItem(o => { uploadFolder(); });
            }
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            tefile.Text = "";
            tefileName.Text = "";
        }

        private void btnfile_MouseEnter(object sender, EventArgs e)
        {
            btnfile.FlatAppearance.BorderSize = 1;
            btnfile.FlatAppearance.BorderColor = Color.FromArgb(0, 192, 192);
           
        }

        private void btnfile_MouseLeave(object sender, EventArgs e)
        {
            btnfile.FlatAppearance.BorderSize = 0;
            btnfile.FlatAppearance.BorderColor = Color.Empty;
        }

        private void btnClear_MouseLeave(object sender, EventArgs e)
        {
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatAppearance.BorderColor = Color.Empty;
        }

        private void btnClear_MouseEnter(object sender, EventArgs e)
        {
            btnClear.FlatAppearance.BorderSize = 1;
            btnClear.FlatAppearance.BorderColor = Color.FromArgb(0, 192, 192);
        }
     

        
      
    }
}
