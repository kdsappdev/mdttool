using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace MDT.Tools.User.Monitor.Plugin
{
    public partial class Form2 : Form
    {
        private delegate void weituo();
  //      public static String mbsk = Application.StartupPath.Substring(0, Application.StartupPath.Length);//相对路径
       

        public Form2()
        {
            InitializeComponent();
            initialize_warning_count();
        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            object a = dropDownButton1.DropDownControl;

            labelControl1.BackColor = Color.FromName("red");
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("当人数总量达到您设置的警报值时，自动开启警报提示！");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

            StreamWriter sw = new StreamWriter("D:\\1.txt");
            string w = "" + dropDownButton1.Text;
            MessageBox.Show(w);
            sw.Write(w);
            sw.Close();
            StreamWriter sw2 = new StreamWriter("D:\\2.txt");
            string w2 = "" + textEdit1.Text;
            sw2.Write(w2);
            sw2.Close();
     

          
            this.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dropDownButton1.Text = "警铃";
            Form1 f1 = new Form1();
       
          
         
        }
        private void barButtonItem2_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dropDownButton1.Text = "斗地主";


            Form1 f1 = new Form1();
          
       
       
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dropDownButton1.Text = "雷霆战机";
          
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dropDownButton1.Text = "流水声";
            Form1 f1 = new Form1();

          

         
        }



        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string resultFile = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "D:\\Patch";
            openFileDialog1.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                resultFile = openFileDialog1.FileName;
            MessageBox.Show(resultFile);


        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
             if (this.textEdit1.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("请输入报警界限");
                this.textEdit1.Focus();
                return;
            }
             if (this.textEdit1.Text.Trim().Equals(string.Empty))
             {
                 MessageBox.Show("报警界限的格式输入有误");
                 this.textEdit1.Focus();
                 return;
             }
             if (this.textEdit2.Text.Trim().Equals(string.Empty))
             {

                 dropDownButton1.Text = "斗地主";
                 textEdit2.Text = Application.StartupPath + @"\resources\UserMonitorResource\music\斗地主.wav";
                 
             }


            //修改配置文件
            SystemConfig.WriteConfigData("user_warning_count", this.textEdit1.Text.Trim());
            SystemConfig.WriteConfigData("music_url", this.textEdit2.Text);
            myMethodDelegate mydelegate = new myMethodDelegate(Form1.soundplyStop);
            mydelegate += Form1.initialize_warning_count;
            mydelegate.Invoke();
            this.Close();
        //    MessageBox.Show("成功保存到配置文件" + Application.StartupPath + "\\plugin\\SystemConfig.xml \n点击读取按钮进行读取!");
        }
      

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
            
            dropDownButton1.Text = "斗地主";
            textEdit2.Text = Application.StartupPath + @"\control\resouse\UserMonitorResource\music\斗地主.wav";
            
        }

        private void barButtonItem3_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dropDownButton1.Text = "雷霆战机";
            textEdit2.Text = Application.StartupPath + @"\control\resouse\UserMonitorResource\music\雷霆战机.wav";
        }
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dropDownButton1.Text = "流水声";
            textEdit2.Text = Application.StartupPath + @"\control\resouse\UserMonitorResource\music\流水声.wav";
           
        }
       

        private void barButtonItem7_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string Pdfpath = "";
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "word Files(*.doc)|*.wav|All Files(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                Pdfpath = op.FileName;
            }
            else
            {
                Pdfpath = "";
            }
            textEdit2.Text = Pdfpath;
        }

        private void simpleButton1_Click_2(object sender, EventArgs e)
        {
            MessageBox.Show("仅支持标准WAV格式的音频！");              
        }

        private void simpleButton3_Click_2(object sender, EventArgs e)
        {
            this.Close();
        }
        public void initialize_warning_count()
        {
            //   string s = SystemConfig.GetConfigData("user_warning_count", string.Empty);
            try
            {
                textEdit1.Text = SystemConfig.GetConfigData("user_warning_count", string.Empty);
            }
            catch (Exception)
            {
                MessageBox.Show("您上次设置的报警界限不合法，请重新设置!现在默认为1000");
                textEdit1.Text = 1000 + "";
            }
        }

     
       

      



       























        }



    }
