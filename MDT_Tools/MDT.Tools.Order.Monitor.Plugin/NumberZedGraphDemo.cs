using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.CEDA.Common;
using ZedGraph;
using Ats.Foundation.Message;
using Ats.Foundation.Message.Serializer;
using com.adaptiveMQ2.client;
using com.adaptiveMQ2.message;
using Message = com.adaptiveMQ2.message.Message;
using MDT.Tools.Order.Monitor.Plugin.Data;
 
using MDT.Tools.Order.Monitor.Plugin.Model;
using MDT.Tools.Order.Monitor.Plugin.Util;
using MDT.Tools.Core.Log;

namespace MDT.Tools.Order.Monitor.Plugin
{
    public class NumberPlugin : ZedGraphControl//成交笔数折线图AmountPlugin
    {
        private AutoResetEvent are = new AutoResetEvent(false);

        private IPointData pointData = new PointData();//存放收到的点

        public CedaManager cedaManager = null;

        private ClientInfo _clientInfo = new ClientInfo();

        public string ip = "";

        public int port = 0;

        public string svrId = "";

        private string topicStr = "monitor.count.order.consuming";//接收数据topic

        private string topichistoryStr = "YM.ORDERS.CACHE.PERIOD";//缓存数据topic

        private GraphPane gp = null;

        public ILog logHelper;

        double i = 10;

        public int c;  //用于改变最大显示点数

        public NumberPlugin()
        {
            //坐标轴数据格式及标题定义
            gp = base.GraphPane;
            gp.Title.Text = "成交笔数监控";    //标题
            gp.XAxis.Title.Text = "时间";    //x轴标题
            gp.YAxis.Title.Text = "成交笔数（/笔）";     //y轴标题
            gp.XAxis.Type = AxisType.Date;    //x轴定义为时间格式
            gp.XAxis.Scale.Format = "HH:mm:ss";    //x轴时间格式化为HH:mm:ss
            gp.XAxis.MajorGrid.IsVisible = true;    //x轴珊格子效果
            gp.YAxis.MajorGrid.IsVisible = true;    //y轴珊格子效果
            gp.YAxis.Scale.Min = 0;//设置y轴最小值为0
            gp.YAxis.Scale.MinorStep = gp.YAxis.Scale.MajorStep = 1;
            gp.YAxis.Scale.Max = i;
            gp.Chart.Border.IsVisible = false;//设置边框为无
            gp.XAxis.MajorTic.IsOpposite = false;//设置X轴对面轴大间隔为无
            gp.XAxis.MinorTic.IsOpposite = false;//设置X轴对面轴小间隔为无
            gp.YAxis.MajorTic.IsOpposite = false;//Y轴对面轴大间隔为无
            gp.YAxis.MinorTic.IsOpposite = false;//Y轴对面轴小间隔为无
            refreshPane();
            cedaManager = new CedaManager(logHelper);
            cedaManager.OnCedaMessage += new Action<Message>(cedaManager_OnCedaMessage);
        }

        //销毁
        void ZedgraphDemo_Disposed(object sender, EventArgs e)
        {
            disconnect();
        }

        //建立连接 
        public void connect()
        {
            
            initialize_ip_port_svrID();
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.setAddress(ip, port);
            clientInfo.setUser("monitor", "monitor");
            clientInfo.Protocol = ClientInfo.PROTOCOL_TCP;
            ThreadPool.QueueUserWorkItem(o =>
            {
                cedaManager.Connect(clientInfo);
                if (cedaManager == null)
                {
                    return;
                }
                if (cedaManager.IsConnected)
                {
                    //string msgbody = MsgHelper.Serializer<Dictionary<string, object>>(dic);
                    cedaManager.SubscribeWithImage(topicStr, svrId);
                    List<String> topicList = new List<String>();
                    topicList.Add(topicStr);
                    cedaManager.Subscribe(topicStr);
                    while (cedaManager.IsConnected)
                    {
                        if (cedaManager.IsConnected)
                        {
                            Message reply = cedaManager.Request(getMessage());
                            CedaObject co = CedaObject.ToCedaObject(reply);
                            if (co != null)
                            {
                                LogHelper.Debug(co.Topic + ":" + co.MessageBody);

                                HisMonitorMessages result = MsgHelper.Deserialize<HisMonitorMessages>(co.MessageBody, MsgSerializerType.Json);
                                lock (pointData)
                                {
                                    setLinesData(result.Data);
                                    createLines();

                                }
                                LogHelper.Debug("init end.");
                                break;
                            }
                            else
                            {
                                LogHelper.Error("reply is null.");
                            }
                        }
                        Thread.Sleep(1000);
                    }

                }
            });
        }

        //设置点集合，存放所有点数据
        public delegate void delSetLinesDatas(List<MonitorMessage> hisMessages);
        private void setLinesData(List<MonitorMessage> hisMessages)
        {
            if (this.InvokeRequired)
            {
                delSetLinesDatas del = new delSetLinesDatas(setLinesData);
                this.Invoke(del, new Object[] { hisMessages });
            }
            else
            {

                if (hisMessages != null && hisMessages.Count > 0)
                {
                    int j = 0;
                    foreach (MonitorMessage st in hisMessages)
                    {

                        string key = st.type;
                        DateTime dt = zedgraphHelper.stringToDateTime(st.date + " " + st.time);
                        XDate xd = new XDate(dt);
                        double d = Convert.ToDouble(st.num);
                        
                        if (d != 0 && d <= i)
                        {
                            j++;
                            PointPair pointPair = new PointPair((double)xd, d);
                            addPointData(key, pointPair);

                        }
                        if (d != 0 && d > i)
                        {
                            j++;
                            i = d;
                            gp.YAxis.Scale.Max = i * 2;
                            gp.YAxis.Scale.MinorStep = gp.YAxis.Scale.MajorStep = i / 10;
                            PointPair pointPair = new PointPair((double)xd, d);
                            addPointData(key, pointPair);
                        }
                       
                       
                    }
                    if (j == 0) {
                        XDate xd = DateTime.Now;
                        double d = Convert.ToDouble(0);
                        PointPair pointPair = new PointPair((double)xd, d);
                        addPointData("orderminute", pointPair);
                    }
                }
            }
        }

        //设置点的组合，以键值对的形式
        private PointPair setPointPair(MonitorMessage st)
        {


            if (string.IsNullOrEmpty(st.time))
            {
                return null;
            }
            else
            {
                DateTime dt = zedgraphHelper.stringToDateTime(st.time);
                XDate xd = new XDate(dt);
                double d = Convert.ToDouble(st.num);
                
                return new PointPair((double)xd, (double)d);
                
            }
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

                    GraphPane gp = base.GraphPane;

                    foreach (string key in pointData.getAllKey())
                    {
                        LineItem lt = gp.AddCurve(key, pointData.getPointPairListByKey(key),
                         Color.Blue, SymbolType.Circle);
                        
                        lt.Line.Width = 2;

                    }
                    refreshPane();
                }
            }
        }

        //主动请求建立连接
        private Message getMessage()
        {

            Message request = request = new Message();
            request.Destination = new Destination(topichistoryStr);//主动请求建立连接，获取缓存数据
            request.SvrID = svrId;
            request.MessageBody.addString((short)3, "{}");
            request.MessageBody.addString((short)4, "JSON");
            request.MessageBody.addInt((short)5, 0);
            request.MessageBody.addString(7, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
            return request;
        }


        //请求订阅
        private void cedaManager_OnCedaMessage(Message obj)
        {
            
            
                CedaObject co = CedaObject.ToCedaObject(obj);
                MonitorMessage ts = MsgHelper.Deserialize<MonitorMessage>(co.MessageBody, MsgSerializerType.Json);
                string key = ts.type;
                DateTime dt = zedgraphHelper.stringToDateTime(ts.date + " " + ts.time);
                XDate xd = new XDate(dt);
                double d = Convert.ToDouble(ts.num);
                if (d != 0 && i > d)
                {
                    PointPair pointPair = new PointPair((double)xd, d);
                    addPointData(key, pointPair);
                    if (pointData.getPointPairListByKey(key).Count <= 0)
                    {
                        this.createLines();
                    }

                    if (pointData.getPointPairListByKey(key).Count >c)
                    {
                        pointData.getPointPairListByKey(key).RemoveRange(0, pointData.getPointPairListByKey(key).Count - c);
                        this.refreshPane();
                    }
                    else
                    {
                        this.refreshPane();
                    }

                }
                

                if (d != 0 && i <= d)
                {
                    i = d;
                    gp.YAxis.Scale.Max = i * 2;
                    gp.YAxis.Scale.MinorStep = gp.YAxis.Scale.MajorStep = i / 10;
                    PointPair pointPair = new PointPair((double)xd, d);
                    addPointData(key, pointPair);
                    if (pointData.getPointPairListByKey(key).Count <= 0)
                    {
                        this.createLines();
                    }

                    if (pointData.getPointPairListByKey(key).Count > c)
                    {
                        pointData.getPointPairListByKey(key).RemoveRange(0, pointData.getPointPairListByKey(key).Count - c);
                        this.refreshPane();
                    }
                    else
                    {
                        this.refreshPane();
                    }


                }
        }
        //刷新界面，填点画线
        public void refreshPane()
        {
            if (this.InvokeRequired)
            {
                Action action = refreshPane;
                this.BeginInvoke(action);
            }
            else
            {
                this.Invalidate();
                this.AxisChange();
            }
        }

        //存点集方法
        public void addPointData(string key, PointPair pointPair)
        {
            pointData.addPointByKey(key, pointPair);
        }

        private void disconnect()
        {
            if (cedaManager != null)
            {
                cedaManager.Disconnect();
                cedaManager.OnCedaMessage -= new Action<com.adaptiveMQ2.message.Message>(cedaManager_OnCedaMessage);
                cedaManager = null;
            }
        }

        //定义屏幕属性
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Name = "NumberPlugin";
            this.Size = new System.Drawing.Size(198, 118);
            this.ResumeLayout(false);
        }
        /// <summary>
        /// 初始化 ip，port，svrid，topicStr，topichistoryStr
        /// </summary>
        private void initialize_ip_port_svrID()
        {

            Event_ConfigInfo configInfo;
            configInfo = IniConfigHelpe.ReadConfigInfo();
            ip = configInfo.Ip;
            port = configInfo.Port;
            svrId = configInfo.svrId;
            topicStr = configInfo.topicStr1;
            topichistoryStr = configInfo.topichistoryStr1;
        }
    }
}
