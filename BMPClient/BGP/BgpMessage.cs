using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BmpListener.Bgp
{
    public abstract class BgpMessage 
    {
        protected BgpMessage(BgpHeader bgpHeader)
        {
            Length = bgpHeader.Length;
            Type = bgpHeader.Type;
        }

        [JsonIgnore]
        public uint Length { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get; }

        public abstract void DecodeFromBytes(ArraySegment<byte> data);

        public static BgpMessage GetBgpMessage(ArraySegment<byte> data)
        {
            var headerData = new ArraySegment<byte>(data.Array, 0, 19);
            var bgpHeader = new BgpHeader(headerData);
            var msgLength = (int) bgpHeader.Length - 19;
            var msgData = new ArraySegment<byte>(data.Array, 19, msgLength);

            switch (bgpHeader.Type)
            {
                case MessageType.Open:
                    return new BgpOpenMessage(bgpHeader, msgData);
                case MessageType.Update:
                    return new BgpUpdateMessage(bgpHeader, msgData);
                case MessageType.Notification:
                    return new BgpNotification(bgpHeader, msgData);
                //case MessageType.Keepalive:
                //    return new BgpKeepAliveMessage(bgpHeader, msgData);
                //case MessageType.RouteRefresh:
                //    return new BgpRouteMessage(data);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}