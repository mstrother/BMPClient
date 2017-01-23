using BmpListener.Bgp;
using Newtonsoft.Json;
using System;

namespace BmpListener.Bmp
{
    public class BmpMessage
    {
        public BmpMessage(BmpHeader header, ArraySegment<byte> data)
        {
            BmpHeader = header;
            PeerHeader = new PeerHeader(data);
            data = new ArraySegment<byte>(data.Array, 42, data.Array.Length - 42);
            switch (header.Type)
            {
                case MessageType.RouteMonitoring:
                    Update = (Bgp.BgpUpdateMessage)Bgp.BgpMessage.GetBgpMessage(data);
                    break;
                case MessageType.StatisticsReport:
                    Body = new StatisticsReport();
                    break;
                case MessageType.PeerDown:
                    Body = new PeerDownNotification(data);
                    break;
                case MessageType.PeerUp:
                    Body = new PeerUpNotification(data);
                    break;
                case MessageType.Initiation:
                    Body = new BmpInitiation();
                    break;
                case MessageType.Termination:
                    Body = new BmpTermination();
                    break;
            }
        }

        public BmpMessage(BmpHeader header)
        {
            BmpHeader = header;
        }

        public BmpHeader BmpHeader { get; private set; }
        public PeerHeader PeerHeader { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IBMPBody Body { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BgpUpdateMessage Update { get; set; }
    }
}