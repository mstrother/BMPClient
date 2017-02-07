using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public abstract class Capability
    {
        //private readonly int length;

        public enum CapabilityCode
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

        protected Capability(ArraySegment<byte> data)
        {
            CapabilityType = (CapabilityCode)data.First();
            Length = data.ElementAt(1);
        }

        public CapabilityCode CapabilityType { get; }
        public int Length { get; }

        public bool ShouldSerializeLength()
        {
            return false;
        }

        public static Capability GetCapability(ArraySegment<byte> data)
        {
            var capabilityType = (CapabilityCode)data.First();
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