using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; //需要添加的命名空间
using System.IO;
using System.Threading;

namespace EEG_Data_Logger
{
    public partial class EEG : Form
    {
        static double[] data_af3 = new double[10000];
        static double[] data_f7 = new double[10000];

        static int count = 0;
        static int count0 = 0;
        static int flag = 0;

        //static string str = "";     
        //static int flag = 0;
        //# region 打开csv文件
        //static string fileName = @"E:\Microsoft\Projects\学习训练\community-sdk-master-9.17\bin\win32\EEG_Data_Logger.csv";
        //static FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
        //static StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));
        //# endregion
        public EEG()
        {
            InitializeComponent();
            InitChart();
        }

        private void EEG_Load(object sender, EventArgs e)
        {

        }
        private void InitChart()
        {
            # region 初始化chart
            DateTime time = DateTime.Now;

            data_af3[0] = 0;
            data_f7[0] = 0;
             
            chartTimer.Tick += chartTimer_Tick;
            chartDemo.DoubleClick += chartDemo_DoubleClick;

            Series series_af3 = chartDemo.Series[0];
            Series series_f7 = chartDemo.Series[1];

            series_af3.ChartType = SeriesChartType.Spline;
            series_f7.ChartType = SeriesChartType.Spline;

            //chartDemo.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";//x轴显示时间格式
            chartDemo.ChartAreas[0].AxisX.ScaleView.Size = 5;
            chartDemo.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chartDemo.ChartAreas[0].AxisX.ScrollBar.Enabled = true;            
            label1.Text = "中";
                      

            # endregion
        }
        //
        //滑动条可以隐藏，隐藏之后不知道怎么恢复，
        //所以就编写了这个双击事件，以恢复滑动条
        //
        void chartDemo_DoubleClick(object sender, EventArgs e)
        {
            # region 编写chart双击事件
            chartDemo.ChartAreas[0].AxisX.ScaleView.Size = 5;
            chartDemo.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chartDemo.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            //throw new NotImplementedException();  
            # endregion
        }

        //public void readFile()
        //{
        //    Series series = chartDemo.Series[0];
        //    # region 读文件内容
        //    if (str != null)
        //    {
        //        str = sr.ReadLine();//读取一行  
        //        if (str == null)
        //        {
        //            //chartTimer.Enabled = false;
        //            series.Points.AddXY(DateTime.Now, 20);
        //        }
        //        else
        //        {
        //            if (flag == 0)
        //            {
        //                str = sr.ReadLine();//读取一行
        //                flag = 1;
        //            }
        //            else
        //            {
        //                String[] eachLine = new String[26];//因为知道每一行excel有26个单元格，所以string[26]  
        //                eachLine = str.Split(',');//因为.csv文件是以逗号分隔单元格里数据的，所以调用分隔函数split                                     
        //                //data1[count] = int.Parse(eachLine[0]);
        //                data1[count] = double.Parse(eachLine[3]) - 4100;
        //                chartDemo.Series[0].BorderDashStyle = ChartDashStyle.Solid;
        //                series.Points.AddXY(DateTime.Now, data1[count]);
        //                chartDemo.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 5;
        //                count++;
        //            }
        //        }

        //    }
        //    # endregion
        //}

        private void chartTimer_Tick(object sender, EventArgs e)
        {
            Series series_AF3 = chartDemo.Series[0];
            Series series_F7 = chartDemo.Series[1];
           
            if (flag == 1)
            {
                series_F7.Enabled = false;
                series_AF3.Enabled = true;
                data_af3[count] = EEG_Logger.data_AF3[count] - 4000;
                chartDemo.Series[0].BorderDashStyle = ChartDashStyle.Solid;
                series_AF3.Points.AddXY(count, data_af3[count]);
                chartDemo.ChartAreas[0].AxisX.ScaleView.Position = series_AF3.Points.Count - 5;
                count++;
            }
            else if(flag == 4)
            {
                series_AF3.Enabled = false;
                series_F7.Enabled = true;
                data_f7[count0] = EEG_Logger.data_F7[count0] - 4000;
                chartDemo.Series[1].BorderDashStyle = ChartDashStyle.Solid;
                series_F7.Points.AddXY(count0, data_f7[count0]);
                chartDemo.ChartAreas[0].AxisX.ScaleView.Position = series_F7.Points.Count - 5; 
                count0++;
            }
            else if (flag == 3)
            {
                series_AF3.Enabled = true;
                series_F7.Enabled = true;

                data_af3[count] = EEG_Logger.data_AF3[count] - 4000;
                chartDemo.Series[0].BorderDashStyle = ChartDashStyle.Solid;
                series_AF3.Points.AddXY(count, data_af3[count]);
                chartDemo.ChartAreas[0].AxisX.ScaleView.Position = series_AF3.Points.Count - 5;
                //count++;

                data_f7[count] = EEG_Logger.data_F7[count] - 4000;
                chartDemo.Series[1].BorderDashStyle = ChartDashStyle.Solid;
                series_F7.Points.AddXY(count, data_f7[count]);
                chartDemo.ChartAreas[0].AxisX.ScaleView.Position = series_F7.Points.Count - 5;
                count++;

            }
            else
            {
                series_AF3.Enabled = false;
                series_F7.Enabled = false;
            }
            
                    
            label1.Text = EEG_Logger.GySteText;
            label2.Text = AverageBandPowers.answer.ToString();
        }
      
        private void F7_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_F7.Checked)
            {
                flag = 2;
            }
            else
            {
                flag = 0;
            }        
             
        }

        private void radio_AF3_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_AF3.Checked)
            {
                flag = 1;
            }
            else
            {
                flag = 0;
            }            
                          
        }

        private void checkBox_AF3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AF3.Checked)
            {
                flag = 3;
            }
            else
            {
                flag = 0;
            }

        }

        private void checkBox_F7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_F7.Checked)
            {
                flag = 4;
            }
            else
            {
                flag = 0;
            }
        }        
    }
}
