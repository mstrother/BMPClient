using System.Collections.Generic;

namespace BmpListener.Serialization.Models
{
    public class UpdateModel
    {
        public PeerHeaderModel Peer { get; set; }
        public string Origin { get; set; }
        public IList<int> AsPath { get; set; }
        public IList<AnnounceModel> Announce { get; set; }
        public IList<WithdrawModel> Withdraw { get; set; }
    }
}
