using System;

namespace BmpListener.Bgp
{
    public class CapabilityMultiProtocol : Capability
    {
        public CapabilityMultiProtocol(byte[] data, int offset) 
            : base(data, offset)
        {
            Decode(data, offset + 2);
        }

        public AddressFamily Afi { get; set; }
        public SubsequentAddressFamily Safi { get; set; }

        // RFC 4760 - Reserved (8 bit) field. SHOULD be set to 0 by the
        // sender and ignored by the receiver.
        public byte Res { get { return 0; } }

        public void Decode(byte[] data, int offset)
        {
            Array.Reverse(data, offset, 2);
            Afi = (AddressFamily)BitConverter.ToInt16(data, offset);
            Safi = (SubsequentAddressFamily)data[offset + 3];
        }
    }
}