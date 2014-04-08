using System;
using System.Collections.Generic;
using System.Text;

namespace WSLight.Layer0
{
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
                datalenlong = (int)head[start + 2] * 256 + (int)head[start + 3];
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
                datalenlong = len;
                maskstart += 8;
            }
            else datalenlong = payloadlen;
            if (HasMask)
            {
                mask = new byte[4];

                for (int i = 0; i < 4; i++)
                {
                    mask[i] = head[maskstart + i];
                }
            }
            //text = null;
        }



        //public string ReadString(byte[] data, int start)
        //{
        //    text = Encoding.UTF8.GetString(data, start, (int)datalen);
        //    return text;
        //}
        //public string text
        //{
        //    get;
        //    private set;
        //}
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
        public long datalenlong
        {
            get;
            private set;
        }
        public int datalen
        {
            get
            {
                return (int)datalenlong;
            }
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
