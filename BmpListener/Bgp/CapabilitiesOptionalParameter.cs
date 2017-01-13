using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Bgp
{
    public class CapabilitiesOptionalParameter 
    {
        public CapabilitiesOptionalParameter(ArraySegment<byte> data)
        {
            DecodeFromBytes(data);
        }
        
        public List<Capability> Capabilities { get; } = new List<Capability>();

        public void DecodeFromBytes(ArraySegment<byte> data)
        {
            var offset = data.Offset;
            var count = data.Count;
            while (data.Count > 0)
            {
                var capability = Capability.GetCapability(data);
                Capabilities.Add(capability);
                var length = capability.Length + 2;
                offset += length;
                count -= length;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }
        }
    }
}