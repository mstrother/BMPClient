using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class PathAttributeASPath : PathAttribute
    {
        public PathAttributeASPath(byte[] data, int offset)
            : base(data, offset)
        {
            ASPaths = new List<ASPathSegment>();
            Decode(data, Offset);
        }

        public IList<ASPathSegment> ASPaths { get; }

        protected void Decode(byte[] data, int offset)
        {
            for (int i = 0; i < Length;)
            {
                //ValidateASPath(data);

                var segmentType = (ASPathSegment.Type)data[offset];
                var asnCount = data[offset + 1];
                var asnList = new List<int>();

                //TO DO 2 byte asn data
                offset += 2;
                for (int maxOffset = 4 * asnCount + offset; offset < maxOffset;)
                {
                    Array.Reverse(data, offset, 4);
                    var asn = BitConverter.ToInt32(data, offset);
                    asnList.Add(asn);
                    offset += 4;
                }

                var asPathSegment = new ASPathSegment(segmentType, asnList);
                ASPaths.Add(asPathSegment);

                i += 4 * asnCount + 2;
            }
        }

        //public bool ValidateASPath(byte[] data, int offset)
        //{
        //    if (data.Length % 2 != 0 || data.Length < 2)
        //    {
        //        return false;
        //    }
        //    if (data[data.Length] == 0 || data.Array[data.Length] > 4)
        //    {
        //        return false;
        //    }
        //    var asnCount = data[data.Offset + 1];
        //    if (asnCount == 0)
        //    {
        //        return false;
        //    }
        //    if (2 + asnCount * 4 > data.Count)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
    }
}