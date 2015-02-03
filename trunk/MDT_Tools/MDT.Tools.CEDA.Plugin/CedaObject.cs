//using Ats.Foundation.Message;
using System;
using System.Collections.Generic;
using Ats.Foundation.Message;
using MDT.Tools.Core.Utils;
using com.adaptiveMQ2.message;
using Atf.Common.Encrypt;

namespace MDT.Tools.CEDA.Plugin
{
    public class CedaObject
    {
        #region 属性
        /// <summary>
        ///  Topic，Topic
        /// </summary>
         public string Topic { get; set; }

        /// <summary>
         ///  Type，SvrID
        /// </summary>
        public string SvrID { get; set; }

        /// <summary>
        /// 消息体，3号域
        /// </summary>
        public string MessageBody { get; set; }
       
        /// <summary>
        /// 消息类型，4号域
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 压缩格式，5号域
        /// </summary>
        public int ZipType { get; set; }
        /// <summary>
        /// 消息体字段长度，6号域
        /// </summary>
        public int MessageSize { get; set; }
 
        /// <summary>
        /// 系统消息发送时间，7号域，格式：YYYYMMDD HH:MI:SS
        /// </summary>
        public String SendTime { get; set; }
       
        /// <summary>
        /// 结果代码，10号域
        /// </summary>
        public String ResultCode { get; set; }
      
        /// <summary>
        /// 请求结果是否成功，11号域
        /// </summary>
        public int Result { get; set; }
      
        /// <summary>
        /// 大消息，1号域
        /// </summary>
        public byte[] BigMessage { get; set; }
        #endregion

        public static Dictionary<string,object> GetLoginResult(Message message)
        {
            Dictionary<string, object> map = null;
            string result = message.MessageBody.getString((short)3);
            try
            {
                map = MsgHelper.Deserialize<Dictionary<string, object>>(result);

                if (map.ContainsKey("type"))
                {
                    map = null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                 
            }
           
            return map;
        }

        public static CedaObject ToCedaObject(Message msg)
        {
            if (msg == null)
                return null;
            CedaObject cedaObject=new CedaObject();

            cedaObject.SvrID = msg.SvrID;
            cedaObject.Topic = msg.Destination.getName();

            MessageBody body = msg.MessageBody;
            if (body == null)
                return cedaObject;

            if (body.haseField((short) 5) != 0)
            {
                try
                {
                    cedaObject.ZipType = body.getInt((short) 5);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    cedaObject.ZipType = 0;
                }
            }
            else
            {
                cedaObject.ZipType = 0;
            }

            if (body.haseField((short) 4) != 0)
            {
                try
                {
                    cedaObject.MessageType = body.getString((short) 4);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }

            if (body.haseField((short) 3) != 0)
            {
                string strMsg = "";
                try
                {
                    switch (cedaObject.ZipType)
                    {
                        case 0:
                            strMsg = body.getString((short) 3);
                            break;
                        case 1:
                            strMsg = body.getGZIPString((short) 3);
                            break;
                        case 3:
                            //strMsg=
                            if (body.haseField((short) 1) != 0)
                                strMsg = com.adaptiveMQ2.utils.Utils.ungzipByte(body.getBlobField());
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                finally
                {
                    cedaObject.MessageBody = strMsg;
                }
            }

            if (body.haseField((short) 6) != 0)
            {
                try
                {
                    cedaObject.MessageSize = body.getInt((short) 6);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }

            if (body.haseField((short) 7) != 0)
            {
                try
                {
                    cedaObject.SendTime = body.getString((short) 7);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }

            if (body.haseField((short) 10) != 0)
            {
                try
                {
                    cedaObject.ResultCode = body.getString((short) 10);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }

            if (body.haseField((short)11) != 0)
            {
                try
                {
                    cedaObject.Result = body.getInt((short) 11);
                }
                catch (MessageBodyException e)
                {
                    LogHelper.Error(e);
                }
            }

            if (body.haseField((short)1) != 0)
            {
                if (cedaObject.ZipType == 3)
                {
                    cedaObject.BigMessage = body.getBlobField();
                }
            }

            return cedaObject;
        }


        public static Message GetLoginMessage(string userName, string pwd, string roleName, string sessionId, bool encryption)
        {
            var ht = new Dictionary<string, object>();
            ht.Add("method", "login");
            var args = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(userName))
                userName = "ADMIN";
            args.Add("userName", userName);
            args.Add("location", "PINGO");
            if (string.IsNullOrEmpty(pwd))
                pwd = "111111";
            args.Add("password", pwd);
            if (string.IsNullOrEmpty(roleName))
                roleName = "1";
            args.Add("roleName", roleName);
            if (string.IsNullOrEmpty(sessionId))
                sessionId = "CCB" + random.Next(10000, 20000);
            args.Add("st",sessionId);
            args.Add("loginType", "PC");
            args.Add("clientType", "E");
            if (encryption)
            {
                string loginInfo = MsgHelper.Serializer<Dictionary<string, object>>(args);
                args = EncryptHelper.getLoginInfo(loginInfo);
            }
           
            ht.Add("parameter", args);
            string str = MsgHelper.Serializer<Dictionary<string, object>>(ht);
            var msg = new com.adaptiveMQ2.message.Message();
            msg.MessageBody.addString((short)3, str);
            return msg;
        }
        static Random random=new Random();

        public static Message GetLoginMessage()
        {
          return  GetLoginMessage("ADMIN", "111111", "1", "CCB" + random.Next(10000, 20000),false);
        }

    }
}