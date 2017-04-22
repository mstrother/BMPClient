namespace BmpListener.Bgp
{
    public abstract class Capability
    {
        public CapabilityCode Code { get; protected set; }
        public int Length { get; protected set; }
        
        public virtual void Decode(byte[] data, int offset)
        {
        }

        public static (Capability capability, int length) DecodeCapability(byte[] data, int offset)
        {
            Capability capability;

            var capabilityType = (CapabilityCode)data[offset];

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
                    capability = new CapabilityUnknown();
                    break;
            }

            capability.Code = capabilityType;
            capability.Length = data[offset + 1];

            if (capability.Length > 0)
            {
                offset += 2;
                capability.Decode(data, offset);
            }

            return (capability, capability.Length + 2);
        }
    }
}