using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.User.Monitor.Plugin
{
    [Serializable]
    public class RSACertContent
    {
        public String PublicKey { set; get; }
        public String PrivateKey { set; get; }
        public String ID { set; get; }
        public bool IsEncrypt { set; get; }
        public Dictionary<String, String> Extra = new Dictionary<String, String>();
    }
}
