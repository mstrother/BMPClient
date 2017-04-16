namespace BmpListener.Serialization.Models
{
    public class PeerUpModel
    {
        public PeerHeaderModel Peer { get; set; }
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }
        public BgpOpenModel SentOpenMessage { get; set; }
        public BgpOpenModel ReceivedOpenMessage { get; set; }
    }
}
