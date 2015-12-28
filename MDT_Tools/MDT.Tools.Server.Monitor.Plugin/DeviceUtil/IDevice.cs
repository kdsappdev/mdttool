using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.DirectX.DirectSound;


namespace MDT.Tools.Server.Monitor.Plugin.DeviceUtil
{
    public interface IDevice
    {
        void init(Control c, CooperativeLevel level);
        void addSecondaryBufferAndPlay(string filePath, string name);
        void startSecondaryBufferByName(string name);
        void stopSecondaryBufferByName(string name);
        void clear();
    }
}
