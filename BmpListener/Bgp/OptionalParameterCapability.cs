using System;
using System.Collections.Generic;
using System.Linq;

namespace BmpListener.Bgp
{
    public class OptionalParameterCapability : OptionalParameter
    {
        public OptionalParameterCapability(ArraySegment<byte> data) : base(ref data)
        {
            Decode(data);
        }

        public Capability[] Capabilities { get; private set; }

        public void Decode(ArraySegment<byte> data)
        {
            var capabilities = new List<Capability>();

            //var offset = data.Offset + 10;
            //var length = (int)data.ElementAt(9);
            //data = new ArraySegment<byte>(data.Array, offset, length);
            //while (data.Count > 0)
            //{
            //    //TODO if OptParamLength <2 return an error  
            //    var optParam = OptionalParameter.GetOptionalParameter(data);
            //    OptionalParameters.Add(optParam);
            //    offset = data.Offset + optParam.ParameterLength + 2;
            //    length = data.Count - optParam.ParameterLength - 2;
            //    data = new ArraySegment<byte>(data.Array, offset, length);
            //}

            for (var i = 0; i < ParameterLength;)
            {
                var type = (Capability.CapabilityCode) data.First();
                i++;
                var length = data.ElementAt(i);
                i++;
                data = new ArraySegment<byte>(bytes, i, length);
                var capability = Capability.GetCapability(type, data);
                if (capability != null)
                    capabilities.Add(capability);
                i += length;
            }
            Capabilities = capabilities.ToArray();
        }
    }
}