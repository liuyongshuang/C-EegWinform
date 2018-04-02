using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; //需要添加的命名空间  
using System.IO;

namespace liveChart
{
    public partial class Form1 : Form
    {
        static double[] data1 = new double[10000];
        static string str = "";
        static int count = 0;
        static int flag = 0;
        # region 打开csv文件
        static string fileName = @"C:\Users\ASUS\Desktop\EEGLogger.csv";
        static FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
        static StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));
        # endregion

        public Form1()
        {
            InitializeComponent();
            InitChart();  
        }        
        private void InitChart()
        {
            # region 初始化chart
            DateTime time = DateTime.Now;
            //chartTimer.Interval = 1000;
            chartTimer.Tick += timer1_Tick;
            chartDemo.DoubleClick += chartDemo_DoubleClick;

            Series series = chartDemo.Series[0];
            series.ChartType = SeriesChartType.Spline;

            chartDemo.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";//x轴显示时间格式
            chartDemo.ChartAreas[0].AxisX.ScaleView.Size = 5;
            chartDemo.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chartDemo.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chartTimer.Start();
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

        public void readFile()
        {
            Series series = chartDemo.Series[0];
            # region 读文件内容
            if(str != null)
            {
                str = sr.ReadLine();//读取一行  
                if (str == null)
                {
                    //chartTimer.Enabled = false;
                    series.Points.AddXY(DateTime.Now, 20);
                }
                else 
                {
                    if(flag == 0)
                    {
                        str = sr.ReadLine();//读取一行
                        flag = 1;
                    }
                    else
                    {
                        String[] eachLine = new String[25];//因为知道每一行excel有2个单元格，所以string[2]  
                        eachLine = str.Split(',');//因为.csv文件是以逗号分隔单元格里数据的，所以调用分隔函数split                                     
                        //data1[count] = int.Parse(eachLine[0]);
                        data1[count] = double.Parse(eachLine[2])-4200;
                        series.Points.AddXY(DateTime.Now, data1[count]);
                        chartDemo.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 5;
                        count++;      
                    }                               
                }
                                           
            }
            # endregion
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            readFile();
            //Random ra = new Random();                      
            //series.Points.AddXY(DateTime.Now, ra.Next(1, 10));                               
            //throw new NotImplementedException();  
        }
    }
}
