using System.Net;

namespace BmpListener.Serialization.Models
{
    public class PeerHeaderModel
    {
        public string Type { get; set; }
        public IPAddress Address { get; set; }
        public int Asn { get; set; }
        public IPAddress Id { get; set; }
    }
}