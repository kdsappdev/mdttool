using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace MDT.Tools.Core.Utils
{
    public class CaptureScreenHelper
    {
        public static Image FullScreen()
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            Image image = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(image);
            g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(width, height));
            return image;
        }

        public static Image ReginScreen()
        {
            Image image = null;
            return image;
        }
    }
}
