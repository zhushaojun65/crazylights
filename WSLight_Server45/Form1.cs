using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSLight_Server45
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        HttpListener httpServer = new HttpListener();
        private void Form1_Load(object sender, EventArgs e)
        {
            httpServer.Prefixes.Add("http://*:808/");
            httpServer.Start();
            Safe_Log("Server on " + "http://*:808/");
            ServerSync();
        }
        bool bExit = false;
        async void ServerSync()
        {
            do
            {
                var content = await httpServer.GetContextAsync();
                if (content.Request.IsWebSocketRequest)
                {
                    parseWS(content);
                }
                else
                {
                    parseHttp(content);
                }

            } while (!bExit);
        }
        async void parseHttp(HttpListenerContext content)
        {
            string outstr = "output ....";
            var bs = System.Text.Encoding.UTF8.GetBytes(outstr);
            await content.Response.OutputStream.WriteAsync(bs, 0, bs.Length);
            content.Response.Close();
        }
        async void parseWS(HttpListenerContext content)
        {
            var wsc = await content.AcceptWebSocketAsync("");
            //wsc.WebSocket.ReceiveAsync()
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
    }
}
