using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Bgp
{
    public class PathAttributeASPath : PathAttribute
    {
        public PathAttributeASPath(ArraySegment<byte> data) : base(data)
        {
        }

        public ASPathSegment[] ASPaths { get; private set; }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            if (data.Count < 2)
                Environment.Exit(0);

            var asPathSegments = new List<ASPathSegment>();

            for (var offset = 0; offset < Length;)
            {
                var asPathSegment = new ASPathSegment {SegmentType = (Bgp.SegmentType) data.ElementAt(offset)};
                offset++;
                asPathSegment.Length = data.ElementAt(offset);
                offset++;
                offset += asPathSegment.Length * 4;

                //if (data.Length - 2 < asPathSegment.Length * 4)
                //{
                //    param bmpHeader length is too short
                //    Environment.Exit(0);
                //}

                var asList = new uint[asPathSegment.Length];

                for (var i = 0; i < asPathSegment.Length; i++)
                {
                    var asNum = data.ToUInt32(4 * i + 2);
                    asList[i] = asNum;
                }

                asPathSegment.ASNs = asList;
                asPathSegments.Add(asPathSegment);
            }

            ASPaths = asPathSegments.ToArray();
        }
    }
}