using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Threading;

namespace MDT.Tools.CEDA.Plugin
{
    public class ChartHelper
    {
        public void MinReaderData(string filePath, string message)
        {
            List<string> listTps = new List<string>();
            List<string> listTime = new List<string>();
            List<string> listDataAll = new List<string>();
            List<string> listDataOnly = new List<string>();

            StreamReader sr = new StreamReader(filePath);
            string data = sr.ReadToEnd();
            string[] dataSplit = data.Split('\r');
            sr.Close();
            foreach (var item in dataSplit)
            {
                string itemTrim = item.Trim();
                listDataAll.Add(itemTrim);
            }
            listDataOnly = listDataAll.Distinct().ToList();

            foreach (var itemSplit in listDataOnly)
            {
                string[] itemData = itemSplit.Split(',');
                if (itemData.Count() == 2)
                {
                    string seconds = itemData[0].ToString();
                    listTime.Add(seconds);
                    if (itemData[1].Length >= 1)
                    {
                        string tpsNm = itemData[1].ToString();
                        listTps.Add(tpsNm);
                    }
                }
            }

            Dictionary<string, string> DicAll = new Dictionary<string, string>();
            Dictionary<String, int> dicMin = new Dictionary<String, int>();

            for (int i = 0; i < listTime.Count; i++)
            {
                DicAll.Add(listTime[i], listTps[i]);
            }
            string first = DicAll.Keys.First();
            string last = DicAll.Keys.Last();
            DateTime startMin = DateTime.Parse(first);
            first = startMin.ToString("yyyy/MM/dd HH:mm:00");
            DateTime endMin = DateTime.Parse(last);
            last = endMin.ToString("yyyy/MM/dd HH:mm:00");

            int intTps = 0;
            foreach (var itemAll in DicAll)
            {
                DateTime dtMin = DateTime.Parse(itemAll.Key);
                string StrMin = dtMin.ToString("yyyy/MM/dd HH:mm:00");

                if (first != StrMin && last != StrMin)
                {
                    if (dicMin.ContainsKey(StrMin))
                    {
                        dicMin.Remove(StrMin);
                        dicMin.Add(StrMin, intTps);
                    }
                    else
                    {
                        intTps = 0;
                        dicMin.Add(StrMin, intTps);
                    }
                    intTps = intTps + int.Parse(itemAll.Value);
                }

            }

            List<string> ltMinTime = new List<string>();
            List<string> ltMinTps = new List<string>();
            foreach (var itemMin in dicMin)
            {
                ltMinTime.Add(itemMin.Key);
                ltMinTps.Add(itemMin.Value.ToString());
            }

            if (ltMinTime.Count > 0 && ltMinTps.Count > 0)
            {

                ThreadPool.QueueUserWorkItem(o =>
                {
                    minChartExcel(ltMinTime, ltMinTps, message, "Min");
                });
            }
            else
            {
                MessageBox.Show("统计时间小于一分钟,请使用秒单位生成表格", "警告");
            }

        }

        public void SecReaderData(string filePath, string message)
        {

            List<string> listTps = new List<string>();
            List<string> listTime = new List<string>();
            List<string> listDataAll = new List<string>();
            List<string> listDataOnly = new List<string>();

            StreamReader sr = new StreamReader(filePath);
            string data = sr.ReadToEnd();
            string[] dataSplit = data.Split('\r');
            sr.Close();
            foreach (var item in dataSplit)
            {
                string itemTrim = item.Trim();
                listDataAll.Add(itemTrim);
            }
            listDataOnly = listDataAll.Distinct().ToList();

            foreach (var itemSplit in listDataOnly)
            {
                string[] itemData = itemSplit.Split(',');
                if (itemData.Count() == 2)
                {
                    string seconds = itemData[0].ToString();
                    listTime.Add(seconds);
                    if (itemData[1].Length >= 1)
                    {
                        string tpsNm = itemData[1].ToString();
                        listTps.Add(tpsNm);
                    }
                }
            }

            if (listTime.Count > 0 && listTps.Count > 0)
            {
                ThreadPool.QueueUserWorkItem(o =>
                    {
                        secChartExcel(listTime, listTps, message, "S");
                    });
            }
            else
            {
                MessageBox.Show("时间小于一秒,无法生成", "警告");
            }

        }

        int xy = 0;
        private static readonly string filePath = Application.StartupPath;

        private void secChartExcel(List<string> listTime, List<string> listTps, string message, string TimeType)
        {
            string fileName = "CEDA_Tps" + DateTime.Now.ToString("yyyy_MM_dd_HHmmss") + ".xlsx";
            string reportTitle = "MDT Smart Kit CEDA Tps统计。" + "\n" + message;
            FileInfo file = new FileInfo(filePath + "\\data\\" + fileName);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = null;
                ExcelChartSerie chartSerie = null;
                ExcelChart chart = null;
                worksheet = package.Workbook.Worksheets.Add("Tps_Data");

                int inttime = listTime.Count;
                if (inttime <= 3600)
                {
                    #region research
                    string[] arrayTime = new string[listTime.Count];
                    listTime.CopyTo(arrayTime, 0);
                    string[] arrayTps = new string[listTps.Count];
                    listTps.CopyTo(arrayTps, 0);

                    DataTable dt = GetDatasec(arrayTime, arrayTps, TimeType);
                    chart = worksheet.Drawings.AddChart("ColumnStackedChart", eChartType.ColumnClustered) as ExcelChart;//设置图表样式
                    chart.Legend.Position = eLegendPosition.Right;
                    chart.Legend.Add();

                    string startTime = listTime[0];
                    string endTime = listTime[listTime.Count - 1];
                    DateTime start = DateTime.Parse(startTime);
                    DateTime end = DateTime.Parse(endTime);
                    string sumTime = (end - start).ToString();
                    string timeTitle = ",开始时间:" + startTime + ",结束时间:" + endTime + ",总耗时:" + sumTime;

                    chart.Title.Text = reportTitle + timeTitle;//设置图表的名称
                    chart.Title.Font.Size = 10;
                    chart.SetSize(1300, 600);//设置图表大小
                    chart.ShowHiddenData = true;
                    chart.XAxis.MinorUnit = 1;//设置X轴的最小刻度

                    //设置月份
                    for (int col = 1; col <= dt.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col].Value = dt.Columns[col - 1].ColumnName;
                        worksheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    //设置数据
                    for (int row = 1; row <= dt.Rows.Count; row++)
                    {
                        for (int col = 1; col <= dt.Columns.Count; col++)
                        {
                            string strValue = dt.Rows[row - 1][col - 1].ToString();
                            if (col == 1)
                            {
                                worksheet.Cells[row + 1, col].Value = strValue;
                                worksheet.Cells[row + 1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            else
                            {
                                double realValue = double.Parse(strValue);
                                worksheet.Cells[row + 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                //worksheet.Cells[row + 1, col].Style.Numberformat.Format = "#0\\.00%";//设置数据的格式为百分比
                                worksheet.Cells[row + 1, col].Value = realValue;
                                if (realValue < 1)//如果小于90%则该单元格底色显示为红色
                                {
                                    worksheet.Cells[row + 1, col].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                }
                                else
                                {
                                    worksheet.Cells[row + 1, col].Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);//如果大于95%则该单元格底色显示为绿色
                                }
                            }
                        }

                        //chart.Series.Add()方法所需参数为：chart.Series.Add(X轴数据区,Y轴数据区)
                        chartSerie = chart.Series.Add(worksheet.Cells[row + 1, 2, row + 1, dt.Columns.Count], worksheet.Cells[1, 2, 1, dt.Columns.Count + 1]);
                        chartSerie.HeaderAddress = worksheet.Cells[row + 1, 1];//设置每条线的名称 2 + dt.Columns.Count - 2
                    }

                    chart.YAxis.MinValue = 0;
                    //chart.SetPosition(200, 50);//可以通过制定左上角坐标来设置图表位置
                    //通过指定图表左上角所在的行和列及对应偏移来指定图表位置
                    //这里arrayTps.Length + 1及3分别表示行和列
                    chart.SetPosition(3, 0, 0, 0);
                    #endregion research
                }
                else
                {
                    int Nm = inttime / 3600;
                    for (int i = 0; i < Nm; i++)
                    {
                        #region research
                        xy = 40 * i;

                        List<string> ltTime = new List<string>();
                        List<string> ltTps = new List<string>();
                        for (int x = 0; x < 3600; x++)
                        {
                            ltTime.Add(listTime[x]);
                        }
                        listTime.RemoveRange(0, 3600);

                        for (int y = 0; y < 3600; y++)
                        {
                            ltTps.Add(listTps[y]);
                        }
                        listTps.RemoveRange(0, 3600);


                        string[] arrayTime = new string[ltTime.Count];
                        ltTime.CopyTo(arrayTime, 0);
                        string[] arrayTps = new string[ltTps.Count];
                        ltTps.CopyTo(arrayTps, 0);

                        string startTime = ltTime[0];
                        string endTime = ltTime[ltTime.Count - 1];
                        DateTime start = DateTime.Parse(startTime);
                        DateTime end = DateTime.Parse(endTime);
                        string sumTime = (end - start).ToString();
                        string timeTitle = ",开始时间:" + startTime + ",结束时间:" + endTime + ",总耗时:" + sumTime;


                        DataTable dt = GetDatasec(arrayTime, arrayTps, TimeType);
                        chart = worksheet.Drawings.AddChart("ColumnStackedChart" + xy.ToString(), eChartType.ColumnClustered) as ExcelChart;//设置图表样式

                        chart.Legend.Position = eLegendPosition.Right;
                        chart.Legend.Add();
                        chart.Title.Text = reportTitle + timeTitle;//设置图表的名称
                        chart.Title.Font.Size = 10;
                        chart.ShowHiddenData = true;
                        //chart.YAxis.MinorUnit = 1;
                        chart.XAxis.MinorUnit = 1;//设置X轴的最小刻度
                        //设置月份
                        for (int col = 1; col <= dt.Columns.Count; col++)
                        {
                            worksheet.Cells[1 + xy, col].Value = dt.Columns[col - 1].ColumnName;
                            worksheet.Cells[1 + xy, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        //设置数据
                        //for (int row = 1; row <= dt.Rows.Count; row++)
                        //{
                        for (int col = 1; col <= dt.Columns.Count; col++)
                        {
                            string strValue = dt.Rows[0][col - 1].ToString();
                            if (col == 1)
                            {
                                worksheet.Cells[2 + xy, col].Value = strValue;
                                worksheet.Cells[2 + xy, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            else
                            {
                                double realValue = double.Parse(strValue);
                                worksheet.Cells[2 + xy, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                //worksheet.Cells[row + 1, col].Style.Numberformat.Format = "#0\\.00%";//设置数据的格式为百分比
                                worksheet.Cells[2 + xy, col].Value = realValue;
                                if (realValue < 1)//如果小于90%则该单元格底色显示为红色
                                {
                                    worksheet.Cells[2 + xy, col].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                }
                                else
                                {
                                    worksheet.Cells[2 + xy, col].Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);//如果大于95%则该单元格底色显示为绿色
                                }
                            }
                            //chartSerie = chart.Series.Add(worksheet.Cells["A2:M2"], worksheet.Cells["B1:M1"]);
                            //chartSerie.HeaderAddress = worksheet.Cells["A2"];
                            //chart.Series.Add()方法所需参数为：chart.Series.Add(X轴数据区,Y轴数据区)

                        }
                        chartSerie = chart.Series.Add(worksheet.Cells[2 + xy, 2, 2 + xy, dt.Columns.Count], worksheet.Cells[1 + xy, 2, 1 + xy, dt.Columns.Count + 1]);
                        chartSerie.HeaderAddress = worksheet.Cells[2 + xy, 1];//设置每条线的名称 2 + dt.Columns.Count - 2

                        //因为假定每家公司至少完成了80%以上，所以这里设置Y轴的最小刻度为80%，这样使图表上的折线更清晰
                        chart.YAxis.MinValue = 0;
                        chart.SetPosition(4 + xy, 0, 0, 0);
                        chart.SetSize(1300, 600);//设置图表大小
                        #endregion research

                    }


                    if (listTime.Count > 0 && listTps.Count > 0)
                    {

                        xy = xy + 40;
                        string[] arrTime = new string[listTime.Count];
                        listTime.CopyTo(arrTime, 0);
                        string[] arrTps = new string[listTps.Count];
                        listTps.CopyTo(arrTps, 0);

                        string startTime2 = listTime[0];
                        string endTime2 = listTime[listTime.Count - 1];

                        DateTime start2 = DateTime.Parse(startTime2);
                        DateTime end2 = DateTime.Parse(endTime2);
                        string sumTime2 = (end2 - start2).ToString();

                        string timeTitle2 = ",开始时间:" + startTime2 + ",结束时间:" + endTime2 + ",总耗时:" + sumTime2;
                        if (arrTime.Length > 0 && arrTps.Length > 0)
                        {

                            DataTable dtSur = GetDatasec(arrTime, arrTps, TimeType);
                            chart = worksheet.Drawings.AddChart("ColumnStackedChart" + xy.ToString(), eChartType.ColumnClustered) as ExcelChart;//设置图表样式

                            chart.Legend.Position = eLegendPosition.Right;
                            chart.Legend.Add();

                            chart.Title.Text = reportTitle + timeTitle2;//设置图表的名称
                            chart.Title.Font.Size = 10;

                            chart.ShowHiddenData = true;
                            //chart.YAxis.MinorUnit = 1;
                            chart.XAxis.MinorUnit = 1;//设置X轴的最小刻度

                            //chart.DataLabel.ShowCategory = true;
                            //chart.DataLabel.ShowPercent = true;//显示百分比
                            //设置月份
                            for (int col = 1; col <= dtSur.Columns.Count; col++)
                            {
                                worksheet.Cells[1 + xy, col].Value = dtSur.Columns[col - 1].ColumnName;
                                worksheet.Cells[1 + xy, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }

                            for (int col = 1; col <= dtSur.Columns.Count; col++)
                            {
                                string strValue = dtSur.Rows[0][col - 1].ToString();
                                if (col == 1)
                                {
                                    worksheet.Cells[2 + xy, col].Value = strValue;
                                    worksheet.Cells[2 + xy, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                }
                                else
                                {
                                    double realValue = double.Parse(strValue);
                                    worksheet.Cells[2 + xy, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    //worksheet.Cells[row + 1, col].Style.Numberformat.Format = "#0\\.00%";//设置数据的格式为百分比
                                    worksheet.Cells[2 + xy, col].Value = realValue;
                                    if (realValue < 1)//如果小于90%则该单元格底色显示为红色
                                    {
                                        worksheet.Cells[2 + xy, col].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    }
                                    else
                                    {
                                        worksheet.Cells[2 + xy, col].Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);//如果大于95%则该单元格底色显示为绿色
                                    }
                                }
                                //}
                                //chartSerie = chart.Series.Add(worksheet.Cells["A2:M2"], worksheet.Cells["B1:M1"]);
                                //chartSerie.HeaderAddress = worksheet.Cells["A2"];
                                //chart.Series.Add()方法所需参数为：chart.Series.Add(X轴数据区,Y轴数据区)

                            }
                            chartSerie = chart.Series.Add(worksheet.Cells[2 + xy, 2, 2 + xy, dtSur.Columns.Count], worksheet.Cells[1 + xy, 2, 1 + xy, dtSur.Columns.Count + 1]);
                            chartSerie.HeaderAddress = worksheet.Cells[2 + xy, 1];//设置每条线的名称 2 + dt.Columns.Count - 2

                            //因为假定每家公司至少完成了80%以上，所以这里设置Y轴的最小刻度为80%，这样使图表上的折线更清晰
                            chart.YAxis.MinValue = 0;
                            //chart.SetPosition(200, 50);//可以通过制定左上角坐标来设置图表位置
                            //通过指定图表左上角所在的行和列及对应偏移来指定图表位置
                            //这里arrayTps.Length + 1及3分别表示行和列
                            chart.SetPosition(3 + xy, 0, 0, 0);
                            chart.SetSize(1300, 600);//设置图表大小

                        }
                    }
                }

                package.Save();//保存文件
                OpenFile(file.ToString());

            }
        }

        private void minChartExcel(List<string> listTime, List<string> listTps, string message, string TimeType)
        {

            string fileName = "CEDA_Tps" + DateTime.Now.ToString("yyyy_MM_dd_HHmmss") + ".xlsx";
            string reportTitle = "MDT Smart Kit CEDA Tps统计。" + "\n" + message;
            FileInfo file = new FileInfo(filePath + "\\data\\" + fileName);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = null;
                ExcelChartSerie chartSerie = null;
                ExcelChart chart = null;

                worksheet = package.Workbook.Worksheets.Add("Tps_Data");

                int inttime = listTime.Count;
                if (inttime <= 720)
                {
                    #region research
                    string[] arrayTime = new string[listTime.Count];
                    listTime.CopyTo(arrayTime, 0);
                    string[] arrayTps = new string[listTps.Count];
                    listTps.CopyTo(arrayTps, 0);

                    DataTable dt = GetDatasec(arrayTime, arrayTps, TimeType);
                    //chart = Worksheet.Drawings.AddChart("ColumnStackedChart", eChartType.Line) as ExcelLineChart;
                    chart = worksheet.Drawings.AddChart("ColumnStackedChart", eChartType.ColumnClustered) as ExcelChart;//设置图表样式
                    chart.Legend.Position = eLegendPosition.Right;
                    chart.Legend.Add();

                    string startTime = listTime[0];
                    string endTime = listTime[listTime.Count - 1];
                    DateTime start = DateTime.Parse(startTime);
                    DateTime end = DateTime.Parse(endTime);
                    string sumTime = (end - start).ToString();
                    string timeTitle = ",开始时间:" + startTime + ",结束时间:" + endTime + ",总耗时:" + sumTime;

                    chart.Title.Text = reportTitle + timeTitle;//设置图表的名称
                    chart.Title.Font.Size = 10;

                    //chart.SetPosition(200, 50);//设置图表位置
                    chart.SetSize(1300, 600);//设置图表大小
                    chart.ShowHiddenData = true;
                    chart.XAxis.MinorUnit = 1;//设置X轴的最小刻度

                    //设置月份
                    for (int col = 1; col <= dt.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col].Value = dt.Columns[col - 1].ColumnName;
                        worksheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    //设置数据
                    for (int row = 1; row <= dt.Rows.Count; row++)
                    {
                        for (int col = 1; col <= dt.Columns.Count; col++)
                        {
                            string strValue = dt.Rows[row - 1][col - 1].ToString();
                            if (col == 1)
                            {
                                worksheet.Cells[row + 1, col].Value = strValue;
                                worksheet.Cells[row + 1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            else
                            {
                                double realValue = double.Parse(strValue);
                                worksheet.Cells[row + 1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                //worksheet.Cells[row + 1, col].Style.Numberformat.Format = "#0\\.00%";//设置数据的格式为百分比
                                worksheet.Cells[row + 1, col].Value = realValue;
                                if (realValue < 1)//如果小于90%则该单元格底色显示为红色
                                {
                                    worksheet.Cells[row + 1, col].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                }
                                else
                                {
                                    worksheet.Cells[row + 1, col].Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);//如果大于95%则该单元格底色显示为绿色
                                }
                            }
                        }

                        //chart.Series.Add()方法所需参数为：chart.Series.Add(X轴数据区,Y轴数据区)
                        chartSerie = chart.Series.Add(worksheet.Cells[row + 1, 2, row + 1, dt.Columns.Count], worksheet.Cells[1, 2, 1, dt.Columns.Count + 1]);
                        chartSerie.HeaderAddress = worksheet.Cells[row + 1, 1];//设置每条线的名称 2 + dt.Columns.Count - 2
                    }
                    //因为假定每家公司至少完成了80%以上，所以这里设置Y轴的最小刻度为80%，这样使图表上的折线更清晰
                    chart.YAxis.MinValue = 0;
                    chart.SetPosition(3, 0, 0, 0);
                    #endregion research
                }
                else
                {
                    int Nm = inttime / 720;
                    for (int i = 0; i < Nm; i++)
                    {
                        #region research
                        xy = 40 * i;

                        List<string> ltTime = new List<string>();
                        List<string> ltTps = new List<string>();
                        for (int x = 0; x < 720; x++)
                        {
                            ltTime.Add(listTime[x]);
                        }
                        listTime.RemoveRange(0, 720);

                        for (int y = 0; y < 720; y++)
                        {
                            ltTps.Add(listTps[y]);
                        }
                        listTps.RemoveRange(0, 720);
                        string[] arrayTime = new string[ltTime.Count];
                        ltTime.CopyTo(arrayTime, 0);
                        string[] arrayTps = new string[ltTps.Count];
                        ltTps.CopyTo(arrayTps, 0);
                        string startTime = ltTime[0];
                        string endTime = ltTime[ltTime.Count - 1];
                        DateTime start = DateTime.Parse(startTime);
                        DateTime end = DateTime.Parse(endTime);
                        string sumTime = (end - start).ToString();
                        string timeTitle = ",开始时间:" + startTime + ",结束时间:" + endTime + ",总耗时:" + sumTime;

                        DataTable dt = GetDatasec(arrayTime, arrayTps, TimeType);
                        //chart = Worksheet.Drawings.AddChart("ColumnStackedChart", eChartType.Line) as ExcelLineChart;
                        chart = worksheet.Drawings.AddChart("ColumnStackedChart" + xy.ToString(), eChartType.ColumnClustered) as ExcelChart;//设置图表样式

                        chart.Legend.Position = eLegendPosition.Right;
                        chart.Legend.Add();
                        chart.Title.Text = reportTitle + timeTitle;//设置图表的名称
                        chart.Title.Font.Size = 10;
                        chart.ShowHiddenData = true;
                        //chart.YAxis.MinorUnit = 1;
                        chart.XAxis.MinorUnit = 1;//设置X轴的最小刻度
                        //设置月份
                        for (int col = 1; col <= dt.Columns.Count; col++)
                        {
                            worksheet.Cells[1 + xy, col].Value = dt.Columns[col - 1].ColumnName;
                            worksheet.Cells[1 + xy, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        //设置数据
                        //for (int row = 1; row <= dt.Rows.Count; row++)
                        //{
                        for (int col = 1; col <= dt.Columns.Count; col++)
                        {
                            string strValue = dt.Rows[0][col - 1].ToString();
                            if (col == 1)
                            {
                                worksheet.Cells[2 + xy, col].Value = strValue;
                                worksheet.Cells[2 + xy, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                            else
                            {
                                double realValue = double.Parse(strValue);
                                worksheet.Cells[2 + xy, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                //worksheet.Cells[row + 1, col].Style.Numberformat.Format = "#0\\.00%";//设置数据的格式为百分比
                                worksheet.Cells[2 + xy, col].Value = realValue;
                                if (realValue < 1)//如果小于90%则该单元格底色显示为红色
                                {
                                    worksheet.Cells[2 + xy, col].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                }
                                else
                                {
                                    worksheet.Cells[2 + xy, col].Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);//如果大于95%则该单元格底色显示为绿色
                                }
                            }

                        }
                        chartSerie = chart.Series.Add(worksheet.Cells[2 + xy, 2, 2 + xy, dt.Columns.Count], worksheet.Cells[1 + xy, 2, 1 + xy, dt.Columns.Count + 1]);
                        chartSerie.HeaderAddress = worksheet.Cells[2 + xy, 1];//设置每条线的名称 2 + dt.Columns.Count - 2

                        chart.YAxis.MinValue = 0;
                        chart.SetPosition(4 + xy, 0, 0, 0);
                        chart.SetSize(1300, 600);//设置图表大小
                        #endregion research

                    }


                    if (listTime.Count > 0 && listTps.Count > 0)
                    {
                        xy = xy + 40;

                        string[] arrTime = new string[listTime.Count];
                        listTime.CopyTo(arrTime, 0);

                        string[] arrTps = new string[listTps.Count];
                        listTps.CopyTo(arrTps, 0);
                        string startTime2 = listTime[0];
                        string endTime2 = listTime[listTime.Count - 1];
                        DateTime start2 = DateTime.Parse(startTime2);
                        DateTime end2 = DateTime.Parse(endTime2);
                        string sumTime2 = (end2 - start2).ToString();

                        string timeTitle2 = ",开始时间:" + startTime2 + ",结束时间:" + endTime2 + ",总耗时:" + sumTime2;
                        if (arrTime.Length > 0 && arrTps.Length > 0)
                        {
                            DataTable dtSur = GetDatasec(arrTime, arrTps, TimeType);
                            chart = worksheet.Drawings.AddChart("ColumnStackedChart" + xy.ToString(), eChartType.ColumnClustered) as ExcelChart;//设置图表样式

                            chart.Legend.Position = eLegendPosition.Right;
                            chart.Legend.Add();

                            chart.Title.Text = reportTitle + timeTitle2;//设置图表的名称
                            chart.Title.Font.Size = 10;

                            chart.ShowHiddenData = true;
                            //chart.YAxis.MinorUnit = 1;
                            chart.XAxis.MinorUnit = 1;//设置X轴的最小刻度
                            //设置月份
                            for (int col = 1; col <= dtSur.Columns.Count; col++)
                            {
                                worksheet.Cells[1 + xy, col].Value = dtSur.Columns[col - 1].ColumnName;
                                worksheet.Cells[1 + xy, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }

                            for (int col = 1; col <= dtSur.Columns.Count; col++)
                            {
                                string strValue = dtSur.Rows[0][col - 1].ToString();
                                if (col == 1)
                                {
                                    worksheet.Cells[2 + xy, col].Value = strValue;
                                    worksheet.Cells[2 + xy, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                }
                                else
                                {
                                    double realValue = double.Parse(strValue);
                                    worksheet.Cells[2 + xy, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    //worksheet.Cells[row + 1, col].Style.Numberformat.Format = "#0\\.00%";//设置数据的格式为百分比
                                    worksheet.Cells[2 + xy, col].Value = realValue;
                                    if (realValue < 1)//如果小于90%则该单元格底色显示为红色
                                    {
                                        worksheet.Cells[2 + xy, col].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    }
                                    else
                                    {
                                        worksheet.Cells[2 + xy, col].Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);//如果大于95%则该单元格底色显示为绿色
                                    }
                                }

                            }
                            chartSerie = chart.Series.Add(worksheet.Cells[2 + xy, 2, 2 + xy, dtSur.Columns.Count], worksheet.Cells[1 + xy, 2, 1 + xy, dtSur.Columns.Count + 1]);
                            chartSerie.HeaderAddress = worksheet.Cells[2 + xy, 1];
                            chart.YAxis.MinValue = 0;
                            chart.SetPosition(3 + xy, 0, 0, 0);
                            chart.SetSize(1300, 600);//设置图表大小

                        }
                    }
                }


                package.Save();//保存文件
                OpenFile(file.ToString());

            }
        }

        private static DataTable GetDatasec(string[] arrayTime, string[] arrayTps, string timeType)
        {
            List<string> ltTime = arrayTime.Distinct().ToList();
            DataTable data = new DataTable();
            DataRow row = null;
            data.Columns.Add(new DataColumn("用时/" + timeType + ":", typeof(string)));
            foreach (string monthName in ltTime)
            {
                data.Columns.Add(new DataColumn(monthName, typeof(double)));
            }

            row = data.NewRow();
            row[0] = "Tps/" + timeType + ":";
            for (int i = 0; i < ltTime.Count; i++)
            {
                row[i + 1] = arrayTps[i];
            }
            data.Rows.Add(row);
            return data;
        }

        public static void OpenFile(string fileName)
        {
            String caption = "打开";
            String title = "是否打开该文件?";

            if (MessageBox.Show(title, caption, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    MessageBox.Show("找不到合适的应用程序来打开该文件。", System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
                }
            }
        }


    }
}
