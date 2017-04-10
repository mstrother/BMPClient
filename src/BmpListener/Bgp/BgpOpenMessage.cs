using BmpListener.MiscUtil.Conversion;
using System;
using System.Collections.Generic;
using System.Net;

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
            MyAS = EndianBitConverter.Big.ToUInt16(data, offset + 1);
            HoldTime = EndianBitConverter.Big.ToInt16(data, offset + 3);

            var ipBytes = new byte[4];
            Array.Copy(data, offset + 4, ipBytes, 0, 4);
            BgpIdentifier = new IPAddress(ipBytes);

            OptionalParametersLength = data[offset + 9];

            offset += 10;

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