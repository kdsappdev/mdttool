using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Model;
using MDT.Tools.DB.Csharp_CodeGen.Plugin.Utils;


namespace MDT.Tools.DB.Csharp_ModelGen.Plugin.UI
{
    public partial class Csharp_CodeGenConfigUI : UserControl
    {
        public Csharp_CodeGenConfigUI()
        {
            InitializeComponent();

        }

        private CsharpCodeGenConfig cmc;
        public CsharpCodeGenConfig CMC
        {
            get
            {
                if (cmc == null)
                {
                    cmc = new CsharpCodeGenConfig();
                }
                cmc.ModelNameSpace = tbModelNameSpace.Text;
                cmc.IDALNameSpace = tbIDALNameSpace.Text;
                cmc.DALNameSpace = tbDALNameSpace.Text;
                cmc.BLLNameSpace = tbBLLNameSpace.Text;
                cmc.PluginName = tbPluginName.Text;

                cmc.OutPut = tbOutPut.Text;
                cmc.TableFilter = tbTableFilter.Text;
                cmc.IsShowGenCode = cbShowForm.Checked;
                cmc.IsShowComment = cbShowComment.Checked;
                if(rbtnDefault.Checked)
                {
                    cmc.CodeRule = rbtnDefault.Text;
                }
                else
                {
                    cmc.CodeRule = rbtnIbatis.Text;
                }
                cmc.Ibatis = tbIbatis.Text;
                return cmc;
            }
        }
        public void Save()
        {
            string msg = "";
            CsharpCodeGenConfig temp = CMC;
            bool status = IniConfigHelper.Write(CMC, ref msg);
            if (status != true)
                throw new Exception(msg);

        }

        private void init()
        {
            cmc = IniConfigHelper.ReadCsharpModelGenConfig();
            tbModelNameSpace.Text = cmc.ModelNameSpace;

            tbIDALNameSpace.Text = cmc.IDALNameSpace;
            tbDALNameSpace.Text = cmc.DALNameSpace;
            tbBLLNameSpace.Text = cmc.BLLNameSpace;
            tbPluginName.Text = cmc.PluginName;

            tbOutPut.Text = cmc.OutPut;
            tbTableFilter.Text = cmc.TableFilter;
            cbShowForm.Checked = cmc.IsShowGenCode;
            cbShowComment.Checked = cmc.IsShowComment;
            if(rbtnDefault.Text==cmc.CodeRule)
            {
                rbtnDefault.Checked = true;
            }
            else
            {
                rbtnIbatis.Checked = true;
            }
            tbIbatis.Text = cmc.Ibatis;
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

        private void rbtnIbatis_CheckedChanged(object sender, EventArgs e)
        {
            setCodeRule(true);
        }

        private void rbtnDefault_CheckedChanged(object sender, EventArgs e)
        {
            setCodeRule(false);
        }
        private void setCodeRule(bool flag)
        {
            tbIbatis.Enabled = flag;
            btnIbatisBrower.Enabled = flag;
        }

        private void btnIbatisBrower_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();

            if (result.Equals(DialogResult.OK))
            {
                tbIbatis.Text = openFileDialog1.FileName;

            }
        }
    }
}
