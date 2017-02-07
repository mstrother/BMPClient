using BmpListener.Bmp;
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

            switch (msg.BmpHeader.MessageType)
            {
                case (BmpMessage.Type.Initiation):
                    Initiation = Initiation;
                    break;
                case (BmpMessage.Type.PeerUp):
                    PeerUp = (PeerUpNotification)msg;
                    break;
                case (BmpMessage.Type.PeerDown):
                    PeerDown = (PeerDownNotification)msg;
                    break;
                case (BmpMessage.Type.RouteMonitoring):
                    Update = new BgpUpdateModel((RouteMonitoring)msg);
                    break;
                case (BmpMessage.Type.Termination):
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