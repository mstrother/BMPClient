namespace BmpListener.Serialization.Models
{
    public class PeerUpModel
    {
        public PeerHeaderModel Peer { get; set; }
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }
    }
}
