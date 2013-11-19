using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Atf.Common;
using Atf.MQ;
using Atf.MQ.Imp;
using Atf.MQIII;
using Ats.Message2;
using MDT.Tools.Core.Utils;
using com.adaptiveMQ2.client;
using com.adaptiveMQ2.event_c;
using com.adaptiveMQ2.message;
using com.adaptiveMQ2.utils;

namespace MDT.Tools.Core.Mq
{
    public delegate void OnMessageDel(Message msg);
    /// <summary>
    /// Mq帮助类
    /// </summary>
    public class Mq2 : IMq,IMessageProcessor,IMessageListener
    {
        private Queue msgQueue = Queue.Synchronized(new Queue());
        private Queue topicQueue = Queue.Synchronized(new Queue());
        private bool isStop = false;
        private AutoResetEvent are1 = new AutoResetEvent(false);
        private AutoResetEvent are2 = new AutoResetEvent(false);
        private MessageHandler handler;
        public event OnMessageDel OnMessageEvent;
        public void Start(ServerInfo si, string serverName)
        {
            ConnectToServerBySync(si, serverName);
        }
        public void Stop()
        {
            try
            {
                isStop = true;

                if (are1 != null)
                {
                    are1.Set();
                    are1.Close();
                }
                if (are2 != null)
                {
                    are2.Set();
                    are2.Close(); ;
                }

                MQIIIManager.Instance.Stop();

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }

        private void ConnectToServerBySync(ServerInfo si, string serverName)
        {
            ThreadPool.QueueUserWorkItem(o => { connectToServer(si, serverName); });
            ThreadPool.QueueUserWorkItem(o => { initDealMq(); });
            ThreadPool.QueueUserWorkItem(o => { initDealSubscribe(); });
        }

        private void connectToServer(ServerInfo si, string serverName)
        {
            LocalEnv.SystemEnv.PutEnvironment("DefaultMQServer", serverName);
            LocalEnv.SystemEnv.PutEnvironment("MQServerList", serverName);
            LocalEnv.SystemEnv.PutEnvironment("MQDefaultTopics", "");
            LocalEnv.ServerInfos.Add(serverName, si);
            MQIIIManager.Instance.StartDefaultServer(getClientInfo(),this);
             
            handler = MQIIIManager.Instance.MQHandler;
            handler = MQIIIManager.Instance.RegisterHandler(serverName);
            //handler.MQMessage += new MessageHandler.OnMQMessageDelegate(handler_MQMessage);

        }
        public void OnMessage(Message msg)
        {
           
            if (msg.Destination.getName().Equals(ConstsMessage.TOPIC_ATS_LOGIN))
            {
                try
                {
                    LogHelper.Debug("--------------------->" + msg.MessageBody.getString((short)3));
                    //MQIIIManager.Instance.StopDefaultServer();
                    MQIIIManager.Instance.Start();
                }
                catch (Exception e)
                {
                    LogHelper.Error(e);
                }
                return;
            }
            if (OnMessageEvent != null)
            {
                OnMessageEvent(msg);
            }
            doMsg(msg);
        }

        void handler_MQMessage(string appName, string topic, Message msg)
        {
            OnMessage(msg);
        }

        private void initDealMq()
        {
            while (!isStop)
            {

                dealMQMessage();
                if (msgQueue.Count.Equals(0))
                {
                    are1.Reset();
                    are1.WaitOne();
                }
            }
        }

        private void initDealSubscribe()
        {
            while (!isStop)
            {

                dealSubscribe();
                if (topicQueue.Count.Equals(0))
                {
                    are2.Reset();
                    are2.WaitOne();
                }
            }
        }

        private void dealMQMessage()
        {
            try
            {
                for (int i = 0; i < msgQueue.Count; i++)
                {
                    Message mm = msgQueue.Dequeue() as Message;
                    if (mm != null)
                    {
                        doMsg(mm);

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        private void doMsg(Message msg)
        {
            
            MessageBody oMessageBody = msg.MessageBody;

            try
            {
                IEnumerator it = oMessageBody.Values;
                while (it.MoveNext())
                {
                    MessageRecord.CField f = it.Current as MessageRecord.CField;
                    LogHelper.Debug(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "rec:[" + f.nPosition + "]" + f.oField);
                }

            }
            catch (Exception e)
            {
                LogHelper.Error(e);
            }
        }
        public void SendMessage(Message mm)
        {
            msgQueue.Enqueue(mm);
            are1.Set();
        }

        private void dealSubscribe()
        {
            try
            {
                for (int i = 0; i < topicQueue.Count; i++)
                {
                    if (handler != null)
                    {
                        if (MQIIIManager.Instance.IsStart)
                        {
                            string topic = topicQueue.Dequeue() as string;
                            if (!string.IsNullOrEmpty(topic))
                            {
                                MQManager.GetInstance().SubScribe(topic,this);
                                //handler.SubscribeTopics(new string[] { topic });
                                LogHelper.Debug(string.Format("topic:{0} subscribe success.", topic));

                            }
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        public void Subscribe(string topic)
        {
            topicQueue.Enqueue(topic);
            are2.Set();
        }

        public void UnSubscribe(string topic)
        {
            if (handler != null)
            {
                handler.UnsubscribeTopics(new string[] { topic });
            }
        }

        private Message getLoginMsg(string userName, string location, string password, string newPassword)
        {
            com.adaptiveMQ2.message.Message msgRequest = new com.adaptiveMQ2.message.Message();
            msgRequest.MessageBody = new com.adaptiveMQ2.message.MessageBody();
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("method", "login");
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("userName", userName);
            parameter.Add("location", location);
            parameter.Add("password", password);
            if (!string.IsNullOrEmpty(newPassword))
            {
                parameter.Add("newPassword", newPassword);
            }
            args.Add("parameter", parameter);
            string jsonArgs = MsgHelper.Serializer<Dictionary<string, object>>(args);
            msgRequest.MessageBody.addString((short)3, jsonArgs);


            return msgRequest;
        }


         

        private ClientInfo getClientInfo()
        {
            ClientInfo info = new ClientInfo();
            ServerInfo defaultMQ = LocalEnv.GetServerInfo(LocalEnv.SystemEnv.GetEnvironment("DefaultMQServer"));
            info.setAddress(defaultMQ.Host, int.Parse(defaultMQ.Port));
            //info.Protocol = ClientInfo.PROTOCOL_HTTP;
            info.setUser("atf.client." + LocalEnv.SystemEnv.GetEnvironment("DefaultMQServer"), "******");
            info.Protocol = 3;
            if (defaultMQ.Protocal == "TCP" || defaultMQ.Protocal == "PROTOCOL_TCP")
            {
                info.Protocol = 1;
            }
            else if (defaultMQ.Protocal == "TCPS" || defaultMQ.Protocal == "PROTOCOL_TCPS")
            {
                info.Protocol = 2;
            }
            else if (defaultMQ.Protocal == "HTTP" || defaultMQ.Protocal == "PROTOCOL_HTTP")
            {
                info.Protocol = 3;
            }
            else if (defaultMQ.Protocal == "HTTPS" || defaultMQ.Protocal == "PROTOCOL_HTTPS")
            {
                info.Protocol = 4;
            }
            info.LoginMessage = getLoginMsg("admin", "UOBS", "111111", "");
            return info;
        }

        #region IMessageListener 成员

        public void onMessage(Message msg)
        {
            LogHelper.Debug("III");
            OnMessage(msg);
        }

        #endregion

        public void ProcessMessage(Message msg)
        {
            LogHelper.Debug("II");
            OnMessage(msg);
        }

        public void OnEvent(int nCode)
        {
            throw new NotImplementedException();
        }
    }
}
