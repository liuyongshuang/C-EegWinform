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
        public Form3()
        {
            InitializeComponent();           
            int count=0;
            int[] data1 =new int[100];
            double[] data2 = new double[100];
            List<int> txData3 = new List<int>() { 3 };
            List<int> tyData3 = new List<int>() { 7 };

            string fileName = @"C:\Users\ASUS\Desktop\abcd.csv";            
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));
            string str = "";           
            while (str != null)
            {
                str = sr.ReadLine();//读取一行  
                if (str == null) break;//读完了就跳出循环  
                String[] eachLine = new String[2];//因为知道每一行excel有2个单元格，所以string[2]  
                eachLine = str.Split(',');//因为.csv文件是以逗号分隔单元格里数据的，所以调用分隔函数split 
                //if (count == 6)
                //{
                //    count = 0;
                //    Thread.Sleep(1000);
                //}
                data1[count] = count+1;
                data2[count] = double.Parse(eachLine[1]);
                count++;            
                                       
            }
            ct.Series[0].Points.DataBindXY(data1, data2); //添加数据
            sr.Close();//关闭读流的对象 
            //ct.Series[1].Points.DataBindXY(txData3, tyData3); 
        }
    }
}
