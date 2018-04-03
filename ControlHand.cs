using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace 控制智能家居
{
    public partial class ControlHand : Form
    {
        public ControlHand()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serial_port();
            if (this.button1.Text == "风扇 关")
            {
                MessageBox.Show("是否打开风扇", "提示");
                this.button1.Text = "风扇 开";
            }
            else
            {
                MessageBox.Show("是否关闭风扇", "提示");
                this.button1.Text = "风扇 关";
            }

        }
        /// <summary>
        /// SQ0:未知
        /// SQ1:布手
        /// SQ2:石头手
        /// SQ3:剪刀手
        /// </summary>
        private void serial_port()
        {       
            string str = "PL0 SQ0 ONCE\r";
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            serialPort1.Open();
            if(serialPort1.IsOpen)
            {
                serialPort1.WriteLine(str);
            }
            serialPort1.Close();
        }
    }
}
