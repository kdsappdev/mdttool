using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace MDT.Tools.Core.UI
{
    public partial class Doc : DockContent
    {
        public Doc()
        {
            InitializeComponent();
        }

		private string m_fileName = string.Empty;
		public string FileName
		{
			get	{	return m_fileName;	}
			set
			{
                try
                {
                    if (value != string.Empty)
                    {
                        Stream s = new FileStream(value, FileMode.Open);

                        FileInfo efInfo = new FileInfo(value);

                        string fext = efInfo.Extension.ToUpper();

                        if (fext.Equals(".RTF"))
                            richTextBox1.LoadFile(s, RichTextBoxStreamType.RichText);
                        else
                            richTextBox1.LoadFile(s, RichTextBoxStreamType.PlainText);
                        s.Close();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
			    m_fileName = value;
				this.ToolTipText = value;
			}
		}
		private bool m_resetText = true;
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (m_resetText)
			{
				m_resetText = false;
				FileName = FileName;
			}
		}

		protected override string GetPersistString()
		{
			return GetType().ToString() + "," + FileName + "," + Text;
		}
    }
}