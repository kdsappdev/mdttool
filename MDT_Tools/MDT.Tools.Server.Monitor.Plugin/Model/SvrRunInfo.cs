using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Server.Monitor.Plugin.Model
{
    public class SvrRunInfo
    {
        public string ServiceName { get; set; }
     //服务名称
        public string Host { get; set; }//ip
        public string Port{ get; set; }//port
        public string Type { get; set; }//主备机

        public string WorkDes{ get; set; }//是否正在工作
        public string IsMainServer { get; set; }
        public string Connectioned { get; set; }//连接状态信息
        public string ClientInfo { get; set; }//客户端信息
        public List<Line> getClientLine()
        {
            List<Line> lt=new List<Line>();
            if(!string.IsNullOrEmpty(ClientInfo))
            {
               string[] strs= ClientInfo.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var str in strs)
                {
                   string[] str2s= str.Split(new string[]{"-"}, StringSplitOptions.RemoveEmptyEntries);
                   Line line = new Line();
                    line.Servername_To = ServiceName;
                    line.Servername_From = "未知";
                    if(str2s.Length>=1)
                    {
                        line.Info = string.Format("{0}->{1}:{2}",str2s[0],Host,Port);
                    }
                    if(str2s.Length>=2)
                    {
                        line.Servername_From = str2s[1];
                        
                    }
                    lt.Add(line);
                }
            }
            return lt;
        }

        public string Info {
            get
            {
                string info = string.Format("服务器{0}:{1}:{2}{3}", Type == "0" ? "(备)" : "(主)", Host, Port, WorkDes == "working" ? "工作中" : "暂停中");
                return info;
            }

        }
    }

    public class Line
    {
        public string Servername_From { get; set; }

        public string Servername_To { get; set; }

        public string Key
        {
            get { string key = string.Format("{0}->{1}", Servername_From, Servername_To);
                return key;
            }
        }

        public string Info { get; set; }
    }
}
