using System;
using System.Collections.Generic;
using Emotiv;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEG_Data_Logger
{
    class EEG_Logger
    {
        EmoEngine engine;   // Access to the EDK is via the EmoEngine 
        int userID = -1;    // userID is used to uniquely identify a user's headset
        string filename = "EEG_Data_Logger.csv"; // output filename  
         
        public static double[] data_AF3 = new double[10000];
        public static double[] data_F7 = new double[10000];

        public static int count_AF3 = 0;
        public static int count_F7 = 0;
        public static string GySteText;
        EEG_Logger()
        {            
            // create the engine
            engine = EmoEngine.Instance;
            engine.UserAdded += new EmoEngine.UserAddedEventHandler(engine_UserAdded_Event);
            
            // connect to Emoengine.            
            engine.Connect();

            // create a header for our output file
            WriteHeader();
        }

        void engine_UserAdded_Event(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("User Added Event has occured");

            // record the user 
            userID = (int)e.userId;

            // enable data aquisition for this user.
            engine.DataAcquisitionEnable((uint)userID, true);
            
            // ask for up to 1 second of buffered data
            engine.DataSetBufferSizeInSec(1); 

        }
        void Run()
        {            
            // Handle any waiting events
            engine.ProcessEvents();

            // If the user has not yet connected, do not proceed
            if ((int)userID == -1)
                return;

            Dictionary<EdkDll.IEE_DataChannel_t, double[]> data = engine.GetData((uint)userID);
            //int result = IEE_GetAverageBandPowers(engineUserID, channelList[i], &theta, &alpha,
                            //&low_beta, &high_beta, &gamma);           

            if (data == null)
            {
                return;
            }

            int _bufferSize = data[EdkDll.IEE_DataChannel_t.IED_TIMESTAMP].Length;

            Console.WriteLine("Writing " + _bufferSize.ToString() + " sample of data ");

            // Write the data to a file
            TextWriter file = new StreamWriter(filename,true);

            for (int i = 0; i < _bufferSize; i++)
            {
                // now write the data
                foreach (EdkDll.IEE_DataChannel_t channel in data.Keys)
                {
                    file.Write(data[channel][i] + ",");
                    if (channel.ToString() == "IED_GYROX")
                    {
                        data[channel][i] = data[channel][i] - 8000;
                        if (data[channel][i] < -1000)
                            GySteText = "右";                        
                        else if (data[channel][i] > 1000)
                            GySteText = "左";                        
                        else
                            GySteText = "中";                                                
                    }
                    if(channel.ToString()=="IED_AF3")
                    {
                        data_AF3[count_AF3] = data[channel][i];
                        count_AF3++;                        
                    }
                    if (channel.ToString()=="IED_F7")
                    {
                        data_F7[count_F7] = data[channel][i];
                        count_F7++;
                    }
                    
                }
                file.WriteLine("");

            }
            file.Close();

        }

        public void WriteHeader()
        {
            TextWriter file = new StreamWriter(filename, false);

            string header = "COUNTER, INTERPOLATED, RAW_CQ, AF3, F7, F3, FC5, T7, P7, O1, O2, P8," +
                "T8, FC6, F4, F8, AF4, GYROX, GYROY, TIMESTAMP, MARKER_HARDWARE, ES_TIMESTAMP, FUNC_ID, FUNC_VALUE, MARKER, SYNC_SIGNAL";
            
            file.WriteLine(header);
            file.Close();
        }
        
        public static void winRun()
        {
        # region 窗口运行程序
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EEG());
        }

        # endregion

        public static void EEG_Data()
        {
            # region 运行设备采集程序
            Console.WriteLine("EEG Data Reader Example");

            EEG_Logger p = new EEG_Logger();

            for (int i = 0; i < 5000; i++)
            {               
                p.Run();
                Thread.Sleep(10);
            }
            # endregion   
        }
        static void Main(string[] args)
        {
            # region 设置多进程处理
            ThreadStart ts1 = new ThreadStart(winRun);
            Thread t1 = new Thread(ts1);
            ThreadStart ts2 = new ThreadStart(EEG_Data);
            Thread t2 = new Thread(ts2);
            ThreadStart ts3 = new ThreadStart(AverageBandPowers.Main_AverageBandPowers);
            Thread t3 = new Thread(ts3);

            t2.Start();
            t3.Start();
            Thread.Sleep(100);
            t1.Start();
            # endregion   
            //AverageBandPowers.Main_AverageBandPowers();
        }
    }

    class AverageBandPowers
    {
        static double[] score =new double[320];
        public static double answer = 0;
        static int count_sense = 0;
        static double sum = 0;
        static int userID1 = -1;
        static string filename1 = "AverageBandPowers.csv";
        static TextWriter file = new StreamWriter(filename1, false);

        static void engine_UserAdded_Event(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("User Added Event has occured");
            userID1 = (int)e.userId;

            EmoEngine.Instance.IEE_FFTSetWindowingType((uint)userID1, EdkDll.IEE_WindowingTypes.IEE_HAMMING);
        }


        public static void Main_AverageBandPowers()
        {            
            for (int t = 0; t < 320;t++ )
            {
                score[t] = 0;
            }
            Console.WriteLine("===================================================================================");
            Console.WriteLine("Example to get the average band power for a specific channel from the latest epoch.");
            Console.WriteLine("===================================================================================");

            AverageBandPowers p = new AverageBandPowers();

            // create the engine
            EmoEngine engine = EmoEngine.Instance;
            engine.UserAdded += new EmoEngine.UserAddedEventHandler(engine_UserAdded_Event);
            engine.Connect();

            string header = "Theta, Alpha, Low_beta, High_beta, Gamma"; ;
            file.WriteLine(header);
            file.WriteLine("");

            EdkDll.IEE_DataChannel_t[] channelList = new EdkDll.IEE_DataChannel_t[5] { EdkDll.IEE_DataChannel_t.IED_AF3, 
                                                                                       EdkDll.IEE_DataChannel_t.IED_AF4, 
                                                                                       EdkDll.IEE_DataChannel_t.IED_T7, 
                                                                                       EdkDll.IEE_DataChannel_t.IED_T8, 
                                                                                       EdkDll.IEE_DataChannel_t.IED_O1 };

            while (true)
            {
                engine.ProcessEvents(10);

                if (userID1 < 0) continue;

                if (Console.KeyAvailable)
                    break;

                double[] alpha = new double[1];
                double[] low_beta = new double[1];
                double[] high_beta = new double[1];
                double[] gamma = new double[1];
                double[] theta = new double[1];

                for (int i = 0; i < 5; i++)
                {
                    Int32 result = engine.IEE_GetAverageBandPowers((uint)userID1, channelList[i], theta, alpha, low_beta, high_beta, gamma);
                    if (result == EdkDll.EDK_OK)
                    {
                        file.Write(theta[0] + ",");
                        file.Write(alpha[0] + ",");
                        file.Write(low_beta[0] + ",");
                        file.Write(high_beta[0] + ",");
                        file.WriteLine(gamma[0] + ",");
                       // Console.WriteLine("Theta: " + theta[0]);
                        /////////////////////////////////////////////////
                        ///////////////////专注度算法////////////////////
                        /////////////////////////////////////////////////
                        score[count_sense] = (high_beta[0] + low_beta[0]) / theta[0];
                        count_sense++;
                        if (count_sense == 320) { count_sense = 0; }
                        if(score[59]!=0)
                        {
                            sum = 0;
                            for(int j = 0;j < 320; j++)
                            {
                                sum = sum + score[j];
                            }
                        }
                        //Console.WriteLine(sum);
                        answer = Math.Sqrt(sum / 10) * 6;
                        while (answer > 100)
                        {
                            answer = answer - 10;
                        }

                        if (answer <= 50)
                        {
                            answer = 100 * Math.Sqrt(answer / 50) * 0.5;
                        }
                        else
                        {
                            answer = 100 * 0.5 * (1 + ((answer - 50) / 50) * ((answer - 50) / 50));
                        }
                        answer = Math.Abs((answer - 20) / 0.8);
                        //Console.WriteLine(answer);
                    }
                }
            }

            file.Close();
            engine.Disconnect();
        }
    }
}
