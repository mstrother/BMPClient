using BmpListener.MiscUtil.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public override void Decode(ArraySegment<byte> data)
        {
            Version = data.First();
            MyAS = EndianBitConverter.Big.ToUInt16(data, 1);
            HoldTime = EndianBitConverter.Big.ToInt16(data, 3);

            var ipBytes = new byte[4];
            Array.Copy(data.Array, data.Offset + 4, ipBytes, 0, 4);
            BgpIdentifier = new IPAddress(ipBytes);

            OptionalParametersLength = data.ElementAt(9);

            var offset = data.Offset + 10;
            var count = OptionalParametersLength;
            data = new ArraySegment<byte>(data.Array, offset, count);

            for (var i = 0; i < OptionalParametersLength;)
            {
                if (i < 2)
                {
                    // Malformed BGP Open message
                }

                OptionalParameter optParam;

                switch (data.First())
                {
                    case 2:
                        optParam = new CapabilitiesParameter();
                        break;
                    default:
                        optParam = new OptionalParameterUnknown();
                        break;
                }

                optParam.Decode(data);
                OptionalParameters.Add(optParam);
                i += optParam.Length + 2;
                offset += optParam.Length + 2;
                count -= optParam.Length + 2;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }
        }
    }
}