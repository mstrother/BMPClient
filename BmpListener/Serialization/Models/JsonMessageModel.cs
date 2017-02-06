using BmpListener.Bmp;
using BmpListener.Serialization.Models;
using System;

namespace BmpListener.Serialization.Models
{
    public class JsonMessageModel
    {
        public JsonMessageModel(BmpMessage msg)
        {
            Version = "0.0.2";
            Id = Guid.NewGuid().ToString("N");
            DateTime = msg?.PeerHeader?.DateTime ?? DateTime.UtcNow;

            if (msg.PeerHeader != null)
            {
                Peer = new BmpPeerHeaderModel(msg.PeerHeader);
            }

            switch (msg.BmpHeader.Type)
            {
                case (MessageType.Initiation):
                    Initiation = Initiation;
                    break;
                case (MessageType.PeerUp):
                    PeerUp = (PeerUpNotification)msg;
                    break;
                case (MessageType.PeerDown):
                    PeerDown = (PeerDownNotification)msg;
                    break;
                case (MessageType.RouteMonitoring):
                    Update = new BgpUpdateModel((RouteMonitoring)msg);
                    break;
                case (MessageType.Termination):
                    break;
                default:
                    break;
            }
        }

        public string Version { get; set; }
        public string Id { get; private set; }
        public DateTime DateTime { get; set; }
        public BmpPeerHeaderModel Peer { get; set; }
        public BgpUpdateModel Update { get; set; }
        public PeerUpNotification PeerUp { get; set; }
        public PeerDownNotification PeerDown { get; set; }
        public BmpInitiation Initiation { get; set; }
    }
}