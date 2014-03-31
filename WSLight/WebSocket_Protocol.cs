using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace WSLight
{
    public class WebSocket_Protocol
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
            if (!response.StartsWith(WSLight.WebSocket_Protocol.m_BadRequestPrefix, StringComparison.OrdinalIgnoreCase))
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

    public class DataFrame
    {
        public DataFrame(byte[] head, int start)
        {
            FIN = ((head[start + 0] & 0x80) == 0x80);
            HasMask = ((head[start + 1] & 0x80) == 0x80);
            OpCode = (OpCodeType)(head[start + 0] & 0x0f);
            RSV1 = ((head[start + 0] & 0x40) == 0x40);
            RSV2 = ((head[start + 0] & 0x20) == 0x20);
            RSV3 = ((head[start + 0] & 0x10) == 0x10);
            int maskstart = 2;
            byte payloadlen = (byte)(head[start + 1] & 0x7f);
            if (payloadlen == 126)
            {
                datalen = (int)head[start + 2] * 256 + (int)head[start + 3];
                maskstart += 2;
            }
            else if (payloadlen == 127)
            {
                long len = 0;
                int n = 1;

                for (int i = 7; i >= 0; i--)
                {
                    len += (int)head[start + i + 2] * n;
                    n *= 256;
                }
                datalen = len;
                maskstart += 8;
            }
            else datalen = payloadlen;
            if (HasMask)
            {
                mask = new byte[4];

                for (int i = 0; i < 4; i++)
                {
                    mask[i] = head[maskstart + i];
                }
            }
            text = null;
        }



        public string ReadString(byte[] data, int start)
        {
            text = Encoding.UTF8.GetString(data, start, (int)datalen);
            return text;
        }
        public string text
        {
            get;
            private set;
        }
        public bool FIN
        {
            get;
            private set;
        }

        public bool RSV1
        {
            get;
            private set;
        }

        public bool RSV2
        {
            get;
            private set;
        }

        public bool RSV3
        {
            get;
            private set;
        }
        public enum OpCodeType
        {
            Text = 1,
            Binary = 2,
            Close = 8,
            Ping = 9,
            Pong = 10,
        }
        public OpCodeType OpCode
        {
            get;
            private set;
        }

        public bool HasMask
        {
            get;
            private set;
        }
        public long datalen
        {
            get;
            private set;
        }
        public byte[] mask
        {
            get;
            private set;
        }
        private static void GenerateMask(byte[] mask, int offset)
        {
            int maxPos = Math.Min(offset + 4, mask.Length);

            for (var i = offset; i < maxPos; i++)
            {
                mask[i] = (byte)m_Random.Next(0, 255);
            }
        }
        static Random m_Random = new Random();
        private static void MaskData(byte[] rawData, int offset, int length, byte[] outputData, int outputOffset, byte[] mask, int maskOffset)
        {
            for (var i = 0; i < length; i++)
            {
                var pos = offset + i;
                outputData[outputOffset++] = (byte)(rawData[pos] ^ mask[maskOffset + i % 4]);
            }
        }

        public static byte[] MakeSendData(byte[] playloadData, int offset, int length, OpCodeType opCode = OpCodeType.Text, bool bFinished = true)
        {
            byte[] fragment;

            int maskLength = 4;

            if (length < 126)
            {
                fragment = new byte[2 + maskLength + length];
                fragment[1] = (byte)length;
            }
            else if (length < 65536)
            {
                fragment = new byte[4 + maskLength + length];
                fragment[1] = (byte)126;
                fragment[2] = (byte)(length / 256);
                fragment[3] = (byte)(length % 256);
            }
            else
            {
                fragment = new byte[10 + maskLength + length];
                fragment[1] = (byte)127;

                int left = length;
                int unit = 256;

                for (int i = 9; i > 1; i--)
                {
                    fragment[i] = (byte)(left % unit);
                    left = left / unit;

                    if (left == 0)
                        break;
                }
            }

            //Set FIN
            fragment[0] = (byte)((byte)opCode | 0x80);

            if (bFinished)
            {
                fragment[0] = (byte)((byte)opCode | 0x80);
            }
            else
            {
                fragment[0] = (byte)((byte)opCode);
            }
            //Set mask bit
            fragment[1] = (byte)(fragment[1] | 0x80);

            GenerateMask(fragment, fragment.Length - maskLength - length);

            if (length > 0)
                MaskData(playloadData, offset, length, fragment, fragment.Length - length, fragment, fragment.Length - maskLength - length);

            return fragment;
        }
    }
}
