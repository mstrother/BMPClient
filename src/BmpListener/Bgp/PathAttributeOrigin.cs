namespace BmpListener.Bgp
{
    public class PathAttributeOrigin : PathAttribute
    {
        public enum Type
        {
            Igp,
            Egp,
            Incomplete
        }

        public Type Origin { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            Origin = (Type)data[offset];
        }
    }
}