using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BmpListener.Bgp
{
    public sealed class BgpOpenMessage : BgpMessage
    {
        public BgpOpenMessage(ArraySegment<byte> data) : base(ref data)
        {
            DecodeFromBytes(data);
        }

        public byte Version { get; private set; }
        public ushort MyAS { get; private set; }
        public ushort HoldTime { get; private set; }
        public IPAddress Id { get; private set; }
        public byte OptParamsLength { get; private set; }
        public List<OptionalParameter> OptionalParameters { get; private set; }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            OptionalParameters = new List<OptionalParameter>();
            Version = data.First();
            //TODO error if BGP version is not 4
            MyAS = data.ToUInt16(1);
            HoldTime = data.ToUInt16(3);
            Id = new IPAddress(data.Skip(5).Take(4).ToArray());
            OptParamsLength = data.ElementAt(9);

            data = new ArraySegment<byte>(data.Array, 29, OptParamsLength);

            //TODO fix offset counter
            for (var offset = 0; offset < OptParamsLength;)
            {
                //TODO if OptParamLength <2 return an error                                         
                var length = data.ElementAt(offset + 1);
                data = new ArraySegment<byte>(data.Array, data.Offset, length + 2);
                var optParam = OptionalParameter.GetOptionalParameter(data);
                OptionalParameters.Add(optParam);
                offset += length + 2;
            }
        }
    }
}