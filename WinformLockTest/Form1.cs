using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinformLockTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool block = true;
        private void button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                block = true;
                Application.Run(new Form2());
                block = false;
            });
            t.Start();
            while (block)
            {
                Thread.Sleep(1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                block = true;


                wpfLockTest.Window1 win = new wpfLockTest.Window1();
                win.Show();
                System.Windows.Application app = new System.Windows.Application();
                app.Run(win);
                //System.Windows.Threading.Dispatcher.Run
                block = false;
            });
           
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            while (block)
            {
                Thread.Sleep(1);
            }

        }
    }
}
