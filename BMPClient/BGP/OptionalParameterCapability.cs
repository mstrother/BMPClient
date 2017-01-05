using System;
using System.Collections.Generic;

namespace BmpListener.Bgp
{
    public class OptionalParameterCapability : OptionalParameter
    {
        public OptionalParameterCapability(ArraySegment<byte> data) : base(data)
        {
            Decode(data);
        }

        public Capability[] ParameterValue { get; private set; }

        public void Decode(ArraySegment<byte> data)
        {
            var capabilities = new List<Capability>();
            var bytes = new byte[ParameterLength];
            Array.Copy(data.Array, data.Offset + 2, bytes, 0, ParameterLength);

            for (var i = 0; i < ParameterLength;)
            {
                var type = (Capability.CapabilityCode) bytes[i];
                i++;
                var length = bytes[i];
                i++;
                data = new ArraySegment<byte>(bytes, i, length);
                var capability = Capability.GetCapability(type, data);
                capabilities.Add(capability);
                i += length;
            }
            ParameterValue = capabilities.ToArray();
        }
    }
}