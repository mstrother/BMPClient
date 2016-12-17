using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMPClient;

namespace BMPClient.BGP
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
