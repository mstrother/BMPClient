using System;
using Newtonsoft.Json;

namespace BmpListener.Bmp
{
    public class BmpHeader
    {
        public BmpHeader(byte[] data)
        {
            ParseBytes(data);
        }

        public byte Version { get; private set; }

        [JsonIgnore]
        public uint Length { get; private set; }

        public MessageType Type { get; private set; }

        public void ParseBytes(byte[] data)
        {
            Version = data[0];
            //if (Version != 3)
            //{
            //    throw new Exception("invalid BMP version");
            //}
            Length = data.ToUInt32(1);
            Type = (MessageType) data[5];
        }
    }
}