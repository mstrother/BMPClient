using System;
using System.Collections.Generic;
using System.Net;
using BmpListener.Utilities;

namespace BmpListener.Bgp
{
    public class BgpOpenMessage : BgpMessage
    {
        public byte Version { get; private set; }
        public int MyAS { get; private set; }
        public int HoldTime { get; private set; }
        public IPAddress BgpIdentifier { get; private set; }
        public int OptionalParametersLength { get; private set; }
        public IList<OptionalParameter> OptionalParameters { get; } = new List<OptionalParameter>();

        public override void Decode(byte[] data, int offset)
        {
            Version = data[offset];
            offset++;

            MyAS = EndianBitConverter.Big.ToUInt16(data, offset);
            offset += 2;

            HoldTime = EndianBitConverter.Big.ToInt16(data, offset);
            offset++;

            var ipBytes = new byte[4];
            Array.Copy(data, offset, ipBytes, 0, 4);
            BgpIdentifier = new IPAddress(ipBytes);

            OptionalParametersLength = data[offset + 9];
            offset++;

            for (var i = 0; i < OptionalParametersLength;)
            {
                if (i < 2)
                {
                    // Malformed BGP Open message
                }

                OptionalParameter optParam;

                switch (data[offset])
                {
                    case 2:
                        optParam = new CapabilitiesParameter();
                        break;
                    default:
                        optParam = new OptionalParameterUnknown();
                        break;
                }

                optParam.Decode(data, offset);
                OptionalParameters.Add(optParam);
                i += optParam.Length + 2;
                offset += optParam.Length + 2;
            }
        }
    }
}