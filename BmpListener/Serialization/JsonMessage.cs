﻿using BmpListener.Bgp;
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
            Version = "0.0.2";
            Id = Guid.NewGuid().ToString("N");
            DateTime = msg?.PeerHeader?.DateTime ?? DateTime.UtcNow;
            Peer = msg.PeerHeader;
        }

        [JsonProperty(Order = 1)]
        public string Version { get; set; }

        [JsonProperty(Order = 2)]
        public string Id { get; private set; }

        [JsonProperty(Order = 3)]
        public DateTime DateTime { get; set; }

        [JsonProperty(Order = 4)]
        public BmpPeerHeader Peer { get; }

        public static JsonMessage Create(BmpMessage msg)
        {
            switch (msg.BmpHeader.MessageType)
            {
                case (BmpMessage.Type.Initiation):
                    return new InitiationMessage(msg);
                case (BmpMessage.Type.PeerUp):
                    return new PeerUpMessage(msg);
                case (BmpMessage.Type.PeerDown):
                    return new PeerDownMessage(msg);
                case (BmpMessage.Type.RouteMonitoring):
                    return new RouteMonitoringMessage(msg);
                case (BmpMessage.Type.Termination):
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
                var peerUpNotification = (PeerUpNotification)msg;
                LocalAddress = peerUpNotification.LocalAddress;
                LocalPort = peerUpNotification.LocalPort;
                RemotePort = peerUpNotification.RemotePort;
                SentOpenMessage = peerUpNotification.SentOpenMessage;
                ReceivedOpenMessage = peerUpNotification.ReceivedOpenMessage;
            }

            [JsonProperty(Order = 5)]
            public IPAddress LocalAddress { get; }

            [JsonProperty(Order = 6)]
            public ushort LocalPort { get; }

            [JsonProperty(Order = 7)]
            public ushort RemotePort { get; }

            [JsonProperty(Order = 8)]
            public BgpMessage SentOpenMessage { get; }

            [JsonProperty(Order = 9)]
            public BgpMessage ReceivedOpenMessage { get; }
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