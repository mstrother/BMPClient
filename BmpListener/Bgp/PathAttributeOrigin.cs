using System;

namespace BmpListener.Bgp
{
    public class PathAttributeOrigin : PathAttribute
    {
        public PathAttributeOrigin(byte[] data, int offset) 
            : base(data, offset)
        {
            Decode(data, Offset);
        }

        public enum Type
        {
            IGP,
            EGP,
            Incomplete
        }

        public Type Origin { get; private set; }

        protected void Decode(byte[] data, int offset)
        {
            Origin = (Type)data[offset];
        }
    }
}