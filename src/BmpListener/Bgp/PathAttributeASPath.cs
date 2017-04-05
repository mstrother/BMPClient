using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Bgp
{
    public class PathAttributeASPath : PathAttribute
    {
        public IList<ASPathSegment> ASPaths { get; } = new List<ASPathSegment>();

        public override void Decode(ArraySegment<byte> data)
        {
            for (int i = 0; i < Length;)
            {
                //ValidateASPath(data);

                var segmentType = (ASPathSegment.Type)data.First();
                var asnCount = data.ElementAt(1);
                var asnList = new List<int>();

                //TO DO 2 byte asn data
                //for (int maxOffset = 4 * asnCount + offset; offset < maxOffset;)
                //{
                //    Array.Reverse(data, offset, 4);
                //    var asn = BitConverter.ToInt32(data, offset);
                //    asnList.Add(asn);
                //    offset += 4;
                //}

                var asPathSegment = new ASPathSegment(segmentType, asnList);
                ASPaths.Add(asPathSegment);

                i += 4 * asnCount + 2;
            }
        }

        //public bool ValidateASPath(byte[] data, int offset)
        //{
        //    if (data.Type % 2 != 0 || data.Type < 2)
        //    {
        //        return false;
        //    }
        //    if (data[data.Type] == 0 || data.Array[data.Type] > 4)
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