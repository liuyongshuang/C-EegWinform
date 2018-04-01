using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace chart的使用
{
    public partial class Form3 : Form
    {
        static int flag = 0;
        static string str = "";
        static int count = 0;
        static int t = 1;
        static int[] data1 = new int[100];
        static double[] data2 = new double[100];
        List<int> txData3 = new List<int>() { 3 };
        List<int> tyData3 = new List<int>() { 7 }; 
        static string fileName = @"C:\Users\ASUS\Desktop\csvtest.csv";
        static FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
        static StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));     
        public Form3()
        {
            InitializeComponent();                       
        }
        public void readFile()
        {            
            while (str != null)
            {                            
                str = sr.ReadLine();//读取一行  
                if (str == null)
                {
                    flag = 1;
                    break;//读完了就跳出循环
                }

                String[] eachLine = new String[2];//因为知道每一行excel有2个单元格，所以string[2]  
                eachLine = str.Split(',');//因为.csv文件是以逗号分隔单元格里数据的，所以调用分隔函数split             
                if(int.Parse(eachLine[0])!=1)
                {
                    while(t!=int.Parse(eachLine[0]))
                    {
                        data1[count] = count;
                        data2[count] = 0;
                        t++;
                        count++;
                    }
                }              
                data1[count] = int.Parse(eachLine[0]);
                data2[count] = double.Parse(eachLine[1]);
                count++;
                if (count == 10)
                {
                    count = 0;
                    break;
                }
                
            }          
        }
        public  void drawChart()
        {
            while(true)
            {
                readFile();
                ct.Series[0].Points.DataBindXY(data1, data2); //添加数据            
                //ct.Series[1].Points.DataBindXY(txData3, tyData3);
                Thread.Sleep(1000);
                if(flag == 1)
                {
                    sr.Close();//关闭读流的对象
                    break;
                }             
            }
                  
        }
        private void button1_Click(object sender, EventArgs e)
        {
            drawChart();
        }
        
    }       
}
