using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZedGraph;

using MDT.Tools.Server.Monitor.Plugin.ZedGraph;

namespace MDT.Tools.Server.Monitor.Plugin.ZedGraph.Data
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
