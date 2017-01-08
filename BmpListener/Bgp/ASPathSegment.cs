namespace BmpListener.Bgp
{
    public class ASPathSegment
    {
        public SegmentType SegmentType { get; set; }
        public uint[] ASNs { get; set; }
    }
}