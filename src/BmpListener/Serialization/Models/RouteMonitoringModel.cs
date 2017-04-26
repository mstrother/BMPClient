using System.Collections.Generic;
using BmpListener.Bgp;

namespace BmpListener.Serialization.Models
{
    public class RouteMonitoringModel
    {
        public PeerHeaderModel Peer { get; set; }
        public PathAttributeOrigin Origin { get; set; }
        public IList<string> Communities { get; set; } = new List<string>();
        public IList<int> AsPath { get; set; }
        public IList<PrefixAnnounceModel> Announce { get; set; } = new List<PrefixAnnounceModel>();
        public IList<PrefixWithdrawal> Withdraw { get; set; } = new List<PrefixWithdrawal>();

        public bool ShouldSerializeCommunities() => Announce.Count > 0;

        public bool ShouldSerializeAnnounce() => Announce.Count > 0;

        public bool ShouldSerializeWithdraw() => Withdraw.Count > 0;
    }
}
