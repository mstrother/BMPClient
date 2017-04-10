using BmpListener.MiscUtil.Conversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace BmpListener.Bgp
{
    public class BgpRouteRefreshMessage : BgpMessage
    {
        public AddressFamily AFI { get; private set; }
        public int Demarcation { get; private set; }
        public SubsequentAddressFamily SAFI { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            AFI = (AddressFamily)EndianBitConverter.Big.ToUInt16(data, offset);
            Demarcation = data[offset + 2];
            SAFI = (SubsequentAddressFamily)data[offset + 3];
        }
    }
}
