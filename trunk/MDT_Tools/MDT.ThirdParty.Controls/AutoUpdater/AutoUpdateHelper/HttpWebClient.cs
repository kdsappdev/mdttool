using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MDT.ThirdParty.Controls
{
    public class HttpWebClient : WebClient
    {
        private int timeOut = 5;
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            //request.Timeout = 1000 * TimeOut;
            //request.ReadWriteTimeout = 1000 * TimeOut;
            return request;
        }
    }
}
