using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MDT.Tools.Order.Monitor.Plugin.Util
{
    public class zedgraphHelper
    {
        public static DateTime stringToDateTime(string str)
        {

            DateTime st = new DateTime();
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > 7)
                {
                    str = str.Insert(4, "/");
                    str = str.Insert(7, "/");
                }
                DateTime.TryParse(str, out st);
            }
            return st;
        }

        //public static 

        public static Color getRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数，没什么好说的
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);

            //  为了在白色背景上显示，尽量生成深色
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;

            return System.Drawing.Color.FromArgb(int_Red, int_Green, int_Blue);
        }
    }
}
