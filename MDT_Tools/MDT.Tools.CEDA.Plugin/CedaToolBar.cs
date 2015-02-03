using System;
using System.Text;
using System.Windows.Forms;

namespace MDT.Tools.CEDA.Plugin
{
    public partial class CedaToolBar : UserControl
    {
        public CedaToolBar()
        {
            InitializeComponent();
            this.Dock = DockStyle.Top;
        }

        public string ClearNameEN { set { tbtnClear.Text = value; } }
        public string SubScribeEN { set { tbtnSubscribe.Text = value; } }
        public string UnSubScribeEN { set { tbtnUnsubscribe.Text = value; } }
        public string JsonEn { set { tbtnJson.Text = value; } }

        public event EventHandler ClearClick
        {
            add { tbtnClear.Click += value; }
            remove { tbtnClear.Click -= value; }
        }

        public event EventHandler SubscribeClick
        {
            add { tbtnSubscribe.Click += value; }
            remove { tbtnSubscribe.Click -= value; }
        }

        public event EventHandler UnSubscribeClick
        {
            add { tbtnUnsubscribe.Click += value; }
            remove { tbtnUnsubscribe.Click -= value; }
        }
        public event EventHandler JsonClick
        {
            add { tbtnJson.Click += value; }
            remove { tbtnJson.Click -= value; }
        }


        /// <summary>
        /// 单独控制格式化按钮
        /// </summary>
        public bool EnableJsonClick
        {
            get { return EnableJsonClick; }
            set { tbtnJson.Enabled = value; }
        }


        /// <summary>
        /// 按钮是否启用
        /// 默认101
        /// 0：enable=false
        /// 1：enable=true
        /// </summary>
        public string EnableCode
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(tbtnSubscribe.Enabled ? "1" : "0");
                sb.Append(tbtnUnsubscribe.Enabled ? "1" : "0");
                sb.Append(tbtnClear.Enabled ? "1" : "0");
                return sb.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    switch (value.Length)
                    {
                        case 3:
                            tbtnSubscribe.Enabled = '1'.Equals(value[0]);
                            tbtnUnsubscribe.Enabled = '1'.Equals(value[1]);
                            tbtnClear.Enabled = '1'.Equals(value[2]);
                            break;
                        default:
                            tbtnSubscribe.Enabled = true;
                            tbtnUnsubscribe.Enabled = false;
                            tbtnClear.Enabled = true;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 按钮是否显示
        /// 默认111
        /// 0:visual=false
        /// 1:visual=true
        /// </summary>
        public string VisualCode
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(tbtnSubscribe.Visible ? "1" : "0");
                sb.Append(tbtnUnsubscribe.Visible ? "1" : "0");
                sb.Append(tbtnClear.Visible ? "1" : "0");
                return sb.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    switch (value.Length)
                    {
                        case 3:
                            tbtnSubscribe.Visible = '1'.Equals(value[0]);
                            tbtnUnsubscribe.Visible = '1'.Equals(value[1]);
                            tbtnClear.Visible = '1'.Equals(value[2]);
                            break;
                        default:
                            tbtnSubscribe.Visible = true;
                            tbtnUnsubscribe.Visible = true;
                            tbtnClear.Visible = true;
                            break;
                    }
                }
            }
        }

    }
}
