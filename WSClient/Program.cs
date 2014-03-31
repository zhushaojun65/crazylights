using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace WSClient
{
    class Program
    {
        static TcpClient tcp = new TcpClient();
        static void Main(string[] args)
        {
            target = new Uri("ws://127.0.0.1:808/");
            tcp.BeginConnect(target.Host, target.Port, onConnect, null);
            Console.WriteLine("Init");
            while (true)
            {

                Console.ReadLine();
                SendHello();
            }
        }
        static Uri target;
        static string safecode;
        static void onConnect(IAsyncResult ar)
        {
            Console.WriteLine("connected.");
            byte[] packet = WSLight.Layer0.HandShake.GenHandshake(out safecode, target);
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

        static void OnSendNormal(IAsyncResult ar)
        {
            tcp.GetStream().EndWrite(ar);
        }
        static void SendHello()
        {
            var cool = Encoding.UTF8.GetBytes("hello world.");
            byte[] bts = WSLight.Layer0.DataFrame.MakeSendData(cool, 0, cool.Length);
            tcp.GetStream().BeginWrite(bts, 0, bts.Length, OnSendNormal, null);
        }
        static void onRead(IAsyncResult ar)
        {
            int readlen = tcp.GetStream().EndRead(ar);
            if (!bHanded)
            {
                string resp = Encoding.UTF8.GetString(buf, 0, readlen);
                bool b = WSLight.Layer0.HandShake.CheckHandshake(resp,safecode);
                if(b)
                {
                    bHanded = true;
                    Console.WriteLine("HandShake succ.");
                    SendHello();
                }
                else
                {
                    Console.WriteLine("HandShake fail." + resp);
                }
            }
            else
            {
                int len;
                WSLight.Layer0.DataFrame frame;
                WSLight.Layer0.HandShake.GetDateFrame(buf, 0, readlen, out frame, out len);
                if(frame!=null)
                {
                    if(frame.datalenlong+len <= readlen&& frame.datalenlong>0 &&frame.OpCode== WSLight.Layer0.DataFrame.OpCodeType.Text)
                    {
                        string str = Encoding.UTF8.GetString(buf, len, frame.datalen);
                        //frame.ReadString(buf, len);
                        Console.WriteLine("read frame:code=" + frame.OpCode + " str=" + str);
                    }
                    else
                    {
                        Console.WriteLine("read frame:code=" + frame.OpCode + " len=" + frame.datalen);
                    }
                }
            }
            tcp.GetStream().BeginRead(buf, 0, 1024, onRead, null);


        }
    }
}
