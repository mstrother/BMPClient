using System;
using System.Linq;

namespace BmpListener.Bmp
{
    public class BmpHeader
    {
        private readonly int bmpVersion = 3;
        
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
                        
            MessageLength = BigEndian.ToInt32(data, 1);
            MessageType = (BmpMessageType)data.ElementAt(5);
        }
    }
}