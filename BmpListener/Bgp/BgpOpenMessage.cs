using System;
using System.Collections.Generic;
using System.Net;

namespace BmpListener.Bgp
{
    public sealed class BgpOpenMessage : BgpMessage
    {
        public BgpOpenMessage(ArraySegment<byte> data) : base(data)
        {
            Capabilities = new List<Capability>();
            DecodeFromBytes(MessageData);
        }

        public byte Version { get; private set; }
        public int MyAS { get; private set; }
        public int HoldTime { get; private set; }
        public IPAddress Id { get; private set; }
        public List<Capability> Capabilities { get; }

        public override void DecodeFromBytes(ArraySegment<byte> data)
        {
            var offset = data.Offset;
            Version = data.Array[offset];
            offset++;

            Array.Reverse(data.Array, offset, 2);
            MyAS = BitConverter.ToUInt16(data.Array, offset);
            offset += 2;

            Array.Reverse(data.Array, offset, 2);
            HoldTime = BitConverter.ToInt16(data.Array, offset);
            offset += 2;
            
            var ipBytes = new byte[4];
            Array.Copy(data.Array, offset, ipBytes, 0, 4);
            Id = new IPAddress(ipBytes);
            offset += 4;

            var optionalParametersLength = (int)data.Array[offset];
            offset++;

            data = new ArraySegment<byte>(data.Array, offset, optionalParametersLength);

            while (data.Count > 0)
            {
                var paramType = data.Array[offset];
                offset++;
                var paramLength = data.Array[offset];
                offset++;
                // use enum for clarity
                if (paramType == 2)
                {
                    var optParamData = new ArraySegment<byte>(data.Array, offset, paramLength);
                    var capabilities = new CapabilitiesOptionalParameter(optParamData);
                    Capabilities.AddRange(capabilities.Capabilities);
                    offset += paramLength;
                    var count = optionalParametersLength - (paramLength + 2);
                    data = new ArraySegment<byte>(data.Array, offset, count);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }
    }
}