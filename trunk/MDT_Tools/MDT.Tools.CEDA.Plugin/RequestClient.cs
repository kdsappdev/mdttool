using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.CEDA.Plugin.DataMemory;
using MDT.Tools.Core.Log;
using MDT.Tools.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;
using com.adaptiveMQ2.client;
using com.adaptiveMQ2.message;

using Message = com.adaptiveMQ2.message.Message;


namespace MDT.Tools.CEDA.Plugin
{
    public partial class RequestClient : DockContent
    {
        private CedaManager _cedaSubscribe = null;
        private ClientInfo _clientInfo = new ClientInfo();
        private ILog logHelper;

        private XMLDataMemory _dataMemory = new XMLDataMemory();
        public static string ModuleName = "Request";
        private bool encryption = false;
        public RequestClient()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            logHelper = new TextLogHelper(txtMsg, this);

            this.Load += new EventHandler(RequestClient_Load);
            this.Disposed += new EventHandler(RequestClient_Disposed);
            this.cbType.SelectedValueChanged += new EventHandler(cbType_SelectedValueChanged);
        }


        void RequestClient_Disposed(object sender, EventArgs e)
        {
            tbtnUnsubscribe_Click(null, null);
        }

        void RequestClient_Load(object sender, EventArgs e)
        {

            InitType();
            InitServiceName();
            SetButtonEnable("1011");

            GetMemory();
            toolTip1.SetToolTip(this.txtHost, "填写方式\n如:ACS方式192.68.1.1/ceda.httpMQTunnel");
        }

        private void InitServiceName()
        {
            object[] temps = { "MQ", "BBOX", "RateInternal" };
            cbServiceName.Items.Clear();
            cbServiceName.Items.AddRange(temps);
            cbServiceName.SelectedIndex = 0;
        }
        void cbType_SelectedValueChanged(object sender, EventArgs e)
        {
            if ("ACS".Equals(cbType.Text.Trim()))
            {
                //cbVS.CheckState = CheckState.Checked;
                //cbVS.Enabled = true;
                //txtPwd.Enabled = true;
                //txtUserName.Enabled = true;
                //txtRole.Enabled = true;
                //ckEncryption.Enabled = true;
            }
            else if ("CEDA".Equals(cbType.Text.Trim()))
            {
                //cbVS.CheckState = CheckState.Unchecked;
                //cbVS.Enabled = false;
                //txtPwd.Enabled = false;
                //txtUserName.Enabled = false;
                //txtRole.Enabled = false;
                //encryption = false;
                //ckEncryption.Enabled = false;
            }
        }

        private void InitType()
        {
            cbType.Items.Clear();
            cbType.Items.AddRange(new object[] { "ACS", "CEDA" });
            cbType.SelectedIndex = 0;
        }

        void _cedaSubscribe_OnCedaMessage(com.adaptiveMQ2.message.Message msg)
        {
            CedaObject co = CedaObject.ToCedaObject(msg);
            string str = string.Format("receive a msg,topic={0},MessageBody={1}", co.Topic, co.MessageBody);

            logHelper.Info(str);
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
                flag = true;
            }
            else
            {
                flag = false;
                logHelper.Info("Please input host and port!");
            }
            return flag;
        }
        Message request;
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
                    host = string.Format("{0}/mq.httpMQTunnel", host);
                }
                _clientInfo.setAddress(host, port);
                if (ckSSL.Checked)
                    _clientInfo.Protocol = ClientInfo.PROTOCOL_HTTPS;
                else
                    _clientInfo.Protocol = ClientInfo.PROTOCOL_HTTP;
                if (cbVS.Checked)
                {
                    _clientInfo.LoginMessage = CedaObject.GetLoginMessage(txtUserName.Text, txtPwd.Text, txtRole.Text, null, encryption);
                }

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

            string temp = string.Format("Address:{0}:{1},Topic:{2}", _clientInfo.AddressHost, _clientInfo.AddressPort,
                txtTopic.Text);
            logHelper.Info(temp);

            request = new Message();
            request.Destination = new Destination(txtTopic.Text.Trim());
            request.SvrID = cbServiceName.Text;
            request.MessageBody.addString((short)3, txtRequest.Text.Trim());
            request.MessageBody.addString((short)4, "JSON");
            request.MessageBody.addInt((short)5, 0);
            request.MessageBody.addString(7, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
            ThreadPool.QueueUserWorkItem(o =>
            {
                _cedaSubscribe.Connect(_clientInfo);
                //if (false)
                //{
                //    if (_cedaSubscribe != null)
                //    {
                //        _cedaSubscribe.Disconnect();
                //        _cedaSubscribe.OnCedaMessage -= new Action<com.adaptiveMQ2.message.Message>(_cedaSubscribe_OnCedaMessage);
                //        _cedaSubscribe = null;
                //    }
                //}

                if (_cedaSubscribe!=null&&_cedaSubscribe.IsConnected)
                {
                    _cedaSubscribe.isStopRequest = false;
                    while (_cedaSubscribe!=null&&!_cedaSubscribe.isStopRequest)
                    {
                        if (_cedaSubscribe.IsConnected)
                        {
                            Message reply = _cedaSubscribe.Request(request);

                            CedaObject co = CedaObject.ToCedaObject(reply);
                            if (co != null)
                            {
                                logHelper.Info(co.Topic + ":" + co.MessageBody);
                            }
                            else
                            {
                                logHelper.Error("reply is null");
                            }
                        }
                        Thread.Sleep(1000);
                    }
                }

            });
            SetButtonEnable("0111");
        }

        private void SetButtonEnable(string flag)
        {
            if (this.InvokeRequired)
            {
                Action<string> method = new Action<string>(SetButtonEnable);
                if (!this.IsDisposed && this.IsHandleCreated)
                    this.Invoke(method, new object[] { flag });
            }
            else
            {
                this.tbtnSubscribe.Enabled = flag[0] == '1' ? true : false;
                this.tbtnUnsubscribe.Enabled = flag[1] == '1' ? true : false;
                this.tbtnClear.Enabled = flag[2] == '1' ? true : false;
                this.tbtnJson.Enabled = flag[3] == '1' ? true : false;
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


        #region Memory
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

            string isACS = GetMemory(MemoryName.IsVS);
            if (!string.IsNullOrEmpty(isACS) && "Y".Equals(isACS))
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

            string serviceName = GetMemory(MemoryName.ServiceName);
            if (!string.IsNullOrEmpty(serviceName))
            {
                cbServiceName.Text = serviceName;
            }

            string requestMsg = GetMemory(MemoryName.RequestMsg);
            if (!string.IsNullOrEmpty(requestMsg))
            {
                txtRequest.Text = requestMsg;
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
            SetMemory(MemoryName.ServiceName, cbServiceName.Text.Trim());
            SetMemory(MemoryName.RequestMsg, txtRequest.Text.Trim());
            SetMemory(MemoryName.UserName, txtUserName.Text.Trim());
            SetMemory(MemoryName.Pwd, txtPwd.Text.Trim());
            SetMemory(MemoryName.RoleName, txtRole.Text.Trim());
            SetMemory(MemoryName.IsEncryption, ckEncryption.Checked ? "Y" : "N");
            SetMemory(MemoryName.IsSSL, ckSSL.Checked ? "Y" : "N");
        }

        private void SetMemory(MemoryName mn, string value)
        {
            _dataMemory.SetData(string.Format("{0}_{1}", ModuleName, mn), value);
        }
        #endregion

        private void txtRequest_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRequest.Text))
            {

                this.tbtnJson.Enabled = false;
            }
            else
            {

                this.tbtnJson.Enabled = true;
            }
        }

        private void tbtnSubscribe_Click(object sender, EventArgs e)
        {
            if (CheckData())
            {
                SetMemory();
                ConnectionServer();
            }
        }

        private void tbtnUnsubscribe_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void tbtnClear_Click(object sender, EventArgs e)
        {
            SetMsg(null);
        }

        private void tbtnJson_Click(object sender, EventArgs e)
        {
             
                this.txtRequest.Text = JsonHelper.JsonFormat(txtRequest.Text);
            
        }

       
    }
}
