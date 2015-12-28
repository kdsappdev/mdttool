using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using MDT.Tools.CEDA.Common;
using MDT.Tools.Core.Log;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using System.Media;
using com.adaptiveMQ2.client;
using MDT.Tools.Core.Utils;
using Ats.Foundation.Message;

using Ats.Foundation.Message.Serializer;

namespace MDT.Tools.User.Monitor.Plugin
{ 
    public delegate void myMethodDelegate(); 
    public partial class Form1 : DockContent
    
    {
       
        static SoundPlayer sp = null;        
        public static int user_warning_count;
    //    public event myMethodDelegate mydele;
        public Form1()
        {
            
            InitializeComponent();
            initialize_soundPlayer();
            initialize_warning_count();
            initialize_AlarmLimit();
            initialize_ip_port_svrID();
         //   ConnectionServer();
            //Disconnect();
            connect();
        }


    /// <summary>
    /// 初始化全局播放器
    /// </summary>
        private void initialize_soundPlayer() {
            
            sp = new SoundPlayer();
            sp.SoundLocation = SystemConfig.GetConfigData("music_url", string.Empty);
            
        }
        /// <summary>
        /// 初始话全局报警阈值
        /// </summary>
        public static void initialize_warning_count() {
         //   string s = SystemConfig.GetConfigData("user_warning_count", string.Empty);
            try
            {   
                user_warning_count = Int32.Parse(SystemConfig.GetConfigData("user_warning_count", string.Empty));
            }catch(Exception){
                MessageBox.Show("您上次设置的报警界限不合法，请重新设置!现在默认为1000");
                user_warning_count = 1000;
                SystemConfig.WriteConfigData("user_warning_count", ""+1000);
        
            }
        }
        /// <summary>
        ///  初始化报警阈值的label显示
        /// </summary>
        private void initialize_AlarmLimit() {
            label_for_AlarmLimit.Text = "用户在线数报警阈值：" + user_warning_count;
        }
        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            String str = Button_for_sound.Text;
           
            if (str == "静音")
            {
                Button_for_sound.Text = "取消静音";
                timer1.Enabled = false;
                sp.Stop();
      
            }
             if (str == "取消静音")
            {
                Button_for_sound.Text = "静音";//播放器重启

                try
                {
                    timer1.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("您的定制音频可能丢失，或者损坏!");
                }
            }



        }
        /// <summary>
        /// 呼出设置窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click_for_Set(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog(this);
        }

        private void labelControl17_MouseMove(object sender, MouseEventArgs e)
        {
            label_for_WarmPrompt.Visible = true;
        }

        private void labelControl17_MouseLeave(object sender, EventArgs e)
        {
            label_for_WarmPrompt.Visible = false;
        }


        private void groupControl1_MouseMove(object sender, MouseEventArgs e)
        {
            Read_cfg();

        }

        private void groupControl2_MouseMove(object sender, MouseEventArgs e)
        {
            Read_cfg();
        }
        private void groupControl3_MouseMove(object sender, MouseEventArgs e)
        {
            Read_cfg();
        }
        private void groupControl4_MouseMove(object sender, MouseEventArgs e)
        {
            Read_cfg();
        }
        /// <summary>
        /// 重新初始化 
        /// </summary>
        public void Read_cfg() {
            initialize_soundPlayer();
            initialize_warning_count();
            label_for_AlarmLimit.Text = "用户在线数报警阈值：" + user_warning_count;
            
        }
        public static void soundplyStop() {
            sp.SoundLocation = SystemConfig.GetConfigData("music_url", string.Empty);
            sp.Stop();
            try { sp.Play(); }
            catch (Exception) { MessageBox.Show("请检查配置文件或者音频丢失！"); }
         
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {       
           
            
            if (Judge_UserCount()) {
                try { sp.Play(); }
                catch (Exception) { MessageBox.Show("请检查配置文件或者音频丢失！"); }

                SystemConfig.WriteConfigData("music_url", Application.StartupPath + @"\control\resouse\UserMonitorResource\music\斗地主.wav");
            }
        }
 
        private Boolean Judge_UserCount() {
            Boolean tempboolean = false;
            try
            {
                if (Int64.Parse(label_for_UserCount.Text) >= user_warning_count)
                {
                    tempboolean = true;
                }
            }catch(Exception ){}
            return tempboolean;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Disconnect();
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

        MonitorUserCount listRes=null;
        
        void _cedaSubscribe_OnCedaMessage(com.adaptiveMQ2.message.Message msg)
        {
         //   MessageBox.Show("Receive Message...");
            CedaObject co = CedaObject.ToCedaObject(msg);

            listRes = MsgHelper.Deserialize<MonitorUserCount>(co.MessageBody, MsgSerializerType.Json);
           // MessageBox.Show(listRes.Time + "        listRes.Time  ");
            string str = string.Format("地址：{2}:{3}  Topic={0},MessageBody=\n{1}", co.Topic, co.MessageBody, ip, port);
            LogHelper.Info(str);
           //消息窗口显示E
            dealwith_msg(listRes);
        }
        private ILog logHelper;
        public string svrId = "";
        string ip = "";
        int port =0;
        private CedaManager _cedaSubscribe = null;
        /// <summary>
        /// 通过此方法，获得与服务器的连接。
        /// </summary>
        public void connect()//建¨立ⅰ?连?接ó
        {
            logHelper = new TextLogHelper(label1, this, ip, port);
            _cedaSubscribe = new CedaManager(logHelper);
            _cedaSubscribe.OnCedaMessage += new Action<com.adaptiveMQ2.message.Message>(_cedaSubscribe_OnCedaMessage);
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.setAddress(ip, port);
            clientInfo.setUser("monitor", "monitor");
            clientInfo.Protocol = ClientInfo.PROTOCOL_TCP;     
            ThreadPool.QueueUserWorkItem(o =>
            {
               
                _cedaSubscribe.Connect(clientInfo);
              labelControl19.Text ="等待连接..";
                if (!_cedaSubscribe.IsConnected)
                {
                    return;
                }
                if (_cedaSubscribe.IsConnected)
                {
                    labelControl19.Text ="连接成功!";
                    List<string> topicList = new List<string>();
                    topicList.Add("monitor.count.user.all");
                    topicList.Add("monitor.count.user.clienttype");
                    topicList.Add("monitor.count.user.endtype");
                    topicList.Add("monitor.count.user.svraddr");
                    _cedaSubscribe.SubscribeWithImage(topicList, svrId, "");
                    labelControl19.Text = "连接成功!";
                }
            });
        }
         

       /**
        * 数据处理方法
        **/
        private void dealwith_msg(MonitorUserCount listR ) {
            if(listR.Type.Equals("countuserall")){label_for_UserCount.Text=listR.Num+"";}
            if(listR.Type.Equals("countuserclienttype")){
                 if(listR.Symbol.Equals("E")){label_for_ExchangeUser.Text=listR.Num+"";}
                 if(listR.Symbol.Equals("C")){label_for_CommonUser.Text=listR.Num+"";}
                 if(listR.Symbol.Equals("M")){label_for_MemberUser.Text=listR.Num+"";}
            }
            

            if(listR.Type.Equals("countuserendtype")){
                 if(listR.Symbol.Equals("Mobile")){label_for_EndMobile.Text=listR.Num+"";}
                 if(listR.Symbol.Equals("PC")){label_for_EndPC.Text=listR.Num+"";}
            }
            if(listR.Type.Equals("countusersvraddr")){
                 if(listR.Symbol.Equals("222.222.222.222")){label_for_svraddr_2.Text=listR.Num+"";}
                 if(listR.Symbol.Equals("111.111.111.111")){label_for_svraddr_1.Text=listR.Num+"";}
           
            }

        }
        /// <summary>
        /// 此方法将 ip ,port ,svrID  从 Application.StartupPath + "\\control\\SystemMonitorConfig.ini 中读出
        /// </summary>

        private void initialize_ip_port_svrID() {

            PFS_ConfigInfo configInfo;
            configInfo = IniConfigHelper.ReadConfigInfo();
            for (int i = 0; i < configInfo.PFSConfigInfoNum; i++)
            {
               ConfigInfo config = configInfo.ConfigInfo[i];
               ip= config.Ip.Trim();
               port= config.Port;
               svrId= config.PFSMonitorName.Trim();
            //   label4.Text += ip;
             //  label2.Text += port;
             //  label3.Text += svrId;

    
            }
        }









   }
}
