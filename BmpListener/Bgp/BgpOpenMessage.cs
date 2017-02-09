using BmpListener.Extensions;
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
        public int MyAS { get; private set; }
        public int HoldTime { get; private set; }
        public IPAddress Id { get; private set; }
        public List<Capability> Capabilities { get; } = new List<Capability>();

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            Version = data.First();
            //TODO error if BGP version is not 4
            MyAS = data.ToInt16(1);
            HoldTime = data.ToInt16(3);
            Id = new IPAddress(data.Skip(5).Take(4).ToArray());

            var offset = data.Offset + 10;
            var count = (int) data.ElementAt(9);
            data = new ArraySegment<byte>(data.Array, offset, count);

            while (data.Count > 0)
            {
                var length = data.ElementAt(1);
                offset += 2;
                count -= 2;
                if (data.First() == 2)
                {
                    var optParamData = new ArraySegment<byte>(data.Array, offset, length);
                    var capabilities = new CapabilitiesOptionalParameter(optParamData);
                    Capabilities.AddRange(capabilities.Capabilities);
                    offset += length;
                    count -= length;
                }
                data = new ArraySegment<byte>(data.Array, offset, count);
            }

            //while (data.Count > 0)
            //{
            //    var paramType = data.First();
            //    length = data.ElementAt(1);
            //    if (paramType == 2)
            //    {
            //        offset += 2;
            //        data = new ArraySegment<byte>(data.Array, offset, length);
            //        var x = new OptionalParameterCapability(data);
            //        OptionalParameters.Add(x);
            //    }
            //    offset += length;
            //    length -= length;
            //    data = new ArraySegment<byte>(data.Array, offset, length);
            //}

            //var optParam = OptionalParameter.GetOptionalParameter(data);
            //OptionalParameters.Add(optParam);
        }


        //}
        //        .ToList();
        //        .Where(y => y.ParameterType == OptionalParameter.Type.Capabilities)
        //    var capabilities = OptionalParameters
        //{

        //private void FlatterOptionalParameters()
    }
}