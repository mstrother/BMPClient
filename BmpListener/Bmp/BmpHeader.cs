using System;
using Newtonsoft.Json;
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
            Type = (MessageType)data.ElementAt(5);
        }

        public byte Version { get; private set; }

        [JsonIgnore]
        public uint Length { get; private set; }

        public MessageType Type { get; private set; }
    }
}