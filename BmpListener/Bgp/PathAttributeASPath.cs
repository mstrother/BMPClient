using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class PathAttributeASPath : PathAttribute
    {
        public PathAttributeASPath(ArraySegment<byte> data) : base(data)
        {
            ASPaths = new List<ASPathSegment>();
            Decode(AttributeValue);
        }

        public IList<ASPathSegment> ASPaths { get; }

        protected void Decode(ArraySegment<byte> data)
        {
            int offset = data.Offset;
            int count = Length;

            while (data.Count > 0)
            {
                ValidateASPath(data);

                var pathSegmentType = (ASPathSegment.Type)data.Array[offset];
                offset++;
                var asnCount = data.Array[offset];
                offset++;
                var asns = new List<int>();

                //TO DO 2 byte asn data
                for (int i = 0; i < asnCount; i++)
                {
                    var index = 4 * i + offset;
                    Array.Reverse(data.Array, index, 4);
                    var asn = BitConverter.ToInt32(data.Array, index);
                    asns.Add(asn);
                }

                var asPathSegment = new ASPathSegment(pathSegmentType, asns);
                ASPaths.Add(asPathSegment);

                offset += asnCount * 4;
                count -= (asnCount * 4) + 2;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }
        }

        public bool ValidateASPath(ArraySegment<byte> data)
        {
            if (data.Array.Length % 2 != 0 || data.Count < 2)
            {
                return false;
            }
            if (data.Array[data.Offset] == 0 || data.Array[data.Offset] > 4)
            {
                return false;
            }
            var asnCount = data.Array[data.Offset + 1];
            if (asnCount == 0)
            {
                return false;
            }
            if (2 + asnCount * 4 > data.Count)
            {
                return false;
            }
            return true;
        }
    }
}