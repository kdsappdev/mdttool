using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Windows.Forms;
using MDT.Tools.ExcelAddin.Utils;

namespace MDT.Tools.ExcelAddin
{
    public partial class ThisAddIn
    {
        ConfigInfo configInfo = IniConfigHelper.ReadConfigInfo();//获取配置文件

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //Consts.ConfigInfo();
            AddToolbar();
            CheckIfMenuBarExists();
            processExcelJob.application = Application;
            QuartService.StartJob(configInfo.VkInterval);
            LogHelper.IsEnable = configInfo.Logon;
            LogHelper.Info("保存的时间：" + configInfo.VkInterval);
        }

        private string menuTag = "A unique tag";

        private void CheckIfMenuBarExists()
        {
            try
            {
                Office.CommandBarPopup foundMenu = (Office.CommandBarPopup)
                    this.Application.CommandBars.ActiveMenuBar.FindControl(
                    Office.MsoControlType.msoControlPopup, System.Type.Missing, menuTag, true, true);

                if (foundMenu != null)
                {
                    foundMenu.Delete(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        Office.CommandBar commandBar;
        Office.CommandBarButton firstButton;
        private void AddToolbar()
        {
            try
            {
                commandBar = Application.CommandBars["Test"];
            }
            catch (ArgumentException)
            {

                // Toolbar named Test does not exist so we should create it.
            }

            if (commandBar == null)
            {
                // Add a commandbar named Test.
                commandBar = Application.CommandBars.Add("Test", 1, missing, true);
            }

            try
            {
                // Add a button to the command bar and an event handler.

                firstButton = (Office.CommandBarButton)commandBar.Controls.Add(1, missing, missing, missing, missing);
                firstButton.Style = Office.MsoButtonStyle.msoButtonCaption;
                firstButton.Caption = configInfo.MenuName;
                firstButton.Click += new Office._CommandBarButtonEvents_ClickEventHandler(ButtonClick);
                commandBar.Visible = true;
            }
            catch (ArgumentException e)
            {
                LogHelper.Error(e);
            }
        }

        // Handles the event when a button on the new toolbar is clicked.
        private void ButtonClick(Office.CommandBarButton ctrl, ref bool cancel)
        {
            try
            {
                ConfigInfo configInfo = IniConfigHelper.ReadConfigInfo();
                ProcessExcelHelper.processExcel(Application);
                LogHelper.Info("手动点击保存");
                MessageBox.Show("保存成功", "提示");
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                MessageBox.Show(e.Message);
            }

        }


        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
