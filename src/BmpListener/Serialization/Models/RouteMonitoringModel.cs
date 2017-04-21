using BmpListener.Bgp;
using System;
using System.Collections.Generic;
using System.Text;

namespace BmpListener.Serialization.Models
{
    public class RouteMonitoringModel
    {
        public PeerHeaderModel Peer { get; set; }
        public string Origin { get; set; }
        public string Community { get; set; }
        public IList<int> AsPath { get; set; }
        public IList<PrefixAnnounceModel> Announce { get; set; } = new List<PrefixAnnounceModel>();
        public IList<PrefixWithdrawal> Withdraw { get; set; } = new List<PrefixWithdrawal>();

        public bool ShouldSerializeAnnounce()
        {
            return Announce.Count > 0;
        }

        public bool ShouldSerializeWithdraw()
        {
            return Withdraw.Count > 0;
        }
    }
}
