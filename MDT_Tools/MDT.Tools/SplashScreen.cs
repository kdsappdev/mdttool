using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MDT.Tools
{
    internal partial class SplashScreen : Form
    {
 
         private Image m_imgImage = null;
        private EventHandler m_evthdlAnimator = null;
        public SplashScreen(Image pic)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer
              | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
          
            m_evthdlAnimator = new EventHandler(OnImageAnimate);
          
            m_imgImage = pic;
            BeginAnimate();
           
           
        }
 
        protected override void OnPaint(PaintEventArgs e)
        {
         
            //base.OnPaint(e);
            Graphics gs = e.Graphics;
            SmoothingMode _oldMode = gs.SmoothingMode;
            gs.SmoothingMode = SmoothingMode.AntiAlias;
            if (m_imgImage != null)
            {
                gs.DrawImage(m_imgImage, new Rectangle(0, 0, this.Width, this.Height));
                UpdateImage(); 
            }
        }
 

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
             if (m_imgImage != null)
            {
                StopAnimate();
                m_imgImage = null;
            }
        }

        internal void BeginAnimate()
        {
           if (m_imgImage == null)
                return;
         
           if (ImageAnimator.CanAnimate(m_imgImage))
           {
                ImageAnimator.Animate(m_imgImage,m_evthdlAnimator);
           }
        }
 
        internal void StopAnimate()
        {
            if (m_imgImage == null)
                return;
 
            if (ImageAnimator.CanAnimate(m_imgImage))
            {
                ImageAnimator.StopAnimate(m_imgImage,m_evthdlAnimator);
            }
        }
 
        private void UpdateImage()
        {
            if (m_imgImage == null)
                return;
 
            if (ImageAnimator.CanAnimate(m_imgImage))
            {
                ImageAnimator.UpdateFrames(m_imgImage);
            }
        }

        private void OnImageAnimate(Object sender,EventArgs e)
        {
            this.Invalidate(); 
        }

    }
}
