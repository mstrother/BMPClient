using System;
using System.Linq;
using Newtonsoft.Json;

namespace BmpListener.Bgp
{
    public class BgpHeader
    {
        public BgpHeader(ArraySegment<byte> data)
        {
            Length = data.ToUInt16(16);
            Type = (MessageType)data.ElementAt(18);
        }
        
        [JsonIgnore]
        public uint Length { get; }
        public MessageType Type { get; }
    }
}