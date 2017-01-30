using BmpListener.Bgp;
using BmpListener.Json;
using Newtonsoft.Json;
using System;
using System.Dynamic;

namespace BmpListener.Bmp
{
    public abstract class BmpMessage
    {
        public BmpMessage(BmpHeader header, ref ArraySegment<byte> data)
        {
            BmpHeader = header;
            PeerHeader = new BmpPeerHeader(data);
            data = new ArraySegment<byte>(data.Array, 42, data.Array.Length - 42);
        }

        public BmpMessage(BmpHeader header)
        {
            BmpHeader = header;
        }

        public BmpHeader BmpHeader { get; private set; }
        public BmpPeerHeader PeerHeader { get; set; }
        
        public string ToJson()
        {
            var serializer = new Json.JsonSerializer();
            return serializer.ToJson(this);
        }

        public static BmpMessage GetBmpMessage(BmpHeader bmpHeader)
        {
            var data = new ArraySegment<byte>();
            return GetBmpMessage(bmpHeader, data);
        }

        public static BmpMessage GetBmpMessage(BmpHeader bmpHeader, ArraySegment<byte> data)
        {
            switch (bmpHeader.Type)
            {
                case MessageType.RouteMonitoring:
                    return new RouteMonitoring(bmpHeader, data);
                case MessageType.StatisticsReport:
                    return new StatisticsReport(bmpHeader, data);
                case MessageType.PeerDown:
                    return new PeerDownNotification(bmpHeader, data);
                case MessageType.PeerUp:
                    return new PeerUpNotification(bmpHeader, data);
                case MessageType.Initiation:
                    return new BmpInitiation(bmpHeader);
                case MessageType.Termination:
                    return new BmpTermination(bmpHeader);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}