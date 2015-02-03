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
        private Dictionary<string, CsharpCodeGenConfig> CsharpCodeGenConfig = new Dictionary<string, CsharpCodeGenConfig>();

        private bool isInit = false;

        public Csharp_CodeGenConfigUI()
        {
            InitializeComponent();

        }

        public CsharpCodeGenConfig getConfigObject()
        {           
           CsharpCodeGenConfig cmc = new CsharpCodeGenConfig();
            cmc.Id = "";
            cmc.DisplayName = cbObject.Text;
            cmc.ModelNameSpace = tbModelNameSpace.Text;
            cmc.IDALNameSpace = tbIDALNameSpace.Text;
            cmc.DALNameSpace = tbDALNameSpace.Text;

            cmc.DALDllName = tbDALDLLName.Text;
            cmc.BLLDllName = tbBLLDLLName.Text;

            cmc.BLLNameSpace = tbBLLNameSpace.Text;
            cmc.DisplayName = cbObject.Text;
            cmc.IsDelete = false;

            
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

        public void Save()
        {

            bool status = false;
            string msg = "";
            CsharpCodeGenConfig tem = getConfigObject();
            IniConfigHelper.writeDefaultDBInfo(tem);
            if (CsharpCodeGenConfig.Count > 0)
            {
                if (CsharpCodeGenConfig.ContainsKey(cbObject.Text))
                {
                    string id = CsharpCodeGenConfig[cbObject.Text].Id;
                    bool isDelete = CsharpCodeGenConfig[cbObject.Text].IsDelete;
                    CsharpCodeGenConfig[cbObject.Text] = null;
                    CsharpCodeGenConfig[cbObject.Text] = tem;
                    CsharpCodeGenConfig[cbObject.Text].Id = id;
                    CsharpCodeGenConfig[cbObject.Text].IsDelete = isDelete;
                }
                else if (!string.IsNullOrEmpty(cbObject.Text))
                {
                    CsharpCodeGenConfig.Add(cbObject.Text, tem);
                    addItem(cbObject.Text);
                }
                foreach (string key in CsharpCodeGenConfig.Keys)
                {
                    status = IniConfigHelper.WriteDBInfo(CsharpCodeGenConfig[key], ref msg);
                    if (!status)
                        throw new Exception(msg);
                    else
                    {
                        status = false;
                        msg = "";
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(tem.DisplayName))
                {
                    status = IniConfigHelper.WriteDBInfo(tem, ref msg);
                    if (!status)
                        throw new Exception(msg);
                    else
                    {
                        CsharpCodeGenConfig.Add(tem.DisplayName, tem);
                        addItem(tem.DisplayName);
                    }
                }
            }
        }

        private void init()
        {
            isInit = true;
            clear();
            CsharpCodeGenConfig.Clear();
          
            string str = IniConfigHelper.ReadDefaultDBInfo();
            IList<CsharpCodeGenConfig> tem = IniConfigHelper.ReadDBInfo();
            CsharpCodeGenConfig.Clear();
            clearCbUI();
            addItem("");
            foreach (CsharpCodeGenConfig config in tem)
            {
                if (!CsharpCodeGenConfig.ContainsKey(config.DisplayName))
                {
                    CsharpCodeGenConfig.Add(config.DisplayName, config);
                    addItem(config.DisplayName);
                }
            }

            if (string.IsNullOrEmpty(str))
            {
                clear();
            }
            else
            {
                if (CsharpCodeGenConfig.Count > 0)
                {
                    foreach (string key in CsharpCodeGenConfig.Keys)
                    {
                        if (key.Equals(str))
                        {
                            CsharpCodeGenConfig[key].IsDelete = false;
                            
                            setUI(CsharpCodeGenConfig[key]);
                            setCbText(CsharpCodeGenConfig[key].DisplayName);
                        }
                    }
                }
            }
            isInit = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (checkData())
            {
                CsharpCodeGenConfig tem = getConfigObject();
                if (CsharpCodeGenConfig.ContainsKey(cbObject.Text))
                {
                    string id = CsharpCodeGenConfig[cbObject.Text].Id;
                    CsharpCodeGenConfig[cbObject.Text] = null;
                    CsharpCodeGenConfig[cbObject.Text] = tem;
                    CsharpCodeGenConfig[cbObject.Text].Id = id;
                }
                else if (!string.IsNullOrEmpty(cbObject.Text))
                {
                    CsharpCodeGenConfig.Add(cbObject.Text, tem);
                    addItem(tem.DisplayName);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbObject.Text))
            {
                if (CsharpCodeGenConfig.ContainsKey(cbObject.Text))
                {
                    CsharpCodeGenConfig[cbObject.Text].IsDelete = true;

                    deleteItem(cbObject.Text);
                }
            }
            clear();
        }

        private void clearCbUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Simple(clearCbUI), null);
            }
            else
            {
                cbObject.Items.Clear();
            }
        }

        private void setCbText(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new setValue(setCbText), new object[] { text });
            }
            else
            {
                cbObject.Text = text;
            }
        }

        private delegate void delegateObject(CsharpCodeGenConfig config);
        private void setUI(CsharpCodeGenConfig config)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new delegateObject(setUI), new object[] { config });
            }
            else
            {
                tbModelNameSpace.Text = config.ModelNameSpace;

                //cbObject.Text = config.DisplayName;
                tbIDALNameSpace.Text = config.IDALNameSpace;
                tbDALNameSpace.Text = config.DALNameSpace;

                tbDALDLLName.Text = config.DALDllName;
                tbBLLDLLName.Text = config.BLLDllName;
                cbObject.Text = config.DisplayName;

                tbBLLNameSpace.Text = config.BLLNameSpace;
                tbPluginName.Text = config.PluginName;

                tbOutPut.Text = config.OutPut;
                tbTableFilter.Text = config.TableFilter;
                cbShowForm.Checked = config.IsShowGenCode;
                cbShowComment.Checked = config.IsShowComment;
                if (rbtnDefault.Text == config.CodeRule)
                {
                    rbtnDefault.Checked = true;
                }
                else
                {
                    rbtnIbatis.Checked = true;
                }
                tbIbatis.Text = config.Ibatis;
            }
        }

        private delegate void Simple();
        private void clear()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Simple(clear), null);
            }
            else
            {
                errorProvider1.Clear();

                cbObject.Text = "";
                
                tbBLLDLLName.Text = "";
                tbBLLNameSpace.Text = "";
                tbDALDLLName.Text = "";
                tbDALNameSpace.Text = "";
                tbIbatis.Text = "";
                tbIDALNameSpace.Text = "";
                tbModelNameSpace.Text = "";
                tbOutPut.Text = "";
                tbPluginName.Text = "";
                tbTableFilter.Text = "";

                rbtnDefault.Checked = true;
                rbtnIbatis.Checked = false;

                cbShowComment.Checked = false;
                cbShowForm.Checked = false;
            }
        }
      

        private bool checkData()
        {
            bool status = true;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(cbObject.Text))
            {
                errorProvider1.SetError(cbObject, "Value is empty.");
                status = false;
                cbObject.Focus();
            }
            return status;
        }

        private delegate void setValue(string displayName);
        private void addItem(string displayName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new setValue(addItem), new object[] { displayName });
            }
            else
            {
                cbObject.Items.Add(displayName);
            }
        }

        private void deleteItem(string displayName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new setValue(deleteItem), new object[] { displayName });
            }
            else
            {
                cbObject.Items.Remove(displayName);
            }
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
            if (!string.IsNullOrEmpty(str) && !str.EndsWith("\\"))
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

        private void cbObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInit)
            {
                if (!string.IsNullOrEmpty(cbObject.Text))
                {
                    if (CsharpCodeGenConfig.Count > 0)
                    {
                        foreach (string key in CsharpCodeGenConfig.Keys)
                        {
                            if (key.Equals(cbObject.Text))
                            {
                                setUI(CsharpCodeGenConfig[key]);
                            }
                        }
                    }
                }
                else
                {
                    clear();
                }
            }
        }  
    }
}
