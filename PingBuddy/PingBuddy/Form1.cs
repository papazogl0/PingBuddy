using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
namespace PingBuddy
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            new Thread(() =>
            {
                pingthread();
            }).Start();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Environment.Exit(0);
        }
        void pingthread()
        {
            notifyIcon1.Icon = Properties.Resources.Dead;
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 1000;
            int i = 0;
            while (true)
            {
                try
                {
                    PingReply reply;
                    reply = pingSender.Send("8.8.8.8", timeout, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        timeout = 1000;
                        if (reply.RoundtripTime < 80)
                            this.Invoke((MethodInvoker)delegate
                            {
                                notifyIcon1.Icon = Properties.Resources.Good;
                                textBox1.Text = reply.RoundtripTime.ToString();
                                notifyIcon1.Text = reply.RoundtripTime.ToString();
                            });
                        if (reply.RoundtripTime > 80 && reply.RoundtripTime < 200)
                            this.Invoke((MethodInvoker)delegate
                            {
                                notifyIcon1.Icon = Properties.Resources.Not_that_good;
                                textBox1.Text = reply.RoundtripTime.ToString();
                                notifyIcon1.Text = reply.RoundtripTime.ToString();
                            });
                        if (reply.RoundtripTime > 200)
                            this.Invoke((MethodInvoker)delegate
                            {
                                textBox1.Text = reply.RoundtripTime.ToString();
                                notifyIcon1.Icon = Properties.Resources.Bad;
                                notifyIcon1.Text = reply.RoundtripTime.ToString();
                            });
                    }
                    else
                    {
                        timeout = timeout + 1000;
                        if (timeout > 30000) timeout = 30000;
                        i++;
                        textBox1.Text = "Waiting " + i.ToString() + " time to reconnect.. Current timeout:" + timeout.ToString() + reply.Status.ToString();
                        notifyIcon1.Icon = Properties.Resources.Dead;
                        notifyIcon1.Text = "Нет связи, ух!";
                    }
                    Thread.Sleep(1000);
                }

                catch (Exception ex)
                {
                    //Do nothing about it.
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
        

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);

        }
    }
}
    

