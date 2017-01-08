using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BmpListener.Bgp
{
    public class PathAttributeOrigin : PathAttribute
    {
        public PathAttributeOrigin(ArraySegment<byte> data) : base(ref data)
        {
            Origin = (Origin)data.ElementAt(0);
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Origin Origin { get; private set; }
    }
}