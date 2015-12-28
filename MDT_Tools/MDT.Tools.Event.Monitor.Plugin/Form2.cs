using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;

namespace MDT.Tools.Event.Monitor.Plugin
{
    public partial class Form2 : DockContent
    {
        string leveltype, infocolor, warncolor, errorcolor, fatalcolor, infovoice, warnvoice, errorvoice, fatalerrorvoice,infovt,warnvt,errorvt,fatalvt;
        public Form2()
        {
            this.MaximizeBox = false;//删除最大化按钮
            this.MinimizeBox = false;//删除最小换按钮
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            VoiceBtTxt();

            ColorBtBgc();


            levelComboBox.Text=leveltype = SystemConfig.GetConfigData("levelComboBox", string.Empty);
            levelComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            infoVoiceT.Text = infovt = SystemConfig.GetConfigData("infoVoiceT", string.Empty);
            infoVoiceT.DropDownStyle = ComboBoxStyle.DropDownList;

            warnVioceT.Text = warnvt = SystemConfig.GetConfigData("warnVioceT", string.Empty);
            warnVioceT.DropDownStyle = ComboBoxStyle.DropDownList;

            errorVoiceT.Text =errorvt= SystemConfig.GetConfigData("errorVoiceT", string.Empty);
            errorVoiceT.DropDownStyle = ComboBoxStyle.DropDownList;

            fatalErrorVT.Text =fatalvt= SystemConfig.GetConfigData("fatalErrorVT", string.Empty);
            fatalErrorVT.DropDownStyle = ComboBoxStyle.DropDownList;
         }

        //委托定义
        public delegate void FatalErrorColorChangeHandler();//定义一个委托
        public event FatalErrorColorChangeHandler FatalErrorColorChang;//定义一个事件


        public void OnColorChange()//用来触发委托
        {
            if (FatalErrorColorChang != null)
            {
                FatalErrorColorChang();
            }

        }

        //颜色改变的监听方法，所有颜色选择框公用的方法
        private void fatalErrorColorPickEdit_TextChanged(object sender, EventArgs e)
        {
            
            colorDialog1.AllowFullOpen = false;
            colorDialog1.AnyColor = true;
            colorDialog1.SolidColorOnly = false;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;
                
                string c = button.Name;
                if (c == "infoColorPickEdit")
                {
                Color color = colorDialog1.Color;
                infoColorPickEdit.BackColor = color;
                infocolor = color.R + "," + color.G + "," + color.B;
                }
                if (c == "warnColorPickEdit")
                {
                    Color color = colorDialog1.Color;
                    warnColorPickEdit.BackColor = color;
                    warncolor = color.R + "," + color.G + "," + color.B;
                }
                if (c == "errorColorPickEdit")
                {
                    Color color = colorDialog1.Color;
                    errorColorPickEdit.BackColor = color;
                    errorcolor = color.R + "," + color.G + "," + color.B;
                }
                if (c == "fatalErrorColorPickEdit")
                {
                    Color color = colorDialog1.Color;
                    fatalErrorColorPickEdit.BackColor = color;
                    fatalcolor = color.R + "," + color.G + "," + color.B;
                }
            
            }

        }
        


        //下拉单选框的监听方法，用来确定筛选过滤的等级
        private void levelComboBoxEdit_TextChanged(object sender, EventArgs e)
        {

            leveltype = levelComboBox.Text;
            
        }

        //所有自选音乐的公用监听方法
        private void infoVoiceBt_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;
            this.openFileDialog1.Filter = "Wav文件|*.wav|Wma文件|*.wma|Wmv文件|*.wmv|mid文件|*.mid|所有格式|*.*";

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = this.openFileDialog1.FileName;
                if (button.Name == "infoVoiceBt")
                {

                    infovoice = FileName.ToString();
                    button.Text = FileName.Substring(FileName.LastIndexOf("/") + 1);
                }
                    if (button.Name == "warningVoiceBt")
                    {
                        warnvoice = FileName.ToString();
                        
                        button.Text = FileName.Substring(FileName.LastIndexOf("/") + 1);
                    }
                    if (button.Name == "errorVoiceBt")
                    {
                        errorvoice = FileName.ToString();
                        
                        button.Text = FileName.Substring(FileName.LastIndexOf("/") + 1);
                    }
                    if (button.Name == "fatalErrorVoiceBt")
                    {
                        fatalerrorvoice = FileName.ToString();
                        
                        button.Text = FileName.Substring(FileName.LastIndexOf("/") + 1);
                    
                }
              }
        }
        //leveltype, infocolor, warncolor, errorcolor, 
        //fatalcolor, infovoice, warnvoice, errorvoice, fatalerrorvoice;
        private void button1_Click(object sender, EventArgs e)
        {
           
            SystemConfig.WriteConfigData("levelComboBox", leveltype);
            SystemConfig.WriteConfigData("infoColorPickEdit", infocolor);
            SystemConfig.WriteConfigData("warnColorPickEdit", warncolor);
            SystemConfig.WriteConfigData("errorColorPickEdit", errorcolor);
            SystemConfig.WriteConfigData("fatalErrorColorPickEdit", fatalcolor);
            SystemConfig.WriteConfigData("infoVoiceBt", infovoice);
            SystemConfig.WriteConfigData("warningVoiceBt", warnvoice);
            SystemConfig.WriteConfigData("errorVoiceBt", errorvoice);
            SystemConfig.WriteConfigData("fatalErrorVoiceBt", fatalerrorvoice);

            SystemConfig.WriteConfigData("infoVoiceT", infovt);
            SystemConfig.WriteConfigData("warnVioceT", warnvt);
            SystemConfig.WriteConfigData("errorVoiceT", errorvt);
            SystemConfig.WriteConfigData("fatalErrorVT", fatalvt);
            OnColorChange();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //设定颜色选择框的颜色
        

       

        private void VoiceT_TextChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cb = sender as System.Windows.Forms.ComboBox;
            if (cb.Name == "infoVoiceT")
            {
            infovt = cb.Text;
            }
            if (cb.Name == "warnVioceT")
            {
                warnvt = cb.Text;
            }
            if (cb.Name == "errorVoiceT")
            {
                errorvt = cb.Text;
            }
            if (cb.Name == "fatalErrorVT")
            {
                fatalvt = cb.Text;
            }
        }

       
        
        
        
        
        
        
        private void VoiceBtTxt() 
        {
             infovoice = SystemConfig.GetConfigData("infoVoiceBt", string.Empty);
             warnvoice = SystemConfig.GetConfigData("warningVoiceBt", string.Empty);
             errorvoice = SystemConfig.GetConfigData("errorVoiceBt", string.Empty);
             fatalerrorvoice = SystemConfig.GetConfigData("fatalErrorVoiceBt", string.Empty);
            if (infovoice != string.Empty)
            { infoVoiceBt.Text = infovoice.Substring(infovoice.LastIndexOf(@"\") + 1, infovoice.Length - infovoice.LastIndexOf(@"\") - 1); }

            if (warnvoice != string.Empty)
            { warningVoiceBt.Text = warnvoice.Substring(warnvoice.LastIndexOf(@"\") + 1, warnvoice.Length - warnvoice.LastIndexOf(@"\") - 1); }

            if (errorvoice != string.Empty)
            { errorVoiceBt.Text = errorvoice.Substring(errorvoice.LastIndexOf(@"\") + 1, errorvoice.Length - errorvoice.LastIndexOf(@"\") - 1); }

            if (fatalerrorvoice != string.Empty)
            { fatalErrorVoiceBt.Text = fatalerrorvoice.Substring(fatalerrorvoice.LastIndexOf(@"\") + 1, fatalerrorvoice.Length - fatalerrorvoice.LastIndexOf(@"\") - 1); }
        }

        private void ColorBtBgc() 
        {
            errorcolor = SystemConfig.GetConfigData("errorColorPickEdit", string.Empty);
            warncolor = SystemConfig.GetConfigData("warnColorPickEdit", string.Empty);
             infocolor = SystemConfig.GetConfigData("infoColorPickEdit", string.Empty);
             fatalcolor = SystemConfig.GetConfigData("fatalErrorColorPickEdit", string.Empty);
            if (infocolor != string.Empty)
            {
                string[] c = infocolor.Split(',');
                infoColorPickEdit.BackColor = Color.FromArgb(Int32.Parse(c[0]), Int32.Parse(c[1]), Int32.Parse(c[2]));
                infoColorPickEdit.Text = "";
            }
            if (warncolor != string.Empty)
            {
                string[] c = warncolor.Split(',');
                warnColorPickEdit.BackColor = Color.FromArgb(Int32.Parse(c[0]), Int32.Parse(c[1]), Int32.Parse(c[2]));
                warnColorPickEdit.Text = "";
            }
            if (errorcolor != string.Empty)
            {
                string[] c = errorcolor.Split(',');
                errorColorPickEdit.BackColor = Color.FromArgb(Int32.Parse(c[0]), Int32.Parse(c[1]), Int32.Parse(c[2]));
                errorColorPickEdit.Text = "";
            }
            if (fatalcolor != string.Empty)
            {
                string[] c = fatalcolor.Split(',');
                fatalErrorColorPickEdit.BackColor = Color.FromArgb(Int32.Parse(c[0]), Int32.Parse(c[1]), Int32.Parse(c[2]));
                fatalErrorColorPickEdit.Text = "";
            }
        }

        private void setDef_Click(object sender, EventArgs e)
        {
            SystemConfig.WriteConfigData("errorColorPickEdit",string.Empty);
            SystemConfig.WriteConfigData("warnColorPickEdit", string.Empty);
            SystemConfig.WriteConfigData("infoColorPickEdit", string.Empty);
            SystemConfig.WriteConfigData("fatalErrorColorPickEdit", string.Empty);

            SystemConfig.WriteConfigData("levelComboBox", "信息");

            SystemConfig.WriteConfigData("infoVoiceBt", string.Empty);
            SystemConfig.WriteConfigData("warningVoiceBt", string.Empty);
            SystemConfig.WriteConfigData("errorVoiceBt", string.Empty);
            SystemConfig.WriteConfigData("fatalErrorVoiceBt", string.Empty);

            SystemConfig.WriteConfigData("infoVoiceT", "响铃一次");
            SystemConfig.WriteConfigData("warnVioceT", "响铃一次");
            SystemConfig.WriteConfigData("errorVoiceT", "响铃一次");
            SystemConfig.WriteConfigData("fatalErrorVT", "循环播放");
        }

        
       

    }

    


}
