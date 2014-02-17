using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Core.Log
{
    public class Log4netLog : ILog
    {
        private Type _currentType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        private log4net.ILog _log = null;
        public Log4netLog()
        {
            _log = log4net.LogManager.GetLogger(_currentType);
            log4net.Config.XmlConfigurator.Configure(
                 new FileInfo(@"control/log4netConfig.config"));
        }

        public void Debug(string str)
        {
            _log.Debug(str);
        }

        public void Warn(string str)
        {
            _log.Warn(str);
        }

        public void Error(string str)
        {
            _log.Error(str);
        }

        public void Error(Exception ex)
        {
            _log.Error(ex);
        }
    }
}
