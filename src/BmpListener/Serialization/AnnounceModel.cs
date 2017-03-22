using BmpListener.Bgp;
using System.Collections.Generic;

namespace BmpListener.Serialization
{
    public class AnnounceModel
    {
        public AnnounceModel(PathAttributeMPReachNLRI mpReachNlri)
        {
            Routes = new List<string>();

            AddressFamily = GetAddressFamily(mpReachNlri.AFI, mpReachNlri.SAFI);
            NextHop = mpReachNlri.NextHop?.ToString();
            for (int i = 0; i < mpReachNlri.NLRI.Count; i++)
            {
                var prefix = mpReachNlri.NLRI[i].ToString();
                Routes.Add(prefix);
            }
        }

        public AnnounceModel(IList<IPAddrPrefix> prefixes, PathAttributeNextHop nextHop)
        {
            Routes = new List<string>();

            AddressFamily = GetAddressFamily(Bgp.AddressFamily.IP, SubsequentAddressFamily.Unicast);
            NextHop = nextHop?.NextHop.ToString();
            for (int i = 0; i < prefixes.Count; i++)
            {
                var prefix = prefixes[i].ToString();
                Routes.Add(prefix);
            }
        }

        public string AddressFamily { get; }
        public string NextHop { get; }
        public IList<string> Routes { get; }

        public string GetAddressFamily(AddressFamily afi, SubsequentAddressFamily safi)
        {
            if (afi == Bgp.AddressFamily.IP && safi == SubsequentAddressFamily.Unicast)
            {
                return "IPv4 Unicast";
            }
            else if (afi == Bgp.AddressFamily.IP6 && safi == SubsequentAddressFamily.Unicast)
            {
                return "IPv6 Unicast";
            }
            else if (afi == Bgp.AddressFamily.IP && safi == SubsequentAddressFamily.Multicast)
            {
                return "IPv4 Multicast";
            }
            else if (afi == Bgp.AddressFamily.IP6 && safi == SubsequentAddressFamily.Multicast)
            {
                return "IPv6 Multicast";
            }
            return null;
        }
    }
}
