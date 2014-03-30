using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSLight_Client45
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void Safe_Log(string str)
        {
            this.Invoke((Action<string>)_Log,new object[]{str});
        }
        void _Log(string str)
        {
            if(this.listBox1.Items.Count>10)
            {
                this.listBox1.Items.RemoveAt(0);
            }
            this.listBox1.Items.Add(str);
        }


    }
}
