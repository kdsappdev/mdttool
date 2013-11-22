using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Ats.Message2;
using MDT.Tools.Core.Log;
using MDT.Tools.Core.Utils;
using com.adaptiveMQ2.client;
using com.adaptiveMQ2.event_c;
using com.adaptiveMQ2.message;
using com.adaptiveMQ2.utils;

namespace MDT.Tools.Core.Mq
{
    public class Mq1 : IEventListener, IMq, IMessageListener
    {
        #region 字段

        private IClientConnection conn = null;
        private IClientSession session = null;
        private ClientInfo info = null;
        private Destination topic = null;

        public string flag = null;
        #endregion



        #region IMessageListener 成员

        public void onMessage(com.adaptiveMQ2.message.Message msg)
        {
            if (msg.Destination.getName().Equals(ConstsMessage.TOPIC_ATS_LOGIN))
            {
                try
                {
                    LogHelper.Debug("--------------------->" + msg.MessageBody.getString((short)3));
                }
                catch (Exception e)
                {
                    LogHelper.Error(e);
                }
                return;
            }
            doMsg(msg);
        }
        private void doMsg(Message msg)
        {
            LogHelper.Debug(flag + " onMessage " + flag);
            LogHelper.Debug(msg.Destination.getName());
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
        public void Subscriber(string str)
        {
            while ( session==null)
            {
                System.Threading.Thread.Sleep(1000);
            }
            Destination destination = new Destination(str);
            MessageConsumer consumer = session.createConsumer(destination);
            consumer.addMessageListener(this);
            LogHelper.Debug(string.Format("Subscriber:{0} Success",str));
        }

        private void recConn()
        {
            //topic需要与发布端topic一致

            info = new ClientInfo();
            info.setUser("MDT.Tools", "test");
            //设置AMQIII的IP地址和端口号，可根据实际情况进行设置
            info.setAddress("192.168.2.122/mq.httpMQTunnel", 19990);
            info.Protocol = ClientInfo.PROTOCOL_HTTP;

            try
            {

                info.LoginMessage = getLoginMsg("admin", "UOBS", "111111", "");
                // 创建连接
                conn = ClientConnectionFactory.createConnection(info);
                conn.addEventListener(this);
                conn.setLoginListener(this);
                conn.start();

                // 订阅topic，接收消息
                session = conn.createSession();

            }
            catch (Exception e)
            {
                LogHelper.Error(e);
            }
        }
        private void recConnBySync()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(o => { recConn(); });
        }
        public void Start()
        {
            recConnBySync();
        }

        #endregion

        #region onEvent
        public void onEvent(int nCode)
        {
            switch (nCode)
            {
                case IEventListener.CONNECTION_CONNECTING:
                    LogHelper.Debug("Event: CONNECTION_CONNECTING");
                    break;
                case IEventListener.CONNECTION_CONNECTED:
                    LogHelper.Debug("Event: CONNECTION_CONNECTED");
                    break;
                case IEventListener.CONNECTION_CLOSED:
					//断开重连
                    recConnBySync();
                    LogHelper.Debug("Event: CONNECTION_CLOSED");
                    break;
                case IEventListener.CONNECTION_RECONNECT:
                    LogHelper.Debug("Event: CONNECTION_RECONNECTED");
                    break;
                case IEventListener.CONNECTION_LOGINING:
                    LogHelper.Debug("Event: CONNECTION_LOGINING");
                    break;
                case IEventListener.CONNECTION_LOGIN_SUCCESS:
                    LogHelper.Debug("Event: CONNECTION_LOGIN_SUCCESS");
                    break;
                case IEventListener.CONNECTION_IO_EXCEPTION:
                    LogHelper.Debug("Event: CONNECTION_IO_EXCEPTION");
                    break;
                case IEventListener.CONNECTION_LOST:
                    LogHelper.Debug("Event: CONNECTION_LOST");
                    
                    break;
                case IEventListener.CONNECTION_TIMEOUT:
                    LogHelper.Debug("Event: CONNECTION_TIMEOUT");
                    break;
            }
        }
        #endregion

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


    }
}
