using System;
using System.Windows.Forms;
using MDT.Tools.Core.Log;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Order.Monitor.Plugin
{
    public class TextLogHelper : ILog
    {
        private Label _txtBox;
        private Control _winf;
        private string ip = "";
        private int port = 0;

        public TextLogHelper(Label txtBox, Control winf, string _ip, int _port)
        {
            this._txtBox = txtBox;
            this._winf = winf;
            ip = _ip;
            port = _port;
        }

        #region ILogHelper 成员

        private const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";

        public void Info(string info)
        {
            SetInfo(string.Format("地址：{2}:{3}  {0}  {1}", DateTime.Now.ToString(DateTimeFormat), info, ip, port));
            LogHelper.Info(string.Format("地址：{2}:{3}  {0}  {1}", DateTime.Now.ToString(DateTimeFormat), info, ip, port));
        }

        public void Debug(string str)
        {
            SetInfo(string.Format("地址：{2}:{3}  {0}  {1}", DateTime.Now.ToString(DateTimeFormat), str, ip, port));
            LogHelper.Debug(string.Format("地址：{2}:{3}  {0}  {1}", DateTime.Now.ToString(DateTimeFormat), str, ip, port));
        }

        public void Warn(string warn)
        {
            SetInfo(string.Format("地址：{2}:{3}  {0}  {1}", DateTime.Now.ToString(DateTimeFormat), warn, ip, port));
            LogHelper.Warn(string.Format("地址：{2}:{3}  {0}  {1}", DateTime.Now.ToString(DateTimeFormat), warn, ip, port));
        }

        public void Error(System.Exception ex)
        {
            string temp = ex.Source + ":" + ex.Message;
            SetInfo(string.Format("{0} ERROR: {1}", DateTime.Now.ToString(DateTimeFormat), temp));
            LogHelper.Error(string.Format("地址：{2}:{3}  {0}  {1}", DateTime.Now.ToString(DateTimeFormat), temp, ip, port));

        }

        public void Error(string error)
        {
            SetInfo(string.Format("{0} ERROR: {1}", DateTime.Now.ToString(DateTimeFormat), error));
            LogHelper.Error(string.Format("地址：{2}:{3}  {0}  {1}", DateTime.Now.ToString(DateTimeFormat), error, ip, port));
        }

        private delegate void SimpleStr(string str);

        private void SetInfo(string info)
        {

            if (!_txtBox.IsDisposed)
            {
                if (_winf.InvokeRequired)
                {
                    var method = new Action<string>(SetInfo);
                    if (!_winf.IsDisposed && _winf.IsHandleCreated)
                        _winf.Invoke(method, new object[] { info });
                }
                else
                {
                    _txtBox.Text = info;
                }
            }
        }
        #endregion
    }
}