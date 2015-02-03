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
                btnSend.Enabled = btnCancel.Enabled = false;
                this.Hide();
                string[] strs = name.Split('|');
                Random random =new Random();
                foreach (var str in strs)
                {
                    var errorReport = str;
                    OssHelper ossHelper = new OssHelper();
                    ossHelper.OssConfig = new OssConfig() { AccessId = accessId, AccessKey = accessKey, BucketName = bucketName };
                    ossHelper.UpLoad(errorReport, Guid.NewGuid().ToString() +".LOG");


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
