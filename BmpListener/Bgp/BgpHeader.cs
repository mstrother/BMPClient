using System;
using System.Linq;
using Newtonsoft.Json;
using BmpListener.Extensions;

namespace BmpListener.Bgp
{
    public class BgpHeader
    {
        public BgpHeader(ArraySegment<byte> data)
        {
            Length = data.ToInt16(16);
            Type = (BgpMessage.Type)data.ElementAt(18);
        }
        
        public int Length { get; }
        public BgpMessage.Type Type { get; }
    }
}