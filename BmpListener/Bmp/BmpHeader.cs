using System;
using System.Linq;
using BmpListener;
using BmpListener.Extensions;

namespace BmpListener.Bmp
{
    public class BmpHeader
    {
        private readonly int bmpVersion = 3;

        public BmpHeader(byte[] data)
        {
            Version = data.First();
            if (Version != bmpVersion)
            {
                throw new NotSupportedException("version error");
            }
            Length = data.ToInt32(1);
            MessageType = (BmpMessage.Type)data.ElementAt(5);
        }

        public byte Version { get; }
        public int Length { get; }
        public BmpMessage.Type MessageType { get; }
    }
}