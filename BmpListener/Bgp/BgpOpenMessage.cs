using System;
using System.Collections.Generic;
using System.Net;

namespace BmpListener.Bgp
{
    public sealed class BgpOpenMessage : BgpMessage
    {
        public BgpOpenMessage(BgpHeader bgpHeader, byte[] data, int offset)
            : base(bgpHeader)
        {
            Capabilities = new List<Capability>();
            DecodeFromBytes(data, offset);
        }

        public byte Version { get; private set; }
        public int MyAS { get; private set; }
        public int HoldTime { get; private set; }
        public IPAddress Id { get; private set; }
        public List<Capability> Capabilities { get; }

        public void DecodeFromBytes(byte[] data, int offset)
        {
            Version = data[offset];

            Array.Reverse(data, offset + 1, 2);
            MyAS = BitConverter.ToUInt16(data, offset + 1);

            Array.Reverse(data, offset + 3, 2);
            HoldTime = BitConverter.ToInt16(data, offset + 3);

            var ipBytes = new byte[4];
            Array.Copy(data, offset + 5, ipBytes, 0, 4);
            Id = new IPAddress(ipBytes);

            var optionalParametersLength = (int)data[offset + 9];

            for (int i = 0; i < optionalParametersLength;)
            {
                if (i < 2)
                {
                    // Malformed BGP Open message
                }
                var paramType = data[offset + 10];
                var paramLength = data[offset + 11];
                if (i < paramLength + 2)
                {
                    // Malformed BGP Open message
                }
                {
                    // use enum for clarity
                    if (paramType == 2)
                    {
                        offset += 12 + i;
                        var capabilities = new CapabilitiesOptionalParameter(data, offset, paramLength);
                        Capabilities.AddRange(capabilities.Capabilities);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }                
                i += paramLength + 2;
            }
        }
    }
}