using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Aliyun.Common.Oss;

namespace MDT.Tools.HelpDesk
{
    internal partial class HelpDesk : Form
    {
        private string pubulicKey = ConfigurationSettings.AppSettings["key1"];
        private string pwd = ConfigurationSettings.AppSettings["key2"];
        private string accessId = "";
        private string assessKey = "";
        private string bucketName = "";
        ValidationCode vc = new ValidationCode();
        public HelpDesk()
        {
            InitializeComponent();
            string[] strs = BigInteger.DecryptRASString(pwd, pubulicKey).Split('|');
            accessId = strs[0];
            assessKey = strs[1];
            bucketName = strs[2];
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbContent.Text.Trim()))
            {
                MessageBox.Show(this, @"问题不能为空，请认真描述你遇到的问题!", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(!vCode.ToLower().Equals(tbVC.Text.Trim().ToLower()))
            {
                MessageBox.Show(this, @"验证码输入有误!", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                genVC();
                return;
            }
            Enabled = false;
            btnSend.Text = "提交中";
            ThreadPool.QueueUserWorkItem(o => {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("报告时间:" + DateTime.Now.ToString() + Environment.NewLine);
                sb.AppendLine("问题描述:");
                sb.AppendLine("\t"+tbContent.Text.Trim());
                sb.AppendLine("屏幕截图:");
                sb.AppendLine("\t" + cbScreen.Checked);
                sb.AppendLine("联系电话:");
                sb.AppendLine("\t"+tbPhone.Text.Trim());
                sb.AppendLine("邮箱地址:");
                sb.AppendLine("\t" + tbMail.Text.Trim());
                OssHelper ossHelper = new OssHelper();
                ossHelper.OssConfig = new OssConfig()
                {
                    AccessId =
                        accessId,
                    AccessKey =
                        assessKey,
                    BucketName = bucketName
                };
                byte[] bt = Encoding.UTF8.GetBytes(sb.ToString());
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                string date = DateTime.Now.ToString("yyyyMMdd");
                using (var ms = new MemoryStream())
                {
                    ms.Write(bt, 0, bt.Length);
                    ms.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    ossHelper.UpLoad(ms, string.Format("{1}/HelpDesk_{0}.txt", time, date));
                }
                if(cbScreen.Checked)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        this.Visible = false;
                    }));
                    Thread.CurrentThread.Join(1000);
                    int width = Screen.PrimaryScreen.Bounds.Width;
                    int height = Screen.PrimaryScreen.Bounds.Height;
                    Image image = new Bitmap(width, height);
                    Graphics g = Graphics.FromImage(image);
                    g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(width, height));
                    using (var ms = new MemoryStream())
                    {
                        image.Save(ms, ImageFormat.Jpeg);
                        ms.Write(bt, 0, bt.Length);
                        ms.Flush();
                        ms.Seek(0, SeekOrigin.Begin);
                        ossHelper.UpLoad(ms, string.Format("{1}/HelpDesk_{0}.jpeg", time, date));
                    }
                }
                sendSuccess();
               
            }
            catch(Exception ex)
            {
                sendFailed();
               
            }
            finally
            {
                Process.GetCurrentProcess().Kill();
            }
            });
        }

        private void sendSuccess()
        {
            if(this.InvokeRequired)
            {
                Action a = sendSuccess;
                this.Invoke(a);
            }
            else
            {
         
            Enabled = true;
            btnSend.Text = "提交";
            MessageBox.Show(this, @"反馈成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void sendFailed()
        {
            if (this.InvokeRequired)
            {
                Action a = sendFailed;
                this.Invoke(a);
            }
            else
            {

                Enabled = true;
                btnSend.Text = "提交";
                MessageBox.Show(this, @"抱歉，系统遇到故障，请稍后在试。", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #region VC
        private string vCode = "";
        
        private void genVC()
        {
            vCode = vc.GenerateCheckCode();
            pbVC.Image = vc.CreateCheckCodeImage(vCode);
        } 
        private void pbVC_Click(object sender, EventArgs e)
        {
            genVC();
        }
        #endregion


        

        private void HelpDesk_Load(object sender, EventArgs e)
        {
            genVC();
        }
    }
}
