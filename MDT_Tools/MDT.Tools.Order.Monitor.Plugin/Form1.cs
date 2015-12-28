using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MDT.Tools.CEDA.Common;
using ZedGraph;
using Ats.Foundation.Message;
using MDT.Tools.Core.Log;
using Ats.Foundation.Message.Serializer;
using WeifenLuo.WinFormsUI.Docking;

using com.adaptiveMQ2.message;
namespace MDT.Tools.Order.Monitor.Plugin
{
    public partial class Form1 : DockContent
    {
       
        private AmountPlugin demo;
        private NumberPlugin demo2;
       
        public string ip = "";

        public int port = 0;

        public string svrId = "";

        Dictionary<string, object> dic = new Dictionary<string, object>();

        com.adaptiveMQ2.message.Message request = new com.adaptiveMQ2.message.Message();
       
        
        private void settext(int str)
        {

           demo.c=str;
           demo2.c = str;
             
        }
        public Form1()
        {

            InitializeComponent();
            demo = new AmountPlugin();
            demo.Dock = DockStyle.Fill;
            demo2 = new NumberPlugin();
            demo2.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Controls.Add(demo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(demo2, 0, 1);
            this.Load += new EventHandler(Form1_Load);
            this.Disposed += new EventHandler(Form1_Disposed);
            this.WindowState = FormWindowState.Normal;

        }

        private void InitializeCompoment()
        {
            throw new NotImplementedException();
        }

        void Form1_Load(object sender, EventArgs e)
        {

            request.Destination = new Destination("YM.MONITOR.EVENT.CHANGEPERIOD");
            Event_ConfigInfo configInfo;
            configInfo = IniConfigHelpe.ReadConfigInfo();
            ip = configInfo.Ip;
            port = configInfo.Port;
            svrId = configInfo.svrId;
            dic.Add("type", "OS");
            dic.Add("period", "");
            demo.logHelper = new TextLogHelper(label2, this, ip, port);
            demo2.logHelper = new TextLogHelper(label3, this, ip, port);
            radioButton1.Checked = true;
            demo.Visible = true;
            demo2.Visible = false;
            demo.c = 30;
            demo2.c = 30;
            demo.connect();
            demo2.connect();

            

            demo.IsShowHScrollBar = true;//交易耗时监控显示横向滚动条
            demo2.IsShowHScrollBar = true;//成交笔数监控显示横向滚动条
            demo.IsShowVScrollBar = true;//交易耗时监控显示纵向滚动条
            demo2.IsShowVScrollBar = true;//成交笔数监控显示纵向滚动条
            demo.IsShowPointValues = true;//显示鼠标所到处点的值
            demo2.IsShowPointValues = true;

        }




        void Form1_Disposed(object sender, EventArgs e)//销毁
        {
            this.Dispose();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            demo.Visible = true;
            demo2.Visible = false;
            label2.Visible = true;
            label3.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            demo.Visible = false;
            demo2.Visible = true;
            label3.Visible = true;
            label2.Visible = false;

        }

        private void TimeBox_TextChanged(object sender, EventArgs e)
        {
            System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^\d+$");
            if (!rex.IsMatch(TimeBox.Text.ToString()))
            {
                if (TimeBox.Text.ToString() == "")
                {
                    return;

                }

                if (TimeBox.Text.ToString() == " ")
                {
                    TimeBox.Text = null;
                    errorProvider1.SetError(TimeBox, "你输入了换一个空格");
                }
                else
                { TimeBox.Text = null; errorProvider1.SetError(TimeBox, "只能输入数字,且不能为0"); }
            }
            else { errorProvider1.SetError(TimeBox, ""); }
        }




        private void sentMsg()
        {

            if (demo.cedaManager.IsConnected)
            {

                com.adaptiveMQ2.message.Message reply = demo.cedaManager.Request(request);
                if (reply != null)
                {
                    CedaObject co = CedaObject.ToCedaObject(reply);
                    TimeChange result = MsgHelper.Deserialize<TimeChange>(co.MessageBody, MsgSerializerType.Json);
                    if (result.Code == "0000") { label4.Text = "现在时间间隔是" + result.period + "秒,在这里输入时间："; }
                    else { MessageBox.Show("出错啦，没有设置成功哦"); label4.Text = "出错了"; }
                }
                else { MessageBox.Show("响应信息异常"); }
            }
            else
            {
                MessageBox.Show("连接失败");
            }
        }

        private void OkBt_Click(object sender, EventArgs e)
        {
            if (TimeBox.Text.ToString() == string.Empty)
            {
                MessageBox.Show("信息为空");
                return;
            }
            if (int.Parse(TimeBox.Text.ToString()) == 0)
            { MessageBox.Show("不能输入0"); TimeBox.Text = ""; return; }
            else
            {
                dic["period"] = TimeBox.Text.ToString().Trim();
            }
            string msgbody = MsgHelper.Serializer<Dictionary<string, object>>(dic);
            
            request.MessageBody.addString((short)3, msgbody);

            request.MessageBody.addString((short)4, "JSON");
            request.MessageBody.addInt((short)5, 0);
            request.MessageBody.addString(7, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));

            sentMsg();
        }

        private void OKbutton2_Click(object sender, EventArgs e)
        {
            if (CountBox.Text.ToString() == string.Empty)
            {
                MessageBox.Show("信息为空");
                return;
            }
            if (int.Parse(CountBox.Text.ToString()) == 0)
            { MessageBox.Show("不能输入0"); CountBox.Text = ""; return; }
            try
            {
                
                this.settext(int.Parse(CountBox.Text));
            }
            catch { MessageBox.Show("你输入的数字有误，请重新输入"); }

            
        }

        private void CountBox_TextChanged(object sender, EventArgs e)
        {
             System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^\d+$");
             if (!rex.IsMatch(CountBox.Text.ToString()))
            {
                if (CountBox.Text.ToString() == "")
                {
                    return;
                }

                if (CountBox.Text.ToString() == " ")
                {
                    CountBox.Text = null;
                    errorProvider1.SetError(CountBox, "你输入了换一个空格");
                }
               
                else
                { CountBox.Text = null; errorProvider1.SetError(CountBox, "只能输入数字,且不能为0"); }
            }
            else { errorProvider1.SetError(CountBox, ""); }
        }
        }

    }


