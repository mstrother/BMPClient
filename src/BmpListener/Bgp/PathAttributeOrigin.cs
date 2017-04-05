using System;
using System.Linq;

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

        public override void Decode(ArraySegment<byte> data)
        {
            Origin = (Type) data.First();
        }
    }
}