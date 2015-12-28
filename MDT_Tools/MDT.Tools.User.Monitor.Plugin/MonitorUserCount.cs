using System;
namespace MDT.Tools.User.Monitor.Plugin
{
    public class MonitorUserCount
    {
        public  static  string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }


        private string symbol;

        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }


        private int num;

        public int Num
        {
            get { return num; }
            set { num = value; }
        }


        private double amount;

        public double  Amount
        {
            get { return amount; }
            set { amount = value; }
        }


        private string date;

        public string Date
        {
            get { return date; }
            set { date = value; }
        }


        private string time;

        public string Time
        {
            get { return time; }
            set { time = value; }
        }


        private string memo;

        public string Memo
        {
            get { return memo; }
            set { memo = value; }
        }
       
 
       
    }
}
