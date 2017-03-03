using System;

namespace BmpListener.Bgp
{
    public abstract class BgpMessage
    {
        protected const int BgpHeaderLength = 19;

        protected BgpMessage(ArraySegment<byte> data)
        {
            Header = new BgpHeader(data);
            var offset = data.Offset + BgpHeaderLength;
            var count = Header.Length - BgpHeaderLength;
            MessageData = new ArraySegment<byte>(data.Array, offset, count);
        }

        public enum Type
        {
            Open = 1,
            Update,
            Notification,
            Keepalive,
            RouteRefresh
        }

        protected ArraySegment<byte> MessageData { get; }
        public BgpHeader Header { get; }
        public abstract void DecodeFromBytes(ArraySegment<byte> data);

        public static BgpMessage GetBgpMessage(ArraySegment<byte> data)
        {
            var msgType = (Type)data.Array[data.Offset + 18];
            switch (msgType)
            {
                case Type.Open:
                    return new BgpOpenMessage(data);
                case Type.Update:
                    return new BgpUpdateMessage(data);
                case Type.Notification:
                    return new BgpNotification(data);
                case Type.Keepalive:
                    throw new NotImplementedException();
                case Type.RouteRefresh:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}