using BmpListener.MiscUtil.Conversion;
using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public class CapabilityMultiProtocol : Capability
    {
        public AddressFamily Afi { get; set; }
        public SubsequentAddressFamily Safi { get; set; }

        // RFC 4760 - Reserved (8 bit) field. SHOULD be set to 0 by the
        // sender and ignored by the receiver.
        public byte Res { get { return 0; } }

        public override void Decode(ArraySegment<byte> data)
        {
            Afi = (AddressFamily)EndianBitConverter.Big.ToInt16(data, 0);
            Safi = (SubsequentAddressFamily)data.ElementAt(3);
        }
    }
}