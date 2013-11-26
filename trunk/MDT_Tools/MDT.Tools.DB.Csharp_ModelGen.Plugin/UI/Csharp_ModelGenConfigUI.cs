using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.DB.Csharp_Model.Plugin.Utils;
using MDT.Tools.DB.Csharp_ModelGen.Plugin.Model;
using MDT.Tools.DB.Csharp_ModelGen.Plugin.Utils;

namespace MDT.Tools.DB.Csharp_ModelGen.Plugin.UI
{
    public partial class Csharp_ModelGenConfigUI : UserControl
    {
        public Csharp_ModelGenConfigUI()
        {
            InitializeComponent();

        }

        private CsharpModelGenConfig cmc;
        public CsharpModelGenConfig CMC
        {
            get
            {
                if (cmc == null)
                {
                    cmc = new CsharpModelGenConfig();
                }
                cmc.NameSpace = tbNameSpace.Text;
                cmc.OutPut = tbOutPut.Text;
                cmc.TableFilter = tbTableFilter.Text;
                cmc.IsShowGenCode = cbShowForm.Checked;
                return cmc;
            }
        }
        public void Save()
        {
            string msg = "";
            CsharpModelGenConfig temp = CMC;
            bool status = IniConfigHelper.Write(CMC, ref msg);
            if (status != true)
                throw new Exception(msg);

        }

        private void init()
        {
            cmc = IniConfigHelper.ReadCsharpModelGenConfig();
            tbNameSpace.Text = cmc.NameSpace;
            tbOutPut.Text = cmc.OutPut;
            tbTableFilter.Text = cmc.TableFilter;
            cbShowForm.Checked = cmc.IsShowGenCode;
        }

        private void btnBrower_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                tbOutPut.Text = folderBrowserDialog1.SelectedPath;
                 
            }
        }

        private void Csharp_ModelGenConfigUI_Load(object sender, EventArgs e)
        {
            init();
        }

        private void tbOutPut_TextChanged(object sender, EventArgs e)
        {
            string str = tbOutPut.Text;
            if (!str.EndsWith("\\"))
            {
                str += "\\";
            }
            tbOutPut.Text = str;
        }
    }
}
