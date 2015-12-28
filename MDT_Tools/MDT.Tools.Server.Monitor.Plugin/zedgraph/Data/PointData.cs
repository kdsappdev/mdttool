using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZedGraph;

using MDT.Tools.Server.Monitor.Plugin.ZedGraph;

namespace MDT.Tools.Server.Monitor.Plugin.ZedGraph.Data
{
    public class PointData : IPointData
    {
        private Dictionary<string, PointPairList> dic = new Dictionary<string, PointPairList>();

        public void clear()
        {
            dic.Clear();
        }

        public void addPointByKey(string key, PointPair point)
        {
            lock (dic)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    if (dic.ContainsKey(key))
                        dic[key].Add(point);
                    else
                    {
                        PointPairList pointList = new PointPairList();
                        pointList.Add(point);
                        dic.Add(key, pointList);
                    }
                }
            }
        }

        public PointPairList getPointPairListByKey(string key)
        {
            PointPairList result = null;
            if (!string.IsNullOrEmpty(key))
            {
                if (dic.ContainsKey(key))
                    result = dic[key];
            }

            return result;
        }

        public List<string> getAllKey()
        {
            return dic.Keys.ToList();
        }

        public bool isContain(string key)
        {
            return string.IsNullOrEmpty(key) ? false : dic.ContainsKey(key);
        }
    }
}
