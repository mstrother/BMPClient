using System;

namespace BmpListener.Bmp
{
    public abstract class BmpMessage
    {
        public BmpHeader BmpHeader { get; private set; }
        public PerPeerHeader PeerHeader { get; private set; }

        public abstract void Decode(ArraySegment<byte> data);
        
        public static BmpMessage DecodeMessage(byte[] data)
        {
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
                    throw new NotImplementedException();
                default:
                    return null;
            }

            msg.BmpHeader = msgHeader;

            var offset = Constants.BmpCommonHeaderLength;
            var count = data.Length - Constants.BmpCommonHeaderLength;

            if (msgHeader.MessageType != BmpMessageType.Initiation)
            {
                msg.PeerHeader = new PerPeerHeader(data, offset);
                offset += Constants.BmpPerPeerHeaderLength;
                count -= Constants.BmpPerPeerHeaderLength;
            }

            var dataSegment = new ArraySegment<byte>(data, offset, count);
            msg.Decode(dataSegment);
            return msg;
        }
    }
}