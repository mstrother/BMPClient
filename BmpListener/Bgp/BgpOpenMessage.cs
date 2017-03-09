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
            OptionalParameters = new List<OptionalParameter>();
            DecodeFromBytes(data, offset);
        }

        public byte Version { get; private set; }
        public int MyAS { get; private set; }
        public int HoldTime { get; private set; }
        public IPAddress Id { get; private set; }
        public IList<OptionalParameter> OptionalParameters { get; }

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

            var optionalParametersLength = data[offset + 9];

            offset += 10;

            for (int i = 0; i < optionalParametersLength;)
            {
                if (i < 2)
                {
                    // Malformed BGP Open message
                }

                OptionalParameter optParam;
                if (data[offset] == 2)
                {
                    optParam = new CapabilitiesOptionalParameter(data, offset);
                }
                else
                {
                    optParam = new CapabilitiesOptionalParameter(data, offset);
                }
                OptionalParameters.Add(optParam);

                offset += optParam.ParameterLength + 2;
                i += optParam.ParameterLength + 2;
            }
        }
    }
}