namespace BmpListener.Bgp
{
    public abstract class BgpMessage
    {
        public BgpHeader Header { get; protected set; } = new BgpHeader();
        
        public abstract void Decode(byte[] data, int offset);

        public static BgpMessage Create(byte[] data, int offset)
        {
            var msgHeader = new BgpHeader(data, offset);

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
            offset += Constants.BgpHeaderLength;

            msg.Decode(data, offset);
            return msg;
        }
    }
}