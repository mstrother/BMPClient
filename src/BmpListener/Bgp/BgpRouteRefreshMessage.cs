using BmpListener.MiscUtil.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BmpListener.Bgp
{
    public class BgpRouteRefreshMessage : BgpMessage
    {
        public AddressFamily AFI { get; private set; }
        public int Demarcation { get; private set; }
        public SubsequentAddressFamily SAFI { get; private set; }

        public override void Decode(ArraySegment<byte> data)
        {
            AFI = (AddressFamily)EndianBitConverter.Big.ToUInt16(data, 0);
            Demarcation = data.ElementAt(2);
            SAFI = (SubsequentAddressFamily)data.ElementAt(3);
        }
    }
}
