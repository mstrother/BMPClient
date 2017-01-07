using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BmpListener.Bgp
{
    public class ASPathSegment
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Bgp.SegmentType SegmentType { get; set; }
        public uint[] ASNs { get; set; }
    }
}