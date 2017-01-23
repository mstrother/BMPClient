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
            Type = bgpHeader.Type;
            Length = (int)bgpHeader.Length;
            var offset = data.Offset + 19;
            var count = Length - 19;
            data = new ArraySegment<byte>(data.Array, offset, count);
        }

        [JsonIgnore]
        public int Length { get; }
        [JsonIgnore]
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