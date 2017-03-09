using BmpListener.Bgp;
using BmpListener.Bmp;
using Newtonsoft.Json;
using System;
using System.Net;

namespace BmpListener.Serialization.Models
{
    public abstract class JsonMessage
    {
        public JsonMessage(BmpMessage msg)
        {
            //Version = BmpListener.Version;                
            Id = Guid.NewGuid().ToString("N");
            DateTime = msg?.PeerHeader?.DateTime ?? System.DateTime.UtcNow;
            Peer = msg.PeerHeader;
        }

        [JsonProperty(Order = 1)]
        public string Version { get; set; }

        [JsonProperty(Order = 2)]
        public string Id { get; private set; }

        [JsonProperty(Order = 3)]
        public DateTimeOffset DateTime { get; set; }

        [JsonProperty(Order = 4)]
        public PerPeerHeader Peer { get; }

        public static JsonMessage Create(BmpMessage msg)
        {
            switch (msg.BmpHeader.MessageType)
            {
                case (BmpMessageType.Initiation):
                    return new InitiationMessage(msg);
                case (BmpMessageType.PeerUp):
                    return new PeerUpMessage(msg);
                case (BmpMessageType.PeerDown):
                    return new PeerDownMessage(msg);
                case (BmpMessageType.RouteMonitoring):
                    return new RouteMonitoringMessage(msg);
                case (BmpMessageType.Termination):
                    return null;
                default:
                    return null;
            }
        }

        private sealed class RouteMonitoringMessage : JsonMessage
        {
            public RouteMonitoringMessage(BmpMessage msg) : base(msg)
            {
                Update = ((RouteMonitoring)msg).BgpMessage;
            }                       

            [JsonProperty(Order = 5)]
            public BgpMessage Update { get; }
        }

        private sealed class InitiationMessage : JsonMessage
        {
            public InitiationMessage(BmpMessage msg) : base(msg)
            {
            }

            [JsonProperty(Order = 5)]
            public BmpInitiation Initiation { get; }
        }

        private sealed class PeerUpMessage : JsonMessage
        {
            public PeerUpMessage(BmpMessage msg) : base(msg)
            {
                PeerUp = (PeerUpNotification)msg;
            }

            [JsonProperty(Order = 5)]
            public PeerUpNotification PeerUp { get; }
        }

        private sealed class PeerDownMessage : JsonMessage
        {
            public PeerDownMessage(BmpMessage msg) : base(msg)
            {
                PeerDown = (PeerDownNotification)msg;
            }

            [JsonProperty(Order = 5)]
            PeerDownNotification PeerDown { get; }
        }
    }
}