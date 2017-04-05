using System;

namespace BmpListener.Bgp
{
    // RFC 6793
    public class CapabilityFourOctetAsNumber : Capability
    {
        public int Asn { get; private set; }

        public override void Decode(ArraySegment<byte> data)
        {
            Asn = BigEndian.ToInt32(data, 0);
        }
    }
}