using System;

namespace BmpListener.Bgp
{
    public class CapabilityFourOctetAsNumber : Capability
    {
        public CapabilityFourOctetAsNumber(ArraySegment<byte> data) : base(data)
        {
            CapabilityValue = data.ToUInt32(2);
        }

        public uint CapabilityValue { get; }
    }
}