using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MDT.Tools.PriceFeeder.Monitor.Model
{
    [Serializable]
    public class ListMONSvr
    {
        private List<MON_Price> mON_Price = new List<MON_Price>();

        public List<MON_Price> MON_Price
        {
            get { return mON_Price; }
            set { mON_Price = value; }
        }

        private List<MON_Svr> mON_Svr = new List<MON_Svr>();

        public List<MON_Svr> MON_Svr
        {
            get { return mON_Svr; }
            set { mON_Svr = value; }
        }
    }

    [Serializable]
    public class MON_Price
    {
        //{"MON_Price":[{"alert":0,"cont":1,"name":"AG","nochange":2}]
        private string _name;

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int _cont;

        public int cont
        {
            get { return _cont; }
            set { _cont = value; }
        }

        private int _nochange;

        public int nochange
        {
            get { return _nochange; }
            set { _nochange = value; }
        }

        private int _alert;

        public int alert
        {
            get
            {
                return _alert;
            }
            set
            {
                _alert = value;
            }
        }

        private Image alerts;

        public Image Alerts
        {
            get { return alerts; }
            set { alerts = value; }
        }
    }


    [Serializable]
    public class MON_Svr
    {
        //{"MON_Price":[{"alert":0,"cont":1,"name":"AG","nochange":2}],
        //"MON_Svr":[{"IPPort":"","Name":"Price DDS","count":1,"stat":0},{"IPPort":"","Name":"Stat DDS","count":0,"stat":0},
        //{"IPPort":"localhost:8194","Name":"Bloomberg","count":32,"stat":1}]} 

        private string iPPort;

        public string IPPort
        {
            get { return iPPort; }
            set { iPPort = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Image status;

        public Image Status
        {
            get { return status; }
            set { status = value; }
        }

        private int _count;

        public int count
        {
            get { return _count; }
            set { _count = value; }
        }

        private int _stat;

        public int stat
        {
            get { return _stat; }
            set { _stat = value; }
        }




    }
}
