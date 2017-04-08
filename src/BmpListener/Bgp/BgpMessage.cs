using System;

namespace BmpListener.Bgp
{
    public abstract class BgpMessage
    {
        public BgpHeader Header { get; private set; }

        public abstract void Decode(ArraySegment<byte> data);

        public static BgpMessage DecodeMessage(ArraySegment<byte> data)
        {
            var msgHeader = new BgpHeader(data);

            var offset = data.Offset + Constants.BgpHeaderLength;
            var count = data.Count - Constants.BgpHeaderLength;
            data = new ArraySegment<byte>(data.Array, offset, count);

            BgpMessage msg;

            switch (msgHeader.Type)
            {
                case BgpMessageType.Open:
                    msg = new BgpOpenMessage();
                    break;
                case BgpMessageType.Update:
                    msg = new BgpUpdateMessage();
                    break;
                case BgpMessageType.Notification:
                    msg = new BgpNotificationMessage();
                    break;
                case BgpMessageType.Keepalive:
                    msg = new BgpKeepAliveMessage();
                    break;
                case BgpMessageType.RouteRefresh:
                    msg = new BgpRouteRefreshMessage();
                    break;
                default:
                    return null;
            }

            msg.Header = msgHeader;
            msg.Decode(data);
            return msg;
        }
    }
}