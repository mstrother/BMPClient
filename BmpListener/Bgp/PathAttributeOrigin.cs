using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public class PathAttributeOrigin : PathAttribute
    {
        public PathAttributeOrigin(ArraySegment<byte> data) : base(ref data)
        {
            Origin = (Origin)data.ElementAt(0);
        }

        public Origin Origin { get; private set; }
    }
}