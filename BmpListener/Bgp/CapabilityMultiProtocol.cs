using System;

namespace BmpListener.Bgp
{
    public class CapabilityMultiProtocol : Capability
    {
        public CapabilityMultiProtocol(ArraySegment<byte> data) : base(data)
        {
            Decode(CapabilityValue);
        }

        public AddressFamily Afi { get; set; }
        public SubsequentAddressFamily Safi { get; set; }

        // RFC 4760 - Reserved (8 bit) field. SHOULD be set to 0 by the
        // sender and ignored by the receiver.
        public byte Res { get { return 0; } }

        public void Decode(ArraySegment<byte> data)
        {
            Array.Reverse(CapabilityValue.Array, data.Offset, 2);
            Afi = (AddressFamily)BitConverter.ToInt16(data.Array, data.Offset);
            Safi = (SubsequentAddressFamily)data.Array[data.Offset + 3];
        }
    }
}