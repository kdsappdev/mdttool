using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZedGraph;

namespace MDT.Tools.Order.Monitor.Plugin.Data
{
    public interface IPointData
    {
        void clear();
        void addPointByKey(string key, PointPair point);
        PointPairList getPointPairListByKey(string key);
        List<string> getAllKey();
        bool isContain(string key);
    }
}
