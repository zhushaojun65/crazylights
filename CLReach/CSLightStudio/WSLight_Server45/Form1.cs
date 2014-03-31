using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
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
            try
            {
                string outstr = "output ....\n" + DateTime.Now + "\n" + content.Request.Url;

                var bs = System.Text.Encoding.UTF8.GetBytes(outstr);
                await content.Response.OutputStream.WriteAsync(bs, 0, bs.Length);
                content.Response.Close();
            }
            catch
            {

            }
        }
        List<byte> bufs = new List<byte>();

        async void SendBack(WebSocket socket, string str)
        {
            var buffer = new ArraySegment<byte>(
                   Encoding.UTF8.GetBytes(str));
            await socket.SendAsync(
                buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        async void parseWS(HttpListenerContext content)
        {
            try
            {
                var wsc = await content.AcceptWebSocketAsync(null);
                Safe_Log("connected 1");
                WebSocket socket = wsc.WebSocket;
                try
                {
                    while (true)
                    {
                        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                        WebSocketReceiveResult result = await socket.ReceiveAsync(
                            buffer, CancellationToken.None);
                        if (socket.State == WebSocketState.Open)
                        {
                            string userMessage = Encoding.UTF8.GetString(
                                buffer.Array, 0, result.Count);
                            userMessage = "You sent: " + userMessage + " at " +
                                DateTime.Now.ToLongTimeString();
                            SendBack(socket, userMessage);

                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch
                {

                }
                try
                {
                    await socket.CloseAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
                }
                catch
                {

                }
            }
            catch
            {

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
    }
}
