using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.DirectX.DirectSound;

namespace MDT.Tools.PFS.Monitor.Plugin.DeviceUtil
{
    public class DeviceImpl : IDevice
    {
        private Device device = new Device();
        private Dictionary<string, SecondaryBuffer> dic = new Dictionary<string, SecondaryBuffer>();

        public void init(Control c, CooperativeLevel level)
        {
            dic.Clear();
            device.SetCooperativeLevel(c, level);
        }

        public void addSecondaryBufferAndPlay(string filePath, string name)
        {
            if (!dic.ContainsKey(name))
            {
                Microsoft.DirectX.DirectSound.BufferDescription buffDes =
                    new Microsoft.DirectX.DirectSound.BufferDescription();
                buffDes.GlobalFocus = true; //设置缓冲区全局获取焦点
                buffDes.ControlVolume = true; //指明缓冲区可以控制声音
                buffDes.ControlPan = true; //指明缓冲区可以控制声道平衡
                SecondaryBuffer sb = new SecondaryBuffer(filePath, buffDes, device);
                dic.Add(name, sb);
                sb.Play(0, BufferPlayFlags.Looping);
            }
        }

        public void startSecondaryBufferByName(string name)
        {
            if (dic.ContainsKey(name))
                dic[name].Play(0, BufferPlayFlags.Looping);
        }

        public void stopSecondaryBufferByName(string name)
        {
            if (dic.ContainsKey(name))
                dic[name].Stop();
        }

        public void clear()
        {
            foreach (KeyValuePair<string, SecondaryBuffer> kv in dic)
            {
                kv.Value.Dispose();
            }

            device.Dispose();
            dic.Clear();
        }
    }
}
