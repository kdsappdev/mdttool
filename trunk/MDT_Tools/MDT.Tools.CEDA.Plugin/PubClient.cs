using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Utils;
using com.adaptiveMQ2.client;
using com.adaptiveMQ2.message;
using MDT.Tools.CEDA.Plugin.DataMemory;
using MDT.Tools.Core.Log;
using WeifenLuo.WinFormsUI.Docking;
using Message = System.Windows.Forms.Message;

namespace MDT.Tools.CEDA.Plugin
{
    public partial class PubClient : DockContent//System.Windows.Forms.Form
    {
        private CedaManager _cedaSubscribe = null;
        private ClientInfo _clientInfo = new ClientInfo();
        private ILog logHelper;

        private XMLDataMemory _dataMemory = new XMLDataMemory();
        public static string ModuleName = "Publish";
        private bool encryption = false;
        public PubClient()
        {
            InitializeComponent();
            logHelper = new TextLogHelper(this.txtMsg, this);
            this.Load += new EventHandler(PubClient_Load);
            btnConnect.Click += new EventHandler(btnConnect_Click);
            btnSend.Click += new EventHandler(btnSend_Click);
            btnStop.Click += new EventHandler(btnStop_Click);
            btnClear.Click += new EventHandler(btnClear_Click);
            tbtnJson.Click += new EventHandler(tbtnJson_Click);
            this.cbType.SelectedValueChanged += new EventHandler(cbType_SelectedValueChanged);
            this.Disposed += new EventHandler(RequestClient_Disposed);

        }

        void tbtnJson_Click(object sender, EventArgs e)
        {
            txtMsgBody.Text = JsonHelper.JsonFormat(txtMsgBody.Text);

        }
        void RequestClient_Disposed(object sender, EventArgs e)
        {
            btnStop_Click(null, null);
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

        void btnClear_Click(object sender, EventArgs e)
        {
            SetMsg(null);
        }

        void btnStop_Click(object sender, EventArgs e)
        {
            Disconnect();
            SetEnable("1001");
        }

        void btnSend_Click(object sender, EventArgs e)
        {
            request = new com.adaptiveMQ2.message.Message();
            request.Destination = new Destination(txtTopic.Text.Trim());
            request.MessageBody.addString((short)3, txtMsgBody.Text.Trim());
            request.MessageBody.addString((short)4, "JSON");
            request.MessageBody.addInt((short)5, 0);
            request.MessageBody.addString(7, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
            if (!_cedaSubscribe.IsConnected)
                ConnectionServer();

            _cedaSubscribe.SendMessage(request);
            logHelper.Info("send a msg,topic: " + request.Destination.getName());
        }

        void btnConnect_Click(object sender, EventArgs e)
        {
            if (CheckData())
            {
                SetMemory();
                ConnectionServer();
                SetEnable("0111");
            }
        }
        com.adaptiveMQ2.message.Message request;
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
            ThreadPool.QueueUserWorkItem(o => _cedaSubscribe.Connect(_clientInfo));
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
        void PubClient_Load(object sender, EventArgs e)
        {
            SetEnable("1001");
            InitType();
            GetMemory();

        }

        private void InitType()
        {
            cbType.Items.Clear();
            cbType.Items.AddRange(new object[] { "ACS", "CEDA" });
            cbType.SelectedIndex = 0;
        }

        private void Disconnect()
        {

            if (_cedaSubscribe != null)
            {
                _cedaSubscribe.Disconnect();
                _cedaSubscribe.OnCedaMessage -= new Action<com.adaptiveMQ2.message.Message>(_cedaSubscribe_OnCedaMessage);
                _cedaSubscribe = null;
            }
        }
        void _cedaSubscribe_OnCedaMessage(com.adaptiveMQ2.message.Message msg)
        {
            CedaObject co = CedaObject.ToCedaObject(msg);
            string str = string.Format("receive a msg,topic={0},MessageBody=\n{1}", co.Topic, co.MessageBody);

            logHelper.Info(str);
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

            //string serviceName = GetMemory(MemoryName.ServiceName);
            //if (!string.IsNullOrEmpty(serviceName))
            //{
            //    cbServiceName.Text = serviceName;
            //}

            string requestMsg = GetMemory(MemoryName.PubMsg);
            if (!string.IsNullOrEmpty(requestMsg))
            {
                txtMsgBody.Text = requestMsg;
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
            //SetMemory(MemoryName.ServiceName, cbServiceName.Text.Trim());
            SetMemory(MemoryName.PubMsg, txtMsgBody.Text.Trim());
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

        private void SetEnable(string str)
        {
            if (!this.IsDisposed && this.IsHandleCreated)
            {
                if (this.InvokeRequired)
                {
                    Action<string> method = SetEnable;
                    this.Invoke(method, new object[] { str });
                }
                else
                {
                    if (!string.IsNullOrEmpty(str) && str.Length == 4)
                    {
                        btnConnect.Enabled = str[0] == '1';
                        btnSend.Enabled = str[1] == '1';
                        btnStop.Enabled = str[2] == '1';
                        btnClear.Enabled = str[3] == '1';
                    }
                    else
                    {
                        btnClear.Enabled = true;
                        btnConnect.Enabled = true;
                        btnSend.Enabled = true;
                        btnStop.Enabled = true;
                    }
                }
            }
        }

        private void txtMsgBody_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMsgBody.Text))
            {
                tbtnJson.Enabled = false;
            }
            else
            {
                tbtnJson.Enabled = true;
            }
        }

      


    }
}
