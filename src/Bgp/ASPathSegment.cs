using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class ASPathSegment
    {
        public ASPathSegment(Type type, IList<int> asns)
        {
            SegmentType = type;
            ASNs = asns;
        }

        public enum Type
        {
            AS_SET = 1,
            AS_SEQUENCE,
            AS_CONFED_SEQUENCE,
            AS_CONFED_SET
        }

        public Type SegmentType { get; set; }
        public IList<int> ASNs { get; set; }
    }
}