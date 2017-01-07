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
            data = new ArraySegment<byte>(data.Array, 0, 19);
            var bgpHeader = new BgpHeader(data);
            var msgLength = (int) bgpHeader.Length - 19;
            data = new ArraySegment<byte>(data.Array, 19, msgLength);

            switch (bgpHeader.Type)
            {
                case MessageType.Open:
                    return new BgpOpenMessage(bgpHeader, data);
                case MessageType.Update:
                    return new BgpUpdateMessage(bgpHeader, data);
                case MessageType.Notification:
                    return new BgpNotification(bgpHeader, data);
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