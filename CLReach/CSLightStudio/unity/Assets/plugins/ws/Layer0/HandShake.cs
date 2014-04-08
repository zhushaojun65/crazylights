using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WSLight.Layer0
{
    public class HandShake
    {
        private const string m_Magic = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

        private const string m_ExpectedAcceptKey = "ExpectedAccept";

        private readonly static char[] m_CrCf = new char[] { '\r', '\n' };
        private static void AppendFormatWithCrCf(StringBuilder builder, string format, object arg)
        {
            builder.AppendFormat(format, arg);
            builder.Append(m_CrCf);
        }

        private static void AppendFormatWithCrCf(StringBuilder builder, string format, params object[] args)
        {
            builder.AppendFormat(format, args);
            builder.Append(m_CrCf);
        }

        private static void AppendWithCrCf(StringBuilder builder, string content)
        {
            builder.Append(content);
            builder.Append(m_CrCf);
        }

        private static void AppendWithCrCf(StringBuilder builder)
        {
            builder.Append(m_CrCf);
        }

        //private static Dictionary<string, string> infos = new Dictionary<string, string>();
        private const string m_BadRequestPrefix = "HTTP/1.1 400 ";



        //生成握手串
        public static byte[] GenHandshake(out string expectedAccept, Uri target, string VersionTag = "13", string SubProtocol = null)
        {

            var secKey = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString().Substring(0, 16)));
            expectedAccept = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(secKey + m_Magic)));


            //infos[m_ExpectedAcceptKey] = expectedAccept;

            var handshakeBuilder = new StringBuilder();

            AppendFormatWithCrCf(handshakeBuilder, "GET {0} HTTP/1.1", target.PathAndQuery);


            AppendWithCrCf(handshakeBuilder, "Upgrade: WebSocket");
            AppendWithCrCf(handshakeBuilder, "Connection: Upgrade");
            handshakeBuilder.Append("Sec-WebSocket-Version: ");
            AppendWithCrCf(handshakeBuilder, VersionTag);
            handshakeBuilder.Append("Sec-WebSocket-Key: ");
            AppendWithCrCf(handshakeBuilder, secKey);
            handshakeBuilder.Append("Host: ");
            AppendWithCrCf(handshakeBuilder, target.Authority);
            handshakeBuilder.Append("Origin: ");
            AppendWithCrCf(handshakeBuilder, target.Host);

            if (!string.IsNullOrEmpty(SubProtocol))
            {
                handshakeBuilder.Append("Sec-WebSocket-Protocol: ");
                AppendWithCrCf(handshakeBuilder, SubProtocol);
            }



            AppendWithCrCf(handshakeBuilder);

            byte[] handshakeBuffer = Encoding.UTF8.GetBytes(handshakeBuilder.ToString());

            return handshakeBuffer;
        }

        //检查握手响应
        public static bool CheckHandshake(string response, string safecode)
        {
            if (!response.StartsWith(WSLight.Layer0.HandShake.m_BadRequestPrefix, StringComparison.OrdinalIgnoreCase))
            {

                //Console.WriteLine("HandShake succ.");
                foreach (var l in response.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var pair = l.Split(new string[] { ":", " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (pair.Length == 2)
                    {
                        //Console.WriteLine(pair[0] + "||" + pair[1]);
                        if (pair[0] == "Sec-WebSocket-Accept")
                        {

                            //Console.WriteLine("Key=" + pair[1] + "(" + (pair[1] == safecode) + ")");
                            if (pair[1] == safecode)
                            {
                                return true;
                                //验证不匹配
                            }
                        }
                    }
                }
            }

            return false;

        }


        //检查数据帧包头长度
        public static void GetDateFrame(byte[] buf, int start, int buflen, out DataFrame frame, out int headLen)
        {
            bool fin = ((buf[start + 0] & 0x80) == 0x80);
            byte payloadlen = (byte)(buf[start + 1] & 0x7f);
            bool mask = ((buf[start + 1] & 0x80) == 0x80);
            int len = 2;
            if (payloadlen == 126) len += 2;
            if (payloadlen == 127) len += 8;
            if (mask) len += 4;

            headLen = len;
            if (buflen >= headLen)
            {
                frame = new DataFrame(buf, start);
            }
            else
            {
                frame = null;
            }
        }
    }

}
