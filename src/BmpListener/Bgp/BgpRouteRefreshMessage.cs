using BmpListener.MiscUtil.Conversion;

namespace BmpListener.Bgp
{
    public class BgpRouteRefreshMessage : BgpMessage
    {
        public AddressFamily Afi { get; private set; }
        public int Demarcation { get; private set; }
        public SubsequentAddressFamily Safi { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            Afi = (AddressFamily)EndianBitConverter.Big.ToUInt16(data, offset);
            Demarcation = data[offset + 2];
            Safi = (SubsequentAddressFamily)data[offset + 3];
        }
    }
}
