using System;

namespace BmpListener.Bmp
{
    public abstract class BmpMessage
    {
        public BmpHeader BmpHeader { get; private set; }
        public PerPeerHeader PeerHeader { get; private set; }

        public abstract void Decode(byte[] data, int offset);

        public static BmpMessage ParseMessage(byte[] data)
        {
            var bmpHeader = new BmpHeader(data);
            
            if (bmpHeader.MessageType == BmpMessageType.Initiation)
            {
                return new BmpInitiation();
            }

            BmpMessage bmpMsg;

            switch (bmpHeader.MessageType)
            {
                case BmpMessageType.RouteMonitoring:
                    bmpMsg = new RouteMonitoring();
                    break;
                case BmpMessageType.StatisticsReport:
                    bmpMsg = new StatisticsReport();
                    break;
                case BmpMessageType.PeerDown:
                    bmpMsg = new PeerDownNotification();
                    break;
                case BmpMessageType.PeerUp:
                    bmpMsg = new PeerUpNotification();
                    break;
                case BmpMessageType.Initiation:
                    bmpMsg = new BmpInitiation();
                    break;
                case BmpMessageType.Termination:
                    throw new NotImplementedException();
                default:
                    return null;
            }

            var perPeerHeader = new PerPeerHeader(data, Constants.BmpCommonHeaderLength);
            var offset = Constants.BmpCommonHeaderLength + Constants.BmpPerPeerHeaderLength;

            bmpMsg.BmpHeader = bmpHeader;
            bmpMsg.PeerHeader = perPeerHeader;
            bmpMsg.Decode(data, offset);
            return bmpMsg;
        }
    }
}