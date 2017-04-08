using BmpListener.MiscUtil.Conversion;
using System;
using System.Linq;

namespace BmpListener.Bgp
{
    // RFC 4760
    public class CapabilityMultiProtocol : Capability
    {
        public AddressFamily Afi { get; set; }
        public SubsequentAddressFamily Safi { get; set; }
        public byte Reserved { get; private set; }

        public override void Decode(ArraySegment<byte> data)
        {
            Afi = (AddressFamily)EndianBitConverter.Big.ToInt16(data, 0);
            Reserved = data.ElementAt(2);
            Safi = (SubsequentAddressFamily)data.ElementAt(3);
        }
    }
}