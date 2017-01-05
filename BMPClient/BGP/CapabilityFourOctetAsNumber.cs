using System;

namespace BmpListener.Bgp
{
    public class CapabilityFourOctetAsNumber : Capability
    {
        public CapabilityFourOctetAsNumber(CapabilityCode capability, ArraySegment<byte> data) 
            : base(capability, data)
        {
            CapabilityValue = data.ToUInt32(0);
        }

        public uint CapabilityValue { get; private set; }
    }
}
