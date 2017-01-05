using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BmpListener.Bgp
{
    public class PathAttributeOrigin : PathAttribute
    {
        public PathAttributeOrigin(ArraySegment<byte> data) : base(data)
        {
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Origin Origin { get; private set; }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            Origin = (Origin) data.ElementAt(0);
        }
    }
}