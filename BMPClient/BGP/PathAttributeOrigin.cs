using System;
using System.Linq;

namespace BmpListener.BGP
{
    public class PathAttributeOrigin : PathAttribute
    {
        public PathAttributeOrigin(ArraySegment<byte> data) : base(data)
        {
        }

        public BGP.Origin Origin { get; private set; }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            Origin = (BGP.Origin) data.ElementAt(0);
        }
    }
}