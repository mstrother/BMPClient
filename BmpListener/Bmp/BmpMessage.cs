using System;

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

        public enum Type
        {
            RouteMonitoring,
            StatisticsReport,
            PeerDown,
            PeerUp,
            Initiation,
            Termination,
            RouteMirroring
        }

        public BmpHeader BmpHeader { get; private set; }
        public BmpPeerHeader PeerHeader { get; set; }
            
        public static BmpMessage GetBmpMessage(BmpHeader bmpHeader)
        {
            var data = new ArraySegment<byte>();
            return Create(bmpHeader, data);
        }

        public static BmpMessage Create(BmpHeader bmpHeader, ArraySegment<byte> data)
        {
            switch (bmpHeader.MessageType)
            {
                case Type.RouteMonitoring:
                    return new RouteMonitoring(bmpHeader, data);
                case Type.StatisticsReport:
                    return new StatisticsReport(bmpHeader, data);
                case Type.PeerDown:
                    return new PeerDownNotification(bmpHeader, data);
                case Type.PeerUp:
                    return new PeerUpNotification(bmpHeader, data);
                case Type.Initiation:
                    return new BmpInitiation(bmpHeader);
                case Type.Termination:
                    return new BmpTermination(bmpHeader);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}