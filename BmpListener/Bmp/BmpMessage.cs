using System;

namespace BmpListener.Bmp
{
    public abstract class BmpMessage
    {
        // RFC 7854
        private const int PerPeerHeaderLength = 42;

        protected BmpMessage(BmpHeader header, ArraySegment<byte> data)
        {
            BmpHeader = header;
            PeerHeader = new PerPeerHeader(data);
            var offset = PerPeerHeaderLength;
            var count = data.Array.Length - offset;
            MessageData = new ArraySegment<byte>(data.Array, offset, count);
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

        protected ArraySegment<byte> MessageData { get; }
        public BmpHeader BmpHeader { get; private set; }
        public PerPeerHeader PeerHeader { get; private set; }

        public static BmpMessage Create(BmpHeader bmpHeader)
        {
            return new BmpInitiation(bmpHeader);
        }

        public static BmpMessage Create(BmpHeader bmpHeader, byte[] data)
        {
            var msg = new ArraySegment<byte>(data);
            switch (bmpHeader.MessageType)
            {
                case Type.RouteMonitoring:
                    return new RouteMonitoring(bmpHeader, msg);
                case Type.StatisticsReport:
                    return new StatisticsReport(bmpHeader, msg);
                case Type.PeerDown:
                    return new PeerDownNotification(bmpHeader, msg);
                case Type.PeerUp:
                    return new PeerUpNotification(bmpHeader, msg);
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