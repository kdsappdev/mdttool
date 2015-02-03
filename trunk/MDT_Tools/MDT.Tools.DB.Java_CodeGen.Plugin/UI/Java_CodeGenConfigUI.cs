using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MDT.Tools.DB.Java_CodeGen.Plugin.Model;
using MDT.Tools.DB.Java_CodeGen.Plugin.Utils;


namespace MDT.Tools.DB.Java_CodeGen.Plugin.UI
{
    public partial class Java_CodeGenConfigUI : UserControl
    {
        private bool isInit = false;

        private Dictionary<string, JavaCodeGenConfig> javaCodeGenConfigs = new Dictionary<string, JavaCodeGenConfig>();

        public Java_CodeGenConfigUI()
        {
            InitializeComponent();

        }

        private JavaCodeGenConfig getConfigObject()
        {
            JavaCodeGenConfig cmc = new JavaCodeGenConfig();

            cmc.Id = "";
            cmc.IsDelete = false;
            cmc.DisplayName = cbObject.Text;
            cmc.BSPackage = tbBSPackage.Text;
            cmc.WSPackage = tbWSPackage.Text;
            cmc.OutPut = tbOutPut.Text;
            cmc.TableFilter = tbTableFilter.Text;
            cmc.IsShowGenCode = cbShowForm.Checked;
            if (rbtnDefault.Checked)
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
            JavaCodeGenConfig temp = getConfigObject();
            status = IniConfigHelper.writeDefaultDBInfo(temp);
            if (!status)
            {
                throw new Exception(msg);
            }
            else
            {
                status = false;
                msg = "";
            }
            if (javaCodeGenConfigs.Count > 0)
            {
                if (javaCodeGenConfigs.ContainsKey(cbObject.Text))
                {
                    string id = javaCodeGenConfigs[cbObject.Text].Id;
                    bool isDelete = javaCodeGenConfigs[cbObject.Text].IsDelete;
                    javaCodeGenConfigs[cbObject.Text] = null;
                    temp.Id = id;
                    temp.IsDelete = isDelete;
                    javaCodeGenConfigs[cbObject.Text] = temp;
                }
                else if (!string.IsNullOrEmpty(cbObject.Text))
                {
                    javaCodeGenConfigs.Add(cbObject.Text, temp);
                    addItem(cbObject.Text);
                }
                foreach (string key in javaCodeGenConfigs.Keys)
                {
                    status = IniConfigHelper.WriteDBInfo(javaCodeGenConfigs[key], ref msg);
                    if (!status)
                    {
                        throw new Exception(msg);
                    }
                    else
                    {
                        status = false;
                        msg = "";
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(temp.DisplayName))
                {
                    status = IniConfigHelper.WriteDBInfo(temp, ref msg);
                    if (!status)
                    {
                        throw new Exception(msg);
                    }
                    else
                    {
                        javaCodeGenConfigs.Add(temp.DisplayName, temp);
                        addItem(temp.DisplayName);
                    }
                }
            }

        }

        private void init()
        {
            isInit = true;
            clear();
            javaCodeGenConfigs.Clear();
            clearCbUI();
            addItem("");
            JavaCodeGenConfig config = IniConfigHelper.getDefaultObject(); 
            
            if (config != null)
            {
                setUI(config);
                setCbText(config.DisplayName);
            }
            IList<JavaCodeGenConfig> configs = IniConfigHelper.ReadDBInfo();
            foreach (JavaCodeGenConfig jcc in configs)
            {
                if (!javaCodeGenConfigs.ContainsKey(jcc.DisplayName))
                {
                    javaCodeGenConfigs.Add(jcc.DisplayName, jcc);
                    addItem(jcc.DisplayName);
                }
            }
            isInit = false;
        }

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

                tbBSPackage.Text = "";
                tbIbatis.Text = "";
                tbOutPut.Text = "";
                tbTableFilter.Text = "";
                tbWSPackage.Text = "";
                cbShowForm.Checked = false;
                rbtnDefault.Checked = true;
                rbtnIbatis.Checked = false;
            }
        }

        private delegate void delegateObject(JavaCodeGenConfig config);
        private void setUI(JavaCodeGenConfig cmc)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new delegateObject(setUI), new object[] { cmc });
            }
            else
            {
                setCbText(cmc.DisplayName);

                tbBSPackage.Text = cmc.BSPackage;
                tbWSPackage.Text = cmc.WSPackage;
                tbOutPut.Text = cmc.OutPut;
                tbTableFilter.Text = cmc.TableFilter;
                cbShowForm.Checked = cmc.IsShowGenCode;
                if (rbtnDefault.Text == cmc.CodeRule)
                {
                    rbtnDefault.Checked = true;
                }
                else
                {
                    rbtnIbatis.Checked = true;
                }
                tbIbatis.Text = cmc.Ibatis;
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

        private delegate void Simple();
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

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbObject.Text))
            {
                if (javaCodeGenConfigs.ContainsKey(cbObject.Text))
                {
                    javaCodeGenConfigs[cbObject.Text].IsDelete = true;

                    deleteItem(cbObject.Text);
                }
            }
            clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (checkData())
            {
                JavaCodeGenConfig config = getConfigObject();
                if (javaCodeGenConfigs.ContainsKey(cbObject.Text))
                {
                    string id = javaCodeGenConfigs[cbObject.Text].Id;
                    config.Id = id;
                    javaCodeGenConfigs[cbObject.Text] = null;
                    javaCodeGenConfigs[cbObject.Text] = config;
                }
                else if (!string.IsNullOrEmpty(cbObject.Text))
                {
                    javaCodeGenConfigs.Add(cbObject.Text, config);
                    addItem(cbObject.Text);
                }
            }
        }

        private void cbObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInit)
            {
                if (!string.IsNullOrEmpty(cbObject.Text))
                {
                    if (javaCodeGenConfigs.Count > 0)
                    {
                        foreach (string key in javaCodeGenConfigs.Keys)
                        {
                            if (key.Equals(cbObject.Text))
                            {
                                setUI(javaCodeGenConfigs[key]);
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
