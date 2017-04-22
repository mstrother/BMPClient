using BmpListener.MiscUtil.Conversion;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class PathAttributeASPath : PathAttribute
    {
        public IList<ASPathSegment> ASPaths { get; } = new List<ASPathSegment>();

        public override void Decode(byte[] data, int offset)
        {
            for (var i = 0; i < Length;)
            {
                //ValidateASPath(data);

                var segmentType = (ASPathSegment.Type)data[offset];
                var asCount = data[offset + 1];
                offset += 2;

                var asList = new List<int>();

                //TO DO 2 byte asn data
                for (var j = 0; j < asCount;)
                {
                    var asn = EndianBitConverter.Big.ToInt32(data, j * 4 + offset);
                    asList.Add(asn);
                    j++;
                }

                var asPathSegment = new ASPathSegment(segmentType, asList);
                ASPaths.Add(asPathSegment);

                offset += 4 * asCount + 2;
                i += offset;
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