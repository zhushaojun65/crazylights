using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    }
}
