using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MDT.Tools.CEDA.Common;
using com.adaptiveMQ2.message;
using MDT.Tools.Core.Log;
using com.adaptiveMQ2.client;
using Ats.Foundation.Message;
using Ats.Foundation.Message.Serializer;
using System.Threading;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;

namespace MDT.Tools.Eventlog.Monitor.Plugin
{
    public partial class Form3 : DockContent
    {
        private CedaManager _cedaSubscribe = null;
        private ILog logHelper;
        string IP;
        int PORT;
        
        
        string msgbody;
        int i ;
        int maxpage;
        Dictionary<string, object> dic1 = new Dictionary<string, object>();
        Dictionary<string,object> dic = new Dictionary<string,object>();
        com.adaptiveMQ2.message.Message request1 = new com.adaptiveMQ2.message.Message();
            
        com.adaptiveMQ2.message.Message request = new com.adaptiveMQ2.message.Message();
        public Form3(string ip,int port)
        {
            InitializeComponent();
            IP = ip;
            PORT = port;
        }

        public void connect()
        {
            logHelper = new TextLogHelper(label5,this, IP, PORT);
            _cedaSubscribe = new CedaManager(logHelper);

            
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.setAddress(IP, PORT);
            clientInfo.setUser("monitor", "monitor");
            clientInfo.Protocol = ClientInfo.PROTOCOL_TCP;

            ThreadPool.QueueUserWorkItem(o =>
            {


                _cedaSubscribe.Connect(clientInfo);
            });

               
        }


        private bool  getMessage(string startdate, string enddate, string eventlevel, string content, int offset)
        {
            string s, e;
            s = string.Format("{0}-{1}-{2} ", startdate.Substring(0, 4), startdate.Substring(4, 2), startdate.Substring(6, 2));
            e = string.Format("{0}-{1}-{2} ", enddate.Substring(0, 4), enddate.Substring(4, 2), enddate.Substring(6, 2));
            if (DateTime.Compare(DateTime.Parse(s), DateTime.Parse(e)) > 0) { MessageBox.Show("日期选择错误");
            return false;
            }
           else
            {
                dic["startdate"] = startdate;
                dic["enddate"] = enddate;
                dic["eventlevel"] = eventlevel;
                dic["content"] = content;
                // dic.Add("count", 20); dic["startdate"] = startdate;
                dic["offset"] = offset;
                msgbody = MsgHelper.Serializer<Dictionary<string, object>>(dic);

                request.MessageBody.addString((short)3, msgbody);

                request.MessageBody.addString((short)4, "JSON");
                request.MessageBody.addInt((short)5, 0);
                request.MessageBody.addString(7, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                return true;
            }
            
        }
        void _cedaSubscribe_OnCedaMessage(HistoryMsg msg)
        {
            if (msg.Data.Count!=0)
            {
                dgv.DataSource = msg.Data;
                levelColor();
            }
            else { MessageBox.Show("没有信息！要不你换个条件试试？？"); }
          }

        private void okBt_Click(object sender, EventArgs e)
        {
            i = 1;

            if( getMessage(startDt.Text.ToString(), endDt.Text.ToString(), levelComboBox.Text.ToString(), contentBox.Text.ToString(), 0)){
            
            sentMsg();

            getPageCount(startDt.Text.ToString(), endDt.Text.ToString(), levelComboBox.Text.ToString(), contentBox.Text.ToString());

            page.Text = i.ToString();
           }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            request1.Destination = new Destination("YM.MONITOR.EVENT.PAGECOUNT");
            request.Destination = new Destination("YM.MONITOR.EVENT.HISTORY");
            this.startDt.CustomFormat = "yyyyMMdd";
            this.endDt.CustomFormat = "yyyyMMdd";
            this.startDt.Format = DateTimePickerFormat.Custom;
            this.endDt.Format = DateTimePickerFormat.Custom;
            this.startDt.Text = null;
            this.endDt.Text = null;
            dgv.ReadOnly = true;
            dgv.Enabled = true;
            connect();
            this.Disposed += new EventHandler(Form1_Disposed);
            dic.Add("startdate", "");
            dic.Add("enddate", "");
            dic.Add("eventlevel", "");
            dic.Add("content", "");
            dic.Add("count", 20);
            dic.Add("offset", "");

            dic1.Add("startdate", "");
            dic1.Add("enddate", "");
            dic1.Add("eventlevel", "");
            dic1.Add("content", "");
            dic1.Add("count", 20);
            dic1.Add("offset", "");
        }
        void Form1_Disposed(object sender, EventArgs e)
        {
            Disconnect();
        }
        private void Disconnect()
        {
            if (_cedaSubscribe != null)
            {
                _cedaSubscribe.Disconnect();
                
                _cedaSubscribe = null;
            }
        }

        private void firstPage_Click(object sender, EventArgs e)
        {
            i = 1;
            page.Text = i.ToString();
            dic["offset"] = 0;
            msgbody = MsgHelper.Serializer<Dictionary<string, object>>(dic);
            request.MessageBody.addString((short)3, msgbody);
            sentMsg();
        }

        private void sentMsg() 
        {
            
            if (_cedaSubscribe.IsConnected)
            {
                
                com.adaptiveMQ2.message.Message reply = _cedaSubscribe.Request(request);
                if (reply != null)
                {
                    CedaObject co = CedaObject.ToCedaObject(reply);
                    HistoryMsg result = MsgHelper.Deserialize<HistoryMsg>(co.MessageBody, MsgSerializerType.Json);
                    _cedaSubscribe_OnCedaMessage(result);
                }
                else { MessageBox.Show("响应信息异常"); }
            }
            else
            {
                MessageBox.Show("连接失败");
            }
        }

        private void last_Click(object sender, EventArgs e)
        {
            i = maxpage;
            page.Text = i.ToString();
            dic["offset"] = (maxpage-1)*20;
            msgbody = MsgHelper.Serializer<Dictionary<string, object>>(dic);
            request.MessageBody.addString((short)3, msgbody);
            sentMsg();
        }

        private void next_Click(object sender, EventArgs e)
        {
            if (i + 1 > maxpage) { MessageBox.Show("已经到最后一页了，再向后，臣妾做不到啊"); }
            else
            {
            i = i + 1;
            page.Text = i.ToString();
            dic["offset"] = (i - 1) * 20;
            msgbody = MsgHelper.Serializer<Dictionary<string, object>>(dic);
            request.MessageBody.addString((short)3, msgbody);
            sentMsg();}
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            if (i - 1 <= 0)
            { MessageBox.Show("已经是首页了，不能往前了呢"); }
            else
            {
                i = i - 1;
                page.Text = i.ToString();
                dic["offset"] = (i - 1) * 20;
                msgbody = MsgHelper.Serializer<Dictionary<string, object>>(dic);
                request.MessageBody.addString((short)3, msgbody);
                sentMsg();
            }
        }

        private void jump_Click(object sender, EventArgs e)
        {
            try
            {
                string s = page.Text;
                int.TryParse(s, out i);
            }
            catch { MessageBox.Show("请输入正确的数字"); }
            if (i <= maxpage && i >= 0)
            {
                page.Text = i.ToString();
                dic["offset"] = (i - 1) * 20;
                msgbody = MsgHelper.Serializer<Dictionary<string, object>>(dic);
                request.MessageBody.addString((short)3, msgbody);
                sentMsg();
            }
            else { MessageBox.Show("输入数字过大或过小，请重新输入"); }
        }
        private void getPageCount(string startdate, string enddate, string eventlevel, string content)
        {
            dic["startdate"] = startdate;
            dic["enddate"] = enddate;
            dic["eventlevel"] = eventlevel;
            dic["content"] = content;
            
            msgbody = MsgHelper.Serializer<Dictionary<string, object>>(dic);
            request1.MessageBody.addString((short)3, msgbody);

            request1.MessageBody.addString((short)4, "JSON");
            request1.MessageBody.addInt((short)5, 0);
            request1.MessageBody.addString(7, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));

            if (_cedaSubscribe.IsConnected)
            {
                com.adaptiveMQ2.message.Message reply1 = _cedaSubscribe.Request(request1);
                if (reply1 != null)
                {
                    CedaObject co = CedaObject.ToCedaObject(reply1);
                    Page result1 = MsgHelper.Deserialize<Page>(co.MessageBody, MsgSerializerType.Json);
                    maxpage = result1.Data;
                    
                    pageCount.Text = "共" + maxpage + "页";
                }
                else { MessageBox.Show("响应信息异常"); }
            }
            else
            {
                MessageBox.Show("连接失败,无法获取最大页数");
            }


        }
       private void  levelColor()
       {
           string str3 = SystemConfig.GetConfigData("errorColorPickEdit", string.Empty);
           string str2 = SystemConfig.GetConfigData("warnColorPickEdit", string.Empty);
           string str1 = SystemConfig.GetConfigData("infoColorPickEdit", string.Empty);
           string str4 = SystemConfig.GetConfigData("fatalErrorColorPickEdit", string.Empty);
           for (int i = 0; i <= 20; i++)
           {
               try
               {
                   if (dgv.Rows[i].Cells[2].Value.ToString().Equals("Info"))
                   {

                       if (!str1.Equals(string.Empty))
                       {
                           string[] c = str1.Split(',');
                           dgv.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(Int32.Parse(c[0]), Int32.Parse(c[1]), Int32.Parse(c[2]));
                       }
                       else { dgv.Rows[i].DefaultCellStyle.BackColor = Color.White; }

                   }
                   if (dgv.Rows[i].Cells[2].Value.ToString() == "Warn")
                   {

                       if (!str2.Equals(string.Empty))
                       {
                           string[] c = str2.Split(',');
                           dgv.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(Int32.Parse(c[0]), Int32.Parse(c[1]), Int32.Parse(c[2]));
                       }
                       else { dgv.Rows[i].DefaultCellStyle.BackColor = Color.Yellow; }
                   }
                   if (dgv.Rows[i].Cells[2].Value.ToString() == "Error")
                   {
                       if (!str3.Equals(string.Empty))
                       {
                           string[] c = str3.Split(',');
                           dgv.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(Int32.Parse(c[0]), Int32.Parse(c[1]), Int32.Parse(c[2]));
                       }
                       else { dgv.Rows[i].DefaultCellStyle.BackColor = Color.Orange; }
                   }
                   if (dgv.Rows[i].Cells[2].Value.ToString() == "Fatalerror")
                   {
                       if (str4.Equals(string.Empty))
                       {
                           string[] c = str4.Split(',');
                           dgv.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(Int32.Parse(c[0]), Int32.Parse(c[1]), Int32.Parse(c[2]));
                       }
                       else { dgv.Rows[i].DefaultCellStyle.BackColor = Color.Red; }
                   }
               }
               catch { continue; }
           }  
       }
    }
}
