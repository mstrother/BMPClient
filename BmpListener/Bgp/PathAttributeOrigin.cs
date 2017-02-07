using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public class PathAttributeOrigin : PathAttribute
    {
        public PathAttributeOrigin(ArraySegment<byte> data) : base(ref data)
        {
            Origin = (Type)data.ElementAt(0);
        }

        public enum Type
        {
            IGP,
            EGP,
            Incomplete
        }

        public Type Origin { get; }
    }
}