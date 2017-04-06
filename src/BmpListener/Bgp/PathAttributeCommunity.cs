﻿using System;

namespace BmpListener.Bgp
{
    public class PathAttributeCommunity : PathAttribute
    {
        public uint Community { get; private set; }

        public override void Decode(ArraySegment<byte> data)
        {
            Community = BigEndian.ToUInt32(data, 0);
        }
    }
}