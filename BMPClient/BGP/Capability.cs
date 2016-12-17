using System;

namespace BMPClient.BGP
{
    public abstract class Capability
    {
        public enum CapabilityCode : byte
        {
            Multiprotocol = 1,
            RouteRefresh = 2,
            //TODO capability code 4
            GracefulRestart = 64,
            FourOctetAs = 65,
            AddPath = 69,
            EnhancedRouteRefresh = 70,
            CiscoRouteRefresh = 128
            //TODO capability code 129
        }

        protected Capability(CapabilityCode capability, ArraySegment<byte> data)
        {
            CapabilityType = capability;
            CapabilityLength = (byte) data.Count;
        }

        public CapabilityCode CapabilityType { get; }
        public byte CapabilityLength { get; }

        public static Capability GetCapability(CapabilityCode capabilityCode, ArraySegment<byte> data)
        {
            switch (capabilityCode)
            {
                case CapabilityCode.Multiprotocol:
                    return new CapabilityMultiProtocol(capabilityCode, data);
                case CapabilityCode.RouteRefresh:
                    return new CapabilityRouteRefresh(capabilityCode, data);
                case CapabilityCode.GracefulRestart:
                    return new CapabilityGracefulRestart(capabilityCode, data);
                case CapabilityCode.FourOctetAs:
                    return new CapabilityFourOctetAsNumber(capabilityCode, data);
                case CapabilityCode.AddPath:
                    return new CapabilityAddPath(capabilityCode, data);
                case CapabilityCode.EnhancedRouteRefresh:
                    return new CapabilityEnhancedRouteRefresh(capabilityCode, data);
                case CapabilityCode.CiscoRouteRefresh:
                    return new CapabilityCiscoRouteRefresh(capabilityCode, data);
                default:
                    return new CapabilityCiscoRouteRefresh(capabilityCode, data);
            }
        }
    }
}