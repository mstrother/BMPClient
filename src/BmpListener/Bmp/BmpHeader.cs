using System;

namespace BmpListener.Bmp
{
    public class BmpHeader
    {
        private readonly int bmpVersion = 3;

        public BmpHeader(byte[] data)
        {
            Decode(data);
        }

        public byte Version { get; private set; }
        public int MessageLength { get; private set; }
        public BmpMessageType MessageType { get; private set; }

        public void Decode(byte[] data)
        {
            Version = data[0];
            if (Version != bmpVersion)
            {
                throw new NotSupportedException("version error");
            }

            Array.Reverse(data, 1, 4);
            MessageLength = BitConverter.ToInt32(data, 1);
            MessageType = (BmpMessageType)data[5];
        }
    }
}