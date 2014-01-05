﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MDT.Tools.Core.Log
{
    interface ILog
    {
        void Debug(string str);
        void Warn(string str);
        void Error(Exception ex);
    }

 
}
