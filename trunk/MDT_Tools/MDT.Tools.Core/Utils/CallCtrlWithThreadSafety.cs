using System.Windows.Forms;
using System.Drawing;
using System;

namespace MDT.Tools.Core.Utils
{
    public class CallCtrlWithThreadSafety
    {


        private delegate void RefreshGridViewDataSourceCallback(DataGridView objCtrl, Control winf);

        public static void RefreshGridViewDataSource(DataGridView objCtrl, Control winf)
        {
            if (objCtrl.InvokeRequired)
            {
                RefreshGridViewDataSourceCallback method = new RefreshGridViewDataSourceCallback(CallCtrlWithThreadSafety.RefreshGridViewDataSource);
                try
                {
                    winf.BeginInvoke(method, new object[] { objCtrl, winf });
                }
                catch
                {
                    try
                    {
                        objCtrl.Refresh();
                    }
                    catch { }
                }
            }
            else
            {
                try
                {
                    objCtrl.Refresh();
                }
                catch (Exception ex)
                {
                    
                   LogHelper.Error(ex);
                }
               
            }
        }


        public static void SetChecked<TObject>(TObject objCtrl, bool isChecked, Control winf) where TObject : CheckBox
        {
            if (objCtrl.InvokeRequired)
            {
                SetCheckedCallback method = new SetCheckedCallback(CallCtrlWithThreadSafety.SetChecked<CheckBox>);
                try
                {
                    objCtrl.Invoke(method, new object[] { objCtrl, isChecked, winf });
                }
                catch
                {
                    try
                    {
                        objCtrl.Checked = isChecked;
                    }
                    catch { }
                }
            }
            else
            {
                objCtrl.Checked = isChecked;
            }
        }

        public static void SetEnable<TObject>(TObject objCtrl, bool enable, Control winf) where TObject : Control
        {
            if (objCtrl.InvokeRequired)
            {
                SetEnableCallback method = new SetEnableCallback(CallCtrlWithThreadSafety.SetEnable<Control>);
                try
                {
                    objCtrl.Invoke(method, new object[] { objCtrl, enable, winf });
                }
                catch
                {
                    try
                    {
                        objCtrl.Enabled = enable;
                    }
                    catch { }
                }
            }
            else
            {
                objCtrl.Enabled = enable;
            }
        }

        public static void SetFocus<TObject>(TObject objCtrl, Control winf) where TObject : Control
        {
            if (objCtrl.InvokeRequired)
            {
                SetFocusCallback method = new SetFocusCallback(CallCtrlWithThreadSafety.SetFocus<Control>);
                try
                {
                    objCtrl.Invoke(method, new object[] { objCtrl, winf });
                }
                catch
                {
                    try
                    {
                        objCtrl.Focus();
                    }
                    catch { }
                }
            }
            else
            {
                objCtrl.Focus();
            }
        }

        public static void SetText<TObject>(TObject objCtrl, string text, Control winf) where TObject : Control
        {
            if (objCtrl.InvokeRequired)
            {
                SetTextCallback method = new SetTextCallback(CallCtrlWithThreadSafety.SetText<Control>);
                try
                {
                    objCtrl.Invoke(method, new object[] { objCtrl, text, winf });
                }
                catch
                {
                    try
                    {
                        objCtrl.Text = text;
                    }
                    catch { }
                }
            }
            else
            {
                objCtrl.Text = text;
            }
        }

        public static void SetVisable<TObject>(TObject objCtrl, bool isVisible, Control winf) where TObject : Control
        {
            if (objCtrl.InvokeRequired)
            {
                SetVisableCallback method = new SetVisableCallback(CallCtrlWithThreadSafety.SetVisable<CheckBox>);
                try
                {
                    objCtrl.Invoke(method, new object[] { objCtrl, isVisible, winf });
                }
                catch
                {
                    try
                    {
                        objCtrl.Visible = isVisible;
                    }
                    catch { }
                }
            }
            else
            {
                objCtrl.Visible = isVisible;
            }
        }

        public static void SetWaitCursor<TObject>(TObject objCtrl, bool enable, Control winf) where TObject : Control
        {
            if (objCtrl.InvokeRequired)
            {
                SetEnableCallback method = new SetEnableCallback(CallCtrlWithThreadSafety.SetWaitCursor<Control>);
                try
                {
                    objCtrl.Invoke(method, new object[] { objCtrl, enable, winf });
                }
                catch
                {
                    try
                    {
                        objCtrl.Cursor = !enable ? Cursors.Default : Cursors.WaitCursor;
                    }
                    catch { }
                }
            }
            else
            {
                objCtrl.Cursor = !enable ? Cursors.Default : Cursors.WaitCursor;
            }
        }

       

        private delegate void SetCheckedCallback(CheckBox objCtrl, bool isCheck, Control winf);

        private delegate void SetCursorCallback(Control objCtrl, bool enable, Control winf);

        private delegate void SetEnableCallback(Control objCtrl, bool enable, Control winf);

        private delegate void SetFocusCallback(Control objCtrl, Control winf);

        private delegate void SetTextCallback(Control objCtrl, string text, Control winf);

        private delegate void SetVisableCallback(CheckBox objCtrl, bool isCheck, Control winf);

        private delegate void SetFontCallback(Control objCtrl, Font font, Control winf);

        public static void SetFont<TObject>(TObject objCtrl, Font font, Control winf) where TObject : Control
        {
            if (objCtrl.InvokeRequired)
            {
                SetVisableCallback method = new SetVisableCallback(CallCtrlWithThreadSafety.SetVisable<CheckBox>);
                try
                {
                    objCtrl.Invoke(method, new object[] { objCtrl, font, winf });
                }
                catch
                {
                    try
                    {
                        objCtrl.Font = font;
                    }
                    catch { }
                }
            }
            else
            {
                objCtrl.Font = font;
            }
        }

        
         
    }
}