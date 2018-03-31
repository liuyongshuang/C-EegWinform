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
           // List<int> txData2 = new List<int>() { 2011, 2012, 2013, 2014, 2015, 2016 };
           // List<int> tyData2 = new List<int>() { 9, 6, 7, 4, 5, 4 };
            int count=0;
            int[] data1 =new int[6];
            int[] data2 = new int[6];
            List<int> txData3 = new List<int>() { 2003 };
            List<int> tyData3 = new List<int>() { 7 };

            string fileName = @"C:\Users\ASUS\Desktop\abcd.csv";
            //FileInfo myFile = new FileInfo(fileName);
            //FileStream fileStream = myFile.OpenWrite();//建立与myFile的数据流通道 
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));
            string str = "";
            //List<string[]> listArr = new List<string[]>();
            while (str != null)
            {
                str = sr.ReadLine();//读取一行  
                if (str == null) break;//读完了就跳出循环  

                String[] eachLine = new String[2];//因为知道每一行excel有2个单元格，所以string[2]  
                eachLine = str.Split(',');//因为.csv文件是以逗号分隔单元格里数据的，所以调用分隔函数split  
                //double ser = double.Parse(eachLine[0]);//年份 
                //double dse = double.Parse(eachLine[1]);//价值 
                data1[count] = int.Parse(eachLine[0]);
                data2[count] = int.Parse(eachLine[1]);
                count++;                                         
            }
            ct.Series[0].Points.DataBindXY(data1, data2); //添加数据
            Thread.Sleep(1000);

            sr.Close();//关闭读流的对象 
            ct.Series[1].Points.DataBindXY(txData3, tyData3); 
        }
    }
}
