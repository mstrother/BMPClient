using System;

namespace BmpListener.Bgp
{
    public class CapabilityFourOctetAsNumber : Capability
    {
        public CapabilityFourOctetAsNumber(byte[] data, int offset) : base(data, offset)
        {
            Decode(data, offset + 2);
        }

        public int Asn { get; private set; }

        public void Decode(byte[] data, int offset)
        {
            Array.Reverse(data, offset, 4);
            Asn = BitConverter.ToInt32(data, offset);
        }
    }
}