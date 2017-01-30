namespace BmpListener.Bgp
{
    public class ASPathSegment
    {
        public SegmentType SegmentType { get; set; }
        public int[] ASNs { get; set; }
    }
}