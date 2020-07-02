using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab13Simulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //mendapatkan semua list port yang ada pada komputer
            String[] portList = System.IO.Ports.SerialPort.GetPortNames();
            foreach (String portName in portList)
                comboBox1.Items.Add(portName);
            comboBox1.Text = comboBox1.Items[comboBox1.Items.Count - 2].ToString();
            comboBox2.Text = comboBox2.Items[0].ToString();
            //keadaan tombol saat form pertama kali di load
            btnStream.Enabled = false;
            btnSmoke.Enabled = false;
            btnWater.Enabled = false;
            btnPower.Enabled = false;
            btnClear.Enabled = false;
            btnAir.Enabled = false;

        }

        bool btn = true;
        private void btnConnect_Click(object sender, EventArgs e)
        {
          
            //keadaan button ketika tombol connect ditekan
            btnStream.Enabled = true;
            btnClear.Enabled = true;
            //pengondisian 
            if (btn)
            {
                //membuka koneksi port
                btnConnect.Text = "Disconnect";
                try
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Int32.Parse(comboBox2.Text);
                    serialPort1.NewLine = "\r\n";
                    serialPort1.Open();
                    //statusstrip ketika terhubung
                    toolStripStatusLabel1.Text = serialPort1.PortName + " is connected.";
                }
                catch (Exception ex)
                {
                    //statusstrip ketika error
                    toolStripStatusLabel1.Text = "ERROR: " + ex.Message.ToString();
                }
                btn = false;
            }
            else
            {
                timer1.Stop();
                //keadaan button ketika tombol disconnect ditekan
                listBox1.Items.Clear();
                btnStream.Enabled = false;
                btnStream.Text = "Start Streaming";
                btnSmoke.Enabled = false;
                btnWater.Enabled = false;
                btnPower.Enabled = false;
                btnAir.Enabled = false;
                btnClear.Enabled = false;
                btnSmoke.Text = "Turn OFF Fan";
                btnSmoke.Text = "Add Smoke";
                btnWater.Text = "Add Water";
                btnPower.Text = "Disconnect Power";
                textTemp.Text = "";
                textHum.Text = "";
                textAir.Text = "";
                textSmoke.Text = "";
                textWater.Text = "";
                textPower.Text = "";
                toolStripStatusLabel2.Text = "";
                
                //menutup koneksi port
                btnConnect.Text = "Connect";
                serialPort1.Close();
                //status strip ketika koneksi terputus
                toolStripStatusLabel1.Text = serialPort1.PortName + " is closed.";
                btn = true;
            }

        }

        //function trigger
        public void trigger(TextBox text)
        {
            if (text.Text == "1")
            {
                text.Text = "1";
            }
            else
            {
                text.Text = "0";
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            //membuat fungsi random
            Random rnd = new Random();              
            int randomTemp = rnd.Next(17,28);
            textTemp.Text = randomTemp.ToString();
            int randomHum = rnd.Next(39,62);
            textHum.Text = randomHum.ToString();
            trigger(textSmoke);
            trigger(textWater);
            trigger(textPower);
            trigger(textAir);
            toolStripStatusLabel1.Text = "Streaming Started...";
            string transmit;
            //membuat data transmit
            transmit = "@," + textTemp.Text + "," + textHum.Text + "," + textAir.Text + "," + textSmoke.Text + "," + textWater.Text + "," + textPower.Text + ",$";
            //menambahkan data transmit ke listbix
            listBox1.Items.Add(transmit);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            //mengirimkan data transmit ke GUI
            serialPort1.WriteLine(transmit);
            
        }
        

        bool stream = true;
        private void btnStream_Click(object sender, EventArgs e)
        {
            //keadaan button saat button stream ditekan
            btnSmoke.Enabled = true;
            btnWater.Enabled = true;
            btnPower.Enabled = true;
            btnAir.Enabled = true;
            btnClear.Enabled = true;
            if (stream)
            {
                //jika button stream ditekan, timer akan aktif
                timer1.Start();
                btnStream.Text = "Stop Streaming";
                stream = false;
            }
            else
            {
                //jika button stop stream ditekan, timer mati
                timer1.Stop();
                //keadaan button ketika tombol stop stream ditekan
                btnStream.Text = "Start Streaming";
                btnSmoke.Enabled = false;
                btnWater.Enabled = false;
                btnPower.Enabled = false;
                btnAir.Enabled = false;
                btnAir.Text = "Turn OFF Fan";
                btnSmoke.Text = "Add Smoke";
                btnWater.Text = "Add Water";
                btnPower.Text = "Disconnect Power";
                textAir.Text = "0";
                textSmoke.Text = "0";
                textWater.Text = "0";
                textPower.Text = "0";
                //status strip ketika koneksi terputus
                toolStripStatusLabel1.Text = "Streaming Stopped";
                stream = true;
            }
        }

        bool smoke = true;
        private void btnSmoke_Click(object sender, EventArgs e)
        {          
            
            if (smoke)
            {
                //jika button ditekan, smoke akan aktif
                textSmoke.Text = "1";
                btnSmoke.Text = "Stop Smoke";
                toolStripStatusLabel2.Text = "Smoke Added..";
                smoke = false;
            }
            else
            {
                textSmoke.Text = "0";
                btnSmoke.Text = "Add Smoke";
                toolStripStatusLabel2.Text = "";
                smoke = true;
            } 
           
        }

        bool water = true;
        private void btnWater_Click(object sender, EventArgs e)
        {
            if (water)
            {
                //jika button ditekan, water akan aktif
                textWater.Text = "1";
                btnWater.Text = "Stop Water";
                toolStripStatusLabel2.Text = "Water Added..";
                water = false;
            }
            else
            {
                textWater.Text = "0";
                btnWater.Text = "Add Water";
                toolStripStatusLabel2.Text = "";
                water = true;
            } 
        }

        bool power = true;
        private void btnPower_Click(object sender, EventArgs e)
        {
            if (power)
            {
                //jika button ditekan, power akan disconnect
                textPower.Text = "1";
                btnPower.Text = "Connect Power";
                toolStripStatusLabel2.Text = "Power Disconnected..";
                power = false;
            }
            else
            {
                textPower.Text = "0";
                btnPower.Text = "Disconnect Power";
                toolStripStatusLabel2.Text = "";
                power = true;
            } 
        }

        

        private void btnClear_Click(object sender, EventArgs e)
        {
            //menghapus data di listbox ketka tombol ditekan
            listBox1.Items.Clear();
        }

        bool air = true;
        private void btnAir_Click(object sender, EventArgs e)
        {
            if (air)
            {
                //jika button ditekan, fan akan mati
                textAir.Text = "1";
                btnAir.Text = "Turn ON Fan";
                toolStripStatusLabel2.Text = "Fan is OFF..";
                air = false;
            }
            else
            {
                textAir.Text = "0";
                btnAir.Text = "Turn OFF Fan";
                toolStripStatusLabel2.Text = "";
                air = true;
            } 
        }  
    }
}
