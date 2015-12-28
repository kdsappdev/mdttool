using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using MDT.Tools.CEDA.Common;
using MDT.Tools.Core.Log;
using ZedGraph;

using Ats.Foundation.Message;
using Ats.Foundation.Message.Serializer;

using MDT.Tools.Core.Utils;

using MDT.Tools.Server.Monitor.Plugin.ZedGraph.Data;
 
using MDT.Tools.Server.Monitor.Plugin.ZedGraph.Model;
using MDT.Tools.Server.Monitor.Plugin.ZedGraph.Util;

using com.adaptiveMQ2.client;
using com.adaptiveMQ2.message;
using Message = com.adaptiveMQ2.message.Message;

namespace MDT.Tools.Server.Monitor.Plugin.zedgraph
{
    public class ZedgraphDemo : ZedGraphControl
    {
        private AutoResetEvent are = new AutoResetEvent(false);

        private System.Timers.Timer timer = new System.Timers.Timer();

        private IPointData pointData = new PointData();

        private CedaManager cedaClient = null;

        private ClientInfo _clientInfo = new ClientInfo();
        public string SoundLocation = "";

        /// <summary>
        /// 连接IP
        /// </summary>
        public string Ip = "192.168.2.16";
        /// <summary>
        /// 端口
        /// </summary>
        public int Port = 9901;

        /// <summary>
        /// 连接监控进程名
        /// </summary>
        public string svrId = "MonitorSvr";


        private string initTopic = "YM.ML.ST";
        /// <summary>
        /// 初始Topic
        /// </summary>
        public string InitTopic
        {
            get { return initTopic; }
            set { initTopic = value; }
        }

        /// <summary>
        /// 请求监控指定进程名
        /// </summary>
        public string RequestSvrId { get; set; }

        private int repeatRequestTime = 2000;

        /// <summary>
        /// 重新连接时间
        /// </summary>
        public int RepeatRequestTime
        {
            get { return repeatRequestTime; }
            set { repeatRequestTime = value; }
        }

        private PointPairList list1 = new PointPairList();
        private PointPairList list2 = new PointPairList();

        /// <summary>
        /// 监控指定topic
        /// </summary>
        private List<string> topicList = new List<string>();

        private GraphPane graphPanel = null;

        /// <summary>
        /// 画图版面
        /// </summary>
        public GraphPane GraphPanel
        {
            get { return graphPanel; }
            set { graphPanel = value; }
        }

        public delegate void SendResult(bool isShow);

        public event SendResult sendResult;

        public ZedgraphDemo(string name,ILog log)
        {
            InitializeComponent();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);


           
            timer.Enabled = true;
            timer.Interval = 2000;
            timer.Start();
            graphPanel = base.GraphPane;

            RequestSvrId = name;

            graphPanel.Title.Text = "监控" + name;
            graphPanel.XAxis.Title.Text = "时间";
            graphPanel.YAxis.Title.Text = "值";

            graphPanel.XAxis.Type = AxisType.DateAsOrdinal;
            graphPanel.XAxis.Scale.Format = "HH:mm";
            graphPanel.XAxis.MajorGrid.IsVisible = true;  //珊格子
            graphPanel.YAxis.MajorGrid.IsVisible = true;
            graphPanel.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, Color.ForestGreen), 45.0F);
            graphPanel.XAxis.Scale.Max = 300;
            graphPanel.YAxis.Scale.Min = 0;

            cedaClient = new CedaManager(log);
            cedaClient.OnCedaMessage += new Action<Message>(cedaClient_OnCedaMessage);
            this.Disposed+=new EventHandler(ZedgraphDemo_Disposed);
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            refreshPane();
        }

        void ZedgraphDemo_Disposed(object sender, EventArgs e)
        {
            disconnect();
        }

        public void connect()
        {
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.setAddress(Ip, Port);
            clientInfo.setUser("zedgraph", "zedgraph");
            clientInfo.Protocol = ClientInfo.PROTOCOL_TCP;

            ThreadPool.QueueUserWorkItem(o =>
            {
                cedaClient.Connect(clientInfo);
                if (cedaClient == null)
                {
                    return;
                }
                if (cedaClient.IsConnected)
                {
                    while (cedaClient != null)
                    {
                        if (cedaClient.IsConnected)
                        {
                            Message reply = cedaClient.Request(getMessage());
                            CedaObject co = CedaObject.ToCedaObject(reply);
                            if (co != null)
                            {
                                LogHelper.Debug(co.Topic + ":" + co.MessageBody);

                                ServerTopicRef result = MsgHelper.Deserialize<ServerTopicRef>(co.MessageBody,
                                    MsgSerializerType.Json);

                                setLinesData(result.TSvrTopicRefs);
                                createLines();

                                LogHelper.Debug("init end.");

                                break;
                            }
                            else
                            {
                                LogHelper.Error("reply is null.");
                            }
                        }
                        Thread.Sleep(repeatRequestTime);
                    }
                }
            });
        }


        private void subTopic()
        {
            if (topicList.Count > 0)
                cedaClient.Subscribe(topicList);

            LogHelper.Debug("Subscribe topic:" + topicStr);

            bool result = true;
            if (topicList.Count == 0)
            {
                result = false;
                disconnect();
                //System.Windows.Forms.MessageBox.Show(this, "当前进程没有监控topic", "提示", System.Windows.Forms.MessageBoxButtons.OK);

            }

            if (sendResult != null)
                sendResult(result);
        }

        string topicStr = "";
        List<string> keys=new List<string>(); 
        /// <summary>
        /// 初始化界面数据
        /// </summary>
        /// <param name="svrTopicRefs"></param>
        private void setLinesData(List<TSvrTopicRef> svrTopicRefs)
        {
            if (svrTopicRefs != null && svrTopicRefs.Count > 0)
            {
                string topicStr = "";
                foreach (TSvrTopicRef st in svrTopicRefs)
                {
                    if (st.Server_key == this.RequestSvrId)
                    {
                        if (!topicList.Contains(st.Topic))
                        {
                            topicStr += st.Topic + ",";
                            topicList.Add(st.Topic);
                        }
                        
                        string key = st.M_Key;
                        if(!keys.Contains(key))
                        {
                            keys.Add(key);
                        }
                      
                        PointPair pointPair = setPointPair(st);
                        if (pointPair != null)
                            addPointData(key, pointPair);
                    }
                }
            }

            subTopic();
        }

        private string showLineKey = "All";
        void lt_CheckedChanged(object sender, EventArgs e)
        {
            showLineKey = ((ToolStripMenuItem)sender).Tag + "";
            
            createLines();
        }

        

        private PointPair setPointPair(TSvrTopicRef st)
        {
            if (string.IsNullOrEmpty(st.M_Updatetime) )
                return null;

            DateTime dt = zedgraphHelper.stringToDateTime(st.M_Updatetime);
            XDate xd = new XDate(dt);

            return new PointPair((double)xd, (double)st.M_Value);
        }

        //画线
       
        private void createLines()
        {
            if (this.InvokeRequired)
            {
                Action action = createLines;
                this.Invoke(action);
            }
            else
            {
                if (pointData.getAllKey().Count > 0)
                {

                    GraphPane graphPanel = base.GraphPane;
                    graphPanel.CurveList.Clear();
                    refreshPane();
                    foreach (string key in pointData.getAllKey())
                    {
                        if (showLineKey=="All"||showLineKey == key)
                            {
                                //string[] arr = key.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                                LineItem lt = graphPanel.AddCurve(key, pointData.getPointPairListByKey(key),
                                    zedgraphHelper.getRandomColor());
                                lt.Line.Width = 2;
                                lt.Line.IsAntiAlias = true;
                                lt.Symbol.IsVisible = false;
                            }
                        
                    
                    }

                    refreshPane();
                }
            }
        }

        private Message getMessage()
        {
            Message request = request = new Message();
            LogHelper.Debug("request initTopic:" + initTopic);
            request.Destination = new Destination(initTopic);
            request.SvrID = svrId;
            string param = "{\"RequestSvrId\":\"" + RequestSvrId + "\"}";
            request.MessageBody.addString((short)3, param);
            //request.MessageBody.addString((short)3, "{}");
            request.MessageBody.addString((short)4, "JSON");
            request.MessageBody.addInt((short)5, 0);
            request.MessageBody.addString(7, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));

            return request;
        }

        private Random rd = new Random();

        private void cedaClient_OnCedaMessage(Message obj)
        {
            CedaObject co = CedaObject.ToCedaObject(obj);

            MonitorMessage ts = MsgHelper.Deserialize<MonitorMessage>(co.MessageBody, MsgSerializerType.Json);
            string key = ts.MKey;

            

            if (string.IsNullOrEmpty(ts.MUpdatetime))
                return;

            DateTime dt = zedgraphHelper.stringToDateTime(ts.MUpdatetime);
            XDate xd = new XDate(dt);
            //XDate xd = new XDate(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            //xd.AddSeconds((double)dt.Second);
            PointPair pointPair = new PointPair((double)xd, (double)ts.MValue);

            if (pointData.isContain(key))
            {
                addPointData(key, pointPair);

                //this.refreshPane();
            }
        }

        public void clear()
        {
            list1.Clear();
            list2.Clear();
        }

        public void refreshPane()
        {
            if (this.InvokeRequired)
            {
                Action action = refreshPane;
                this.BeginInvoke(action);
            }
            else
            {
                //this.Invalidate();
                this.AxisChange();
                this.Refresh();
                
            }
        }

        public void addPointData(string key, PointPair pointPair)
        {
            pointData.addPointByKey(key, pointPair);
        }

        private void disconnect()
        {
            if (cedaClient != null)
            {
                cedaClient.Disconnect();
                cedaClient.OnCedaMessage -= new Action<com.adaptiveMQ2.message.Message>(cedaClient_OnCedaMessage);
                cedaClient = null;

                timer.Enabled = false;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ZedgraphDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.DoubleBuffered = true;
            this.IsPrintFillPage = false;
            this.IsPrintKeepAspectRatio = false;
            this.IsShowCopyMessage = false;
            this.Name = "ZedgraphDemo";
            this.Size = new System.Drawing.Size(528, 388);
            this.Load += new System.EventHandler(this.ZedgraphDemo_Load);
            this.ContextMenuBuilder += new ContextMenuBuilderEventHandler(this.ZedgraphDemo_ContextMenuBuilder);
            this.ResumeLayout(false);

        }

        private void ZedgraphDemo_Load(object sender, EventArgs e)
        {
           
        }

        private bool isFirst = false;
        private void ZedgraphDemo_ContextMenuBuilder(ZedGraphControl sender, System.Windows.Forms.ContextMenuStrip menuStrip, Point mousePt, ContextMenuObjectState objState)
        {
            try
            {
                foreach (var key in keys)
                    {
                        ToolStripMenuItem tsmi = new ToolStripMenuItem(key);
                        tsmi.Tag = tsmi.Text;
                        tsmi.Click += new EventHandler(lt_CheckedChanged);
                        menuStrip.Items.Add(tsmi);
                    }
                    ToolStripMenuItem tsmiAll = new ToolStripMenuItem("All");
                    tsmiAll.Tag = tsmiAll.Text;
                    tsmiAll.Click += new EventHandler(lt_CheckedChanged);
                    menuStrip.Items.Add(tsmiAll);
                
                foreach( ToolStripMenuItem item in menuStrip.Items )
                {
                    if( ( string )item.Tag == "copy" )                // “复制”菜单项
                    {
                        menuStrip.Items.Remove( item );//移除菜单项
                        item.Visible = false; //不显示
                        break;
                    }
                }
                foreach( ToolStripMenuItem item in menuStrip.Items )
                {
                    if( ( string )item.Tag == "page_setup" )                // “页面设置”菜单项
                    {
                        menuStrip.Items.Remove( item );//移除菜单项
                        item.Visible = false; //不显示
                        break;
                    }
                }
                foreach( ToolStripMenuItem item in menuStrip.Items )
                {
                    if( ( string )item.Tag == "print" )                // “打印”菜单项
                    {
                        menuStrip.Items.Remove( item );//移除菜单项
                        item.Visible = false; //不显示
                        break;
                    }
                }
                 
                 
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
