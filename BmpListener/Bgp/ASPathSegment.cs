namespace BmpListener.Bgp
{
    public class ASPathSegment
    {
        public ASPathSegment(Type type, int[] asns)
        {
            SegmentType = Type;
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
        public int[] ASNs { get; set; }
    }
}