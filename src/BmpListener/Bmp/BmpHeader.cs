using System;
using BmpListener.Utilities;

namespace BmpListener.Bmp
{
    public class BmpHeader
    {
        public BmpHeader()
        {
        }

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
            if (Version != 3)
            {
                throw new NotSupportedException("version error");
            }
            
            MessageLength = EndianBitConverter.Big.ToInt32(data, 1);
            MessageType = (BmpMessageType)data[5];
        }
    }
}