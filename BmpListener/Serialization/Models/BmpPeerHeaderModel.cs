using BmpListener.Bmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BmpListener.Serialization.Models
{
    public class BmpPeerHeaderModel
    {
        public BmpPeerHeaderModel(BmpPeerHeader peerHeader)
        {
            Asn = peerHeader.AS;
            Ip = peerHeader.PeerAddress;
            Id = peerHeader.PeerBGPId;
            Distinguisher = peerHeader.PeerDistinguisher;
            Type = peerHeader.Type;
            PostPolicy = peerHeader.IsPostPolicy;
        }

        public uint Asn { get; set; }
        public IPAddress Ip { get; set; }
        public IPAddress Id { get; set; }
        public ulong Distinguisher { get; set; }
        public PeerType Type { get; set; }
        public bool PostPolicy { get; set; }
    }
}
