using System;

namespace BmpListener.Bmp
{
    public class BmpHeader
    {
        private readonly int bmpVersion = 3;

        public BmpHeader(byte[] data)
        {
            Version = data[0];
            if (Version != bmpVersion)
            {
                throw new NotSupportedException("version error");
            }

            Array.Reverse(data, 1, 4);
            MessageLength = BitConverter.ToInt32(data, 1);
            MessageType = (BmpMessage.Type)data[5];
        }

        public byte Version { get; }
        public int MessageLength { get; }
        public BmpMessage.Type MessageType { get; }
    }
}