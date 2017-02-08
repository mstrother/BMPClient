using BmpListener.Bgp;

namespace BmpListener.Extensions
{
    public static class EnumExtension
    {
        public static string ToFriendlyString(this AddressFamily afi)
        {
            switch (afi)
            {
                case AddressFamily.IP:
                    return "IPv4";
                case AddressFamily.IP6:
                    return "IPv6";
                case AddressFamily.L2VPN:
                    return "L2VPN";
                default:
                    return null;
            }
        }

        public static string ToFriendlyString(this SubsequentAddressFamily safi)
        {
            switch(safi)
            {
                case SubsequentAddressFamily.Multicast:
                        return "Multicast";
                case SubsequentAddressFamily.Unicast:
                    return "Unicast";
                default:
                    return null;
            }
        }
    }
}
