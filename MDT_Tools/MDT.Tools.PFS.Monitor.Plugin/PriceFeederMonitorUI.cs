using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.CEDA.Common;
using MDT.Tools.PFS.Monitor.Plugin.ConfigMsg;
using System.Threading;
using com.adaptiveMQ2.client;
using MDT.Tools.Core.Log;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.Core.Utils;
using Ats.Foundation.Message;
using MDT.Tools.PriceFeeder.Monitor.Model;
using MDT.Tools.PFS.Monitor.Plugin.Properties;
using System.Media;

using MDT.Tools.PFS.Monitor.Plugin.DeviceUtil;

namespace MDT.Tools.PFS.Monitor.Plugin
{
    public partial class PriceFeederMonitorUI : DockContent
    {
        public PriceFeederMonitorUI(string _ip, int _port)
        {
            InitializeComponent();
            ip = _ip;
            port = _port;
            this.Load += new EventHandler(PriceFeederMonitorUI_Load);
            this.Disposed += new EventHandler(PriceFeederMonitorUI_Disposed);

            device = new DeviceImpl();
            device.init(this, Microsoft.DirectX.DirectSound.CooperativeLevel.Priority);
        }

        private CedaManager _cedaSubscribe = null;
        private ClientInfo _clientInfo = new ClientInfo();
        private ILog logHelper;
        private PFS_ConfigInfo configInfo;

        private IDevice device = null;

        string ip = "";
        int port = 0;
        void PriceFeederMonitorUI_Load(object sender, EventArgs e)
        {
            logHelper = new TextLogHelper(txtMsg, this, ip, port);
            configInfo = IniConfigHelper.ReadConfigInfo();//获取配置文件

          
            
            ConnectionServer();
        }

        void PriceFeederMonitorUI_Disposed(object sender, EventArgs e)
        {
            Disconnect();

            device.clear();
        }

        private void ConnectionServer()
        {
            _cedaSubscribe = new CedaManager(logHelper);
            _cedaSubscribe.OnCedaMessage += new Action<com.adaptiveMQ2.message.Message>(_cedaSubscribe_OnCedaMessage);


            _clientInfo = new ClientInfo();
            _clientInfo.setAddress(ip, port);
            _clientInfo.setUser("monitor", "monitor");
            _clientInfo.Protocol = ClientInfo.PROTOCOL_TCP;

            ThreadPool.QueueUserWorkItem(o =>
            {
                _cedaSubscribe.Connect(_clientInfo);
                if (_cedaSubscribe == null)
                {
                    return;
                }
                if (_cedaSubscribe.IsConnected)
                {
                    _cedaSubscribe.Subscribe("YM.MONITOR");
                }
            });
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

            ListMONSvr listRes = MsgHelper.Deserialize<ListMONSvr>(co.MessageBody);
 
            setGridView(listRes);
            string str = string.Format("地址：{2}:{3}  Topic={0},MessageBody=\n{1}", co.Topic, co.MessageBody, ip, port);
            LogHelper.Info(str);
        }
        int num = 0;
        private delegate void delSetGridView(ListMONSvr listRes);
        private void setGridView(ListMONSvr listRes)
        {
            if (this.InvokeRequired)
            {
                delSetGridView del = new delSetGridView(setGridView);
                this.Invoke(del, new object[] { listRes });
            }
            else
            {
                List<MON_Svr> listTemp = new List<MON_Svr>();
                foreach (MON_Svr item in listRes.MON_Svr)
                {
                    if (item.stat == 0)
                    {
                        item.Status = Resources.sad;
                    }
                    else if (item.stat == 1)
                    {
                        item.Status = Resources.smile;
                    }
                    listTemp.Add(item);
                }

                List<MON_Price> listTempPrice = new List<MON_Price>();
                int i = 0;
                foreach (MON_Price item in listRes.MON_Price)
                {
                    if (item.alert == 0)
                    {
                        item.Alerts = Resources.smile;
                    }
                    else if (item.alert == 1)
                    {
                        item.Alerts = Resources.sad;
                    }
                    if (configInfo.AlertState != 0)
                    {
                        if (item.nochange >= configInfo.AlertState)
                        {
                            i++;
                        }
                    }
                    else
                    {
                        if (item.nochange >= 60)
                        {
                            i++;
                        }
                    }
                    listTempPrice.Add(item);
                }
               
                //SoundPlayer warnSoundPlayer = new SoundPlayer();
                if (i > 3)
                {
                    //warnSoundPlayer.SoundLocation = Application.StartupPath + configInfo.SoundAlert;    
                    device.addSecondaryBufferAndPlay(Application.StartupPath + configInfo.SoundAlert, "SymbolPriceDelay");                     
                    label1.Text = "产品报价延迟！";
                    //if (warnSoundPlayer != null)
                    //{
                    //    warnSoundPlayer.Play();                   
                    //}
                }
                else
                {                
                  // warnSoundPlayer.Stop();
                    device.stopSecondaryBufferByName("SymbolPiceDelay");
                   label1.Text = "";
                }
             
                //colTemp.Visible = false;
                dataGridView1.DataSource = listTemp;
                dataGridView2.DataSource = listTempPrice;
                num++;
                string ipMsg = " 地址:" + ip + ":" + port + "  " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                //txtMsg.Text = num % 2 == 0 ? ipMsg + "  数据正在接收。" : ipMsg + "  数据正在接收。。。。";
                string tempInt = "";
                if (num > 9)
                {
                    tempInt = num.ToString().Substring(num.ToString().Length - 1, 1);
                }
                else
                {
                    tempInt = num.ToString();
                }

                string[] one = { "1", "4", "7" };
                string[] two = { "2", "5", "8" };
                string[] three = { "0", "3", "6", "9" };

                if (one.Contains(tempInt))
                {
                    txtMsg.Text = ipMsg + "  数据正在接收。";
                }
                else if (two.Contains(tempInt))
                {
                    txtMsg.Text = ipMsg + "  数据正在接收。。。";
                }
                else if (three.Contains(tempInt))
                {
                    txtMsg.Text = ipMsg + "  数据正在接收。。。。。";
                }
            }
        }

        

        

    }
}
