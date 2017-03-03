using System;

namespace BmpListener.Bgp
{
    public class PathAttributeOrigin : PathAttribute
    {
        public PathAttributeOrigin(ArraySegment<byte> data) : base(data)
        {
            Decode(AttributeValue);
        }

        public enum Type
        {
            IGP,
            EGP,
            Incomplete
        }

        public Type Origin { get; private set; }

        protected void Decode(ArraySegment<byte> data)
        {
            Origin = (Type)data.Array[data.Offset];
        }
    }
}