using BmpListener.MiscUtil.Conversion;
using System;
using System.Linq;

namespace BmpListener.Bmp
{
    public class BmpHeader
    {
        private readonly int bmpVersion = 3;

        EndianBitConverter bigEndianBitConverter = EndianBitConverter.Big;
        
        public byte Version { get; private set; }
        public int MessageLength { get; private set; }
        public BmpMessageType MessageType { get; private set; }

        public void Decode(ArraySegment<byte> data)
        {
            Version = data.Array[0];
            if (Version != bmpVersion)
            {
                throw new NotSupportedException("version error");
            }
                        
            MessageLength = bigEndianBitConverter.ToInt32(data, 1);
            MessageType = (BmpMessageType)data.ElementAt(5);
        }
    }
}