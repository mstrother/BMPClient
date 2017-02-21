using BmpListener.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Bgp
{
    public class PathAttributeCommunity : PathAttribute
    {
        public PathAttributeCommunity(ArraySegment<byte> data) : base(ref data)
        {
            DecodeFromByes(data);
        }

        public uint Community { get; private set; }

        public void DecodeFromByes(ArraySegment<byte> data)
        {
            var bytes = data.Reverse().Skip(0).Take(4).ToArray();
            Community = BitConverter.ToUInt32(bytes, 0);
        }
    }
}
