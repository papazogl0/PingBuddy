using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;


namespace PingBuddy
{
    public partial class MainWindow : Form
    {
        public MainWindow(string[] args)
        {
            InitializeComponent();
            int timeout = 1000;
            string address = "1.1.1.1";
            if (args.Length ==1 )
            {
                address = args[0];

            }
            if (args.Length ==2)
            {
                address = args[0];
                if (Int32.TryParse(args[1], out timeout) != true)
                    timeout = 1000;
            }
            
            Thread Simplethread = new Thread(() =>
            {
                while (true)
                {
                    pingthread(address, timeout);
                }
            }
            );
            Simplethread.Start();
        }

        


        private void Form1_Load(object sender, EventArgs e)
        {
           
            
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Opacity != 0)
            {
                this.Opacity = 0;
                this.ShowInTaskbar = false;
            }
            else
            {
                this.Opacity = 100;
                this.ShowInTaskbar = true;
            }
        }

        void pingthread(string address, int timeout)
        {
            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send(address, timeout);
                if (reply != null)
                {
                    
                    if (reply.RoundtripTime < 80)
                        this.Invoke((MethodInvoker)delegate
                        {
                            notifyIcon1.Icon = Properties.Resources.Good;
                            textBox1.Text = reply.RoundtripTime.ToString()+" "+reply.Status.ToString();
                            notifyIcon1.Text = reply.RoundtripTime.ToString();
                        });
                    if (reply.RoundtripTime > 80 && reply.RoundtripTime < 200)
                        this.Invoke((MethodInvoker)delegate
                        {
                            notifyIcon1.Icon = Properties.Resources.Not_that_good;
                            textBox1.Text = reply.RoundtripTime.ToString() + " " + reply.Status.ToString();
                            notifyIcon1.Text = reply.RoundtripTime.ToString();
                        });
                    if (reply.RoundtripTime > 200)
                        this.Invoke((MethodInvoker)delegate
                        {
                            textBox1.Text = reply.RoundtripTime.ToString() + " " + reply.Status.ToString();
                            notifyIcon1.Icon = Properties.Resources.Bad;
                            notifyIcon1.Text = reply.RoundtripTime.ToString();
                        });
                }
            }
            catch
            {
                this.Invoke((MethodInvoker)delegate
                {
                    notifyIcon1.Icon = Properties.Resources.Dead;
                    notifyIcon1.Text = "Dead network?";
                });
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
    

