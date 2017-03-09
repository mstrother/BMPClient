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
                case BmpMessageType.RouteMonitoring:
                    return new RouteMonitoring(bmpHeader, data);
                case BmpMessageType.StatisticsReport:
                    return new StatisticsReport(bmpHeader, data);
                case BmpMessageType.PeerDown:
                    return new PeerDownNotification(bmpHeader, data);
                case BmpMessageType.PeerUp:
                    return new PeerUpNotification(bmpHeader, data);
                case BmpMessageType.Initiation:
                    return new BmpInitiation(bmpHeader);
                case BmpMessageType.Termination:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}