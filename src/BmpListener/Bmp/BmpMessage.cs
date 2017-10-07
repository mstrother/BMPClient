namespace BmpListener.Bmp
{
    public abstract class BmpMessage
    {
        public BmpHeader BmpHeader { get; private set; }
        public PerPeerHeader PeerHeader { get; private set; }

        public abstract void Decode(byte[] data, int offset);

        public static BmpMessage Create(byte[] data)
        {
            var commonHeaderLength = 6;
            var perPeerHeaderLength = 42;

            var msgHeader = new BmpHeader(data);
            BmpMessage msg;
            
            switch (msgHeader.MessageType)
            {
                case BmpMessageType.Initiation:
                    msg = new BmpInitiation();
                    break;
                case BmpMessageType.RouteMonitoring:
                    msg = new RouteMonitoring();
                    break;
                case BmpMessageType.StatisticsReport:
                    msg = new StatisticsReport();
                    break;
                case BmpMessageType.PeerDown:
                    msg = new PeerDownNotification();
                    break;
                case BmpMessageType.PeerUp:
                    msg = new PeerUpNotification();
                    break;
                case BmpMessageType.Termination:
                    msg = new BmpTermination();
                    break;
                default:
                    return null;
            }

            msg.BmpHeader = msgHeader;
            var offset = commonHeaderLength;

            if (msgHeader.MessageType != BmpMessageType.Initiation)
            {
                msg.PeerHeader = new PerPeerHeader(data, offset);
                offset += perPeerHeaderLength;
            }

            msg.Decode(data, offset);
            return msg;
        }
    }
}