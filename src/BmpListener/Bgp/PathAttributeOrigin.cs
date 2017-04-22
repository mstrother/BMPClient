namespace BmpListener.Bgp
{
    public class PathAttributeOrigin : PathAttribute
    {
        public enum Type
        {
            IGP,
            EGP,
            Incomplete
        }

        public Type Origin { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            Origin = (Type)data[offset];
        }
    }
}