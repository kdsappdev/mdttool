using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using MDT.Tools.Aliyun.Monitor.Plugin.Model;
using System.Data;
using MDT.Tools.Aliyun.Monitor.Plugin.Utils;
using MDT.Tools.Aliyun.Monitor.Plugin.Quartz;
using MDT.Tools.Core.Utils;

namespace MDT.Tools.Aliyun.Monitor.Plugin.Monitor
{
    public class FilesHelper : IFilesHelper
    {
        private List<MFileInfo> FList = new List<MFileInfo>();
        private SqlLiteHelper sqlLiteHelper = new SqlLiteHelper();
        private DataTable loadData = new DataTable();

        #region load
        public DataTable load(string mNmae, string bName)
        {
            //SQLiteParameter[] param = null;
            //string sql = "select SeqNo,FileName, Size, LastModified, MonitorName, BucketName,Status from Monitor where  Status ='Y' and  MonitorName='" + mNmae + "' and BucketName ='" + bName + "'";
            //loadData = sqlLiteHelper.ExecuteDataTable(sql, param);


            if (loadData.Rows.Count == 0)
            {
                DataColumn dc1 = new DataColumn("SeqNo", Type.GetType("System.Int64"));
                DataColumn dc2 = new DataColumn("FileName", Type.GetType("System.String"));
                DataColumn dc3 = new DataColumn("Size", Type.GetType("System.String"));
                DataColumn dc4 = new DataColumn("LastModified", Type.GetType("System.DateTime"));
                DataColumn dc5 = new DataColumn("MonitorName", Type.GetType("System.String"));
                DataColumn dc6 = new DataColumn("BucketName", Type.GetType("System.String"));
                DataColumn dc7 = new DataColumn("Status", Type.GetType("System.String"));
                loadData.Columns.Add(dc1);
                loadData.Columns.Add(dc2);
                loadData.Columns.Add(dc3);
                loadData.Columns.Add(dc4);
                loadData.Columns.Add(dc5);
                loadData.Columns.Add(dc6);
                loadData.Columns.Add(dc7);
            }

            return GetNewDataTable(loadData);
        }
        #endregion

        #region  DataTable copy
        /// <summary>
        /// 执行DataTable中的查询返回新的DataTable
        /// </summary>
        /// <param name="dt">源数据DataTable</param>
        /// <returns></returns>
        public DataTable GetNewDataTable(DataTable dt)
        {

            DataTable newdt = new DataTable();
            try
            {
                newdt = dt.Clone();
                DataRow[] dr = dt.Select("Status='Y'", "LastModified desc");
                for (int i = 0; i < dr.Length; i++)
                {
                    newdt.ImportRow((DataRow)dr[i]);
                }
                return newdt;//返回的查询结果
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return newdt;
            }
        }
        #endregion 

        #region 判断DataTale中判断某个字段中包含某个数据
        /// <summary>
        /// 判断DataTale中判断某个字段中包含某个数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public  Boolean IsColumnIncludeData(DataTable dt,string FileName, DateTime LastModified,string Size)
        {
            if(dt==null || dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                DataRow[] dataRows = dt.Select("FileName='" + FileName + "'");
                 if (dataRows.Length.Equals(1))
                 {
                     try
                     {
                         string status = dataRows[0]["Status"].ToString();
                         if (status == "Y")
                         {
                            // string sql = "update  Monitor set LastModified =@LastModified,Size=@Size where SeqNo=" + dataRows[0]["seqNo"];
                             //SQLiteParameter[] sqllite = new SQLiteParameter[]{
                             // CreateParameter("@LastModified",DbType.DateTime,LastModified),
                             // CreateParameter("@Size",DbType.String,Size) };
                             //int i = sqlLiteHelper.ExecuteNonQuery(sql, sqllite);
                             //if (i > 0)
                             //{
                                 dataRows[0]["LastModified"] = LastModified;
                                 dataRows[0]["Size"] = Size;
                             //}
                         }
                     }
                     catch (Exception e)
                     {
                         LogHelper.Error(e.Message);
                     }
                     return true;
                 }
                 else
                 {
                     return false;
                 }
            }

        }
        #endregion 


        #region selectParam
        public DataTable selectParam(string bName, string mNmae, string FileName)
        {
            SQLiteParameter[] param = null;
            DataTable dt = new DataTable();
            string sql = "select SeqNo,FileName, Size, LastModified, MonitorName, BucketName,Status from Monitor where  BucketName ='" + bName + "' ";
            if (!string.IsNullOrEmpty(mNmae))
            {
                sql += " and MonitorName='" + mNmae + "'";
            }
            if (!string.IsNullOrEmpty(FileName))
            {
                sql += " and FileName='" + FileName + "'";
            }
            sql += "order by LastModified desc";
            dt = sqlLiteHelper.ExecuteDataTable(sql, param);

            return dt;
        }
        #endregion

        public DataTable select()
        {
            return GetNewDataTable(loadData);
        }

        #region insert
        public int insert(MFileInfo fi)
        {
            int i = 0;
            lock (fi)
            {
                  DataTable dt =  GetNewDataTable(loadData);
                  if (!IsColumnIncludeData(dt, fi.FileName, fi.LastModified, fi.Size))
                  {
                   

                      //string sql = "insert into Monitor (FileName, Size, LastModified, MonitorName, BucketName,Status) values(@FileName,@Size,@LastModified, @MonitorName,@BucketName,@Status)";
                      //SQLiteParameter[] sqllite = new SQLiteParameter[]{
                      //          CreateParameter("@FileName",DbType.String,fi.FileName),
                      //          CreateParameter("@Size",DbType.String,fi.Size),
                      //          CreateParameter("@LastModified",DbType.DateTime,fi.LastModified),
                      //          CreateParameter("@MonitorName",DbType.String,fi.MonitorName),
                      //          CreateParameter("@BucketName",DbType.String, fi.BucketName),
                      //          CreateParameter("@Status",DbType.String, "Y")
                      //          };

                      //i = sqlLiteHelper.ExecuteNonQuery(sql, sqllite);
                      //if (i > 0)
                      //{

                      //    string s1 = queryMaxId("SeqNo", fi.MonitorName, fi.BucketName);
                      //    if (!string.IsNullOrEmpty(s1))
                      //    {
                              loadData.Rows.Add(new object[] { fi.SeqNo, fi.FileName, fi.Size, fi.LastModified, fi.MonitorName, fi.BucketName, "Y" });

                          //}
                      //}
                  }
               
            }
            return i;
        }
        #endregion

        public int update(string seqNo)
        {
            string sql = "update  Monitor set Status ='N' where SeqNo="+seqNo;
             SQLiteParameter[] sqllite = new SQLiteParameter[]{};
            int i= sqlLiteHelper.ExecuteNonQuery(sql, sqllite);
            if (i > 0)
            { 
               DataRow[] dataRows= loadData.Select("SeqNo ="+seqNo+"");
               if (dataRows.Count() > 0)
               {
                   dataRows[0]["Status"] = "N";
               }
            }
            return i;

        }

        #region  查询最大 Id
        public string queryMaxId(string Column,string MonitorName,string BucketName)
        {
            SQLiteParameter[] param = null;
            string seqNo = null;
            string sql = "SELECT   MAX(" + Column + ") AS seqNo FROM  Monitor where Status ='Y'   and MonitorName ='" + MonitorName + "' and BucketName ='" + BucketName + "'";
            DataTable dt = sqlLiteHelper.ExecuteDataTable(sql, param);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                seqNo = dt.Rows[i]["seqNo"].ToString();
            }
            return seqNo;

        }
        #endregion

        #region CreateParameter(parameterName,parameterType,parameterValue)
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="parameterType">参数类型</param>
        /// <param name="parameterValue">参数值</param>
        /// <returns>返回创建的参数</returns>
        public static SQLiteParameter CreateParameter(string parameterName, DbType parameterType, object parameterValue)
        {
            SQLiteParameter parameter = new SQLiteParameter();
            parameter.DbType = parameterType;
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            return parameter;
        }
        #endregion

        public void init()
        {
            WriteLogScheduler.init();
        }


        public void Start()
        {
            if (!WriteLogScheduler.IsStart())
            {
                WriteLogScheduler.Start();
            }
        }

        public void Stop()
        {
            if (WriteLogScheduler.IsStart())
            {
                WriteLogScheduler.Stop();
            }
        }
        public void Add(string VKInterval, MonitorUI mu, string jobName)
        {
            WriteLogScheduler.Add(VKInterval, mu, jobName);
        }

        public void AddDayTiming(string DayTiming, MonitorUI mu, string jobName)
        {
            WriteLogScheduler.AddDayTiming(DayTiming, mu, jobName);
        }

        public void deleteJob(string jobName)
        {
            WriteLogScheduler.deleteJob(jobName, jobName);
        }

    }
}
