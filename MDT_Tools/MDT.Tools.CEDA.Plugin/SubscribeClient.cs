using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Ats.Foundation.Message;
using MDT.Tools.CEDA.Plugin;
using MDT.Tools.CEDA.Plugin.DataMemory;
using MDT.Tools.Core.Log;
using MDT.Tools.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;
using com.adaptiveMQ2.client;
using System.Timers;
using System.Text.RegularExpressions;
using System.IO;

namespace MDT.Tools.CEDA.Plugin
{
    public partial class SubscribeClient : DockContent
    {
        private IClientConnection conn = null;
        private ClientInfo _clientInfo = new ClientInfo();

        private CedaManager _cedaSubscribe = null;
        private ILog logHelper;
        private XMLDataMemory _dataMemory = new XMLDataMemory();
        public static string ModuleName = "Subscribe";
        private bool encryption = false; 
        public SubscribeClient()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            logHelper = new TextLogHelper(txtMsg, this);

            this.Load += new EventHandler(SubscribeClient_Load);

            this.Disposed += new EventHandler(SubscribeClient_Disposed);
            this.cbType.SelectedValueChanged += new EventHandler(cbType_SelectedValueChanged);

        }
        //测试大数据
        //string temPath = DataPath + "\\data\\" + "StreamWriter3wan.data";
        private void SecBtnCheck_Click(object sender, EventArgs e)
        {
            string temp = filePath + "." + Guid.NewGuid().ToString();

            if (sw != null)
            {
                sw.Flush();
            }
            File.Copy(filePath, temp);
            //统计
            string Message = string.Format("类型:{0},地址:{1},端口:{2},主题:{3}", cbType.Text, txtHost.Text, txtPort.Text, txtTopic.Text);

            ChartHelper chart = new ChartHelper();
            chart.SecReaderData(temp, Message);

            File.Delete(temp);

        }



        private void MinBtnCheck_Click(object sender, EventArgs e)
        {
            string temp = filePath + "." + Guid.NewGuid().ToString();

            if (sw != null)
            {
                sw.Flush();
            }
            File.Copy(filePath, temp);
            //统计
            string Message = string.Format("类型:{0},地址:{1},端口:{2},主题:{3}", cbType.Text, txtHost.Text, txtPort.Text, txtTopic.Text);

            ChartHelper chart = new ChartHelper();
            chart.MinReaderData(temp, Message);

            File.Delete(temp);
        }


        void SubscribeClient_Disposed(object sender, EventArgs e)
        {
            if (aTimer != null)
            {
                aTimer.Stop();
                aTimer.Close();
            }

            if (sw != null)
            {
                sw.Dispose();
                //sw.Close();
            }
            if (fs != null)
            {
                fs.Close();
            }

            tbtnUnsubscribe_Click(null, null);

        }

        void cbType_SelectedValueChanged(object sender, EventArgs e)
        {
            //_clientInfo=new ClientInfo();
            if ("ACS".Equals(cbType.Text.Trim()))
            {
                //txtHost.Text = "192.168.20.2/mq.httpMQTunnel";
                //txtPort.Text = "29990";
                //txtTopic.Text = "OHLC.*";
                //cbVS.CheckState = CheckState.Checked;
                //cbVS.Enabled = true;
                //txtUserName.Enabled = true;
                //txtPwd.Enabled = true;
                //txtRole.Enabled = true;
                //ckEncryption.Enabled = true;
            }
            else if ("CEDA".Equals(cbType.Text.Trim()))
            {
                //txtHost.Text = "192.168.20.2";
                //txtPort.Text = "8001";
                //txtTopic.Text = "OHLC.*";
                //cbVS.CheckState = CheckState.Unchecked;
                //cbVS.Enabled = false;
                //txtUserName.Enabled = false;
                //txtPwd.Enabled = false;
                //txtRole.Enabled = false;
                //encryption = false;
                //ckEncryption.Enabled = false;
            }
        }

        private static readonly string DataPath = Application.StartupPath;

        private StreamWriter sw;

        private FileStream fs;

        //Guid.NewGuid().ToString();
        string filePath = "";
        void SubscribeClient_Load(object sender, EventArgs e)
        {
            //setUpFile();
            InitType();
            SetButtonEnable("1010");
            GetMemory();
            
        }





        private void InitType()
        {
            cbType.Items.Clear();
            cbType.Items.AddRange(new object[] { "ACS", "CEDA" });
            cbType.SelectedIndex = 0;
        }
        int TopicCount = 0;
        int OneSCount = 0;
        int ClearCount = 0;
        int OneSMaxCount = 0;



        DateTime StartTime, StopTime;
        void _cedaSubscribe_OnCedaMessage(com.adaptiveMQ2.message.Message msg)
        {

            TopicCount++;
            OneSCount++;

            //StopTime = DateTime.Now;
            //TimeSpan ts = StopTime - StartTime;
            //int hourTime = ts.Hours;
            //int minuteTime = ts.Minutes;
            //int secondTime = (hourTime * 3600) + (minuteTime * 60) + ts.Seconds;
            //labelTime.Text = secondTime.ToString() + "S";


            labelCount.Text = TopicCount.ToString();
            int clear = -1;
            if (!int.TryParse(txtClear.Text, out clear))
            {
                clear = int.MaxValue;
            }
            if (ClearCount >= clear)
            {
                if (ClearCount > 0)
                {
                    SetMsg(null);
                    ClearCount = 0;
                }
            }
            else
            {
                ClearCount++;
                CedaObject co = CedaObject.ToCedaObject(msg);
                string str = string.Format("receive a msg,topic={0},MessageBody=\n{1}", co.Topic, co.MessageBody);

                logHelper.Info(str);
            }


        }


        private static System.Timers.Timer aTimer;
        private void tbtnSubscribe_Click(object sender, EventArgs e)
        {

            setUpFile();

            aTimer = new System.Timers.Timer(10000);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;

            StartTime = DateTime.Now;
            TopicCount = 0;
            labelCount.Text = "0";
            OneSCount = 0;
            label1SCount.Text = "0";
            labelMTCount.Text = "0";
            labelTime.Text = "0S";
            OneSMaxCount = 0;
            label1MaxCount.Text = "0";


            if (CheckData())
            {
                SetMemory();
                ConnectionServer();
            }
        }

        private void setUpFile()
        {

            string guid = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = guid.ToString() + ".data";
            filePath = DataPath + "\\data\\" + fileName;

            fs = new FileStream(filePath, FileMode.Create);
            sw = new StreamWriter(fs, Encoding.UTF8);
        }
        private void tbtnUnsubscribe_Click(object sender, EventArgs e)
        {
            if (aTimer != null)
            {
                aTimer.Enabled = false;
                aTimer.Close();
            }

            if (sw != null)
            {
                sw.Close();
            }
            if (fs != null)
            {
                fs.Close();
            }

            sw = null;
            fs = null;

            Disconnect();
        }


        private delegate void DeleOnTimed(object source, ElapsedEventArgs e);
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                DeleOnTimed del = new DeleOnTimed(OnTimedEvent);
                this.Invoke(del, new object[] { source, e });
            }
            else
            {
                StopTime = DateTime.Now;
                TimeSpan ts = StopTime - StartTime;
                int hourTime = ts.Hours;
                int minuteTime = ts.Minutes;
                int secondTime = (hourTime * 3600) + (minuteTime * 60) + ts.Seconds;


                labelTime.Text = secondTime.ToString() + "S";
                if (secondTime > 0)
                {
                    double meanTime = TopicCount / secondTime;
                    labelMTCount.Text = meanTime.ToString();
                }

                label1SCount.Text = OneSCount.ToString();
                LogHelper.Debug(string.Format("类型:{0},地址:{1},端口:{2},主题:{3},最近1秒内tps/s:{4}", cbType.Text, txtHost.Text, txtPort.Text, txtTopic.Text, label1SCount.Text));

                if (sw != null)
                {
                    sw.WriteLine(string.Format("{0},{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), label1SCount.Text));
                }


                if (OneSCount > OneSMaxCount)
                {
                    OneSMaxCount = OneSCount;
                    label1MaxCount.Text = OneSMaxCount.ToString();
                }
                OneSCount = 0;
            }
        }
        private void Disconnect()
        {
            if (_cedaSubscribe != null)
            {
                _cedaSubscribe.Disconnect();

                _cedaSubscribe.OnCedaMessage -= new Action<com.adaptiveMQ2.message.Message>(_cedaSubscribe_OnCedaMessage);
                _cedaSubscribe = null;
            }
            SetButtonEnable("1011");
        }

        private bool CheckData()
        {
            bool flag;
            if (!string.IsNullOrEmpty(txtHost.Text.Trim()) && !string.IsNullOrEmpty(txtPort.Text.Trim()) && !string.IsNullOrEmpty(txtTopic.Text.Trim()))
            {
                if(rbtnSubImage.Checked&&string.IsNullOrEmpty(cbServiceName.Text.Trim()))
                {
                    logHelper.Info("Please input service name!");
                }
                flag = true;
            }
            else
            {
                flag = false;
                logHelper.Info("Please input host,port,topic!");
            }
            return flag;
        }

        private void ConnectionServer()
        {
            _cedaSubscribe = new CedaManager(logHelper);
            _cedaSubscribe.OnCedaMessage += new Action<com.adaptiveMQ2.message.Message>(_cedaSubscribe_OnCedaMessage);
            string host = txtHost.Text.Trim();
            int port = 0;
            if (!Int32.TryParse(txtPort.Text.Trim(), out port))
            {
                logHelper.Warn("port is error!");
                return;
            }

            _clientInfo = new ClientInfo();


            if ("ACS".Equals(cbType.Text.Trim()))
            {
                if (!host.EndsWith(".httpMQTunnel"))
                {
                    host = string.Format("{0}/ceda.httpMQTunnel", host);
                }
                _clientInfo.setAddress(host, port);
                if (ckSSL.Checked)
                    _clientInfo.Protocol = ClientInfo.PROTOCOL_HTTPS;
                else
                    _clientInfo.Protocol = ClientInfo.PROTOCOL_HTTP;
                if (cbVS.Checked)
                    _clientInfo.LoginMessage = CedaObject.GetLoginMessage(txtUserName.Text, txtPwd.Text, txtRole.Text, null, encryption);
            }
            else if ("CEDA".Equals(cbType.Text.Trim()))
            {
                _clientInfo.setAddress(host, port);
                _clientInfo.setUser("test", "test");
                if (ckSSL.Checked)
                    _clientInfo.Protocol = ClientInfo.PROTOCOL_TCPS;
                else
                    _clientInfo.Protocol = ClientInfo.PROTOCOL_TCP;
                if (cbVS.Checked)
                    _clientInfo.LoginMessage = CedaObject.GetLoginMessage(txtUserName.Text, txtPwd.Text, txtRole.Text, null, encryption);
            }

            ThreadPool.QueueUserWorkItem(o =>
            {
                _cedaSubscribe.Connect(_clientInfo);
                if (_cedaSubscribe == null)
                {
                    return;
                }
                if (_cedaSubscribe.IsConnected)
                {
                    if (rbtnSub.Checked)
                    {
                        _cedaSubscribe.Subscribe(txtTopic.Text.Trim());
                    }else if(rbtnSubImage.Checked)
                    {
                        _cedaSubscribe.SubscribeWithImage(txtTopic.Text.Trim(),cbServiceName.Text.Trim());
                    }

                }

            });
            SetButtonEnable("0111");
        }

        private void SetButtonEnable(string value)
        {
            if (this.InvokeRequired)
            {
                Action<string> method = new Action<string>(SetButtonEnable);
                if (!this.IsDisposed && this.IsHandleCreated)
                    this.Invoke(method, new object[] { value });
            }
            else
            {
                tbtnSubscribe.Enabled = '1'.Equals(value[0]);
                tbtnUnsubscribe.Enabled = '1'.Equals(value[1]);
                tbtnClear.Enabled = '1'.Equals(value[2]);
                toolStripSplitButton1.Enabled = '1'.Equals(value[3]);

            }
        }

        private void SetMsg(string msg)
        {
            if (this.InvokeRequired)
            {
                Action<string> method = new Action<string>(SetMsg);
                if (!this.IsDisposed && this.IsHandleCreated)
                    this.Invoke(method, new object[] { msg });
            }
            else
            {
                if (string.IsNullOrEmpty(msg))
                {
                    txtMsg.Clear();
                }
                else
                {
                    txtMsg.AppendText(msg + "\n");
                    txtMsg.ScrollToCaret();
                }
            }
        }




        private void tbtnClear_Click(object sender, EventArgs e)
        {
            SetMsg(null);
        }

        #region data Memoryd
        private void GetMemory()
        {
            string address = GetMemory(MemoryName.Address);
            if (!string.IsNullOrEmpty(address))
            {
                txtHost.Text = address;
            }

            string port = GetMemory(MemoryName.Port);
            if (!string.IsNullOrEmpty(port))
                txtPort.Text = port;

            string isVS = GetMemory(MemoryName.IsVS);
            if (!string.IsNullOrEmpty(isVS) && "Y".Equals(isVS))
            {
                cbVS.CheckState = CheckState.Checked;
            }
            else
            {
                cbVS.CheckState = CheckState.Unchecked;
            }

            string topic = GetMemory(MemoryName.Topic);
            if (!string.IsNullOrEmpty(topic))
            {
                txtTopic.Text = topic;
            }

            string serverType = GetMemory(MemoryName.ServerType);
            if (!string.IsNullOrEmpty(serverType))
            {
                cbType.Text = serverType;
            }

            string userName = GetMemory(MemoryName.UserName);
            if (!string.IsNullOrEmpty(userName))
                txtUserName.Text = userName;

            string pwd = GetMemory(MemoryName.Pwd);
            if (!string.IsNullOrEmpty(pwd))
                txtPwd.Text = pwd;

            string roleName = GetMemory(MemoryName.RoleName);
            if (!string.IsNullOrEmpty(roleName))
                txtRole.Text = roleName;
            //string serviceName = GetMemory(MemoryName.ServiceName);
            //if (!string.IsNullOrEmpty(serviceName))
            //{
            //    cbServiceName.Text = serviceName;
            //}

            //string requestMsg = GetMemory(MemoryName.RequestMsg);
            //if (!string.IsNullOrEmpty(requestMsg))
            //{
            //    txtRequest.Text = requestMsg;
            //}
            string clearName = GetMemory(MemoryName.ClearNm);
            //if (!string.IsNullOrEmpty(clearName))
                txtClear.Text = clearName;
           cbServiceName.Text= GetMemory(MemoryName.ServiceName);
            string subType = GetMemory(MemoryName.SubType);
            if(subType=="1")
            {
                rbtnSub.Checked = true;
            }
            else if(subType=="2")
            {
                rbtnSubImage.Checked = true;
            }
            string isEncryption = GetMemory(MemoryName.IsEncryption);
            if (!string.IsNullOrEmpty(isEncryption) && "Y".Equals(isEncryption))
            {
                ckEncryption.CheckState = CheckState.Checked;
            }
            else
            {
                ckEncryption.CheckState = CheckState.Unchecked;
            }
            string isSSL = GetMemory(MemoryName.IsSSL);
            if (!string.IsNullOrEmpty(isSSL) && "Y".Equals(isSSL))
            {
                ckSSL.CheckState = CheckState.Checked;
            }
            else
            {
                ckSSL.CheckState = CheckState.Unchecked;
            }
        }

        private string GetMemory(MemoryName mn)
        {
            //string temp = "";
            return _dataMemory.GetData(string.Format("{0}_{1}", ModuleName, mn));
        }


        private void SetMemory()
        {
            SetMemory(MemoryName.ServerType, cbType.Text.Trim());
            SetMemory(MemoryName.Address, txtHost.Text.Trim());
            SetMemory(MemoryName.Port, txtPort.Text.Trim());
            SetMemory(MemoryName.IsVS, cbVS.Checked ? "Y" : "N");
            SetMemory(MemoryName.Topic, txtTopic.Text.Trim());
            SetMemory(MemoryName.UserName, txtUserName.Text.Trim());
            SetMemory(MemoryName.Pwd, txtPwd.Text.Trim());
            SetMemory(MemoryName.RoleName, txtRole.Text.Trim());
            //SetMemory(MemoryName.ServiceName, cbServiceName.Text.Trim());
            //SetMemory(MemoryName.RequestMsg, txtRequest.Text.Trim());
            SetMemory(MemoryName.ClearNm, txtClear.Text.Trim());
            SetMemory(MemoryName.ServiceName,cbServiceName.Text);
            SetMemory(MemoryName.IsEncryption, ckEncryption.Checked ? "Y" : "N");
            SetMemory(MemoryName.IsSSL, ckSSL.Checked ? "Y" : "N");
            if(rbtnSub.Checked)
            {
                SetMemory(MemoryName.SubType, "1");
            }
            else
            {
                SetMemory(MemoryName.SubType, "2");
            }
           
        }
        private void SetMemory(MemoryName mn, string value)
        {
            _dataMemory.SetData(string.Format("{0}_{1}", ModuleName, mn), value);
        }
        #endregion

        private string pattern = @"^[\-]?[0-9]*$";
        private string temp = String.Empty;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Match m = Regex.Match(this.txtClear.Text, pattern);   // 匹配正则表达式

            if (!m.Success)   // 输入的不是数字
            {
                this.txtClear.Text = temp;   // textBox内容不变
                // 将光标定位到文本框的最后
                this.txtClear.SelectionStart = this.txtClear.Text.Length;
            }
            else   // 输入的是数字
            {
                temp = this.txtClear.Text;   // 将现在textBox的值保存下来
            }
        }

        private void rbtnSubImage_CheckedChanged(object sender, EventArgs e)
        {
           lcServerName.Visible = cbServiceName.Visible = rbtnSubImage.Checked;
        }
    }
}
