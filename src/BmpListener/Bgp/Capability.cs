using System;
using System.Linq;

namespace BmpListener.Bgp
{
    public abstract class Capability
    {
        public CapabilityCode Code { get; protected set; }
        public int Length { get; protected set; }
        
        public virtual void Decode(ArraySegment<byte> data)
        {
        }

        public static (Capability capability, int length) DecodeCapability(ArraySegment<byte> data)
        {
            Capability capability;

            var capabilityType = (CapabilityCode)data.First();

            switch (capabilityType)
            {
                case CapabilityCode.Multiprotocol:
                    capability = new CapabilityMultiProtocol();
                    break;
                case CapabilityCode.RouteRefresh:
                    capability = new CapabilityRouteRefresh();
                    break;
                case CapabilityCode.GracefulRestart:
                    capability = new CapabilityGracefulRestart();
                    break;
                case CapabilityCode.FourOctetAs:
                    capability = new CapabilityFourOctetAsNumber();
                    break;
                case CapabilityCode.AddPath:
                    capability = new CapabilityAddPath();
                    break;
                case CapabilityCode.EnhancedRouteRefresh:
                    capability = new CapabilityEnhancedRouteRefresh();
                    break;
                case CapabilityCode.CiscoRouteRefresh:
                    capability = new CapabilityCiscoRouteRefresh();
                    break;
                default:
                    throw new Exception();
            }

            capability.Code = capabilityType;
            capability.Length = data.ElementAt(1);

            if (capability.Length > 0)
            {
                data = new ArraySegment<byte>(data.Array, data.Offset + 2, capability.Length);
                capability.Decode(data);
            }

            return (capability, capability.Length + 2);
        }
    }
}