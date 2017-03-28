using System;

namespace BmpListener.Bgp
{
    public abstract class BgpMessage
    {
        protected BgpMessage(byte[] data, int offset)
        {
            Header = new BgpHeader(data, offset);
        }   
        
        public BgpHeader Header { get; }

        public static BgpMessage GetBgpMessage(byte[] data)
        {
            return Create(data, 0);
        }

        public static BgpMessage Create(byte[] data, int offset)
        {
            switch ((BgpMessageType)data[offset + 18])
            {
                case BgpMessageType.Open:
                    return new BgpOpenMessage(data, offset);
                case BgpMessageType.Update:
                    return new BgpUpdateMessage(data, offset);
                case BgpMessageType.Notification:
                    return new BgpNotification(data, offset);
                case BgpMessageType.Keepalive:
                    return new BgpKeepAliveMessage(data, offset);
                case BgpMessageType.RouteRefresh:
                    throw new NotImplementedException();
                default:
                    return null;
            }
        }
    }
}