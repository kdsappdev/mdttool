using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MDT.Tools.CEDA.Common;
using WeifenLuo.WinFormsUI.Docking;
using com.adaptiveMQ2.client;
using MDT.Tools.Core.Log;
using System.Threading;
using Ats.Foundation.Message;
using MDT.Tools.Event.Monitor.Plugin.Model;
using Microsoft.DirectX.DirectSound;
using Microsoft.DirectX;
using System.IO;
using com.adaptiveMQ2.message;
using Ats.Foundation.Message.Serializer;

namespace MDT.Tools.Event.Monitor.Plugin
{
    public partial class Form1 : DockContent
    {
        private Form2 f2;

        bool isClose;
        string ip = "";
        string serId = "";
        int port = 0;
        private CedaManager _cedaSubscribe = null;
        private ClientInfo _clientInfo;
        private ILog logHelper;
        SecondaryBuffer ApplicationBuffer;
        Device device;
        Dictionary<string, object> dic = new Dictionary<string, object>();
        com.adaptiveMQ2.message.Message request = new com.adaptiveMQ2.message.Message();
        Queue<EventMessage> msgqueue = new Queue<EventMessage>();//用来作为table的数据源
        int x;

        public Form1(string _ip, int _port, string _serId)
        {
            ip = _ip;
            port = _port;
            serId = _serId;
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            request.Destination = new Destination("YM.MONITOR.EVENT.CHANGEPERIOD");
            dic.Add("type", "PE");
            dic.Add("period", "");

            this.Load += new EventHandler(Form1_Load);
            this.Disposed += new EventHandler(Form1_Disposed);



            dgv.ReadOnly = true;
            dgv.Enabled = true;

            device = new Device();
            device.SetCooperativeLevel(this, Microsoft.DirectX.DirectSound.CooperativeLevel.Priority);

            logHelper = new TextLogHelper(txtMsg, this, ip, port);

            ConnectionServer();

            for (int i = 0; i < 20; i++)
            {
                EventMessage em = new EventMessage();

                msgqueue.Enqueue(em);
            }

            InitDt(new EventMessage());
        }

        void Form1_Disposed(object sender, EventArgs e)
        {
            Disconnect();

            if (null != ApplicationBuffer)
            {
                ApplicationBuffer.Stop();
            }
        }

        //在MsgGridView中插入值
        private void InitDt(EventMessage msg)
        {



            msgqueue.Dequeue();
            msgqueue.Enqueue(msg);

            dgv.DataSource = msgqueue.Reverse().ToList();
            //RowColor();
            dgv.Refresh();

            RowColor();





        }

        //配置按钮的事件监听方法
        private void SettingButton_Click(object sender, EventArgs e)
        {
            f2 = new Form2();
            f2.FatalErrorColorChang += new Form2.FatalErrorColorChangeHandler(FatalErrorColorChang);//绑定（严重的错误）委托方法
            f2.Show(this);

        }

        private delegate void setLevel(EventMessage msg);//新建一个委托，用来对外来的进程事件做响应
        #region 处理消息
        //判断信息级别，分配相对应的处理方法
        private void SetLevel(EventMessage msg)
        {
            if (this.InvokeRequired)
            {
                setLevel del = new setLevel(SetLevel);
                this.Invoke(del, msg);
            }
            else
            {
                string s = string.Format("{0}-{1}-{2} ", msg.date.Substring(0, 4), msg.date.Substring(4, 2), msg.date.Substring(6, 2));
                msg.date = s;
                string level = SystemConfig.GetConfigData("levelComboBox", string.Empty);


                if (!level.Equals(string.Empty))
                {
                    if (level.Equals("信息")) { info(msg); }
                    if (level.Equals("警告")) { warn(msg); }
                    if (level.Equals("错误")) { error(msg); }
                    if (level.Equals("严重的错误")) { fatalerror(msg); }
                }
                else
                {
                    info(msg);
                }

            }
        }

        void info(EventMessage msg)
        {
            InitDt(msg);
            Voice(msg);
        }

        void warn(EventMessage msg)
        {
            if (msg.level.ToString() == "Info")
            {
                return;
            }
            else { InitDt(msg); Voice(msg); }
        }

        void error(EventMessage msg)
        {
            if (msg.level.ToString() == "Warn" || msg.level.Trim() == "Info")
            {
                return;
            }
            else { InitDt(msg); Voice(msg); }
        }

        void fatalerror(EventMessage msg)
        {
            if (msg.level.ToString() == "FatalError")
            {
                InitDt(msg);
                Voice(msg);
            }
            else { return; }
        }

        //设置对应的级别的背景颜色
        public void FatalErrorColorChang()
        {
            RowColor();
        }
        // Color.FromArgb(Int32.Parse(c[0]), Int32.Parse(c[1]), Int32.Parse(c[2]));
        //判断是什么级别的信息，并且播放制定声音
        private void Voice(EventMessage msg)
        {
            if (!isClose)
            {

                if (msg.level.Trim() == "Info")
                {
                    string voicePath = SystemConfig.GetConfigData("infoVoiceBt", string.Empty);
                    string s = SystemConfig.GetConfigData("infoVoiceT", string.Empty);
                    if (voicePath == string.Empty)
                    {
                        string path = Application.StartupPath + @"\control\resouse\recycle.wav";
                        PlaySound(path, s);
                    }
                    else
                    {
                        string path = voicePath;
                        PlaySound(path, s);
                    }
                }
                if (msg.level.Trim() == "Warn")
                {
                    string voicePath = SystemConfig.GetConfigData("warningVoiceBt", string.Empty);
                    string s = SystemConfig.GetConfigData("warnVioceT", string.Empty);
                    if (voicePath == string.Empty)
                    {
                        string path = Application.StartupPath.ToString() + @"\control\resouse\ir_begin.wav";
                        PlaySound(path, s);
                    }
                    else
                    {
                        string path = voicePath;
                        PlaySound(path, s);
                    }
                }
                if (msg.level.Trim() == "Error")
                {
                    string voicePath = SystemConfig.GetConfigData("errorVoiceBt", string.Empty);
                    string s = SystemConfig.GetConfigData("errorVoiceT", string.Empty);
                    if (voicePath == string.Empty)
                    {
                        string path = Application.StartupPath.ToString() + @"\control\resouse\notify.wav";
                        PlaySound(path, s);
                    }
                    else
                    {
                        string path = voicePath;
                        PlaySound(path, s);
                    }
                }
                if (msg.level.Trim() == "FatalError")
                {
                    string voicePath = SystemConfig.GetConfigData("fatalErrorVoiceBt", string.Empty);
                    string s = SystemConfig.GetConfigData("fatalErrorVT", string.Empty);
                    if (voicePath == string.Empty)
                    {
                        string path = Application.StartupPath.ToString() + @"\control\resouse\Windows User Account Control.wav";
                        PlaySound(path, s);
                    }
                    else
                    {
                        string path = voicePath;
                        PlaySound(path, s);
                    }
                }
                else
                {
                    return;

                }
            }
        }
        #endregion
        #region 播放声音
        //用来播放声音
        private void PlaySound(string path, string s)
        {

            if (!path.Equals(string.Empty))
            {

                if (File.Exists(path))
                {
                    BufferDescription buff = new BufferDescription();
                    buff.GlobalFocus = true;

                    getsound(path, buff);
                    if (s.Equals(string.Empty))
                    {
                        ApplicationBuffer.Play(0, BufferPlayFlags.Default);//播放一次

                    }
                    else
                    {
                        if (s.Equals("响铃一次")) { ApplicationBuffer.Play(0, BufferPlayFlags.Default); }
                        if (s.Equals("循环播放")) { ApplicationBuffer.Play(0, BufferPlayFlags.Looping); }
                    }
                }
                else { MessageBox.Show(" 音乐文件找不到了"); }
            }
            else
            {
                MessageBox.Show("没有获取到音乐文件路径");
            }
        }

        private Boolean getsound(string soundpath, BufferDescription b)//为选择的声音开辟第二缓冲区
        {
            if (null != ApplicationBuffer)
            {
                ApplicationBuffer.Stop();
            }
            try
            {
                ApplicationBuffer = new SecondaryBuffer(soundpath, b, device);

            }
            catch (SoundException)
            {
                return false;
            }
            return true;
        }
        #endregion
        //连接服务器，并订阅目标消息
        private void ConnectionServer()
        {
            //MessageBox.Show("kaishilianjie");
            _cedaSubscribe = new CedaManager(logHelper);
            _cedaSubscribe.OnCedaMessage += new Action<com.adaptiveMQ2.message.Message>(_cedaSubscribe_OnCedaMessage);



            _clientInfo = new ClientInfo();
            _clientInfo.setAddress(ip, port);
            _clientInfo.setUser("monitor", "monitor");
            _clientInfo.Protocol = ClientInfo.PROTOCOL_TCP;
            //MessageBox.Show("启动线程，并订阅消息");
            try
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    _cedaSubscribe.Connect(_clientInfo);
                    if (_cedaSubscribe == null)
                    {
                        return;
                    }
                    if (_cedaSubscribe.IsConnected)
                    {
                        string msgbody = MsgHelper.Serializer<Dictionary<string, object>>(dic);

                        request.MessageBody.addString((short)3, msgbody);

                        request.MessageBody.addString((short)4, "JSON");
                        request.MessageBody.addInt((short)5, 0);
                        request.MessageBody.addString(7, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));

                        sentMsg();
                        _cedaSubscribe.Subscribe("monitor.event.*");

                    }
                });
            }
            catch { txtMsg.Text = "连接失败，请检查链接。SerID:" + serId + "   IP:" + ip + "   PORT:" + port; }
        }

        //当程序关闭时释放资源
        private void Disconnect()
        {
            if (_cedaSubscribe != null)
            {
                _cedaSubscribe.Disconnect();
                _cedaSubscribe.OnCedaMessage -= new Action<com.adaptiveMQ2.message.Message>(_cedaSubscribe_OnCedaMessage);
                _cedaSubscribe = null;
            }
        }

        //接受信息并处理信息的方法
        void _cedaSubscribe_OnCedaMessage(com.adaptiveMQ2.message.Message msg)
        {
            //MessageBox.Show("收到消息");
            CedaObject co = CedaObject.ToCedaObject(msg);

            EventMessage listRes = MsgHelper.Deserialize<EventMessage>(co.MessageBody);

            SetLevel(listRes);

        }

        //当用户看到信息时可以手动暂时关闭声音
        private void soundOff_Click(object sender, EventArgs e)
        {
            if (null != ApplicationBuffer)
            {
                ApplicationBuffer.Stop();
            }
            if (soundOff.Text.ToString() == "开启声音") { isClose = false; soundOff.Text = "静音"; return; }
            if (soundOff.Text.ToString() == "静音") { isClose = true; soundOff.Text = "开启声音"; return; }
        }
        #region 设置颜色
        //从文件中得到每个级别的颜色数据，设置每个级别的背景色
        private void RowColor()
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
        #endregion
        private void sentMsg()
        {

            if (_cedaSubscribe.IsConnected)
            {

                com.adaptiveMQ2.message.Message reply = _cedaSubscribe.Request(request);
                if (reply != null)
                {
                    CedaObject co = CedaObject.ToCedaObject(reply);
                    TimeChange result = MsgHelper.Deserialize<TimeChange>(co.MessageBody, MsgSerializerType.Json);
                    if (result.Code == "0000") { TmMsg.Text = "现在时间间隔是" + result.period + "秒,在这里输入时间："; }
                    else { MessageBox.Show("出错啦，没有设置成功哦"); TmMsg.Text = "出错了"; }
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
            if (int.Parse(TimeBox.Text.ToString()) == 0 | int.Parse(TimeBox.Text.ToString()) < 0) { MessageBox.Show("输入信息有误，不能为0或者负数。"); TimeBox.Text = ""; return; }
            else
            {
                dic["period"] = TimeBox.Text.ToString().Trim();
                TimeBox.Text = "";
            }
            string msgbody = MsgHelper.Serializer<Dictionary<string, object>>(dic);

            request.MessageBody.addString((short)3, msgbody);

            request.MessageBody.addString((short)4, "JSON");
            request.MessageBody.addInt((short)5, 0);
            request.MessageBody.addString(7, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));

            sentMsg();
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
                { TimeBox.Text = null; errorProvider1.SetError(TimeBox, "只能输入数字,且不能为0或者负数"); }
            }
            else { errorProvider1.SetError(TimeBox, ""); }
        }

        private void TimeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) { OkBt_Click(OkBt, null); }
        }

    }
}
