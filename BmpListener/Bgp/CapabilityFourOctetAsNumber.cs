using System;

namespace BmpListener.Bgp
{
    public class CapabilityFourOctetAsNumber : Capability
    {
        public CapabilityFourOctetAsNumber(ArraySegment<byte> data) : base(data)
        {
            Decode(CapabilityValue);
        }

        public int Asn { get; private set; }
        
        public void Decode(ArraySegment<byte> data)
        {
            Array.Reverse(CapabilityValue.Array, data.Offset, 4);
            Asn = BitConverter.ToInt32(data.Array, data.Offset);
        }
    }
}