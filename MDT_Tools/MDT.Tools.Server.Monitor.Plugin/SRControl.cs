using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Ats.Foundation.Message;
using MDT.Tools.CEDA.Common;
using MDT.Tools.Core.Log;
using MDT.Tools.Core.Utils;
using MDT.Tools.Server.Monitor.Plugin.Model;
using Northwoods.Go;
using Northwoods.Go.Layout;
using WeifenLuo.WinFormsUI.Docking;
using com.adaptiveMQ2.client;
using com.adaptiveMQ2.message;
using Message = com.adaptiveMQ2.message.Message;
using Timer = System.Windows.Forms.Timer;

using MDT.Tools.Server.Monitor.Plugin.DeviceUtil;
using MDT.Tools.Server.Monitor.Plugin.zedgraph;

namespace MDT.Tools.Server.Monitor.Plugin
{
    public partial class SRControl : DockContent
    {
        internal Timer timer1;
        static string loc = "SM.xml";
        private CedaManager _cedaSubscribe = null;
        private ClientInfo _clientInfo = new ClientInfo();
        private TextLogHelper logHelper;
        public string SoundLocation="";
       public string ip = "192.168.2.59";
       public int port = 9901;
        public string svrId = "MonitorSvr";
        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        private bool isLoad = false;

        private IDevice device = null;
         
        //SoundPlayer warnSoundPlayer = new SoundPlayer();
        public SRControl()
        {
            InitializeComponent();

            ToolStripMenuItem btnSave = new ToolStripMenuItem();
            btnSave.Text = "保存布局";
            btnSave.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;

            btnSave.Click += new EventHandler(btnSave_Click);

            ToolStripMenuItem btnDelete = new ToolStripMenuItem();
            btnDelete.Text = "删除布局";
            btnDelete.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;

            btnDelete.Click += new EventHandler(btnDelete_Click);

            ToolStripMenuItem btnClear = new ToolStripMenuItem();
            btnClear.Text = "清除警告信息";
            btnClear.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;

            btnClear.Click += new EventHandler(btnClear_Click);
            contextMenuStrip.Items.Add(btnSave);
            contextMenuStrip.Items.Add(btnDelete);
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add(btnClear);
            this.Disposed += new EventHandler(SRControl_Disposed);
            goView1.NewLinkPrototype = new AnimatedLink();
            this.goView1.NewLinkClass = typeof(AnimatedLink);
            StateChartDocument filedoc = StateChartDocument.LoadXml(loc);
            if (filedoc == null)
            {
                filedoc = new StateChartDocument();
                GoComment com = StateChartDocument.NewComment();
                com.Width = this.Width;
                com.Visible = false;
                filedoc.Add(com);
            }
            else
            {
                isLoad = true;
            }
            GoDocument doc = filedoc;
            this.goView1.Document = doc;
            goView1.ObjectDoubleClicked += new GoObjectEventHandler(goView1_ObjectDoubleClicked);
            goView1.Document.UndoManager = new GoUndoManager();
            logHelper = new TextLogHelper(label1, this, ip, port);
            logHelper.SRControl = this;
            _cedaSubscribe = new CedaManager(logHelper);
            timer1 = new Timer();


            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            //setTimeEnable(true);
            //_cedaSubscribe.SRControl = this;
            //warnSoundPlayer.SoundLocation = Consts.SoundLocation;

            device = new DeviceImpl();
            device.init(this, Microsoft.DirectX.DirectSound.CooperativeLevel.Priority);
        }

        private void setStatus(bool status)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<bool>(setStatus), new object[] {status});
            else
            {
                if (status)
                    this.Cursor = Cursors.Default;
                else
                    this.Cursor = Cursors.WaitCursor;
            }
        }

        private void goView1_ObjectDoubleClicked(object sender, GoObjectEventArgs e)
        {
            setStatus(false);
            GoBasicNode node = e.GoObject.TopLevelObject as GoBasicNode;
            
            if (node != null)
                createZedgraph(node);
            else
                setStatus(true);
        }

        private Form form = null;
        private ZedgraphDemo zedgraph = null;
        private void createZedgraph(GoBasicNode node)
        {
            LogHelper.Debug("create node:" + node.Text);
            form = new Form();
            form.WindowState = FormWindowState.Maximized;
            zedgraph = new ZedgraphDemo(node.Text,logHelper);
            zedgraph.Ip = ip;
            zedgraph.Port=port;
            zedgraph.sendResult += new ZedgraphDemo.SendResult(zedgraph_sendResult);
            zedgraph.Dock = DockStyle.Fill;
            zedgraph.RequestSvrId = node.Text;
            zedgraph.IsPrintFillPage = false;
            zedgraph.IsPrintKeepAspectRatio = false;
            zedgraph.IsPrintScaleAll = false;
            zedgraph.IsShowCopyMessage = false;
          
            
            form.Controls.Add(zedgraph);
            zedgraph.connect();
            form.Width = 100;
            form.Height = 100;
            //form.Show(this);
            
        }

        private void showForm(bool isShow)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<bool>(showForm), new object[] { isShow });
            else
            {
                if (form != null)
                {
                    if (isShow)
                        form.Show();
                    else
                    {
                        zedgraph = null;
                        form = null;
                    }
                }
            }
        }

        void zedgraph_sendResult(bool isShow)
        {
            ThreadPool.QueueUserWorkItem(
                o => {
                         try
                         {
                             showForm(isShow);
                         }
                         catch (Exception e)
                         {
                             LogHelper.Error(e);
                         }
                         finally
                         {
                             setStatus(true);
                         }
                });
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(loc))
                {
                    File.Delete(loc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "删除失败: " + ex.Message);
                
            }
             
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            getCommon().Text = "";
        }

        #region
        void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
      
        public bool Save()
        {
            FileStream file = null;
            try
            {
                file = File.Open(loc, FileMode.OpenOrCreate);
                StoreStateChart(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "保存失败: " + ex.Message);
                return false;
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
            return true;

        }
        public void StoreStateChart(FileStream ofile)
        {
            StateChartDocument doc = (StateChartDocument)goView1.Document;
            doc.StoreXml(ofile);
            doc.IsModified = false;

        }

        #endregion
        void SRControl_Disposed(object sender, EventArgs e)
        {
            setTimeEnable(false);
            Disconnect();

            device.clear();
        }

        #region node ,line 预警

        private void nodeWarn()
        {
            goView1.Document.SkipsUndoManager = true;
            foreach (GoObject obj in goView1.Document)
            {
                GoBasicNode n = obj as GoBasicNode;
                if (n != null)
                {
                    if (n.UserFlags == 0)
                    {
                        n.Shape.PenColor = Color.Red;
                        if (n.Shape.PenWidth == 2)
                            n.Shape.PenWidth = 4;
                        else
                            n.Shape.PenWidth = 2;
                        isWarn = true;

                        //device.addSecondaryBufferAndPlay(Consts.SoundLocation, n.Text);
                    }
                    else
                    {
                        n.Shape.PenColor = Color.White;
                        n.Shape.PenWidth = 2;

                        //device.stopSecondaryBufferByName(n.Text);
                    }
                    if(n.ToolTipText.Contains("备"))
                    {
                        n.Shape.BrushForeColor = Color.Yellow;
                    }
                    else if (n.ToolTipText.Contains("主"))
                    {
                        n.Shape.FillShapeHighlight(Color.FromArgb(80, 180, 240), Color.FromArgb(255, 255, 255));
                    }
                }
            }
            goView1.Document.SkipsUndoManager = false;
        }

        private bool isWarn = false;
        private bool isPlay = false;
        private bool isStop = false;

        public void setTimeEnable(bool flag)
        {
            if (this.InvokeRequired)
            {
                Action<bool> s = setTimeEnable;
                this.Invoke(s, new object[] {flag});
            }
            else
            {
                timer1.Enabled = flag;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                lock (this)
                {
                    isWarn = false;
                    if (_cedaSubscribe.IsConnected)
                    {
                        setCommon();
                        nodeWarn();
                        lineWarn();
                    }
                    monitorConWarn();
                    if (isWarn)
                    {
                        if (!isPlay)
                        {
                            isPlay = true;

                            // warnSoundPlayer.PlayLooping();
                            device.addSecondaryBufferAndPlay(Consts.SoundLocation, "warn");
                        }
                        isStop = false;
                    }
                    else
                    {
                        if (!isStop)
                        {
                            //warnSoundPlayer.Stop();
                            device.stopSecondaryBufferByName("warn");
                            isStop = true;
                        }
                        isPlay = false;
                    }
                }
            }
            catch (Exception ex)
            {
              LogHelper.Error(ex);
            }
           
        }

        private void setCommon()
        {
            GoComment comment = getCommon();
            if (string.IsNullOrEmpty(comment.Text))
            {
                comment.Visible = false;
            }
            else
            {
                comment.Visible = true;
            }
        }
        private void monitorConWarn()
        {
            if (!_cedaSubscribe.IsConnected)
            {
                isWarn = true;
                label1.ForeColor = Color.Red;
                label1.Font=new Font("宋体",14,FontStyle.Bold);

                //device.addSecondaryBufferAndPlay(Consts.SoundLocation, "MonitorConWarn");
            }
            else
            {
                //device.stopSecondaryBufferByName("MonitorConWarn");
                label1.ForeColor = SystemColors.ControlText;
                label1.Font = new Font("宋体", 9);
            }

        }

        private void lineWarn()
        {
            goView1.Document.SkipsUndoManager = true;
            foreach (GoObject obj in goView1.Document)
            {
                AnimatedLink link = obj as AnimatedLink;
               
                if (link != null)
                {
                    if (link.UserFlags == 1)
                    {
                        // link.PenColor = (Color) link.UserObject;
                        link.PenColor = Color.Green;
                        link.PenWidth = 2;
                        link.Step();

                        //device.stopSecondaryBufferByName(link.Text);
                    }
                    else if (link.UserFlags == 0)
                    {
                        link.PenColor = Color.Red;
                        if (link.PenWidth == 2)
                            link.PenWidth = 4;
                        else
                            link.PenWidth = 2;
                        isWarn = true;

                        //device.addSecondaryBufferAndPlay(Consts.SoundLocation, link.Text);
                       
                    }
                }
            }
            goView1.Document.SkipsUndoManager = false;
        }

        #endregion

        #region data

        private MSD msd = null;
        private MSD getMSD()
        {
            MSD msd = new MSD();
            msd.svrinfos.Add(new TSvrinfo() { ServiceName = "Asvr" });
            msd.svrinfos.Add(new TSvrinfo() { ServiceName = "AMQ1" });
            msd.svrinfos.Add(new TSvrinfo() { ServiceName = "AMQ2" });
            msd.svrrefs.Add(new TSvrRef() { Servername_From = "AMQ1", Servername_To = "Asvr" });
            msd.svrrefs.Add(new TSvrRef() { Servername_From = "AMQ2", Servername_To = "Asvr" });

            Message request = new Message();
            request.Destination = new Destination("YM.M.SD");
            request.SvrID = svrId;
            request.MessageBody.addString((short)3, "{}");
            request.MessageBody.addString((short)4, "JSON");
            request.MessageBody.addInt((short)5, 0);
            request.MessageBody.addString(7, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
            Message reply = _cedaSubscribe.Request(request);
            CedaObject co = CedaObject.ToCedaObject(reply);
            if (co.MessageBody != null)
            {
                msd = MsgHelper.Deserialize<MSDResult>(co.MessageBody).MSD;
            }
            return msd;
        }

        #endregion

        #region init node

        Dictionary<string, List<string>> dicNodeLines = new Dictionary<string, List<string>>();
        private void SRControl_Load(object sender, EventArgs e)
        {
            if(SPlugin.configInfo!=null)
            {
                foreach (var c in SPlugin.configInfo.ConfigInfo)
                {
                    if(c.SMonitorName==this.DockHandler.TabText)
                    {
                        ip = c.Ip;
                        port = c.Port;
                        logHelper.ip = ip;
                        logHelper.port = port;
                        setTimeEnable(true);
                        ConnectionServer();
                        break;
                    }
                }
                
            }
            


        }

        private void craeteNodeLine()
        {
            if (msd != null && !isLoad)
            {
                if (this.InvokeRequired)
                {
                    Action a = craeteNodeLine;
                    this.Invoke(a);
                }
                else
                {

                    goView1.StartTransaction();

                    GoLayoutRandom l = new GoLayoutRandom();
                    RectangleF r = goView1.DisplayRectangle;

                    l.Document = goView1.Document;
                    l.MinX = (int)((r.X / goView1.DocScale) + 100);
                    l.MaxX = (int)((r.Width - r.X) / goView1.DocScale - 100);
                    l.MinY = (int)((r.Y / goView1.DocScale) + 100);
                    l.MaxY = (int)((r.Height - r.Y) / goView1.DocScale - 100);

                    foreach (var s in msd.svrinfos)
                    {
                        GoBasicNode state = StateChartDocument.NewNode();
                        state.ToolTipText = "";
                        state.Text = s.ServiceName;
                        state.UserFlags = 0;
                        if (!isNodes(s.ServiceName))
                        {
                            goView1.Document.Add(state);
                        }
                    }
                    foreach (var s in msd.svrrefs)
                    {
                        addLine(s, Color.Green);
                    }
                    l.PerformLayout();

                    goView1.FinishTransaction("added  node");
                }
            }
        }

        private delegate void addLineDel(TSvrRef s, Color c);
        private void addLine(TSvrRef s, Color c)
        {
            if (!isLoad)
            {
                if (this.InvokeRequired)
                {
                    addLineDel a = addLine;
                    this.Invoke(a, new object[]
                                       {
                                           s, c
                                       }
                        );
                }
                else
                {
                    lock (this)
                    {


                        if (!string.IsNullOrEmpty(s.Servername_From) && !string.IsNullOrEmpty(s.Servername_To) &&
                            isNodes(s.Servername_From) && isNodes(s.Servername_To))
                        {

                            AnimatedLink link = new AnimatedLink();
                            link.AdjustingStyle = GoLinkAdjustingStyle.Scale;
                            link.BrushColor = c;
                            link.PenColor = c;
                            link.PenWidth = 2;
                            link.FromPort = getNodes(s.Servername_From).Port;
                            link.ToPort = getNodes(s.Servername_To).Port;
                            link.UserFlags = 0;
                            link.UserObject = c;

                            string key = string.Format("{0}->{1}", s.Servername_From, s.Servername_To);
                            link.Text = key;
                            if (!isLines(key))
                            {
                                List<string> lines = new List<string>();
                                if (!dicNodeLines.ContainsKey(s.Servername_To))
                                {
                                    dicNodeLines.Add(s.Servername_To, lines);
                                }
                                else
                                {
                                    lines = dicNodeLines[s.Servername_To];
                                }
                                lines.Add(key);


                                goView1.Document.Add(link);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region ceda
        private void ConnectionServer()
        {
            
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
                    msd = getMSD();

                    craeteNodeLine();
                    _cedaSubscribe.SubscribeWithImage("YM.M.SI.*", svrId);
                    _cedaSubscribe.Subscribe("YM.M.W.*");
                }
            });
        }
        public GoComment getCommon()
        {
            GoComment node = null;
            foreach (GoObject obj in goView1.Document)
            {
                node = obj as GoComment;
                if (node != null)
                {
                     
                        break;
                     
                }
            }
            return node;
        }

        public List<string> getNodeLines(string serviceName)
        {
            List<string> lt = new List<string>();
            foreach (GoObject obj in goView1.Document)
            {
                AnimatedLink node = obj as AnimatedLink;
                if (node != null)
                {
                    string serverName_to = node.Text.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    if (serviceName == serverName_to)
                    {
                        lt.Add(node.Text);
                    }
                }
            }
            return lt;
        }
        private GoBasicNode getNodes(string name)
        {
            GoBasicNode node = null;
            foreach (GoObject obj in goView1.Document)
            {
                node = obj as GoBasicNode;
                if (node != null)
                {
                    if (node.Text == name)
                    {
                        break;
                    }
                }
            }
            return node;
        }
        private bool isNodes(string name)
        {
            bool flag = false;
            foreach (GoObject obj in goView1.Document)
            {
                GoBasicNode node = obj as GoBasicNode;
                if (node != null)
                {
                    if (node.Text == name)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }
        private bool isLines(string name)
        {
            bool flag = false;
            foreach (GoObject obj in goView1.Document)
            {
                AnimatedLink node = obj as AnimatedLink;
                if (node != null)
                {
                    if (node.Text == name)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }
        private AnimatedLink getLines(string name)
        {
            AnimatedLink line = null;
            foreach (GoObject obj in goView1.Document)
            {
                line = obj as AnimatedLink;
                if (line != null)
                {
                    if (line.Text == name)
                    {
                        break;
                    }
                }
            }
            return line;
        }
        Dictionary<string, SvrRunInfo> dicSvrRunInfo = new Dictionary<string, SvrRunInfo>();
        private void _cedaSubscribe_OnCedaMessage(Message obj)
        {
            CedaObject co = CedaObject.ToCedaObject(obj);

            string str = WarnInfo(co);

            str = SvrRunInfo(co);

            logHelper.Debug(str);
        }
        public string WarnInfo(CedaObject co)
        {
             string str="";
             if (co.Topic.Contains("YM.M.W"))
             {
                 TMonitorLog tml = MsgHelper.Deserialize<TMonitorLog>(co.MessageBody);
                 str = string.Format("{0} {1}--{2}", tml.MUpdatetime, tml.MDesc, tml.MKey);
                 addCommon(str);
             }
            return str;
        }

        public delegate void SimpleDel(string str);
        public void addCommon(string str)
        {
            if(this.InvokeRequired)
            {
                SimpleDel s = addCommon;
                this.Invoke(s, new object[] {str});
            }
            else
            {
                GoComment comment = getCommon();
                comment.Text = string.Format("{0}\n{1}", comment.Text, str);
                LogHelper.Info(str);
                
            }
        }

       
        private string SvrRunInfo(CedaObject co)
        {
            string str="";
            if (co.Topic.Contains("YM.M.SI"))
            {
                SvrRunInfo sri = MsgHelper.Deserialize<SvrRunInfo>(co.MessageBody);

                if (!dicSvrRunInfo.ContainsKey(co.Topic))
                {
                    dicSvrRunInfo.Add(co.Topic, sri);
                }
                else
                {
                    dicSvrRunInfo[co.Topic] = sri;
                }
                GoBasicNode node = getNodes(sri.ServiceName);
                if (node != null)
                {
                    if (sri.WorkDes == "working" || sri.Connectioned == "ok")
                    {
                        node.UserFlags = 1;
                    }
                    else
                    {
                        node.UserFlags = 0;
                        
                    }
                     
                    node.ToolTipText = sri.Info;
                }
                List<string> lines = getNodeLines(sri.ServiceName);

                List<Line> cLines = sri.getClientLine();
                foreach (var line in cLines)
                {
                    if (lines.Contains(line.Key))
                    {
                        AnimatedLink link = getLines(line.Key);
                        if (link != null)
                        {
                            link.ToolTipText = line.Info;
                            link.UserFlags = 1;
                            lines.Remove(line.Key);
                        }
                    }
                    else
                    {
                        TSvrRef s = new TSvrRef()
                                        {Servername_From = line.Servername_From, Servername_To = line.Servername_To};
                        addLine(s, Color.Yellow);
                    }
                }
                foreach (var line in lines)
                {
                    AnimatedLink link = getLines(line);
                    if (link != null)
                    {
                        link.UserFlags = 0;
                        link.ToolTipText = "已断开";
                    }
                }

                str = string.Format("Topic={0},MessageBody={1}", co.Topic, co.MessageBody);
            }
            return str;
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

        #endregion



        private void goView1_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(goView1, e.Location);
            }
        }
    }
}
