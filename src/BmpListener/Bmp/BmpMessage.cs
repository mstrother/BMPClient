using System;

namespace BmpListener.Bmp
{
    public abstract class BmpMessage : IBgpMessage
    {
        public BmpHeader BmpHeader { get; set; }
        public PerPeerHeader PeerHeader { get; set; }

        public abstract void Decode(ArraySegment<byte> data);

        public static BmpMessage DecodeMessage(byte[] bytes)
        {
            var data = new ArraySegment<byte>(bytes);
            return DecodeMessage(data);
        }

        public static BmpMessage DecodeMessage(ArraySegment<byte> data)
        {
            var msgHeader = new BmpHeader();
            msgHeader.Decode(data);

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

            var offset = data.Offset + Constants.BmpCommonHeaderLength;
            var count = data.Count - Constants.BmpCommonHeaderLength;
            data = new ArraySegment<byte>(data.Array, offset, count);

            if (msgHeader.MessageType != BmpMessageType.Initiation)
            {
                msg.PeerHeader = new PerPeerHeader();
                msg.PeerHeader.Decode(data);

                offset += Constants.BmpPerPeerHeaderLength;
                count -= Constants.BmpPerPeerHeaderLength;
                data = new ArraySegment<byte>(data.Array, offset, count);
            }

            msg.Decode(data);
            return msg;
        }
    }
}