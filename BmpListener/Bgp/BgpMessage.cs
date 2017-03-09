using System;

namespace BmpListener.Bgp
{
    public abstract class BgpMessage
    {
        protected BgpMessage(BgpHeader bgpHeader)
        {
            Header = bgpHeader;
        }

        public enum Type
        {
            Open = 1,
            Update,
            Notification,
            Keepalive,
            RouteRefresh
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
                case Type.Open:
                    return new BgpOpenMessage(bgpHeader, data, offset);
                case Type.Update:
                    return new BgpUpdateMessage(bgpHeader, data, offset);
                case Type.Notification:
                    return new BgpNotification(bgpHeader, data);
                case Type.Keepalive:
                    return new BgpKeepAliveMessage(bgpHeader);
                case Type.RouteRefresh:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}