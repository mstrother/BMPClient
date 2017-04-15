using System;

namespace BmpListener.Serialization.Models
{
    public class PeerHeaderModel
    {
        public string Type { get; set; }
        public string Address { get; set; }
        public int Asn { get; set; }
        public string Id { get; set; }
        public DateTime Time { get; set; }
    }
}