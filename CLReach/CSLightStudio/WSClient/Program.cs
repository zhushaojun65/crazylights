using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using WebSocket4Net.Protocol.FramePartReader;

namespace WSClient
{
    class Program
    {
        static TcpClient tcp = new TcpClient();
        static void Main(string[] args)
        {
            target = new Uri("ws://127.0.0.1:808/");
            tcp.BeginConnect(target.Host, target.Port, onConnect, null);

            while (true)
            {
                Console.WriteLine("Init");
                Console.ReadLine();
            }
        }
        static Uri target;
        static string safecode;
        static void onConnect(IAsyncResult ar)
        {
            Console.WriteLine("connected.");
            byte[] packet = WSLight.WebSocket_Protocol.GenHandshake(out safecode, target);
            tcp.GetStream().BeginWrite(packet, 0, packet.Length, onHandshakeSend, null);
            Console.WriteLine("Handshake.Key=" + safecode);
        }
        static byte[] buf = new byte[1024];
        static void onHandshakeSend(IAsyncResult ar)
        {
            tcp.GetStream().EndWrite(ar);
            tcp.GetStream().BeginRead(buf, 0, 1024, onRead, null);
        }
        static bool bHanded = false;
        static void onRead(IAsyncResult ar)
        {
            int readlen = tcp.GetStream().EndRead(ar);
            if (!bHanded)
            {
                string resp = Encoding.UTF8.GetString(buf, 0, readlen);
                bool b = WSLight.WebSocket_Protocol.CheckHandshake(resp,safecode);
                if(b)
                {
                    bHanded = true;
                    Console.WriteLine("HandShake succ.");
                }
                //if (!resp.StartsWith(WSLight.WebSocket_Protocol.m_BadRequestPrefix, StringComparison.OrdinalIgnoreCase))
                //{
                //    bHanded = true;
                //    Console.WriteLine("HandShake succ.");
                //    foreach (var l in resp.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
                //    {
                //        var pair = l.Split(new string[] { ":", " " }, StringSplitOptions.RemoveEmptyEntries);
                //        if (pair.Length == 2)
                //        {
                //            Console.WriteLine(pair[0] + "||" + pair[1]);
                //            if (pair[0] == "Sec-WebSocket-Accept")
                //            {

                //                Console.WriteLine("Key=" + pair[1] + "(" + (pair[1] == safecode) + ")");
                //                if (pair[1] != safecode)
                //                {
                //                    //验证不匹配
                //                }
                //            }
                //        }
                //    }
                //}
                else
                {
                    Console.WriteLine("HandShake fail." + resp);
                }
            }
            else
            {
                //后面就是二进制数据帧
                Console.WriteLine("read:" + readlen);
            }
            tcp.GetStream().BeginRead(buf, 0, 1024, onRead, null);


        }
    }
}
