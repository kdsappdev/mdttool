using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDT.Tools.Server.Monitor.Plugin.Model
{
    /// <summary>
    /// ATS_MONITOR_LOG
    /// </summary>
    /// <remarks>
    /// 2015.07.09: 创建. Csharp代码生成插件(V1.0.0.1) 模板版本(v1.0.0.0)<br/>
    /// </remarks>
    [Serializable]
    public class TMonitorLog
    {
        #region 字段

        private string _mKey;
        private decimal? _mValue;
        private string _mWarnlevel;
        private string _mType;
        private string _mDesc;
        private string _mUpdatetime;

        #endregion

        #region 属性

        /// <summary>
        /// M_KEY
        /// </summary>
        public string MKey
        {
            get { return _mKey; }
            set { _mKey = value; }
        }

        /// <summary>
        /// M_VALUE
        /// </summary>
        public decimal? MValue
        {
            get { return _mValue; }
            set { _mValue = value; }
        }

        /// <summary>
        /// M_WARNLEVEL
        /// </summary>
        public string MWarnlevel
        {
            get { return _mWarnlevel; }
            set { _mWarnlevel = value; }
        }

        /// <summary>
        /// M_TYPE
        /// </summary>
        public string MType
        {
            get { return _mType; }
            set { _mType = value; }
        }

        /// <summary>
        /// M_DESC
        /// </summary>
        public string MDesc
        {
            get { return _mDesc; }
            set { _mDesc = value; }
        }

        /// <summary>
        /// M_UPDATETIME
        /// </summary>
        public string MUpdatetime
        {
            get { return _mUpdatetime; }
            set { _mUpdatetime = value; }
        }


        #endregion

    }
}
