using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

public class Layer0Test : MonoBehaviour {
    static TcpClient tcp = new TcpClient();

    static Uri target;
    static string safecode;
    static void onConnect(IAsyncResult ar)
    {
        Debug.Log("connected.");
        byte[] packet = WSLight.Layer0.HandShake.GenHandshake(out safecode, target);
        tcp.GetStream().BeginWrite(packet, 0, packet.Length, onHandshakeSend, null);
        Debug.Log("Handshake.Key=" + safecode);
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
        var cool = System.Text.Encoding.UTF8.GetBytes("hello world.");
        byte[] bts = WSLight.Layer0.DataFrame.MakeSendData(cool, 0, cool.Length);
        tcp.GetStream().BeginWrite(bts, 0, bts.Length, OnSendNormal, null);
    }
    static void onRead(IAsyncResult ar)
    {
        int readlen = tcp.GetStream().EndRead(ar);
        if (!bHanded)
        {
            string resp = System.Text.Encoding.UTF8.GetString(buf, 0, readlen);
            bool b = WSLight.Layer0.HandShake.CheckHandshake(resp, safecode);
            if (b)
            {
                bHanded = true;
                Debug.Log("HandShake succ.");
                SendHello();
            }
            else
            {
                Debug.Log("HandShake fail." + resp);
            }
        }
        else
        {
            int len;
            WSLight.Layer0.DataFrame frame;
            WSLight.Layer0.HandShake.GetDateFrame(buf, 0, readlen, out frame, out len);
            if (frame != null)
            {
                if (frame.datalenlong + len <= readlen && frame.datalenlong > 0 && frame.OpCode == WSLight.Layer0.DataFrame.OpCodeType.Text)
                {
                    string str = System.Text.Encoding.UTF8.GetString(buf, len, frame.datalen);
                    //frame.ReadString(buf, len);
                    Debug.Log("read frame:code=" + frame.OpCode + " str=" + str);
                }
                else
                {
                    Debug.Log("read frame:code=" + frame.OpCode + " len=" + frame.datalen);
                }
            }
        }
        tcp.GetStream().BeginRead(buf, 0, 1024, onRead, null);


    }
	// Use this for initialization
	void Start () {
        target = new Uri("ws://127.0.0.1:808/");
        tcp.BeginConnect(target.Host, target.Port, onConnect, null);
        Debug.Log("Init");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if(GUI.Button(new Rect(0,0,100,50),"SendHello."))
        {
            SendHello();
        }
    }
}
