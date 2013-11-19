using System;
using System.Collections.Generic;
using System.Text;
using Atf.Common;
using MDT.Tools.Core.Mq;
using com.adaptiveMQ2.client;

namespace MDT.Tools.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] topics = System.Configuration.ConfigurationSettings.AppSettings["topic"].Split(new char[] { ',' });

            string count = System.Configuration.ConfigurationSettings.AppSettings["session"];
            string mqT = System.Configuration.ConfigurationSettings.AppSettings["Mq"];
            if (mqT == "1")
            {
                if (!count.Equals("1"))
                {
                    for (int i = 0; i < topics.Length; i++)
                    {
                        Mq1 mq = new Mq1();
                        if (i % 2 != 0)
                        {
                            mq.flag = "---------";
                        }
                        else
                        {
                            mq.flag = "*********";
                        }
                        mq.Start();

                        mq.Subscriber(topics[i]);

                    }
                }
                else
                {
                    Mq1 mq = new Mq1();
                    mq.flag = "---------";
                    mq.Start();
                    for (int i = 0; i < topics.Length; i++)
                    {
                        mq.Subscriber(topics[i]);
                    }

                }
            }
            if (mqT == "2")
            {



                Mq2 mq2 = new Mq2();


                ServerInfo si = new ServerInfo();
                si.Host = "192.168.2.122/mq.httpMQTunnel";
                si.Port = "19990";
                si.Protocal = "PROTOCOL_HTTP";
                mq2.Start(si, "MQ");

                for (int i = 0; i < topics.Length; i++)
                {
                    mq2.Subscribe(topics[i]);
                }
            }

            System.Console.ReadLine();

        }
    }
}
