using System;
using System.Linq;

namespace BmpListener.BGP
{
    public class BGPHeader
    {
        public BGPHeader(ArraySegment<byte> data)
        {
            Length = data.ToUInt16(16);
            Type = (BGP.MessageType)data.ElementAt(18);
        }
        
        public uint Length { get; }
        public BGP.MessageType Type { get; }
    }
}