using System;
using System.Linq;
using BmpListener;

namespace BmpListener.Bmp
{
    public class BmpHeader
    {
        public BmpHeader(byte[] data)
        {
            Version = data.First();
            //if (Version != 3)
            //{
            //    throw new Exception("invalid BMP version");
            //}
            Length = data.ToUInt32(1);
            MessageType = (BmpMessage.Type)data.ElementAt(5);
        }

        public byte Version { get; }
        public uint Length { get; }
        public BmpMessage.Type MessageType { get; }
    }
}