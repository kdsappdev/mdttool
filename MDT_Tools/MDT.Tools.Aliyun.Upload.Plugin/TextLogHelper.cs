using System;
using System.Windows.Forms;
using MDT.Tools.Core.Log;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Aliyun.Upload.Plugin
{
    public class TextLogHelper:ILog
    {
        private RichTextBox _txtBox;
        private Control _winf;

        public TextLogHelper(RichTextBox txtBox,Control winf)
        {
            this._txtBox = txtBox;
            this._winf = winf;
        }

        #region ILogHelper 成员

        private const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss.fff";

        public void Info(string info)
        {
             LogHelper.Info(info);
             SetInfo(info = string.Format("{0} INFO: {1}", DateTime.Now.ToString(DateTimeFormat), info));
           
        }

        public void Debug(string str)
        {
            LogHelper.Debug(str);
            SetInfo(str = string.Format("{0} DEBUG: {1}", DateTime.Now.ToString(DateTimeFormat), str));
            
        }

        public void Warn(string warn)
        {
            LogHelper.Warn(warn);
            SetInfo(warn = string.Format("{0} WARN: {1}", DateTime.Now.ToString(DateTimeFormat), warn));
        }

        public void Error(System.Exception ex)
        {
            string temp = ex.Source + ":" + ex.Message;
            LogHelper.Error(temp);
            SetInfo(temp = string.Format("{0} ERROR: {1}", DateTime.Now.ToString(DateTimeFormat), temp));
        }

        public void Error(string error)
        {
            LogHelper.Error(error);
            SetInfo(error =string.Format("{0} ERROR: {1}", DateTime.Now.ToString(DateTimeFormat), error));
            
        }

        private delegate void SimpleStr(string str );

        private void SetInfo(string info)
        {
           
            if (!_txtBox.IsDisposed)
            {
                if (_winf.InvokeRequired)
                {
                    var method = new Action<string>(SetInfo);
                    if (!_winf.IsDisposed && _winf.IsHandleCreated)
                        _winf.Invoke(method, new object[] {info});
                }
                else
                {
                   
                    _txtBox.AppendText(info + "\n");
                    _txtBox.ScrollToCaret();
                }
            }
        }
        #endregion
    }
}