using System;

namespace BmpListener.Bgp
{
    public abstract class Capability
    {
        protected Capability(ArraySegment<byte> data)
        {
            var offset = data.Offset;
            CapabilityType = (CapabilityCode)data.Array[offset];
            offset++;
            CapabilityLength = data.Array[offset];
            offset++;
            CapabilityValue =
                new ArraySegment<byte>(data.Array, offset, CapabilityLength);
        }

        protected ArraySegment<byte> CapabilityValue { get; set; }

        public CapabilityCode CapabilityType { get; }
        public int CapabilityLength { get; }

        public static Capability GetCapability(ArraySegment<byte> data)
        {
            var offset = data.Offset;
            var capabilityType = (CapabilityCode)data.Array[offset];

            switch (capabilityType)
            {
                case CapabilityCode.Multiprotocol:
                    return new CapabilityMultiProtocol(data);
                case CapabilityCode.RouteRefresh:
                    return new CapabilityRouteRefresh(data);
                case CapabilityCode.GracefulRestart:
                    return new CapabilityGracefulRestart(data);
                case CapabilityCode.FourOctetAs:
                    return new CapabilityFourOctetAsNumber(data);
                case CapabilityCode.AddPath:
                    return new CapabilityAddPath(data);
                case CapabilityCode.EnhancedRouteRefresh:
                    return new CapabilityEnhancedRouteRefresh(data);
                case CapabilityCode.CiscoRouteRefresh:
                    return new CapabilityCiscoRouteRefresh(data);
                default:
                    return null;
            }
        }
    }
}