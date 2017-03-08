using System;

namespace BmpListener.Bmp
{
    public abstract class BmpMessage
    {
        protected BmpMessage(BmpHeader header, byte[] data)
        {
            BmpHeader = header;
            PeerHeader = new PerPeerHeader(data);
        }

        protected BmpMessage(BmpHeader header)
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
        
        public BmpHeader BmpHeader { get; }
        public PerPeerHeader PeerHeader { get; }

        public static BmpMessage Create(BmpHeader bmpHeader)
        {
            return new BmpInitiation(bmpHeader);
        }

        public static BmpMessage Create(BmpHeader bmpHeader, byte[] data)
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
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}