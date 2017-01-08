using System;
using System.Linq;
using Newtonsoft.Json;

namespace BmpListener.Bgp
{
    public abstract class BgpMessage
    {
        protected BgpMessage(ref ArraySegment<byte> data)
        {
            var bgpHeader = new BgpHeader(data);
            Length = bgpHeader.Length;
            Type = bgpHeader.Type;
            var msgLength = (int) bgpHeader.Length - 19;
            data = new ArraySegment<byte>(data.Array, 19, msgLength);
        }

        [JsonIgnore]
        public uint Length { get; }

        public MessageType Type { get; }

        public abstract void DecodeFromBytes(ArraySegment<byte> data);

        public static BgpMessage GetBgpMessage(ArraySegment<byte> data)
        {
            var msgType = (MessageType) data.ElementAt(18);
            switch (msgType)
            {
                case MessageType.Open:
                    return new BgpOpenMessage(data);
                case MessageType.Update:
                    return new BgpUpdateMessage(data);
                case MessageType.Notification:
                    return new BgpNotification(data);
                case MessageType.Keepalive:
                    throw new NotImplementedException();
                case MessageType.RouteRefresh:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}