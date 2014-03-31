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
        private static void AppendFormatWithCrCf( StringBuilder builder, string format, object arg)
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


        public static byte[] GenHandshake(out string expectedAccept, Uri target, string VersionTag = "13", string SubProtocol = null)
        {

            var secKey = Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString().Substring(0, 16)));
            expectedAccept = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(secKey + m_Magic)));


            //infos[m_ExpectedAcceptKey] = expectedAccept;

            var handshakeBuilder = new StringBuilder();

            AppendFormatWithCrCf(handshakeBuilder,"GET {0} HTTP/1.1",target.PathAndQuery);


            AppendWithCrCf(handshakeBuilder,"Upgrade: WebSocket");
            AppendWithCrCf(handshakeBuilder,"Connection: Upgrade");
            handshakeBuilder.Append("Sec-WebSocket-Version: ");
            AppendWithCrCf(handshakeBuilder,VersionTag);
            handshakeBuilder.Append("Sec-WebSocket-Key: ");
            AppendWithCrCf(handshakeBuilder,secKey);
            handshakeBuilder.Append("Host: ");
            AppendWithCrCf(handshakeBuilder, target.Authority);
            handshakeBuilder.Append("Origin: ");
            AppendWithCrCf(handshakeBuilder, target.Host);

            if (!string.IsNullOrEmpty(SubProtocol))
            {
                handshakeBuilder.Append("Sec-WebSocket-Protocol: ");
                AppendWithCrCf(handshakeBuilder,SubProtocol);
            }



            AppendWithCrCf(handshakeBuilder);

            byte[] handshakeBuffer = Encoding.UTF8.GetBytes(handshakeBuilder.ToString());

            return handshakeBuffer;
        }

        public static bool CheckHandshake(string response,string safecode)
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
        public const string m_BadRequestPrefix = "HTTP/1.1 400 ";
    }
}
