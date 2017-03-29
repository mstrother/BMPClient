using System;

namespace BmpListener.Bgp
{
    public abstract class BgpMessage
    {
        public BgpHeader Header { get; protected set; }

        public abstract void Decode(byte[] data, int offset);

        public static BgpMessage ParseMessage(byte[] data)
        {
            return ParseMessage(data, 0);
        }

        public static BgpMessage ParseMessage(byte[] data, int offset)
        {
            var bgpHeader = new BgpHeader();
            bgpHeader.Decode(data, offset);
            BgpMessage bgpMsg;

            switch (bgpHeader.Type)
            {
                case BgpMessageType.Open:
                    bgpMsg = new BgpOpenMessage();
                    break;
                case BgpMessageType.Update:
                    bgpMsg = new BgpUpdateMessage();
                    break;
                case BgpMessageType.Notification:
                    bgpMsg = new BgpNotification();
                    break;
                case BgpMessageType.Keepalive:
                    bgpMsg = new BgpKeepAliveMessage();
                    break;
                case BgpMessageType.RouteRefresh:
                    throw new NotImplementedException();
                default:
                    return null;
            }

            offset += Constants.BgpHeaderLength;

            bgpMsg.Header = bgpHeader;
            bgpMsg.Decode(data, offset);
            return bgpMsg;
        }
    }
}