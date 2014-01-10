using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MDT.Tools.ExcelAddin
{
    public class MailHelper
    {
        public class SmtpConext
        {
            private string host = "";
            private int port = 0;
            private bool enableSsl = false;
            private string userName = "";
            private string userPwd = "";

            public string Host
            {
                get { return host; }
                set { host = value; }
            }

            public int Port
            {
                get { return port; }
                set { port = value; }
            }

            public bool EnableSsl
            {
                get { return enableSsl; }
                set { enableSsl = value; }
            }
            public string UserName
            {
                get { return userName; }
                set { userName = value; }
            }
            public string UserPwd
            {
                get { return userPwd; }
                set { userPwd = value; }
            }

        }

        public static void Send(SmtpConext smtpContext, MailMessage msg)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Host = smtpContext.Host;
            if (smtpContext.EnableSsl)
            {
                smtp.EnableSsl = smtpContext.EnableSsl;
            }
            if (!smtpContext.Port.Equals(0))
            {
                smtp.Port = smtpContext.Port;
            }
            if (!string.IsNullOrEmpty(smtpContext.UserName))
            {
                smtp.Credentials = new NetworkCredential(smtpContext.UserName, smtpContext.UserPwd);
            }
            smtp.SendCompleted += new SendCompletedEventHandler(smtp_SendCompleted);
            //smtp.Send(msg);
            smtp.SendAsync(msg, null);
        }

        static void smtp_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            LogHelper.Info("邮件发送成功");
        }

        public static void Send(SmtpConext smtpContext, string from, List<string> to, List<string> cc, string subject, string htmlBody)
        {
            Send(smtpContext, from, to, cc, subject, htmlBody);
        }

        public static void Send(SmtpConext smtpContext, string from, List<string> to, List<string> cc, string subject, string htmlBody, List<string> attachments)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from);
            msg.Headers.Add("Disposition-Notification-To", from);
            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            msg.Headers.Add("ReturnReceipt", "1");
            foreach (string s in to)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    msg.To.Add(s);
                }
            }
            if (cc != null)
            {
                foreach (string s in cc)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        msg.CC.Add(s);
                    }
                }
            }

            msg.Subject = subject;
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Body = htmlBody;
            msg.Priority = MailPriority.High;
            if (attachments != null)
            {
                foreach (string attachment in attachments)
                {
                    if (!string.IsNullOrEmpty(attachment) && File.Exists(attachment))
                    {
                        msg.Attachments.Add(new Attachment(attachment));
                    }
                }
            }
            Send(smtpContext, msg);
        }
    }
}
