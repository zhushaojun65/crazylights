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


            Connect();
        }
        System.Net.WebSockets.ClientWebSocket ws;
        async void Connect()
        {
            ws = new System.Net.WebSockets.ClientWebSocket();
            await ws.ConnectAsync(new Uri("ws://localhost:808/dd"), System.Threading.CancellationToken.None);
            ArraySegment<byte> sbuf = new ArraySegment<byte>(Encoding.UTF8.GetBytes("hello there."));
            await ws.SendAsync(sbuf, System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
            while (true)
            {
                ArraySegment<byte> buf = new ArraySegment<byte>(new byte[1024]);
                var result = await ws.ReceiveAsync(buf, System.Threading.CancellationToken.None);
                if (ws.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    string str = Encoding.UTF8.GetString(
                        buf.Array, 0, result.Count);

                    Safe_Log(str);
                }
                else
                {
                    break;
                }
            }
        }
        void Safe_Log(string str)
        {
            this.Invoke((Action<string>)_Log, new object[] { str });
        }
        void _Log(string str)
        {
            if (this.listBox1.Items.Count > 10)
            {
                this.listBox1.Items.RemoveAt(0);
            }
            this.listBox1.Items.Add(str);
        }
        async void SendAsync()
        {
            ArraySegment<byte> sbuf = new ArraySegment<byte>(Encoding.UTF8.GetBytes("hello there."));
            await ws.SendAsync(sbuf, System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (ws != null && ws.State == System.Net.WebSockets.WebSocketState.Open)
            {
                SendAsync();
            }
        }


    }
}
