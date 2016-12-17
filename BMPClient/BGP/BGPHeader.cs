using System;
using System.Linq;
using Newtonsoft.Json;

namespace BMPClient.BGP
{
    public class BGPHeader
    {
        public BGPHeader(ArraySegment<byte> data)
        {
            Length = data.ToUInt16(16);
            Type = (BGP.MessageType)data.ElementAt(18);
        }

        [JsonIgnore]
        public byte[] Marker { get; }

        public uint Length { get; }
        public BGP.MessageType Type { get; }
    }
}