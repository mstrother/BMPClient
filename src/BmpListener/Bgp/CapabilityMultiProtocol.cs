using BmpListener.Utilities;

namespace BmpListener.Bgp
{
    // RFC 4760
    public class CapabilityMultiProtocol : Capability
    {
        public AddressFamily Afi { get; set; }
        public SubsequentAddressFamily Safi { get; set; }
        public byte Reserved { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            Afi = (AddressFamily)EndianBitConverter.Big.ToInt16(data, offset);
            Reserved = data[offset + 2];
            Safi = (SubsequentAddressFamily)data[offset + 3];
        }
    }
}