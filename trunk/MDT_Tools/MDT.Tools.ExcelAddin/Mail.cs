using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace MDT.Tools.ExcelAddin
{
    public class Mail
    {

        public static void sends()
        {
            ConfigInfo configInfo = IniConfigHelper.ReadConfigInfo();

            MailHelper.SmtpConext sc = new MailHelper.SmtpConext();
            sc.Host = configInfo.SHost;
            sc.Port = configInfo.SPort;
            sc.UserName = configInfo.UserName;
            sc.UserPwd = configInfo.UserPwd;
            sc.EnableSsl = configInfo.EnableSsl;
            
            string from = configInfo.From;
            string subject = configInfo.Subject;
            string body = "警告：数据异常";
            //o as string;//消息体

            List<string> to = new List<string>();
            if (!string.IsNullOrEmpty(configInfo.To))
            {
                string[] strs = configInfo.To.Split(new char[] { ';', ',', ' ' },
                                                    StringSplitOptions.RemoveEmptyEntries);
                to.AddRange(strs);
            }

            List<string> cc = new List<string>();
            if (!string.IsNullOrEmpty(configInfo.Cc))
            {
                string[] strs = configInfo.Cc.Split(new char[] { ';', ',', ' ' },
                                                    StringSplitOptions.RemoveEmptyEntries);
                cc.AddRange(strs);
            }

            List<string> attachments = new List<string>();
            Image image = CaptureScreenHelper.FullScreen();
            DateTime now = DateTime.Now;
            string fileName = Consts.ErrorImagePath + string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}.jpg", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            image.Save(fileName);
            attachments.Add(fileName);

            MailHelper.Send(sc, from, to, cc, subject, body, attachments);
        }

    }
}
