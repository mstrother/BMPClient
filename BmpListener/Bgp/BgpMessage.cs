using System;

namespace BmpListener.Bgp
{
    public abstract class BgpMessage
    {
        protected BgpMessage(BgpHeader bgpHeader)
        {
            Header = bgpHeader;
        }   
        
        public BgpHeader Header { get; }

        public static BgpMessage GetBgpMessage(byte[] data)
        {
            return GetBgpMessage(data, 0);
        }

        public static BgpMessage GetBgpMessage(byte[] data, int offset)
        {
            var bgpHeader = new BgpHeader(data, offset);
            offset += Constants.BgpHeaderLength;

            switch (bgpHeader.Type)
            {
                case BgpMessageType.Open:
                    return new BgpOpenMessage(bgpHeader, data, offset);
                case BgpMessageType.Update:
                    return new BgpUpdateMessage(bgpHeader, data, offset);
                case BgpMessageType.Notification:
                    return new BgpNotification(bgpHeader, data);
                case BgpMessageType.Keepalive:
                    return new BgpKeepAliveMessage(bgpHeader);
                case BgpMessageType.RouteRefresh:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}