using BmpListener.Utilities;

namespace BmpListener.Bgp
{
    // RFC 6793
    public class CapabilityFourOctetAsNumber : Capability
    {
        public int Asn { get; private set; }

        public override void Decode(byte[] data, int offset)
        {
            Asn = EndianBitConverter.Big.ToInt32(data, offset);
        }
    }
}