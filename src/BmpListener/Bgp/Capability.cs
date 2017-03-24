using System;

namespace BmpListener.Bgp
{
    public abstract class Capability
    {
        protected Capability(byte[] data, int offset)
        {
            CapabilityType = (CapabilityCode)data[offset];
            CapabilityLength = data[offset + 1];
        }
        
        public CapabilityCode CapabilityType { get; }
        public int CapabilityLength { get; }

        public static Capability GetCapability(byte[] data, int offset)
        {            
            switch ((CapabilityCode)data[offset])
            {
                case CapabilityCode.Multiprotocol:
                    return new CapabilityMultiProtocol(data, offset);
                case CapabilityCode.RouteRefresh:
                    return new CapabilityRouteRefresh(data, offset);
                case CapabilityCode.GracefulRestart:
                    return new CapabilityGracefulRestart(data, offset);
                case CapabilityCode.FourOctetAs:
                    return new CapabilityFourOctetAsNumber(data, offset);
                case CapabilityCode.AddPath:
                    return new CapabilityAddPath(data, offset);
                case CapabilityCode.EnhancedRouteRefresh:
                    return new CapabilityEnhancedRouteRefresh(data, offset);
                case CapabilityCode.CiscoRouteRefresh:
                    return new CapabilityCiscoRouteRefresh(data, offset);
                default:
                    return null;
            }
        }
    }
}