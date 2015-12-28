using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.Aliyun.Common.Oss;

namespace MDT.Tools.ErrorReport
{
    public partial class ErrorForm : Form
    {
        private string accessId;
        private string bucketName;
        private string accessKey;
        private string name = "";
        public string Prefix = "";
        private ErrorForm()
        {
            InitializeComponent();
        }
        public ErrorForm(string accessid, string accesskey, string bucketName,string name)
            : this()
        {
            this.accessId = accessid;
            this.accessKey = accesskey;
            this.bucketName = bucketName;
            this.name = name;

        }


        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                btnSend.Enabled = btnCancel.Enabled = false;
                
                string[] strs = name.Split('|');
                string[] prefixs = Prefix.Split('|');
                for(int i=0;i<strs.Length;i++)
                {
                    var errorReport = strs[i];
                    OssHelper ossHelper = new OssHelper();
                    ossHelper.OssConfig = new OssConfig() { AccessId = accessId, AccessKey = accessKey, BucketName = bucketName };
                    string guid = Guid.NewGuid().ToString() + ".LOG";
                    if (prefixs.Length == strs.Length)
                    {
                        if (!string.IsNullOrEmpty(prefixs[i]))
                        {
                            guid = prefixs[i] + ".LOG";
                        }
                    }
                    ossHelper.UpLoad(errorReport, guid);


                }
               
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                Application.Exit();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
