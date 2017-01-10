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
        public List<OptionalParameter> OptionalParameters { get; private set; }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            OptionalParameters = new List<OptionalParameter>();
            Version = data.First();
            //TODO error if BGP version is not 4
            MyAS = data.ToUInt16(1);
            HoldTime = data.ToUInt16(3);
            Id = new IPAddress(data.Skip(5).Take(4).ToArray());

            var offset = data.Offset + 10;
            var length = (int)data.ElementAt(9);
            data = new ArraySegment<byte>(data.Array, offset, length);
            while (data.Count > 0)
            {
                //TODO if OptParamLength <2 return an error  
                var optParam = OptionalParameter.GetOptionalParameter(data);
                OptionalParameters.Add(optParam);
                offset = data.Offset + optParam.ParameterLength + 2;
                length = data.Count - optParam.ParameterLength - 2;
                data = new ArraySegment<byte>(data.Array, offset, length);
            }
        }
    }
}