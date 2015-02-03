using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MDT.Tools.MetaDesinger.Plugin.UI
{
    public partial class DesingerUI : UserControl
    {
        DesingerHost desingerHost = new DesingerHost();
        internal DesingerLayer desingerLayer1;
        public DesingerUI()
        {
            InitializeComponent();
            desingerHost.TopLevel = false;

            desingerHost.Dock = DockStyle.Fill;
            desingerHost.Location = new Point(0, 0);
            Controls.Add(desingerHost);

            desingerHost.Show();

            desingerLayer1 = new DesingerLayer();
            desingerLayer1.DesingerHost = desingerHost;
            desingerLayer1.Location = new Point(0, 0);
            desingerLayer1.Dock = DockStyle.Fill;
            Controls.Add(desingerLayer1);
            desingerLayer1.Show();
            desingerLayer1.BringToFront();
            Application.AddMessageFilter(new MessageFilter(desingerHost, this));
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
    class MessageFilter : IMessageFilter
    {
        DesingerHost _thehost;
        DesingerUI _theDesignerBoard;
        public MessageFilter(DesingerHost hostFrame, DesingerUI designer)
        {
            _thehost = hostFrame;
            _theDesignerBoard = designer;
        }
        #region IMessageFilter 成员
        public bool PreFilterMessage(ref Message m) //过滤所有控件的WM_PAINT消息
        {
            Control ctrl = (Control)Control.FromHandle(m.HWnd);
            if (_thehost != null && _theDesignerBoard != null && _thehost.Controls.Contains(ctrl) && m.Msg == 0x000F) // 0x000F == WM_PAINT
            {
                _theDesignerBoard.Refresh();
                return true;
            }
            return false;
        }
        #endregion
    }
}
